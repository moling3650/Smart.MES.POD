using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BLL
{
    public class Bom
    {
        public static string GetBomDetail(string json)
        {
            string[] str = json.Split(',');
            string mat_code = str[0].ToString();
            string product_code = str[1].ToString();
            string bom_code = str[2].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = context.V_Bom_Detail.Where(X => X.mat_code == mat_code && X.product_code == product_code && X.bom_code == bom_code);
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }
            return null;
        }
        public static string GetBomDetailBaseQty(string json)
        {
            string[] str = json.Split(',');
            string mat_code = str[0].ToString();
            string product_code = str[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = context.V_Bom_Detail.Where(X => X.mat_code == mat_code && X.product_code == product_code).ToList();
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var[0].base_qty);
                }
            }
            return null;
        }


        public static string GetBomById(string bom_id)
        {
            int bomid = Convert.ToInt32(bom_id);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = context.B_Bom.Where(X => X.bom_id == bomid);
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }
            return null;
        }


    }
}