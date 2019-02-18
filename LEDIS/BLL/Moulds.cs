using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BLL
{
    public class Moulds
    {
        /// <summary>
        /// 获取指定模具
        /// </summary>
        /// <param name="station_code"></param>
        /// <returns></returns>
        public static string GetMouldByCode(string mouldCode)
        {
            //V_Station_Machine
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                try
                {
                    //var context = LEDAO.APIGateWay.GetEntityContext();
                    var res = (from a in context.B_Moulds
                               join b in context.B_Mould_Model on a.model_code equals b.model_code
                               where
                                    a.mould_code == mouldCode
                               select new
                               {
                                   a.machine_code,
                                   a.station_code,
                                   a.description,
                                   a.input_time,
                                   a.state,
                                   a.supplier,
                                   a.type_id,
                                   a.model_code,
                                   a.mould_name,
                                   a.mould_code,
                                   a.id,
                                   mould_kind_id = b.kind_id
                               }).FirstOrDefault();
                    if (res!=null)
                    {
                        return JsonConvert.SerializeObject(res);
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                return null;
            }
        }

        /// <summary>
        /// 获取模具安装数据
        /// </summary>
        /// <param name="station_code"></param>
        /// <returns></returns>
        public static string GetMouldInstall(string mouldCode)
        {
            //V_Station_Machine
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                try
                {
                    //var context = LEDAO.APIGateWay.GetEntityContext();
                    var res = (from a in context.P_Machine_Mould_Install 
                               where a.mould_code==mouldCode
                            select a).FirstOrDefault();
                    if (res != null)
                    {
                        return JsonConvert.SerializeObject(res);
                    }
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
                return null;
            }
        }

        /// <summary>
        /// 模具安装
        /// </summary>
        /// <param name="station_code"></param>
        /// <returns></returns>
        public static void MouldInstall(string paramStr)
        {
            string[] pams = paramStr.Split(',');
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                LEDAO.P_Machine_Mould_Install pmi = new LEDAO.P_Machine_Mould_Install();
                pmi.machine_code = pams[0];
                pmi.station_code = pams[1];
                pmi.mould_code = pams[2];
                pmi.install_time = DateTime.Parse(pams[3]);
                pmi.emp_code = pams[4];
                context.P_Machine_Mould_Install.AddObject(pmi);
                context.SaveChanges();
            }
        }


        /// <summary>
        /// 卸模
        /// </summary>
        /// <param name="paramStr"></param>
        public static void MouldUninstall(string mouldCode)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.P_Machine_Mould_Install where u.mould_code == mouldCode select u);
                foreach (var item in var)
                {
                    context.P_Machine_Mould_Install.DeleteObject(item);
                }
                context.SaveChanges();
            }
        }
    }
}