using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using System.Data;
using Newtonsoft.Json;

namespace BLL
{
    public class WorkShop
    {
        //B_WorkShop 表操作
        
        /// <summary>
        /// 获取指定工作组对应的车间信息（车间代码，车间名称）
        /// </summary>
        /// <param name="group_code"></param>
        /// <returns></returns>
        public static string GetWorkShopInfo(string strgroupcode)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.B_WorkShop
                       join b in context.B_WorkGroup on a.wsid equals b.wsid
                       where
                         b.group_code == strgroupcode
                       select new
                       {
                           a.ws_code,
                           a.ws_name
                       }).Distinct().ToList();
            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res.ToList());
            }
            return null;
        }
    }
}   