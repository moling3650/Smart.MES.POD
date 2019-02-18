using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;
using System.Data;

namespace BLL
{
    public class SFC
    {

        //获取SFC的状态
        public static string GetSfcState(string sfc)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = context.P_SFC_State.Where(x => x.SFC == sfc).ToList();

            if (res.Count() > 0)
            {

                return JsonConvert.SerializeObject(res[0]);
            }
            return null;
        }
        //获取SFC的产品的等级
        public static string GetSfcGrade(string sfc)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.V_SFC_Grade where b.SFC == sfc select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //判断SFC是否符合规则,condition=工单号,SFC内容
        //public static string CheckSfcRule(string condition)
        //{
        //    string[] cds = condition.Split(',');
        //    string order = cds[0];
        //    string sfc = cds[1];

        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    using (context)
        //    { 
        //        var list=
        //    }

        //}

        public static string GetSFCQty(string order)
        {

            var context = LEDAO.APIGateWay.GetEntityContext();
            string strend = "END";
            var res = context.P_SFC_State.Where(c => c.order_no == order & c.state > -1 & c.now_process == strend).Sum(c => c.qty);  //??? 计算工单下已完成批次数量
            return res.ToString();
        }
        public static string GetSFCQty_1(string data)
        {

            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] Arrdata = data.Split(',');
            string order = Arrdata[0].ToString();
            string procrss_code = Arrdata[1].ToString();

            var res = context.P_SFC_State.Where(c => c.order_no == order & c.state > -1 & c.from_process == procrss_code).Sum(c => c.qty);  //??? 计算工单下已完成批次数量
            return res.ToString();
        }
        public static string GetSFCQty_2(string data)
        {

            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] Arrdata = data.Split(',');
            string order = Arrdata[0].ToString();
            string procrss_code = Arrdata[1].ToString();

            var res = context.P_SFC_Process_IOLog.Where(c => c.order_no == order & c.pass == 1 & c.process_code == procrss_code).Sum(c => c.qty);
            return res.ToString();
        }

        //根据父工单获取子工单中批次列表
        public static string GetLstSFCByParentorder(string parstrparentorder)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = context.P_BarCodeBing.Where(c => c.parent_order == parstrparentorder).OrderBy(x => x.barcode).ToList();
            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res);
            }
            return null;
        }
        //降级
        public static void UpdateSFCGrade(string data)
        {
            string[] str = data.Split(',');
            string sfc = str[0];
            string gradecode = str[1].ToString();
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_SFC_State.Where(X => X.SFC == sfc).FirstOrDefault();
                model.grade_id = gradecode;
                context.SaveChanges();
            }
        }
        //修改SFC信息，状态，时间
        public static string UpDataSFCData(string json)
        {
            LEDAO.P_SFC_State Psfcstate = JsonConvert.DeserializeObject<LEDAO.P_SFC_State>(json);
            if (string.IsNullOrWhiteSpace(Psfcstate.order_no))
            {
                return "Fail,缺少工单参数";
            }
            if (string.IsNullOrWhiteSpace(Psfcstate.SFC))
            {
                return "Fail,缺少批次参数";
            }
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    var model = context.P_SFC_State.Where(X => X.order_no == Psfcstate.order_no && X.SFC == Psfcstate.SFC).FirstOrDefault();
                    if (Psfcstate.state != null)
                    {
                        model.state = Psfcstate.state;
                    }
                    //model.end_time = context.NewDate().First();
                    context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                return "Fail," + exp.Message;
            }
            return "OK";
        }
        //修改SFC IOLog当前工序
        public static string UpDataSFCIOLogData(string json)
        {
            string[] str = json.Split(',');
            if (str.Length < 4)
            {
                return "Fail,缺少传入参数";
            }
            string strorderno = str[0];
            string strsfc = str[1];
            string strnowprocess = str[2];
            string strstationcode = str[3];
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    var model = context.P_SFC_Process_IOLog.Where(X => X.order_no == strorderno && X.SFC == strsfc && X.input_time == null && X.output_time == null).FirstOrDefault();
                    model.process_code = strnowprocess;
                    model.station_code = strstationcode;
                    context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                return "Fail," + exp.Message;
            }
            return "OK";
        }
        //修改 SFC当前工序以及过站时间 + SFC IOLog当前工序==开始维修
        public static string UpDataSFCInfoAndSFCIOLogData(string json)
        {
            //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
            string[] strinputpar = json.Split(',');
            if (strinputpar.Length < 10)
            {
                return "Fail,参数不全";
            }
            string order = strinputpar[0];
            string sfc = strinputpar[1];
            //LEDAO.P_SFC_State Psfcstate = JsonConvert.DeserializeObject<LEDAO.P_SFC_State>(json);
            //if (string.IsNullOrWhiteSpace(Psfcstate.order_no))
            //{
            //    return "Fail,缺少工单参数";
            //}
            //if (string.IsNullOrWhiteSpace(Psfcstate.SFC))
            //{
            //    return "Fail,缺少批次参数";
            //}
            //if (string.IsNullOrWhiteSpace(Psfcstate.now_process))
            //{
            //    return "Fail,缺少工序参数";
            //}
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    var model = context.P_SFC_State.Where(X => X.order_no == order && X.SFC == sfc).FirstOrDefault();
                    if (model != null)
                    {
                        model.from_process = strinputpar[9];
                        model.now_process = strinputpar[2];
                        model.process_time = context.NewDate().First();
                        model.fail_times = model.fail_times + int.Parse(strinputpar[4]);
                        if (strinputpar[3] != "999")
                        {
                            model.state = int.Parse(strinputpar[3]);
                        }
                        if (strinputpar[6] != "grade_id")
                        {
                            try
                            {
                                model.grade_id = strinputpar[6];
                                model.grade_type = strinputpar[7];
                            }
                            catch (Exception exp)
                            {
                            }
                        }
                    }

                    //var modelIOLog = context.P_SFC_Process_IOLog.Where(X => X.order_no == strorderno && X.SFC == strsfc && X.output_time == null).FirstOrDefault();
                    var modelIOLog = context.P_SFC_Process_IOLog.Where(X => X.order_no == order && X.SFC == sfc).OrderByDescending(y => y.id).FirstOrDefault();
                    if (modelIOLog != null)
                    {
                        modelIOLog.process_code = strinputpar[2];
                        modelIOLog.input_time = context.NewDate().First();
                        modelIOLog.fail_times = modelIOLog.fail_times + int.Parse(strinputpar[8]);
                        modelIOLog.pass = modelIOLog.pass + int.Parse(strinputpar[5]);
                        modelIOLog.station_code = null;
                        modelIOLog.output_time = null;
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                return "Fail," + exp.Message;
            }
            return "OK";
        }
        public static string GetSFCIOLogInfo(string json)
        {
            string[] str = json.Split(',');
            if (str.Length < 2)
            {
                return null;
            }
            string strsfc = str[0];
            string strnowprocess = str[1];
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = context.P_SFC_Process_IOLog.Where(c => c.SFC == strsfc && c.process_code == strnowprocess && c.pass == 1).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res);
                }
            }
            catch (Exception exp)
            {
                //
            }
            return null;
        }
        //保存待判数据
        public static void InsertSFCJude(string json)
        {
            LEDAO.P_SFC_Jude tem = JsonConvert.DeserializeObject<LEDAO.P_SFC_Jude>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_SFC_Jude.AddObject(tem);
                context.SaveChanges();
            }
        }
        
        /// <summary>
        /// 根据批次及待判码获取数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetJudeSFC(string data)
        {
            string[] Arrdata = data.Split(',');
            string judecode = Arrdata[0].ToString();
            string sfc = Arrdata[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_SFC_Jude.Where(X => X.sfc == sfc && X.state == 0 && X.jude_code == judecode) select a).ToList();
            if (res.Count > 0)
            {
                return JsonConvert.SerializeObject(res);
            }
            return null;
        }
        //取消待判，直接删除该批次
        public static void DeleteJude(string sfc)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.P_SFC_Jude where u.sfc == sfc select u);
                foreach (var item in var)
                {
                    context.P_SFC_Jude.DeleteObject(item);
                }
                context.SaveChanges();
            }
        }
    }
}