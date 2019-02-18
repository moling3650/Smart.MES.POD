using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using System.Data;
using Newtonsoft.Json;

namespace BLL
{
    public class Process
    {
        /// <summary>
        /// 获取指定IP对应的工站集合
        /// </summary>
        /// <returns></returns>
        public static List<B_StationList> GetStationList(string ip)
        {
            //string sql = "select station_code,station_name,ip_address,process_code from b_stationList where ip_address='" + ip + "'";
            //DataTable dt = DB.Database.getDataTable(sql);
            //IList<B_StationList> list = LETools.ModelConvertHelper<B_StationList>.ConvertToModel(dt);

            //return list.ToList<B_StationList>();

            //var context = LEDAO.APIGateWay.GetEntityContext();
            //var results = context.S_Employee.Where(c => c.emp_code == "10000");

            //if (results.Count() > 0)
            //{
            //    return JsonConvert.SerializeObject(results.ToList<S_Employee>()[0]);
            //}
            //return null;
            return null;
        }

        /// <summary>
        /// 获取指定IP对应的工序集合
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static string GetProcessList(string ip)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_ProcessList
                           join b in context.B_StationList on a.process_code equals b.process_code
                           where
                             b.ip_address == ip
                           select new
                           {
                               a.process_code,
                               a.process_name,
                               a.type_id,
                               a.group_code
                           }).Distinct().ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception exp)
            {
            }
            return null;
        }

        /// <summary>
        /// 获取工序下的工站
        /// </summary>
        /// <param name="processCode"></param>
        /// <returns></returns>
        public static string GetProcess(string processCode)
        {
            //var context = LEDAO.APIGateWay.GetEntityContext();
            //var res = context.B_ProcessList.Where(x => x.process_code == processCode);
            //if (res.Count() > 0)
            //{
            //    return JsonConvert.SerializeObject(res.ToList()[0]);
            //}
            //return null;
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = context.V_ProcessList_Workshop.Where(x => x.process_code == processCode);
            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res.ToList()[0]);
            }
            return null;
        }


        /// <summary>
        /// 获取OK流的PID值
        /// </summary>
        /// <param name="split">工单，工艺,当前工序</param>
        /// <returns></returns>
        public static string GetFlowDetailOK(string split)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    string[] Array = split.Split(',');
                    string flowcode = Array[0];
                    string processfrom = Array[1];
                    var res = context.B_Process_Flow_Detail.Where(x => x.flow_code == flowcode && x.process_from == processfrom && x.process_result == "OK").Select(c => c.pid);
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res.First(), true, null);
                    }
                    return ConResult.GetJsonResult(null, false, "工单指定工艺与当前工序不匹配");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetESOP(string proccess)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    var res = context.B_Process_Flow_Detail.Where(x => x.process_from == proccess && x.process_result == "OK").Select(c => c.imageUrl).ToList();
                    foreach (var item in res)
                    {
                        if (item != null)
                        {
                            return JsonConvert.SerializeObject(item);
                        }
                    }
                    return null;

                }


            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetPIDESOP(string pid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            int intpid = Convert.ToInt32(pid);
            try
            {
                using (context)
                {
                    var res = context.B_Process_Flow_Detail.Where(x => x.pid == intpid).Select(c => c.imageUrl).ToList();
                    if (res.Count > 0)
                    {
                        JsonConvert.SerializeObject(res[0]);
                    }
                    return null;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 获取该工序是否严格管控
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string GetStrict(string pid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            int intpid = Convert.ToInt32(pid);
            try
            {
                using (context)
                {
                    var res = context.B_Process_Flow_Detail.Where(x => x.pid == intpid).Select(c => c.strict).ToList();
                    if (res.Count > 0)
                    {
                        return JsonConvert.SerializeObject(res[0]);
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //根据工单，获取工艺流，根据工艺流得到工序
        public static string GetProcessFlowDetail(string parstrorderno)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var rec = (from a in context.P_WorkOrder
                           join b in context.B_Process_Flow_Detail on a.flow_code equals b.flow_code
                           join c in context.B_ProcessList on b.process_from equals c.process_code
                           where (a.order_no == parstrorderno)
                           orderby c.idx
                           select new
                           {
                               process_code = c.process_code,
                               process_name = c.process_name,
                               group_code = c.group_code,
                               route_type = c.route_type
                           }).ToList();
                if (rec.Count > 0)
                {
                    return JsonConvert.SerializeObject(rec);
                }
            }
            return null;
        }
        //根据工单，批次，获取工艺流，根据工艺流得到当前批次已过站的工序
        public static string GetProcessFlowDetail_1(string data)
        {
            string[] strdata = data.Split(',');
            string parstrorderno = strdata[0].ToString();
            string sfc = strdata[1].ToString();
            var db = LEDAO.APIGateWay.GetEntityContext();
            using (db)
            {
                var rec = (from a in db.P_WorkOrder
                           join b in db.B_Process_Flow_Detail on a.flow_code equals b.flow_code into b_join
                           from b in b_join.DefaultIfEmpty()
                           join c in db.B_ProcessList on new { Process_from = b.process_from } equals new { Process_from = c.process_code } into c_join
                           from c in c_join.DefaultIfEmpty()
                           join d in
                               (
                                   (from p_sfc_process_iolog in db.P_SFC_Process_IOLog
                                    where
                                      p_sfc_process_iolog.SFC == sfc &&
                                       p_sfc_process_iolog.pass == 1
                                    select new
                                    {
                                        p_sfc_process_iolog
                                    }))
                                 on new { a.order_no, c.process_code }
                             equals new { d.p_sfc_process_iolog.order_no, d.p_sfc_process_iolog.process_code } into d_join
                           from d in d_join.DefaultIfEmpty()
                           where
                             a.order_no == parstrorderno &&
                             d.p_sfc_process_iolog.output_time != null
                           select new
                           {
                               Process_code = c.process_code,
                               Process_name = c.process_name,
                               Group_code = c.group_code,
                               Route_type = c.route_type
                           }).ToList();
                if (rec.Count > 0)
                {
                    return JsonConvert.SerializeObject(rec);
                }
            }
            return null;
        }
        public static string GetProcessType(string parstrtypecode)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            if (!string.IsNullOrWhiteSpace(parstrtypecode))
            {
                var res = context.B_ProcessType.Where(x => x.type_code == parstrtypecode);
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            else
            {
                var res = context.B_ProcessType;
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            return null;
        }
        //获取服务器时间
        public static string GetServerTime(string parstrtypecode)
        {
            return DateTime.Now.ToString();
        }
    }
}