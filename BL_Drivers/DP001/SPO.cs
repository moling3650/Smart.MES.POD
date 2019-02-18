using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using LEDAO;
using System.Data;
using Newtonsoft.Json;

namespace DP001
{
    public class SPO:ILE.ISPO
    {
        public IResult DoWork(IJob job, string val)
        {
            IResult res = new LEResult();
            try
            {

                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string jsdata = client.RunServerAPI("BLL.Employee", "GetStaff", val);
                if (jsdata == "")
                {
                    res.ExtMessage = "员工号不存在!";
                    res.Result = false;
                    return res;
                }
                string jsdata1 = client.RunServerAPI("BLL.Employee", "GetStaffEnable", val);
                if (jsdata1 == "")
                {
                    res.ExtMessage = "员工号已停用,请联系管理员!";
                    res.Result = false;
                    return res;
                }
                S_Employee emp = JsonConvert.DeserializeObject<S_Employee>(jsdata);
                job.EmpCode = emp.emp_code;
                job.StepList[job.StepIdx].StepValue = val;
                job.StepList[job.StepIdx].Completed = true;  //当前步骤完成
                res.Result = true;
                return res;
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
                res.ExtMessage = "员工信息获取失败!";
                res.Result = false;
                return res;
            }
        }

        public IResult DoWork(IJob jobModel)
        {
            //DataTable dt = DB.Database.getDataTable("select * from s_employee");
            //int i = dt.Rows.Count;
            //IResult res = new LEResult();
            //res.Result = true;
            //res.ExtMessage = i.ToString();
            //return res;
            return null;
        }
    }
}
