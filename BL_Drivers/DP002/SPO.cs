using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ILE;
using LEDAO;
using Newtonsoft.Json;

namespace DP002
{
    public class SPO : ISPO
    {
        //工单号
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

                //首先判断输入的是否是条码，如果是工单号带出，不是的话，则说明
                //string dt = client.RunServerAPI("BLL.SSW", "GetWorkerOrderbyBar", val);
                //List<P_BarCodeBing> proc = JsonConvert.DeserializeObject<List<P_BarCodeBing>>(dt);
                //if (proc != null)
                //{
                //    val = proc[0].order.ToString();
                //}
                //else
                //{
                //    val = val;
                //    string dts = client.RunServerAPI("BLL.SSW", "GetOrderNo", job.ProcessCode + "," + val);
                //    List<V_WorkOrder_Product> pro = JsonConvert.DeserializeObject<List<V_WorkOrder_Product>>(dts);
                //    if (pro != null)
                //    {
                //        val = pro[0].order_no;
                //    }
                //}
                string ResProduct = client.RunServerAPI("BLL.Product", "GetProductFlow", val);
                res = JsonConvert.DeserializeObject<ILE.LEResult>(ResProduct);
                if (!res.Result)
                {
                    return res;
                }

                V_ProductFlow produc = JsonConvert.DeserializeObject<V_ProductFlow>(res.obj.ToString());

                string strRes = client.RunServerAPI("BLL.Process", "GetFlowDetailOK", produc.flow_code + "," + job.ProcessCode);
                res = JsonConvert.DeserializeObject<ILE.LEResult>(strRes);

                if (!res.Result)
                {
                    return res;
                }
                ////根据工单查询bom基数，然后赋给job
                //int baseqty = Convert.ToInt32(client.RunServerAPI("BLL.Bom", "GetBomDetailBaseQty", val));

                string strqty = client.RunServerAPI("BLL.SFC", "GetSFCQty_2", val + "," + job.ProcessCode);
                decimal.TryParse(strqty, out qyt);
                job.FatherOrderNO = produc.parent_order;
                job.Pid = Convert.ToInt32(res.obj);
                job.Product = produc.product_code;
                job.FlowCode = produc.flow_code;
                //job.baseqty = baseqty;
                job.MaxQTY = produc.max_qty.Value;
                job.MaxQTYOrder = produc.qty.Value;
                //job.QTYOrder = qyt;
                //if (job.QTYOrder >= job.MaxQTYOrder)
                //{
                //    res.ExtMessage = "工单完成数已达上限";
                //    res.Result = false;
                //    return res;
                //}
                job.OrderNO = val;
                job.StepList[job.StepIdx].StepValue = val;
                job.StepList[job.StepIdx].Completed = true;

                res.Result = true;
                return res;
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
                res.ExtMessage = "DP002 catch ... " + exc.Message;
                res.Result = false;
                return res;
            }
        }

        public IResult DoWork(IJob jobModel)
        {
            throw new NotImplementedException();
        }
    }
}
