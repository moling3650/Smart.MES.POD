using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;

namespace BLL
{
    public class Employee
    {
        /// <summary>
        /// 判断员工是否为管理员
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static bool CheckStaff(string sid, string pwd)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var results = context.S_Employee.Where(c => c.emp_code == sid & c.pass_wprd == pwd & c.is_staff == 1);

            if (results.Count() > 0)
            {
                return true;
            }
            return false;

        }

        public static string GetStaff(string sid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var results = context.S_Employee.Where(c => c.emp_code == sid);
            
            if (results.Count() > 0)
            {
                return  JsonConvert.SerializeObject(results.ToList<S_Employee>()[0]);
            }
            return "false";

        }
    }
}