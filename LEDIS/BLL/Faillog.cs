using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;

namespace BLL
{
    public class Faillog
    {
        public static string AddFailLog(string parstrPFailLog)
        {
            LEDAO.P_FailLog PFailLog = JsonConvert.DeserializeObject<LEDAO.P_FailLog>(parstrPFailLog);
            if (PFailLog == null)
            {
                return "0";
            }
            if (string.IsNullOrWhiteSpace(PFailLog.order_no)
                || string.IsNullOrWhiteSpace(PFailLog.sfc))
            {
                return "0";
            }
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                PFailLog.input_time = context.NewDate().First();
                context.P_FailLog.AddObject(PFailLog);
                context.SaveChanges();
            }

            return "1";
        }
        //跟新记录，根据fid，更新状态，完成时间
        public static string UpdateFailLog(string parstrPFailLog)
        {
            LEDAO.P_FailLog PFailLog = JsonConvert.DeserializeObject<LEDAO.P_FailLog>(parstrPFailLog);
            if (PFailLog == null)
            {
                return "0";
            }
            if (PFailLog.fid < 1)
            {
                return "0";
            }
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    var var = (from u in context.P_FailLog where u.fid == PFailLog.fid select u).FirstOrDefault();
                    if (var != null)
                    {
                        var.state = PFailLog.state;
                        var.repair_remark = PFailLog.repair_remark;
                        var.process_code = PFailLog.process_code;
                        var.Disposal_Process = PFailLog.Disposal_Process;
                        var.finish_time = context.NewDate().First();
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception exp)
            {
                return "0";
            }
            return "1";
        }
        public static string GetFailTimesBySFC(string parstrSFC)
        {
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    var var = from u in context.P_FailLog where u.sfc == parstrSFC select u;
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                }
            }
            catch(Exception exp)
            {

            }
            return null;
        }
        // 查询P_FailLog 数据
        public static string GetFileLogBySFC(string strsfc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strsfc))
                {
                    return null;
                } 
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    var var = context.P_FailLog.Where(X => X.sfc == strsfc && X.state == 0).OrderBy(Y => Y.input_time);
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                }
            }
            catch (Exception exp)
            {
            }
            return null;
        }
        // 查询P_FailLog 数据
        public static string GetFileLog(string json)
        {
            string[] arrstr = json.Split(',');
            string strsfc = "";//str[0];
            string strprocsscode = "";// str[1];
            if (arrstr.Length > 1)
            {
                strprocsscode = arrstr[0];
                strsfc = arrstr[1];
            }
            else
            {
                strprocsscode = arrstr[0];
            }
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                if (string.IsNullOrWhiteSpace(strsfc))
                {
                    var var = context.P_FailLog.Where(X => X.process_code == strprocsscode && X.state == 0).OrderBy(Y => Y.input_time);
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                }
                else
                {
                    var var = context.P_FailLog.Where(X => X.process_code == strprocsscode && X.sfc == strsfc && X.state == 0).OrderBy(Y => Y.input_time);
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                }

            }
            return null;
        }
        // 查询P_FailLog 数据，查询状态为0的数据
        public static string GetFileLogAndName(string json)
        {
            string[] arrstr = json.Split(',');
            string strsfc = "";//str[0];
            if (arrstr.Length > 0)
            {
                strsfc = arrstr[0];
            }
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                if (string.IsNullOrWhiteSpace(strsfc))
                {
                    var var = context.V_P_FailLog_Name.Where(X => X.state == 0).OrderBy(Y =>Y.input_time);
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                }
                else
                {
                    var var = context.V_P_FailLog_Name.Where(X => X.sfc == strsfc && X.state == 0).OrderBy(Y => Y.input_time);
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                }

            }
            return null;
        }
        public static string GetPDate(string strgroup)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    List<sp_Count_PDate_Result> P_Date1 = context.sp_Count_PDate(strgroup).ToList();
                    return Convert.ToDateTime(P_Date1[0].P_date).ToString();
                }
            }
            catch (Exception exp)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取班别
        /// </summary>
        /// <param name="strgroup"></param>
        /// <returns></returns>
        public static string GetClassCode(string strgroup)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    List<B_WorkGroup_Class> ClassCode = context.sp_Count_Class(strgroup).ToList();
                    return ClassCode[0].class_code.ToString();
                }
            }
            catch (Exception exp)
            {
                return null;
            }
        }
    }
}