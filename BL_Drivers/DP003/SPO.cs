using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ILE;
using LEDAO;
using Newtonsoft.Json;
using System.Net;
using LEMES_POD;

namespace DP003
{
    public class SPO : ISPO
    {
        //主批次号
        public SPO()
        {

        }

        public IResult DoWork(IJob job, string val)
        {
            IResult res = new LEResult();
            decimal qyt = 0;
            try
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string product_code = job.Product;
                string rule_code = client.RunServerAPI("BLL.Product", "GetRuleByProductCode", product_code);
                //var pattern = @"^([0-9]{4}-).+$";
                if (!string.IsNullOrEmpty(rule_code))
                {
                    var pattern = @rule_code;
                    bool result = System.Text.RegularExpressions.Regex.IsMatch(val, pattern);
                    if (!result)
                    {
                        res.Result = false;
                        res.ExtMessage = "条码不符合规则，请重新扫描";
                        return res;
                    }
                }
                string jsdata = client.RunServerAPI("BLL.SFC", "GetSfcState", val);
                #region --返回空
                if (string.IsNullOrEmpty(jsdata))
                {
                    if (job.RouteType == "首工站")
                    {

                        //根据sfc在P_SFC_State表中查询是否存在未维修记录
                        string strclFailLog = client.RunServerAPI("BLL.Faillog", "GetFileLogBySFC", val);
                        if (!string.IsNullOrWhiteSpace(strclFailLog))
                        {
                            List<P_FailLog> dt = new List<P_FailLog>();
                            dt = JsonConvert.DeserializeObject<List<P_FailLog>>(strclFailLog);
                            if (dt.Count > 0)
                            {
                                res.ExtMessage = "当前批次[" + val.ToString() + "]已进入维修工序,请检查";
                                res.Result = false;
                                return res;
                            }
                        }
                        string ResProduct1 = client.RunServerAPI("BLL.Product", "GetProductFlow", job.OrderNO);
                        res = JsonConvert.DeserializeObject<ILE.LEResult>(ResProduct1);
                        V_ProductFlow produc1 = JsonConvert.DeserializeObject<V_ProductFlow>(res.obj.ToString());

                        string strRes1 = client.RunServerAPI("BLL.Process", "GetFlowDetailOK", produc1.flow_code + "," + job.ProcessCode);
                        res = JsonConvert.DeserializeObject<ILE.LEResult>(strRes1);

                        if (!res.Result)
                        {
                            return res;
                        }
                        string strqty = client.RunServerAPI("BLL.SFC", "GetSFCQty_2", job.OrderNO + "," + job.ProcessCode);
                        decimal.TryParse(strqty, out qyt);
                        job.FatherOrderNO = produc1.parent_order;
                        job.Pid = Convert.ToInt32(res.obj);
                        job.Product = produc1.product_code;
                        job.FlowCode = produc1.flow_code;
                        job.MaxQTY = produc1.max_qty.Value;
                        job.MaxQTYOrder = produc1.qty.Value;
                        job.QTYOrder = qyt;
                        if (job.QTYOrder >= job.MaxQTYOrder)
                        {
                            res.ExtMessage = "工单完成数已达上限";
                            res.Result = false;
                            return res;
                        }
                        //验证是否打印绑定
                        ILE.IResult res1 = DP003.CheckPrintBing.PrintBing(job, client, val, res);
                        if (!res1.Result)
                        {
                            return res1;
                        }
                        job.start_time = DateTime.Parse(client.RunServerAPI("BLL.Process", "GetServerTime", ""));
                        job.SFC = val;
                        job.StepList[job.StepIdx].StepValue = val;
                        job.StepList[job.StepIdx].Completed = true;
                        res.Result = true;
                        return res;
                    }
                    res.Result = false;
                    res.ExtMessage = "没有成品批次号";
                    return res;
                }
                #endregion
                P_SFC_State sfc = JsonConvert.DeserializeObject<P_SFC_State>(jsdata);
                if (job.Pid == 0)
                {
                    string ResProduct1 = client.RunServerAPI("BLL.Product", "GetProductFlow", sfc.order_no);
                    res = JsonConvert.DeserializeObject<ILE.LEResult>(ResProduct1);
                    V_ProductFlow produc1 = JsonConvert.DeserializeObject<V_ProductFlow>(res.obj.ToString());
                    string strRes1 = client.RunServerAPI("BLL.Process", "GetFlowDetailOK", produc1.flow_code + "," + job.ProcessCode);
                    res = JsonConvert.DeserializeObject<ILE.LEResult>(strRes1);

                    if (!res.Result)
                    {
                        return res;
                    }
                    string strqty = client.RunServerAPI("BLL.SFC", "GetSFCQty_2", sfc.order_no + "," + job.ProcessCode);
                    decimal.TryParse(strqty, out qyt);
                    job.MaxQTY = produc1.max_qty.Value;
                    job.MaxQTYOrder = produc1.qty.Value;
                    job.QTYOrder = qyt;
                    if (job.RouteType == "首工站" && job.QTYOrder >= job.MaxQTYOrder)
                    {
                        res.ExtMessage = "工单完成数已达上限";
                        res.Result = false;
                        return res;
                    }
                    job.FatherOrderNO = produc1.parent_order;
                    job.Pid = Convert.ToInt32(res.obj);
                    job.Product = produc1.product_code;
                    job.FlowCode = produc1.flow_code;
                }
                string Pid = job.Pid.ToString();
                string Strict = client.RunServerAPI("BLL.Process", "GetStrict", Pid);
                if (Strict == "1")
                {
                    switch (sfc.state)
                    {
                        case -1:
                            res.Result = false;
                            res.ExtMessage = "成品批次已报废";
                            return res;
                        case 0:
                            res.Result = false;
                            res.ExtMessage = "成品批次已停用";
                            return res;
                        case 1:
                            break;
                        case 2:
                            res.Result = false;
                            res.ExtMessage = "成品批次已完成";
                            return res;

                    }
                    if (job.ProcessCode != sfc.now_process)
                    {
                        string processs_code = sfc.now_process;
                        string processJson = client.RunServerAPI("BLL.Process", "GetProcess", processs_code);
                        V_ProcessList_Workshop list = JsonConvert.DeserializeObject<V_ProcessList_Workshop>(processJson);
                        string Process_name = list.process_name;
                        res.Result = false;
                        res.ExtMessage = "成品批次工序是[" + Process_name + "]";
                        return res;
                    }
                }

                job.OrderNO = sfc.order_no;
                /////////////////////////////////////
                //判断是否严格控制该工序，严格控制则执行以下代码，不严格控制则随意工序，不做控制
                /////////////////////////////////////
                //验证是否打印绑定
                ILE.IResult resResult = DP003.CheckPrintBing.PrintBing(job, client, val, res);
                if (!resResult.Result)
                {
                    return resResult;
                }
                /////
                string ResProduct = client.RunServerAPI("BLL.Product", "GetProductFlow", job.OrderNO);
                res = JsonConvert.DeserializeObject<ILE.LEResult>(ResProduct);

                if (!res.Result && Strict == "1")
                {
                    return res;
                }

                V_ProductFlow produc = JsonConvert.DeserializeObject<V_ProductFlow>(res.obj.ToString());
                string strRes = client.RunServerAPI("BLL.Process", "GetFlowDetailOK", produc.flow_code + "," + job.ProcessCode);
                res = JsonConvert.DeserializeObject<ILE.LEResult>(strRes);
                if (res.obj == null)
                {
                    return res;
                }
                if (!res.Result && Strict == "1")
                {
                    return res;
                }

                string strqty1 = client.RunServerAPI("BLL.SFC", "GetSFCQty_2", job.OrderNO + "," + job.ProcessCode);
                decimal.TryParse(strqty1, out qyt);
                job.FatherOrderNO = produc.parent_order;
                job.Pid = Convert.ToInt32(res.obj);

                job.Product = produc.product_code;
                job.FlowCode = produc.flow_code;
                job.MaxQTY = produc.max_qty.Value;
                job.MaxQTYOrder = produc.qty.Value;
                job.QTYOrder = qyt;
                job.SFC = val;
                //
                job.start_time = DateTime.Parse(client.RunServerAPI("BLL.Process", "GetServerTime", ""));
                //job.start_time = DateTime.Now;
                job.QTY = sfc.qty.Value;
                job.StepList[job.StepIdx].StepValue = val;
                job.StepList[job.StepIdx].Completed = true;
                res.Result = true;
                return res;
            }

            catch (Exception exc)
            {
                res.Result = false;
                return res;
            }
        }

        public IResult DoWork(ILE.IJob jobModel)
        {
            return null;
        }


    }
}
