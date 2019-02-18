using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using Newtonsoft.Json;

namespace LEMES_POD.BLL
{
    public class ServiceReference
    {
        public static IResult DISResult(string spc,string APIName,string json)
        {
            string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI(spc, APIName, json);
            ILE.IResult res = JsonConvert.DeserializeObject<LEResult>(strRes);
            return res;
        }

        public static string DISObject(string spc, string APIName, string json)
        {
            string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI(spc, APIName, json);
            return strRes;
        }
    }
}
