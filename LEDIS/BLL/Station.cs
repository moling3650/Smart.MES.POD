using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using Newtonsoft.Json;

namespace BLL
{
    public class Station
    {
        public static string GetStationList(string condition)
        {
            string[] cds = condition.Split(',');
            string ip = cds[0];
            string process = cds[1];

            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var res = context.B_StationList.Where(x => x.ip_address == ip & x.process_code == process & x.is_formal == 1);
                if (res.Count() > 0)
                {

                    return JsonConvert.SerializeObject(res.ToList());
                }
                return null;
            }
        }
        public static string GetStationListByProcess(string strprocess)
        {
            if (string.IsNullOrWhiteSpace(strprocess))
            {
                return null;
            }
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var res = context.B_StationList.Where(x => x.process_code == strprocess);

                if (res.Count() > 0)
                {

                    return JsonConvert.SerializeObject(res.ToList());
                }
                return null;
            }
        }
        public static string GetStationEquipmentByStation(string strstation)
        {
            if (string.IsNullOrWhiteSpace(strstation))
            {
                return null;
            }
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var res = context.B_StationMachine.Where(x => x.station_code == strstation);

                if (res.Count() > 0)
                {

                    return JsonConvert.SerializeObject(res.ToList());
                }
                return null;
            }
        }
        /// <summary>
        /// 根据工位编号和批次获取已经完成的数据
        /// </summary>
        /// <param name="strstation"></param>
        /// <returns></returns>
        public static string GetStationOrder(string strstation)
        {
            if (string.IsNullOrWhiteSpace(strstation))
            {
                return null;
            }
            //string[] arrStr = strstation.Split(',');
            //string station_code = arrStr[0].ToString();
            //string sfc = arrStr[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var res = context.P_Order_Station.Where(x => x.station_code == strstation);
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
                return null;
            }
        }
        /// <summary>
        ///删除上次执行的数据 
        /// </summary>
        /// <param name="machine_code"></param>
        public static void DeleteStationOrder(string strstation)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                //string[] arrStr = data.Split(',');
                //string station_code = arrStr[0].ToString();
                //string sfc = arrStr[1].ToString();
                var var = (from u in context.P_Order_Station where u.station_code == strstation select u).FirstOrDefault();
                context.P_Order_Station.DeleteObject(var);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="json"></param>
        public static void AddStationOrder(string json)
        {
            LEDAO.P_Order_Station tem = JsonConvert.DeserializeObject<LEDAO.P_Order_Station>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                using (context)
                {
                    context.P_Order_Station.AddObject(tem);
                    context.SaveChanges();
                }
            }
        }
    }
}