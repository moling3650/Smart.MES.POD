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
            var results = context.S_Employee.Where(c => c.emp_code == sid & c.password == pwd & c.is_staff == 1);

            if (results.Count() > 0)
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// 员工登录检查
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public static string CheckLogin(string strjson)
        {
            string sid="";
            string pwd="";
            string[] arritem = strjson.Split(';');
            if (arritem.Length < 2)
            {
                return "Fail:输入参数有误";
            }
            sid = arritem[0];
            pwd = arritem[1];

            var context = LEDAO.APIGateWay.GetEntityContext();
            var results = context.S_Employee.Where(c => c.emp_code == sid & c.password == pwd);

            if (results.Count() > 0)
            {
                return "OK";
            }
            return "Fail:用户编号或密码有误";

        }
        public static string CheckFuncCode(string strjson)
        {
            string sid = "";
            string FuncCode = "";
            string[] arritem = strjson.Split(';');
            if (arritem.Length < 2)
            {
                return "Fail:输入参数有误";
            }
            sid = arritem[0];
            FuncCode = arritem[1];

            var context = LEDAO.APIGateWay.GetEntityContext();
            var results = context.V_Employee_Menu.Where(c => c.emp_code == sid & c.url == FuncCode);

            if (results.Count() > 0)
            {
                return "OK";
            }
            return "Fail:当前用户不存在["+FuncCode+"]功能";
        }
        public static string GetStaff(string sid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var results = context.S_Employee.Where(c => c.emp_code == sid);
            if (results.Count() > 0)
            {
                return  JsonConvert.SerializeObject(results.ToList<S_Employee>()[0]);
            }
            return "";
        }
        public static string GetStaffEnable(string sid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var results = context.S_Employee.Where(c => c.emp_code == sid && c.enable==1);
            if (results.Count() > 0)
            {
                return JsonConvert.SerializeObject(results.ToList<S_Employee>()[0]);
            }
            return "";
        }
    }
}