using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;
using System.Data;

namespace BLL
{
    public class WorkOrder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public static string GetOrderByID(string orderNO)
        {
            var context = APIGateWay.GetEntityContext();
            var res = context.P_WorkOrder.Where(x => x.order_no == orderNO).ToList();

            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res);
            }
            return null;
        }
        /// <summary>
        /// 验证工单
        /// </summary>
        /// <param name="array">字符串数组，工艺 </param>
        /// <returns></returns>
        public static string GetProcessWork(string process)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = context.sp_Check_WIP_Work(process, "").ToList();

                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res);
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }

        }
        //通过时间获取工单
        public static string GetOrderByTime(string planned_time)
        {
            string[] pp = planned_time.Split(',');
            string planned = pp[0];
            string process = pp[1];
            string planned1 = pp[2];
            //string flow_code = pp[2];
            DateTime planned_time1 = Convert.ToDateTime(planned);
            DateTime planned_time2 = Convert.ToDateTime(planned1);
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.V_WorkOrder_Product orderby b.main_order where ((b.planned_time >= planned_time1 && b.planned_time <= planned_time2) && b.state != 2 && b.process_code == process && b.flow_state == 1) select new { parent_order = b.parent_order, order_no = b.order_no, main_order = b.main_order, product_code = b.product_code, qty = b.qty, planned_time = b.planned_time, flow_code = b.flow_code, product_name = b.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        /// <summary>
        /// 获取指定工单，已过指定工站数量
        /// </summary>
        /// <param name="array">工单，工序代码，工位代码 </param>
        /// <returns></returns>
        public static string GetSFCCountByOrder(string strorderinfo)
        {
            string[] pp = strorderinfo.Split(',');
            if (pp.Length < 3)
            {
                return null;
            }
            string strorderno = pp[0];
            string processcode = pp[1];
            string stationcode = pp[2];
            var context = LEDAO.APIGateWay.GetEntityContext();
            if (stationcode == "00")  //不需要按照工位查询
            {
                var var = from b in context.P_SFC_Process_IOLog
                          where (b.order_no == strorderno && b.process_code == processcode && b.output_time != null)
                          group b by new { b.order_no, b.SFC, b.qty } into g
                          select new
                          {
                              order_no = g.Key.order_no,
                              SFC = g.Key.SFC,
                              qty = g.Key.qty,
                              pass = g.Max(b => b.pass)
                          };
                if (var.Count() > 0)
                {

                    return JsonConvert.SerializeObject(var.ToList());
                }
            }
            else
            {
                //需要按照工位查询
                var var = from b in context.P_SFC_Process_IOLog
                          where (b.order_no == strorderno && b.process_code == processcode && b.station_code == stationcode && b.output_time != null)
                          group b by new { b.order_no, b.SFC, b.qty } into g
                          select new
                          {
                              order_no = g.Key.order_no,
                              SFC = g.Key.SFC,
                              qty = g.Key.qty,
                              pass = g.Max(b => b.pass)
                          };
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }

            return null;
        }


    }

}