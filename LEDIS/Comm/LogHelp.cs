using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LEDIS.Comm
{
    public class LogHelp
    {
        public static void LogError(Exception ex,string type)
        {
            var context= LEDAO.APIGateWay.GetEntityContext();
            string s = string.Format("insert into ");
            
        }
    }
}