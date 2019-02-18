using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;
using System.Data;

namespace BLL
{
    public class Pack
    {

        /// <summary>
        /// 加载SFC
        /// </summary>
        /// <param name="split">SFC，</param>
        /// <returns></returns>
        public static string GetSfcTray(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();

            using (context)
            {


                var mn = (from v_smp in context.P_Tray_Detail
                          where
                            v_smp.tray_sfc == SFC
                          select new { v_smp.sfc }).ToList();


                List<P_SFC_State> res = new List<P_SFC_State>();
                foreach (var item in mn)
                {
                    var str = context.P_SFC_State.Where(x => x.SFC == item.sfc && x.is_tray == 1).OrderBy(x => x.id).ToList();
                    res.AddRange(str);
                }
                return JsonConvert.SerializeObject(res);
            }
        }








        public static string GetSfcState(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();

            using (context)
            {
                var res = context.P_SFC_State.Where(x => x.SFC == SFC).OrderBy(x => x.id);
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
                else
                {
                    return null;
                }
            }



        }
        public static string GetSfcID(string ID)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            int id = Convert.ToInt32(ID);
            using (context)
            {
                var res = context.P_SFC_State.Where(x => x.id == id).OrderBy(x => x.id);
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
                else
                {
                    return null;
                }
            }



        }

        public static void AddSFCState(string json)
        {
            RMessage rm = new RMessage();
            LEDAO.P_SFC_State oqc = JsonConvert.DeserializeObject<LEDAO.P_SFC_State>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    context.P_SFC_State.AddObject(oqc);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                rm.MessageStr = ex.Message;

            }

        }
        public static string AddSFC(string json)
        {
            RMessage rm = new RMessage();
            LEDAO.P_Tray_Detail oqc = JsonConvert.DeserializeObject<LEDAO.P_Tray_Detail>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    context.P_Tray_Detail.AddObject(oqc);
                    context.SaveChanges();
                    return null;

                }
            }
            catch (Exception ex)
            {

                rm.MessageStr = ex.Message;
                return JsonConvert.SerializeObject(oqc);
            }

        }
        public static void DelSFC(string json)
        {
            RMessage rm = new RMessage();
            string[] str = json.Split(',');
            string sfc = str[0];
            string tray_sfc = str[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    var var = (context.P_Tray_Detail.Where(X => X.sfc == sfc && X.tray_sfc == tray_sfc)).Single();
                    context.P_Tray_Detail.DeleteObject(var);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                rm.MessageStr = ex.Message;
            }

        }

        public static string GetTrayDetail(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();

            using (context)
            {

                var res = (from v_smp in context.P_Tray_Detail
                           where
                             v_smp.tray_sfc == SFC
                           select new { v_smp.sfc }).ToList();


                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }

            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SFC"></param>
        /// <returns></returns>
        public static string GetLine(string data)
        {
            string[] var = data.Split(',');
            string ip = var[0];
            string station = var[1];
            string process = var[2];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_Station_Line
                       where
                         a.ip_address == ip &&
                         a.station_code == station &&
                         a.process_code == process
                       select new
                       {
                           a.line_code,
                       }).ToList();
            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res.ToList());
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SFC"></param>
        /// <returns></returns>
        public static string GetMainWorder(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var res = (from v in context.P_BarCodeBing
                           where
                             v.barcode == SFC
                           select new { main_order = v.main_order, order = v.order }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }

            return null;
        }
    }
}