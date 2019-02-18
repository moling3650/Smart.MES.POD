using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LEMES_LEOQC.Tools
{
    public class ServiceReferenceManager
    {
        static ServiceReference.ServiceClient client;

        //获取全局唯一的ServiceClient
        public static ServiceReference.ServiceClient GetClient()
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
    }
}
