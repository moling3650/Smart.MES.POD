using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace BLL
{
    public class WorkDispatching
    {
        //获取指定工位全部有效派工单
        public static string GetOrderByTime(string station_code)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var =from a in context.P_WorkDispatching
                            join b in context.B_Product on a.product_code equals b.product_code
                            where
                              a.state == 1 &&
                              a.station_code == station_code
                            select new {
                              a.description,
                              a.emp_code,
                              a.planned_cplt_time,
                              a.planned_start_time,
                              a.planned_start_date,
                              a.mould_code,
                              a.state,
                              a.station_code,
                              a.cplt_qty,
                              a.qty,
                              a.product_code,
                              a.link_no,
                              a.dispatching_no,
                              a.order_no,
                              a.id,
                              b.product_name
                            };

            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //获取指定单号派工单
        public static string GetOrderByNO(string dispatchNo)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.P_WorkDispatching
                      where (b.dispatching_no ==dispatchNo)
                      select b).FirstOrDefault();
            if (var!=null)
            {
                return JsonConvert.SerializeObject(var);
            }
            return null;
        }

        ////获取指定工位全部有效派工单
        //public static string GetOrderByTime(string station_code)
        //{
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    var var = from b in context.P_WorkDispatching
        //              orderby b.planned_start_time
        //              where (b.state == 1 && b.station_code == station_code)
        //              select b;
        //    if (var.Count() > 0)
        //    {
        //        return JsonConvert.SerializeObject(var.ToList());
        //    }
        //    return null;
        //}

        public static string OrderReport(string param)
        {
            string dspNO; string orderNO; decimal cpltQTY;
            string[] paramlist=param.Split(',');
            dspNO = paramlist[0];
            orderNO = paramlist[1];
            cpltQTY = decimal.Parse(paramlist[2]);
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    context.ExecuteStoreCommand("update P_WorkDispatching set cplt_qty=cplt_qty+@cplt_qty where dispatching_no=@dspNO", new System.Data.SqlClient.SqlParameter[] { new SqlParameter("@cplt_qty", cpltQTY), new SqlParameter("@dspNO",dspNO) });
                    context.ExecuteStoreCommand("update P_WorkOrder set cplt_qty=cplt_qty+@cplt_qty where order_no=@orderNO", new System.Data.SqlClient.SqlParameter[] { new SqlParameter("@cplt_qty", cpltQTY), new SqlParameter("@orderNO", orderNO) });

                    //context.SaveChanges();
                }
            }
            catch(Exception exc)
            {
                return exc.Message;
            }

            return null;
        }
    }

    
}