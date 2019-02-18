using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LEDAO;
using Newtonsoft.Json;
using ILE;

namespace DP003
{
    class CheckPrintBing
    {
        public static ILE.IResult PrintBing(ILE.IJob job, ServiceReference.ServiceClient client, string val, IResult res)
        {
            ////////////////////////////////////////////////////////
            string product_code = job.Product;
            string ResultProduct = client.RunServerAPI("BLL.SSW", "GetProduct", product_code);
            B_Product productObj = JsonConvert.DeserializeObject<B_Product>(ResultProduct);
            string Print_bind = string.Empty;
            if (productObj != null)
            {
                Print_bind = productObj.print_bind.ToString();
            }
            if (Print_bind == "1")
            {
                //根据工单号查询主工单
                string order = job.OrderNO;
                string ResultMainOrder = client.RunServerAPI("BLL.SSW", "GetMainOrderByOrderNo", order);
                List<P_WorkOrder> workorder = JsonConvert.DeserializeObject<List<P_WorkOrder>>(ResultMainOrder);
                string Main_Order = workorder[0].main_order;
                //通过电池块批次查询主工单
                string SFC = val.Replace("#", "");
                string Result = client.RunServerAPI("BLL.Pack", "GetMainWorder", SFC);
                List<P_BarCodeBing> MainWork = JsonConvert.DeserializeObject<List<P_BarCodeBing>>(Result);
                if (MainWork != null)
                {
                    string Main_Or = MainWork[0].main_order;
                    string order_no = MainWork[0].order;

                    if (Main_Order != Main_Or || order_no != order)
                    {
                        res.ExtMessage = "该批次不属于当前工单下的批次";
                        res.Result = false;
                        return res;
                    }
                }
                else
                {
                    res.ExtMessage = "该批次不存在于条码清单";
                    res.Result = false;
                    return res;
                }

            }
            ////////////////////////////////////////////////////////
            res.Result = true;
            return res;
        }
    }
}
