using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LeMesTools
{
    public class ServiceReferenceManager
    {
        static ServiceReference.ServiceClient client;

        //获取全局唯一的ServiceClient
        private static ServiceReference.ServiceClient GetClient()
        {
            if (client == null)
            {
                client = new ServiceReference.ServiceClient();
                return client;
            }
            else
            {
                return client;
            }
        }

        public static string GetDataJson(string spc,string APIName,string JsonData)
        {
            return GetClient().RunServerAPI(spc, APIName, JsonData);
        }

        public static string GetDataJson(string spc, string APIName, params string[] strArry)
        {
            string str = string.Join(",", strArry);
            return GetClient().RunServerAPI(spc, APIName, str);

        }
    }
}
