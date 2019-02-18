using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Net;
using LEDAO;
using System.Windows.Forms;

namespace DP_Expend
{
    public class SPO : ILE.ISPO
    {
        /// <summary>
        /// 手动耗料驱动
        /// </summary>
        /// <param name="jobModel"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public ILE.IResult DoWork(ILE.IJob jobModel, string val)
        {
            ILE.LEResult result = new ILE.LEResult();
            try
            {
                ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                string orderon = jobModel.OrderNO;
                string matCode = jobModel.StepList[jobModel.StepIdx].Matcode;
                string Station = jobModel.StationCode;
                string Workshop = jobModel.workshop;
                string lot = val;
                string product_code = jobModel.Product;
                string flow_code = jobModel.FlowCode.ToString();
                //根据工艺查找bom_id
                int bom_id = int.Parse(clien.RunServerAPI("BLL.Product", "GetBomByFlow", flow_code));
                float consume_percent = jobModel.StepList[jobModel.StepIdx].consume_percent;
                int enable = int.Parse(clien.RunServerAPI("BLL.Product", "GetMatIsEnable", matCode + "," + product_code + "," + bom_id.ToString()));
                int baseqty = int.Parse(clien.RunServerAPI("BLL.Bom", "GetBomDetailBaseQty", matCode + "," + product_code));
                //为了提示清晰，分三步验证
                //第一次验证，判断该物料存在
                string json = orderon + "," + matCode + "," + Station + "," + lot;
                string ResultJson = clien.RunServerAPI("BLL.SFCConsume", "GetMaterialConsumptionManual", json);
                result = JsonConvert.DeserializeObject<ILE.LEResult>(ResultJson);
                //第二次验证，判断该物料是否已被使用
                if (result.Result)
                {
                    string ResultJson1 = clien.RunServerAPI("BLL.SFCConsume", "GetMaterialConsumptionManual1", json);
                    result = JsonConvert.DeserializeObject<ILE.LEResult>(ResultJson1);
                }
                //第三次验证，判断该物料是否存在于该车间
                if (result.Result)
                {
                    string json1 = orderon + "," + matCode + "," + Station + "," + lot + "," + Workshop;
                    string ResultJson2 = clien.RunServerAPI("BLL.SFCConsume", "GetMaterialConsumptionManual2", json1);
                    result = JsonConvert.DeserializeObject<ILE.LEResult>(ResultJson2);
                }

                if (result.Result)
                {
                    List<LEDAO.P_GetMaterialConsumption_Result> strList = JsonConvert.DeserializeObject<List<LEDAO.P_GetMaterialConsumption_Result>>(result.obj.ToString());
                    jobModel.StepList[jobModel.StepIdx].StepDetail = new List<ILE.StepData>();
                    List<ILE.StepData> stepDetail = new List<ILE.StepData>();

                    string flowcode = jobModel.FlowCode;

                    string processFlow = clien.RunServerAPI("BLL.ProcessFlow", "GetProcessFlow", flowcode);
                    List<B_Process_Flow> processFlowList = JsonConvert.DeserializeObject<List<B_Process_Flow>>(processFlow);
                    string bomid = Convert.ToString(processFlowList[0].bom_id);


                    string bom = clien.RunServerAPI("BLL.Bom", "GetBomById", bomid);
                    List<B_Bom> bomList = JsonConvert.DeserializeObject<List<B_Bom>>(bom);
                    string bom_code = bomList[0].bom_code;

                    string Bom_detail = clien.RunServerAPI("BLL.Bom", "GetBomDetail", matCode + "," + product_code + "," + bom_code);
                    List<V_Bom_Detail> Bom_Detail = JsonConvert.DeserializeObject<List<V_Bom_Detail>>(Bom_detail);
                    decimal _qty = 0;
                    if (Bom_detail != null)
                    {
                        _qty = (decimal)Bom_Detail[0].qty;
                    }
                    foreach (LEDAO.P_GetMaterialConsumption_Result item in strList)
                    {
                        if (enable == 1)
                        {
                            //允许超越不做任何管控，随便
                        }
                        else
                        {
                            //判断该批次物料是否已经被耗完
                            if (item.lot_qty < jobModel.QTY * (_qty / baseqty) * Convert.ToDecimal(consume_percent))
                            {
                                result.ExtMessage = "该批次物料剩余不足";
                                result.Result = false;
                                return result;
                            }
                            if (decimal.Parse((item.lot_qty).ToString()) < 0)
                            {
                                result.ExtMessage = "该批次物料已用完";
                                result.Result = false;
                                return result;
                            }
                        }
                        #region
                        if (item.input_hour != null
                            && (decimal)item.input_hour > 0)
                        {
                            if (item.input_hour < (decimal)item.currinput_hour)
                            {
                                //当前投入时间差大于设置时间（分钟）
                                DialogResult dr = MessageBox.Show("当前投入时间差大于设置时间,确认是否继续？", "提示", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    //用户选择确认的操作
                                    //MessageBox.Show("您选择的是【确认】");
                                }
                                else if (dr == DialogResult.Cancel)
                                {
                                    //用户选择取消的操作
                                    //MessageBox.Show("您选择的是【取消】");
                                    result.ExtMessage = "当前投入时间差大于设置时间";
                                    result.Result = false;
                                    return result;
                                }
                            }
                        }
                        if (item.feed_hour != null
                            && (decimal)item.feed_hour > 0)
                        {
                            if (item.feed_hour < (decimal)item.currfeed_hour)
                            {
                                //当前耗料时间差大于设置时间（分钟）
                                DialogResult dr = MessageBox.Show("当前耗料时间差大于设置时间,确认是否继续？", "提示", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    //用户选择确认的操作
                                    //MessageBox.Show("您选择的是【确认】");
                                }
                                else if (dr == DialogResult.Cancel)
                                {
                                    //用户选择取消的操作
                                    //MessageBox.Show("您选择的是【取消】");
                                    result.ExtMessage = "当前耗料时间差大于设置时间";
                                    result.Result = false;
                                    return result;
                                }
                            }
                        }
                        #endregion

                        ILE.StepData stepd = new ILE.StepData();
                        stepd.wip_id = item.wip_id.ToString();
                        stepd.lotqty = item.lot_qty;
                        stepd.StepVal = item.lot_no;
                        stepd.matCode = item.mat_code;
                        stepd.qty = 1;
                        stepd.InPutDate = DateTime.Now;
                        stepDetail.Add(stepd);
                    }
                    jobModel.StepList[jobModel.StepIdx].StepValue = val;
                    jobModel.StepList[jobModel.StepIdx].StepDetail.AddRange(stepDetail);
                    jobModel.StepList[jobModel.StepIdx].Completed = true;
                    //循环stepList,判断该物料在数据保存之前是否已经扫描
                    string mbm = clien.RunServerAPI("BLL.SFCConsume", "GetMaterialOrProductMbm", matCode);
                    if (mbm == "1")
                    {
                        for (int i = jobModel.IndexBack; i < jobModel.StepIdx; i++)
                        {
                            if (jobModel.StepList[jobModel.StepIdx].StepValue == jobModel.StepList[i].StepValue)
                            {
                                result.ExtMessage = "该物料[" + jobModel.StepList[jobModel.StepIdx].StepValue + "]已扫描，不可重复扫描";
                                result.Result = false;
                                return result;
                            }
                        }
                    }
                    result.obj = null;
                }
                return result;
            }
            catch (Exception ex)
            {

                result.ExtMessage = ex.Message;
                result.Result = false;
                return result;
            }
        }

        /// <summary>
        /// 自动耗料驱动
        /// </summary>
        /// <param name="jobModel"></param>
        /// <returns></returns>
        public ILE.IResult DoWork(ILE.IJob jobModel)
        {
            ILE.LEResult result = new ILE.LEResult();
            try
            {
                ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                string orderon = jobModel.OrderNO;//工单
                string matCode = jobModel.StepList[jobModel.StepIdx].Matcode;//物料
                string Station = jobModel.StationCode;//工位
                string product_code = jobModel.Product;
                string flow_code = jobModel.FlowCode.ToString();
                //根据工艺查找bom_id
                int bom_id = int.Parse(clien.RunServerAPI("BLL.Product", "GetBomByFlow", flow_code));

                int enable = int.Parse(clien.RunServerAPI("BLL.Product", "GetMatIsEnable", matCode + "," + product_code + "," + bom_id.ToString()));
                string json = orderon + "," + matCode + "," + Station;
                string ResultJson = clien.RunServerAPI("BLL.SFCConsume", "GetMaterialConsumptionAuto", json);
                result = JsonConvert.DeserializeObject<ILE.LEResult>(ResultJson);
                int baseqty = int.Parse(clien.RunServerAPI("BLL.Bom", "GetBomDetailBaseQty", matCode + "," + product_code));
                if (result.Result)
                {
                    List<LEDAO.P_GetMaterialConsumption_Result> strList1 = JsonConvert.DeserializeObject<List<LEDAO.P_GetMaterialConsumption_Result>>(result.obj.ToString());
                    //将获取的物料批次按时间顺序升序排列
                    var strList = strList1.OrderBy(x => x.feel_time).ToList();

                    jobModel.StepList[jobModel.StepIdx].StepDetail = new List<ILE.StepData>();
                    List<ILE.StepData> stepDetail = new List<ILE.StepData>();


                    string flowcode = jobModel.FlowCode;

                    string processFlow = clien.RunServerAPI("BLL.ProcessFlow", "GetProcessFlow", flowcode);
                    List<B_Process_Flow> processFlowList = JsonConvert.DeserializeObject<List<B_Process_Flow>>(processFlow);
                    string bomid = Convert.ToString(processFlowList[0].bom_id);


                    string bom = clien.RunServerAPI("BLL.Bom", "GetBomById", bomid);
                    List<B_Bom> bomList = JsonConvert.DeserializeObject<List<B_Bom>>(bom);
                    string bom_code = bomList[0].bom_code;


                    string Bom_detail = clien.RunServerAPI("BLL.Bom", "GetBomDetail", matCode + "," + product_code + "," + bom_code);
                    List<V_Bom_Detail> Bom_Detail = JsonConvert.DeserializeObject<List<V_Bom_Detail>>(Bom_detail);
                    decimal _qty = 0;
                    if (Bom_detail != null)
                    {
                        _qty = (decimal)Bom_Detail[0].qty;
                    }
                    //判断该物料是否允许超越
                    if (enable == 1)
                    {
                        //允许超越随便
                    }
                    else
                    {
                        decimal sumqty = 0;
                        //float consume_percent = jobModel.StepList[jobModel.StepIdx].consume_percent;
                        float consume_percent = 0;
                        for (int index = jobModel.IndexBack; index < jobModel.StepList.Count; index++)
                        {
                            if (jobModel.StepList[index].Matcode == matCode)
                            {
                                consume_percent += jobModel.StepList[index].consume_percent;
                            }
                        }
                        foreach (LEDAO.P_GetMaterialConsumption_Result item in strList)
                        {
                            if (item.lot_qty > 0)
                            {
                                sumqty += decimal.Parse((item.lot_qty).ToString());
                            }
                            else
                            {
                                sumqty += 0;
                            }
                        }
                        if (sumqty < jobModel.QTY * (_qty / baseqty)* (decimal)consume_percent)
                        {
                            result.ExtMessage = "剩余物料不足生产，请投料";
                            result.Result = false;
                            return result;
                        }
                        if (sumqty <= 0)
                        {
                            result.ExtMessage = "物料已用完，请投料";
                            result.Result = false;
                            return result;
                        }
                    }

                    decimal lot_qty = 0;
                    foreach (LEDAO.P_GetMaterialConsumption_Result item in strList)
                    {
                        #region
                        if (item.input_hour != null
                            && (decimal)item.input_hour > 0)
                        {
                            if (item.input_hour < (decimal)item.currinput_hour)
                            {
                                //当前投入时间差大于设置时间（分钟）
                                DialogResult dr = MessageBox.Show("当前投入时间差大于设置时间,确认是否继续？", "提示", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    //用户选择确认的操作
                                    //MessageBox.Show("您选择的是【确认】");
                                }
                                else if (dr == DialogResult.Cancel)
                                {
                                    //用户选择取消的操作
                                    //MessageBox.Show("您选择的是【取消】");
                                    result.ExtMessage = "当前投入时间差大于设置时间";
                                    result.Result = false;
                                    return result;
                                }
                            }
                        }
                        if (item.feed_hour != null
                            && (decimal)item.feed_hour > 0)
                        {
                            if (item.feed_hour < (decimal)item.currfeed_hour)
                            {
                                //当前耗料时间差大于设置时间（分钟）
                                DialogResult dr = MessageBox.Show("当前耗料时间差大于设置时间,确认是否继续？", "提示", MessageBoxButtons.OKCancel);
                                if (dr == DialogResult.OK)
                                {
                                    //用户选择确认的操作
                                    //MessageBox.Show("您选择的是【确认】");
                                }
                                else if (dr == DialogResult.Cancel)
                                {
                                    //用户选择取消的操作
                                    //MessageBox.Show("您选择的是【取消】");
                                    result.ExtMessage = "当前耗料时间差大于设置时间";
                                    result.Result = false;
                                    return result;
                                }
                            }
                        }
                        #endregion

                        ILE.StepData stepd = new ILE.StepData();
                        if (jobModel.StepList[jobModel.StepIdx].consume_type == 1) //平均消耗
                        {
                            stepd.StepVal = item.lot_no;
                            stepd.lotqty = item.lot_qty;
                            stepd.seed_id = item.wip_id.ToString();
                            stepd.matCode = item.mat_code;
                            stepd.consume_type = jobModel.StepList[jobModel.StepIdx].consume_type;
                            stepd.qty = 1;
                            stepd.InPutDate = DateTime.Now;
                            stepDetail.Add(stepd);
                        }
                        if (jobModel.StepList[jobModel.StepIdx].consume_type == 2) //顺序消耗
                        {
                            decimal qty = jobModel.QTY * (_qty / baseqty);
                            if (item.lot_qty > 0)
                            {
                                lot_qty += Math.Abs((decimal)(item.lot_qty));
                            }
                            else
                            {
                                lot_qty += 0;
                            }
                            if (item != strList[strList.Count - 1] && (item.lot_qty) <= 0)
                            {
                                continue;
                            }

                            stepd.StepVal = item.lot_no;
                            stepd.lotqty = item.lot_qty;
                            stepd.seed_id = item.wip_id.ToString();
                            stepd.matCode = item.mat_code;
                            stepd.consume_type = jobModel.StepList[jobModel.StepIdx].consume_type;
                            stepd.qty = 1;
                            stepd.InPutDate = DateTime.Now;
                            stepDetail.Add(stepd);

                            if (lot_qty > qty)
                            {
                                break;
                            }
                        }
                    }
                    jobModel.StepList[jobModel.StepIdx].StepValue = "";//随便赋值，使其不为null
                    jobModel.StepList[jobModel.StepIdx].StepDetail.AddRange(stepDetail);
                    jobModel.StepList[jobModel.StepIdx].Completed = true;
                    //jobModel.start_time=date
                    result.obj = null;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.ExtMessage = ex.Message;
                result.Result = false;
                return result;
            }

        }
    }
}
