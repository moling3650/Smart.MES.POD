using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;

namespace BLL
{
    public class NGCode
    {
        public static string GetTypecodeByPro(string json)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.B_Product where u.product_code == json select u.type_code).FirstOrDefault();
                return var;
            }
        }
        public static string GetNGCodeByType(string typecode)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = from u in context.B_NG_Code orderby u.idx  where u.type_code == typecode select u;
                
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }
            return null;
        }

        /// <summary>
        /// 通过产品编号获取全部不良编码，带不良类型名称
        /// </summary>
        /// <param name="product_code"></param>
        /// <returns></returns>
        public static string GetNGCode(string product_code)
        {
            string typecode = GetTypecodeByPro(product_code);
            if (typecode == null)
                return null;
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = from a in context.B_NG_Code 
                          join b in context.B_NG_Type on a.type_code equals b.type_code
                          where
                            a.product_type == typecode
                          select new
                          {
                              a.decription,
                              a.ng_name,
                              a.ng_code,
                              a.idx,
                              a.ng_id,
                              a.type_code,
                              b.type_name
                          };

                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }
            return null;
        }

        //public static string GetNGCode(string json)
        //{
        //    using (var context = LEDAO.APIGateWay.GetEntityContext())
        //    {
        //        var results = context.B_NG_Code.Where(X => X.ng_code == json);
        //        if (results.Count() > 0)
        //        {
        //            return JsonConvert.SerializeObject(results.ToList<B_NG_Code>()[0]);
        //        }
        //        return "";
        //    }
        //}
    }
}