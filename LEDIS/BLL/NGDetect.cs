using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BLL
{
    public class NGDetect
    {
        public static string GetNGProc(string json)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = from u in context.B_NG_Code where u.ng_code == json select u;
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());

                }
                else
                {
                    return null;
                }
            }
           
        }
        public static string GetNGReason(string parstrProductType)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                if (!string.IsNullOrWhiteSpace(parstrProductType))
                {
                    var var = from u in context.B_NG_Reason where u.type_code == parstrProductType select u;
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());

                    }
                }
                else
                {
                    var var = from u in context.B_NG_Reason select u;
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());

                    }
                }
            }
            return null;
        }
        public static string GetBNGReasonType(string parstrreasontypecode)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                if (!string.IsNullOrWhiteSpace(parstrreasontypecode))
                {
                    var var = from u in context.B_NG_ReasonType where u.reasontype_code == parstrreasontypecode select u;
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());

                    }
                }
                else
                {
                    var var = from u in context.B_NG_ReasonType select u;
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());

                    }
                }
            }
            return null;
        }
    }
}