using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BLL
{
    public class ProcessFlow
    {
        public static string GetProcessFlow(string flow_code)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = context.B_Process_Flow.Where(X => X.flow_code == flow_code);
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }
            return null;
        }

    }
}