using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;


namespace BLL
{
    public class JobSubmit
    {

        public static string JobUpload(string json)
        {
            string[] str1 = json.Split('|');
            string strJson = str1[0];
            string strStrict = str1[1];
            string strRes;
            ILE.IJob job = JsonConvert.DeserializeObject<ILE.Model.Job>(strJson);

            ILE.IResult res = CheckJob(job, strStrict);
            if (!res.Result)
            {
                strRes = JsonConvert.SerializeObject(res);
                return strRes;
            }

            try
            {
                #region 数据提交

                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    //用于删除
                    //var context1 = LEDAO.APIGateWay.GetEntityContext();
                    //结果
                    string strResult = job.Result == true ? "OK" : "NG";
                    string strProcessCode = job.ProcessCode;
                    string strSFC = job.SFC;
                    //工艺流程
                    B_Process_Flow_Detail flowDetail = context.B_Process_Flow_Detail.Where(c => c.process_from == strProcessCode && c.flow_code == job.FlowCode && c.process_result == strResult).First();
                    //提交时间
                    DateTime? newDate = context.NewDate().First();
                    //判断是否为最后一步工艺流程
                    bool BEnd = flowDetail.process_next == "END" ? true : false;
                    //查询SFC状态
                    List<LEDAO.P_SFC_State> sfcList = context.P_SFC_State.Where(c => c.SFC == strSFC).ToList();
                    //获取班别
                    List<B_WorkGroup_Class> Class_codeList = context.sp_Count_Class(job.group_code).ToList();
                    string Class_Code = string.Empty;
                    if (Class_codeList.Count != 0)
                    {
                        Class_Code = Class_codeList[0].class_code.ToString();
                    }
                    else
                    {
                        Class_Code = "";
                    }
                    //获取P_Date
                    List<sp_Count_PDate_Result> P_Date1 = context.sp_Count_PDate(job.group_code).ToList();
                    DateTime P_Date = Convert.ToDateTime(P_Date1[0].P_date);

                    //根据工单号获取父工单及主工单
                    P_WorkOrder ListOrder = context.P_WorkOrder.Where(c => c.order_no == job.OrderNO).First();
                    //根据Product_code获取mat_code
                    //V_Bom_Detail ListBom_Detail = context.V_Bom_Detail.Where(c => c.product_code == job.Product).First();

                    //SFC状态P_SFC_STATE
                    LEDAO.P_SFC_State sfcstate;
                    if (sfcList.Count > 0)
                    {
                        sfcstate = sfcList.First();
                        //流程NG
                        if (strResult == "NG")
                        {
                            sfcstate.fail_times++;
                        }
                        //工艺是否结束
                        if (BEnd && job.Result)
                        {
                            sfcstate.state = 2;
                            sfcstate.end_time = newDate;
                            sfcstate.end_Date = P_Date;
                        }
                        sfcstate.from_process = sfcstate.now_process;
                        sfcstate.now_process = flowDetail.process_next;
                        sfcstate.process_time = newDate;
                        context.ObjectStateManager.ChangeObjectState(sfcstate, System.Data.EntityState.Modified);
                    }
                    else
                    {
                        sfcstate = new LEDAO.P_SFC_State();
                        sfcstate.order_no = job.OrderNO;
                        sfcstate.SFC = job.SFC;
                        sfcstate.initqty = job.QTY;
                        sfcstate.qty = job.QTY;
                        sfcstate.is_tray = job.IsPack;
                        sfcstate.fail_times = strResult == "OK" ? 0 : 1;
                        sfcstate.state = 1;
                        sfcstate.from_process = job.ProcessCode;
                        sfcstate.now_process = flowDetail.process_next;
                        //车间别
                        sfcstate.workshop = job.workshop;
                        //班别，P_Date
                        sfcstate.begin_classcode = Class_Code;
                        sfcstate.begin_Date = P_Date;
                        sfcstate.end_classcode = Class_Code;
                        sfcstate.end_Date = P_Date;
                        //父工单及主工单
                        sfcstate.parent_order = ListOrder.parent_order;
                        sfcstate.main_order = ListOrder.main_order;
                        //mat_code
                        sfcstate.mat_code = ListOrder.product_code;  //ListBom_Detail.mat_code;

                        //工艺是否结束
                        if (BEnd && job.Result)
                        {
                            sfcstate.state = 2;
                            sfcstate.end_time = newDate;
                            sfcstate.end_Date = P_Date;
                            #region
                            //P_WorkOrder orderlst = context.P_WorkOrder.Where(c => c.order_no == job.OrderNO).FirstOrDefault();
                            //decimal orderCount = 0; //工单总完成数
                            //var lst = context.P_GetOrderCompCount(job.OrderNO).FirstOrDefault();
                            //if (lst!=null)
                            //{
                            //    orderCount = (decimal)(lst.qty);
                            //}
                            //if (orderlst!=null)
                            //{
                            //    if (orderlst.qty <= orderCount + job.QTY)
                            //    {
                            //        orderlst.state = 2;
                            //        //context.ObjectStateManager.ChangeObjectState(orderlst, System.Data.EntityState.Modified);
                            //    }
                            //}
                            #endregion
                        }

                        else
                        {
                            sfcstate.state = 1;
                        }
                        sfcstate.input_station = job.StationCode;
                        //sfcstate.begin_time = newDate;
                        sfcstate.begin_time = job.start_time;
                        sfcstate.process_time = newDate;
                        context.P_SFC_State.AddObject(sfcstate);
                    }


                    //过站记录
                    P_SFC_Process_IOLog sfcIOLog;
                    int max = context.P_SFC_Process_IOLog.Where(c => c.SFC == strSFC && c.process_code == job.ProcessCode && c.output_time != null).Count();
                    List<LEDAO.P_SFC_Process_IOLog> sfcIOLogList1 = context.P_SFC_Process_IOLog.Where(c => c.SFC == strSFC && c.process_code == job.ProcessCode && c.output_time == null).ToList();
                    if (sfcIOLogList1.Count > 0)
                    {
                        //将station_code 和 output_time为NULL 的数据删掉（next站记录）
                        var var = from u in context.P_SFC_Process_IOLog where u.station_code == null && u.output_time == null && u.SFC == job.SFC select u;
                        foreach (var del in var)
                        {
                            context.P_SFC_Process_IOLog.DeleteObject(del);
                            //context.SaveChanges();
                        }
                    }
                    List<LEDAO.P_SFC_Process_IOLog> sfcIOLogList2 = context.P_SFC_Process_IOLog.Where(c => c.SFC == strSFC && c.output_time != null).ToList();
                    List<LEDAO.P_SFC_Process_IOLog> sfcIOLogList = context.P_SFC_Process_IOLog.Where(c => c.SFC == strSFC && c.process_code == job.ProcessCode && c.output_time != null).ToList();
                    if (sfcIOLogList.Count > 0)
                    {
                        #region
                        //sfcIOLog = sfcIOLogList.First();
                        //sfcIOLog.fail_times = sfcstate.fail_times;
                        //sfcIOLog.pass = max + 1;
                        //sfcIOLog.output_time = newDate;
                        //sfcIOLog.station_code = job.StationCode;
                        //sfcIOLog.emp_code = job.EmpCode;
                        //context.ObjectStateManager.ChangeObjectState(sfcIOLog, System.Data.EntityState.Modified);
                        #endregion
                        //将station_code 和 output_time为NULL 的数据删掉
                        var var = from u in context.P_SFC_Process_IOLog where u.station_code == null && u.output_time == null && u.SFC == job.SFC select u;
                        foreach (var del in var)
                        {
                            context.P_SFC_Process_IOLog.DeleteObject(del);
                        }
                        //context.SaveChanges();

                        List<P_SFC_Process_IOLog> modle = context.P_SFC_Process_IOLog.Where(X => X.station_code != null && X.output_time != null && X.SFC == job.SFC && X.process_code == job.ProcessCode).ToList();
                        foreach (var mod in modle)
                        {
                            sfcIOLog = mod;
                            sfcIOLog.pass = sfcIOLog.pass - 1;
                            context.ObjectStateManager.ChangeObjectState(sfcIOLog, System.Data.EntityState.Modified);
                        }
                        sfcIOLog = new P_SFC_Process_IOLog();
                        sfcIOLog.order_no = job.OrderNO;
                        sfcIOLog.SFC = job.SFC;
                        sfcIOLog.initqty = job.QTY;   // 需要验证数据正确性
                        sfcIOLog.qty = job.QTY;
                        sfcIOLog.fail_times = sfcstate.fail_times;
                        sfcIOLog.group_code = flowDetail.process_from_group;
                        sfcIOLog.process_code = job.ProcessCode;
                        sfcIOLog.station_code = job.StationCode;
                        if (sfcIOLogList2.Count > 0)
                        {
                            sfcIOLog.input_time = sfcIOLogList2[sfcIOLogList2.Count - 1].output_time;
                        }
                        else
                        {
                            sfcIOLog.input_time = newDate;
                        }
                        sfcIOLog.output_time = newDate;
                        sfcIOLog.emp_code = job.EmpCode;
                        //sfcIOLog.pass = max + 1;
                        sfcIOLog.pass = 1;
                        //车间别
                        sfcIOLog.workshop = job.workshop;
                        sfcIOLog.class_code = Class_Code;
                        sfcIOLog.p_date = P_Date;
                        sfcIOLog.start_time = job.start_time;
                        //sfcIOLog.p_date 没确定
                        context.P_SFC_Process_IOLog.AddObject(sfcIOLog);
                    }
                    else
                    {
                        //SFC记录
                        sfcIOLog = new P_SFC_Process_IOLog();
                        sfcIOLog.order_no = job.OrderNO;
                        sfcIOLog.SFC = job.SFC;
                        sfcIOLog.initqty = job.QTY;
                        sfcIOLog.qty = job.QTY;
                        sfcIOLog.fail_times = sfcstate.fail_times;
                        sfcIOLog.group_code = flowDetail.process_from_group;
                        sfcIOLog.process_code = job.ProcessCode;
                        sfcIOLog.station_code = job.StationCode;
                        if (sfcIOLogList2.Count > 0)
                        {
                            sfcIOLog.input_time = sfcIOLogList2[sfcIOLogList2.Count - 1].output_time; 
                        }
                        else
                        {
                            sfcIOLog.input_time = job.start_time;
                        }
                        sfcIOLog.output_time = newDate;
                        sfcIOLog.emp_code = job.EmpCode;
                        sfcIOLog.pass = 1;
                        //车间别
                        sfcIOLog.workshop = job.workshop;
                        sfcIOLog.class_code = Class_Code;
                        sfcIOLog.p_date = P_Date;
                        sfcIOLog.start_time = job.start_time;
                        //sfcIOLog.p_date 没确定
                        context.P_SFC_Process_IOLog.AddObject(sfcIOLog);
                    }

                    //传建下一个过站记录
                    P_SFC_Process_IOLog sfcIOLogNext;
                    if (!string.IsNullOrEmpty(flowDetail.process_next) && flowDetail.process_next != "END")
                    {
                        //SFC记录
                        sfcIOLogNext = new P_SFC_Process_IOLog();
                        sfcIOLogNext.order_no = job.OrderNO;
                        sfcIOLogNext.SFC = job.SFC;
                        sfcIOLogNext.qty = job.QTY;
                        sfcIOLogNext.group_code = flowDetail.process_next_group;
                        sfcIOLogNext.process_code = flowDetail.process_next;
                        sfcIOLogNext.fail_times = sfcstate.fail_times;
                        sfcIOLogNext.pass = 0;
                        sfcIOLogNext.input_time = newDate;
                        //车间别
                        sfcIOLogNext.workshop = job.workshop;
                        sfcIOLogNext.class_code = Class_Code;
                        sfcIOLogNext.p_date = P_Date;
                        sfcIOLogNext.start_time = job.start_time;
                        context.P_SFC_Process_IOLog.AddObject(sfcIOLogNext);
                    }


                    //工步字段
                    LEDAO.P_SFC_ProcessData sfcdata;
                    LEDAO.P_Mat_WIP_Seed wip_seed;
                    LEDAO.P_Material_WIP material_wip;
                    LEDAO.P_SFC_ProcessSonData processSonData;
                    LEDAO.P_SFC_Jude sfc_jude;
                    for (int i = job.IndexBack; i < job.StepList.Count; i++)
                    {
                        ILE.Step step = job.StepList[i];
                        if (step.IsRecord != 1 | step.StepDetail == null)
                        {
                            continue;
                        }
                        //保存子工步数据
                        if (step.IsRecord != 1 | step.StepSonDetail != null)
                        {
                            foreach (var stepd in step.StepSonDetail)
                            {
                                processSonData = new P_SFC_ProcessSonData();
                                processSonData.orderno = job.OrderNO;
                                processSonData.sfc = job.SFC;
                                processSonData.stepid = stepd.stepsoncode;
                                processSonData.stepval = stepd.StepVal;
                                processSonData.steppre = stepd.stepSonPre;
                                processSonData.parentstepid = step.StepCode;
                                processSonData.stepconclude = stepd.stepconclude;
                                processSonData.isfedbatch = 0;
                                processSonData.inputtime = DateTime.Now;
                                context.P_SFC_ProcessSonData.AddObject(processSonData);
                            }
                        }
                        //手动耗料
                        string lot_no = job.StepList[i].StepValue;
                        string mat_code = job.StepList[i].Matcode;
                        string product = job.Product;
                        string drive = job.StepList[i].DriveName;
                        var res2 = (from a in context.P_Material_WIP.Where(X => X.lot_no == lot_no) select new { lot_no = a.lot_no }).ToList();
                        if (drive == "DP_Expend" && res2.Count != 0)
                        {
                            //如果所消耗物料是单件，把WIP表中此批次的STATE改为2
                            var res1 = (from a in context.V_ProductMaterial.Where(X => X.p_code == mat_code) select new { product_code = a.p_code, max_qty = a.max_qty }).ToList();
                            string maxqty = res1[0].max_qty.ToString();
                            if (maxqty == "1.000")
                            {
                                var model = context.P_Material_WIP.Where(X => X.lot_no == lot_no).FirstOrDefault();
                                model.state = 2;
                            }
                        }


                        string flowcode = job.FlowCode;

 
                        var res3 = (from a in context.B_Process_Flow.Where(X => X.flow_code == flowcode) select new { flow_name = a.flow_name,bom_id = a.bom_id }).ToList();

                        int bomid =Convert.ToInt32(res3[0].bom_id);

                        var res4 = (from a in context.B_Bom.Where(X => X.bom_id == bomid) select new { bom_code = a.bom_code}).ToList();

                        string bomcode = res4[0].bom_code;

                        var bom_detail = (from a in context.V_Bom_Detail.Where(X => X.mat_code == mat_code && X.product_code == product && X.bom_code == bomcode) select new { qty = a.qty, baseqty = a.base_qty }).ToList();

                        
                        decimal mat_qty = 0;
                        int baseqty = 0;
                        if (bom_detail != null && bom_detail.Count > 0)
                        {
                            for (int m = 0; m < bom_detail.Count; m++)
                            {
                                mat_qty = (decimal)(bom_detail[m].qty);
                                baseqty = (int)bom_detail[m].baseqty;
                            }
                        }
                        else
                        {
                            mat_qty = 1;
                            baseqty = 1;
                        }
                        float percentum = job.StepList[i].consume_percent;
                        decimal qty = job.QTY * (mat_qty / baseqty) * Convert.ToDecimal(percentum);
                        foreach (var stepdata in step.StepDetail)
                        {

                            #region 扣料
                            //自动投料时扣料
                            if (stepdata.seed_id != "" && !string.IsNullOrEmpty(stepdata.seed_id))
                            {
                                string Usedqty = "0";
                                //int seed_id = 0;
                                int seed_id = Convert.ToInt32(stepdata.seed_id.ToString());
                                decimal lot_qty = decimal.Parse((stepdata.lotqty).ToString());
                                decimal surplusqty = 0;
                                if (step.StepDetail.Count > 1)
                                {
                                    //1：平均消耗 2：顺序消耗
                                    if (stepdata.consume_type == 1)
                                    {
                                        List<ILE.StepData> stepDetail = new List<ILE.StepData>();
                                        bool isPositive = false; ;
                                        decimal qtySum = 0;
                                        foreach (var stepdata2 in step.StepDetail)
                                        {
                                            if (stepdata2.lotqty > 0 && stepdata2 == step.StepDetail[step.StepDetail.Count - 1])
                                            {
                                                isPositive = true;
                                                break;
                                            }
                                        }
                                        if (isPositive)
                                        {
                                            foreach (var stepdata3 in step.StepDetail)
                                            {
                                                if (stepdata3.lotqty > 0)
                                                {
                                                    stepDetail.Add(stepdata3);
                                                }
                                            }
                                            foreach (var stepdata1 in stepDetail)
                                            {
                                                qtySum += Math.Abs(decimal.Parse((stepdata1.lotqty).ToString()));
                                            }
                                        }
                                        else
                                        {
                                            foreach (var stepdata1 in step.StepDetail)
                                            {
                                                qtySum += Math.Abs(decimal.Parse((stepdata1.lotqty).ToString()));
                                            }
                                        }
                                        Usedqty = (lot_qty * (qty / qtySum)).ToString("#0.0000");
                                    }
                                    else if (stepdata.consume_type == 2)
                                    {
                                        if (lot_qty > 0)
                                        {
                                            surplusqty = (qty - Math.Abs(lot_qty));
                                        }
                                        else
                                        {
                                            surplusqty = qty;
                                        }
                                        if (surplusqty >= 0)
                                        {
                                            Usedqty = lot_qty.ToString();
                                            if (lot_qty > 0)
                                            {
                                                qty -= Math.Abs(lot_qty);
                                            }
                                            else
                                            {
                                                qty = qty;
                                            }
                                        }
                                        if (stepdata != step.StepDetail[step.StepDetail.Count - 1] && (decimal.Parse(Usedqty) < 0 || decimal.Parse(Usedqty) == 0))
                                        {
                                            Usedqty = "0";
                                        }
                                        //允许超越时算法(做的多,投的料少，且允许超越)
                                        if (stepdata == step.StepDetail[step.StepDetail.Count - 1] && surplusqty > 0)
                                        {
                                            //Usedqty = (Math.Abs(surplusqty)).ToString();
                                            Usedqty = ((job.QTY * (mat_qty / baseqty))).ToString();
                                        }
                                        //做的少，投的料多，最后一个批次扣料算法
                                        if (surplusqty < 0 && qty != 0)
                                        {
                                            Usedqty = (lot_qty - Math.Abs(surplusqty)).ToString();
                                            qty = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    Usedqty = (job.QTY * (mat_qty / baseqty) * Convert.ToDecimal(percentum)).ToString();
                                }
                                List<P_Mat_WIP_Seed> modle = context.P_Mat_WIP_Seed.Where(X => X.wip_id == seed_id && X.station_code == job.StationCode).ToList();
                                foreach (var mod in modle)
                                {
                                    wip_seed = mod;
                                    if (stepdata == step.StepDetail[step.StepDetail.Count - 1] && (stepdata.lotqty < 0 || stepdata.lotqty == 0) && stepdata.consume_type == 2)
                                    {
                                        wip_seed.lot_qty -= (job.QTY * (mat_qty / baseqty)) * decimal.Parse(percentum.ToString());
                                    }
                                    else
                                    {
                                        if (stepdata == step.StepDetail[step.StepDetail.Count - 1] && surplusqty > 0 && stepdata.consume_type == 2)
                                        {
                                            wip_seed.lot_qty -= decimal.Parse(Usedqty) * decimal.Parse(percentum.ToString());
                                        }
                                        else
                                        {
                                            wip_seed.lot_qty -= Math.Abs(decimal.Parse(Usedqty));
                                        }
                                    }
                                    context.ObjectStateManager.ChangeObjectState(wip_seed, System.Data.EntityState.Modified);
                                }

                            }

                            //手动投料时扣料
                            if (stepdata.wip_id != "" && !string.IsNullOrEmpty(stepdata.wip_id))
                            {
                                decimal usedqty = job.QTY * (mat_qty / baseqty);
                                int wip_id = Convert.ToInt32(stepdata.wip_id);
                                List<P_Material_WIP> modle = context.P_Material_WIP.Where(X => X.id == wip_id).ToList();
                                foreach (var mod in modle)
                                {
                                    material_wip = mod;
                                    material_wip.lot_qty -= decimal.Parse(usedqty.ToString()) * decimal.Parse(percentum.ToString());
                                    context.ObjectStateManager.ChangeObjectState(material_wip, System.Data.EntityState.Modified);
                                }
                            }
                            #endregion

                            #region 保存待判数据
                            string lot_sfc = stepdata.StepVal.ToString();
                            List<LEDAO.P_SFC_Jude> Listjude = context.P_SFC_Jude.Where(c => c.sfc == lot_sfc && c.state == 0).ToList();
                            if (Listjude != null)
                            {
                                foreach (var jude in Listjude)
                                {
                                    sfc_jude = new P_SFC_Jude();
                                    sfc_jude.jude_code = jude.jude_code;
                                    sfc_jude.jude_name = jude.jude_name;
                                    sfc_jude.sfc = job.SFC;
                                    sfc_jude.state = 0;
                                    context.P_SFC_Jude.AddObject(sfc_jude);
                                }

                            }
                            #endregion

                            //保存P_SFC_ProcessData
                            int stepid = Convert.ToInt32(job.StepList[i].StepID);
                            List<LEDAO.P_SFC_ProcessData> sfcDataList = context.P_SFC_ProcessData.Where(c => c.SFC == job.SFC && c.step_id == stepid).ToList();
                            if (sfcDataList.Count > 0)
                            {
                                #region
                                List<P_SFC_ProcessData> model = context.P_SFC_ProcessData.Where(c => c.SFC == job.SFC && c.step_id == stepid).ToList();
                                foreach (var mod in model)
                                {
                                    sfcdata = mod;
                                    sfcdata.pass = sfcdata.pass - 1;
                                    context.ObjectStateManager.ChangeObjectState(sfcdata, System.Data.EntityState.Modified);
                                }
                                #endregion

                                //判断当前数据库中有几条数据，pass则+1
                                sfcdata = new LEDAO.P_SFC_ProcessData();
                                sfcdata.idx = step.Idx;
                                sfcdata.order_no = job.OrderNO;
                                sfcdata.pid = job.Pid;
                                sfcdata.step_id = step.StepID;
                                sfcdata.SFC = job.SFC;
                                sfcdata.step_type = step.StepType;
                                sfcdata.station_code = job.StationCode;
                                sfcdata.fail_times = sfcstate.fail_times;
                                sfcdata.mat_code = stepdata.matCode;
                                //sfcdata.pass = sfcDataList.Count + 1;
                                sfcdata.pass = 1;
                                sfcdata.qty = stepdata.qty;
                                sfcdata.val = stepdata.StepVal;
                                sfcdata.input_time = stepdata.InPutDate;
                                sfcdata.unit = step.Unit;
                                //车间别
                                sfcstate.workshop = job.workshop;
                                sfcdata.class_code = Class_Code;
                                sfcdata.P_Date = P_Date;
                                sfcdata.emp_code = job.EmpCode;
                                context.P_SFC_ProcessData.AddObject(sfcdata);
                            }
                            else
                            {
                                sfcdata = new LEDAO.P_SFC_ProcessData();
                                sfcdata.idx = step.Idx;
                                sfcdata.order_no = job.OrderNO;
                                sfcdata.pid = job.Pid;
                                sfcdata.step_id = step.StepID;
                                sfcdata.SFC = job.SFC;
                                sfcdata.step_type = step.StepType;
                                sfcdata.station_code = job.StationCode;
                                sfcdata.fail_times = sfcstate.fail_times;
                                sfcdata.mat_code = stepdata.matCode;
                                //sfcdata.pass = sfcIOLog.pass;
                                sfcdata.pass = 1;
                                sfcdata.qty = stepdata.qty;
                                sfcdata.val = stepdata.StepVal;
                                sfcdata.input_time = stepdata.InPutDate;
                                sfcdata.unit = step.Unit;
                                sfcdata.emp_code = job.EmpCode;
                                //车间别
                                sfcdata.workshop = job.workshop;
                                sfcdata.class_code = Class_Code;
                                sfcdata.P_Date = P_Date;
                                context.P_SFC_ProcessData.AddObject(sfcdata);
                            }
                        }
                    }

                    if (job.QTYOrder + job.QTY >= job.MaxQTYOrder)
                    {
                        LEDAO.P_WorkOrder order = context.P_WorkOrder.Where(c => c.order_no == job.OrderNO).First();
                        order.state = 2;
                        context.ObjectStateManager.ChangeObjectState(order, System.Data.EntityState.Modified);
                    }
                    LEDAO.P_Material_WIP wip;
                    if (BEnd && job.Result)
                    {
                        try
                        {
                            List<LEDAO.P_Material_WIP> wip1 = context.P_Material_WIP.Where(c => c.lot_no == job.SFC && c.workshop == job.workshop && c.mat_code == job.Product).ToList();
                            if (wip1.Count < 1)
                            {
                                wip = new P_Material_WIP();
                                wip.lot_no = job.SFC;
                                wip.input_qty = job.QTY;
                                wip.lot_qty = job.QTY;
                                wip.mat_code = job.Product;
                                wip.input_time = newDate;
                                wip.Parent_order = job.FatherOrderNO;
                                wip.order_no = job.OrderNO;
                                wip.bill_no = job.OrderNO;
                                wip.parent_station = job.StationCode;
                                wip.state = 0; 
                                //车间别
                                wip.workshop = job.workshop;
                                context.P_Material_WIP.AddObject(wip);  
                            }
                        }
                        catch
                        {

                        }
                    }

                    //LEDAO.P_FailLog failLog;
                    //LEDAO.P_Fail_Detail failDetail;
                    //if (job.NGPheno.Count > 0)
                    //{
                    //    failLog = new P_FailLog();
                    //    failLog.fail_times = sfcstate.fail_times;
                    //    failLog.from_emp = job.EmpCode;
                    //    failLog.from_process = job.ProcessCode;
                    //    failLog.from_station = job.StationCode;
                    //    failLog.input_time = newDate;
                    //    failLog.order_no = job.OrderNO;
                    //    failLog.sfc = job.SFC;
                    //    failLog.state = job.Result ? 0 : 1;
                    //    failLog.Disposal_Process = flowDetail.disposal_code;
                    //    failLog.process_code = flowDetail.process_next;
                    //    failLog.fguid = System.Guid.NewGuid().ToString();
                    //    context.P_FailLog.AddObject(failLog);

                    //    for (int i = 0; i < job.NGPheno.Count; i++)
                    //    {
                    //        failDetail = new P_Fail_Detail();
                    //        failDetail.fguid = failLog.fguid;
                    //        failDetail.input_time = newDate;
                    //        failDetail.ng_code = job.NGPheno[i];
                    //        failDetail.order_no = job.OrderNO;
                    //        failDetail.sfc = job.SFC;
                    //        context.P_Fail_Detail.AddObject(failDetail);
                    //    }
                    //}
                    context.SaveChanges();
                }
                #endregion

                res.ExtMessage = "提交完成";
                res.Result = true;
                strRes = JsonConvert.SerializeObject(res);
                return strRes;
            }
            catch (Exception exc)
            {
                res.ExtMessage = "提交异常:" + exc.Message;
                res.Result = false;
                strRes = JsonConvert.SerializeObject(res);
                return strRes;
            }
        }

        //检验Job数据
        static ILE.IResult CheckJob(ILE.IJob job, string strStrict)
        {
            ILE.IResult res = new ILE.LEResult();
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                LEDAO.P_Result va = context.sp_CheckJobSumit(job.FlowCode, job.ProcessCode, job.StationCode, job.SFC, job.EmpCode, job.RouteType, job.Result == true ? "OK" : "NG", strStrict).First();

                res.ExtMessage = va.ExtMessage;
                res.Result = va.Result == "true" ? true : false;
                return res;
            }
        }

        public static string CommitReTest(string json)
        {
            try
            {
                //参数解析 批次号集合多个已","隔开;重测工序代码;工序类别（首工序，中间工序...）;物料代码;工单;车间代码
                string[] arrstrjsonitem = json.Split(';');
                if (arrstrjsonitem.Length < 6)
                {
                    return "CommitReTest-Fail;传入参数不全";
                }
                string[] arrstrsfcitem = arrstrjsonitem[0].Split(',');
                if (arrstrsfcitem.Length < 1)
                {
                    return "CommitReTest-Fail;传入批次号为空";
                }
                List<string> lstsfcitem = arrstrsfcitem.ToList();
                string strprocess = arrstrjsonitem[1].Trim();
                string strprocesstype = arrstrjsonitem[2].Trim();
                string strmatcode = arrstrjsonitem[3].Trim();
                string strworkorder = arrstrjsonitem[4].Trim();
                string strworkshop = arrstrjsonitem[5].Trim();
                //提交结果
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    //--del P_Material_WIP --删记录
                    var varWIP = from u in context.P_Material_WIP where lstsfcitem.Contains(u.lot_no) && u.workshop == strworkshop select u;
                    foreach (var del in varWIP)
                    {
                        context.P_Material_WIP.DeleteObject(del);
                    }

                    //--mod P_SFC_State   --- state
                    var varsfcstate = from u in context.P_SFC_State where lstsfcitem.Contains(u.SFC) && u.workshop == strworkshop select u;
                    foreach (var mod in varsfcstate)
                    {
                        mod.state = 1;
                        mod.now_process = strprocess;
                        mod.process_time = context.NewDate().First();
                    }
                    #region 先不修改P_SFC_Process_IOLog 数据，待验证
                    ////--mod P_SFC_Process_IOLog  --首工序不用处理，非首工序，需要把对应工序增加一条记录
                    //List<string> lstafterprocess = new List<string>();
                    //if (!strprocesstype.Contains("首工站"))
                    //{
                    //    //获取返回工序后面所有工序
                    //    string strprocesslist = Process.GetProcessFlowDetail(strworkorder);
                    //    if (string.IsNullOrWhiteSpace(strprocesslist))
                    //    {
                    //        //没有数据
                    //        return "CommitReTest-Fail;获取到当前工序集合失败";
                    //    }

                    //    List<B_ProcessList> lstprocess = JsonConvert.DeserializeObject<List<B_ProcessList>>(strprocesslist);
                    //    if (lstprocess.Count <= 0)
                    //    {
                    //        //没有数据
                    //        return "CommitReTest-Fail;获取到当前工序集合失败";
                    //    }
                    //    int istart = 0;
                    //    for (int pi = 0; pi < lstprocess.Count; pi++)
                    //    {
                    //        if (lstprocess[pi].process_code.ToString().Trim() == strprocess)
                    //        {
                    //            istart = 1;
                    //        }
                    //        if (istart == 1
                    //            && !lstprocess[pi].process_code.ToString().Trim().Contains("END"))
                    //        {
                    //            lstafterprocess.Add(lstprocess[pi].process_code.ToString().Trim());
                    //        }
                    //    }
                    //    if(lstafterprocess.Count >0)
                    //    {
                    //        var varsfciolog = from u in context.P_SFC_Process_IOLog where lstsfcitem.Contains(u.SFC) && lstafterprocess.Contains(u.process_code)  select u;
                    //        foreach (var iolog in varsfciolog)
                    //        {
                    //            iolog.output_time = null;
                    //            iolog.station_code = null;
                    //        }
                    //    }
                    //}
                    #endregion
                    context.SaveChanges();
                }
                return "OK";
            }
            catch (Exception exp)
            {
                return "CommitReTest-Fail;catch:" + exp.Message.ToString();
            }
        }
        public static string CommitFailNewSFCInfo(string json)
        {
            string iRtn = "0"; //0-成功，非0-失败
            try
            {
                string[] arrstrjsonitem = json.Split(';');
                if (arrstrjsonitem.Length < 3)
                {
                    iRtn = "1:传入参数不全";  //传入参数不全
                    return iRtn;
                }
                string stroldsfc = arrstrjsonitem[0];
                string strnewsfc = arrstrjsonitem[1];
                string strfailqty = arrstrjsonitem[2];
                decimal dfailqty = decimal.Parse(strfailqty);
                //提交结果
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    #region 删除数据
                    var delstate = from u in context.P_SFC_State where u.SFC == strnewsfc select u;
                    foreach (var del in delstate)
                    {
                        context.P_SFC_State.DeleteObject(del);
                    }
                    var delilog = from u in context.P_SFC_Process_IOLog where u.SFC == strnewsfc select u;
                    foreach (var del in delilog)
                    {
                        context.P_SFC_Process_IOLog.DeleteObject(del);
                    }
                    var delData = from u in context.P_SFC_ProcessData where u.SFC == strnewsfc select u;
                    foreach (var del in delData)
                    {
                        context.P_SFC_ProcessData.DeleteObject(del);
                    }
                    var delWip = from u in context.P_Material_WIP where u.lot_no == strnewsfc select u;
                    foreach (var del in delWip)
                    {
                        context.P_Material_WIP.DeleteObject(del);
                    }
                    #endregion
                    //context.SaveChanges();   //先提交
                    #region  增加数据
                    //--P_SFC_State
                    var varstate = from u in context.P_SFC_State where u.SFC == stroldsfc select u;
                    foreach (var cnew in varstate)
                    {
                        cnew.qty = cnew.qty - dfailqty;

                        //产生批次记录
                        P_SFC_State pnewsfcstate = new P_SFC_State();

                        //pnewsfcstate = cnew;
                        //pnewsfcstate.id = 0;
                        pnewsfcstate.grade_id = cnew.grade_id;
                        pnewsfcstate.grade_type = cnew.grade_type;
                        pnewsfcstate.input_station = cnew.input_station;
                        pnewsfcstate.is_tray = cnew.is_tray;
                        pnewsfcstate.main_order = cnew.main_order;
                        pnewsfcstate.mat_code = cnew.mat_code;
                        pnewsfcstate.now_process = cnew.now_process;
                        pnewsfcstate.order_no = cnew.order_no;
                        pnewsfcstate.parent_order = cnew.parent_order;
                        pnewsfcstate.process_time = cnew.process_time;
                        pnewsfcstate.SFC = strnewsfc;
                        pnewsfcstate.qty = dfailqty;
                        pnewsfcstate.initqty = dfailqty;
                        pnewsfcstate.state = cnew.state;
                        pnewsfcstate.workshop = cnew.workshop;
                        pnewsfcstate.fail_times = cnew.fail_times;
                        pnewsfcstate.from_process = cnew.from_process;
                        pnewsfcstate.begin_time = cnew.begin_time;
                        pnewsfcstate.end_time = cnew.end_time;
                        pnewsfcstate.begin_Date = cnew.begin_Date;
                        pnewsfcstate.begin_classcode = cnew.begin_classcode;
                        pnewsfcstate.end_Date = cnew.end_Date;
                        pnewsfcstate.end_classcode = cnew.end_classcode;

                        context.P_SFC_State.AddObject(pnewsfcstate);
                    }
                    //--P_SFC_Process_IOLog
                    var varIoLog = from u in context.P_SFC_Process_IOLog where u.SFC == stroldsfc select u;
                    foreach (var cnewiolog in varIoLog)
                    {
                        cnewiolog.qty = cnewiolog.qty - dfailqty;

                        //产生批次记录
                        P_SFC_Process_IOLog pnewsfcIolog = new P_SFC_Process_IOLog();

                        //pnewsfcIolog = cnewiolog;
                        //pnewsfcIolog.id = 0;
                        pnewsfcIolog.class_code = cnewiolog.class_code;
                        pnewsfcIolog.emp_code = cnewiolog.emp_code;
                        pnewsfcIolog.fail_times = cnewiolog.fail_times;
                        pnewsfcIolog.group_code = cnewiolog.group_code;
                        pnewsfcIolog.input_time = cnewiolog.input_time;
                        pnewsfcIolog.order_no = cnewiolog.order_no;
                        pnewsfcIolog.output_time = cnewiolog.output_time;
                        pnewsfcIolog.p_date = cnewiolog.p_date;
                        pnewsfcIolog.pass = cnewiolog.pass;
                        pnewsfcIolog.process_code = cnewiolog.process_code;
                        pnewsfcIolog.start_time = cnewiolog.start_time;
                        pnewsfcIolog.station_code = cnewiolog.station_code;
                        pnewsfcIolog.workshop = cnewiolog.workshop;

                        pnewsfcIolog.SFC = strnewsfc;
                        pnewsfcIolog.qty = dfailqty;


                        context.P_SFC_Process_IOLog.AddObject(pnewsfcIolog);
                    }
                    //--P_SFC_ProcessData
                    var varData = from u in context.P_SFC_ProcessData where u.SFC == stroldsfc select u;
                    foreach (var cnewData in varData)
                    {
                        //cnewData.qty = cnewData.qty - dfailqty;   //当前P_SFC_ProcessData qty为0，不需要修改数据 

                        //产生批次记录
                        P_SFC_ProcessData pnewsfcData = new P_SFC_ProcessData();

                        //pnewsfcData = cnewData;
                        //pnewsfcData.id = 0;
                        pnewsfcData.class_code = cnewData.class_code;
                        pnewsfcData.emp_code = cnewData.emp_code;
                        pnewsfcData.fail_times = cnewData.fail_times;
                        pnewsfcData.idx = cnewData.idx;
                        pnewsfcData.input_time = cnewData.input_time;
                        pnewsfcData.mat_code = cnewData.mat_code;
                        pnewsfcData.order_no = cnewData.order_no;
                        pnewsfcData.P_Date = cnewData.P_Date;
                        pnewsfcData.pass = cnewData.pass;
                        pnewsfcData.pid = cnewData.pid;
                        pnewsfcData.qty = 0;
                        pnewsfcData.SFC = strnewsfc;
                        pnewsfcData.station_code = cnewData.station_code;
                        pnewsfcData.step_id = cnewData.step_id;
                        pnewsfcData.step_type = cnewData.step_type;
                        pnewsfcData.unit = cnewData.unit;
                        pnewsfcData.val = cnewData.val;
                        pnewsfcData.workshop = cnewData.workshop;

                        context.P_SFC_ProcessData.AddObject(pnewsfcData);
                    }
                    //--P_Material_WIP
                    var varWip = from u in context.P_Material_WIP where u.lot_no == stroldsfc select u;
                    foreach (var cnewWip in varWip)
                    {
                        if (cnewWip.lot_qty - dfailqty < 0)
                        {
                            continue;
                        }
                        cnewWip.lot_qty = cnewWip.lot_qty - dfailqty;
                        #region
                        //产生批次记录
                        //P_Material_WIP pnewsfcWip = new P_Material_WIP();
                        //pnewsfcWip.bill_no = cnewWip.bill_no;
                        //pnewsfcWip.feed_time = cnewWip.feed_time;
                        //pnewsfcWip.input_time = cnewWip.input_time;
                        //pnewsfcWip.lot_qty = dfailqty;
                        //pnewsfcWip.lot_no = strnewsfc;
                        //pnewsfcWip.mat_code = cnewWip.mat_code;
                        //pnewsfcWip.order_no = cnewWip.order_no;
                        //pnewsfcWip.Parent_order = cnewWip.Parent_order;
                        //pnewsfcWip.parent_station = cnewWip.parent_station;
                        //pnewsfcWip.point_id = cnewWip.point_id;
                        //pnewsfcWip.state = cnewWip.state;
                        //pnewsfcWip.station_code = cnewWip.station_code;
                        //pnewsfcWip.workshop = cnewWip.workshop;
                        //context.P_Material_WIP.AddObject(pnewsfcWip);
                        #endregion
                    }
                    #endregion

                    #region 修改被拆分批次
                    var varsfcstate = from u in context.P_SFC_State where u.SFC == stroldsfc select u;
                    foreach (var mod in varsfcstate)
                    {
                        mod.initqty -= dfailqty;
                        //mod.qty -= dfailqty;
                    }

                    #endregion
                    context.SaveChanges();
                }
                return iRtn;
            }
            catch (Exception exp)
            {
                iRtn = "9:" + exp.Message;
                return iRtn;
            }
        }
    }
}