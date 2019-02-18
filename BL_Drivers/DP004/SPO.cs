using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
namespace DP004
{
    public class SPO:ILE.ISPO
    {
        //主批次号
        public SPO()
        {

        }

        public IResult DoWork(IJob job, string val)
        {
            IResult res = new LEResult();
            try
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                decimal decval=Convert.ToDecimal(val);
                if (decval > job.MaxQTY)
                {
                    res.Result = false;
                    res.ExtMessage = "批次数量超出范围";
                    return res;
                }
                decimal decqty=job.MaxQTYOrder-job.QTYOrder;
                if (decval > decqty)
                {
                    res.Result = false;
                    res.ExtMessage = "数量超出工单剩余计划数[" + decqty.ToString()+ "]";
                    return res;
                }
                res.Result = true;
                job.QTY = int.Parse(val);
                job.StepList[job.StepIdx].StepValue = decval.ToString();
                job.StepList[job.StepIdx].Completed = true;
                return res;
            }
            catch (Exception exc)
            {
                //MessageBox.Show(exc.Message);
                res.Result = false;
                return res;
            }
        }

        public IResult DoWork(ILE.IJob jobModel)
        {
            //DataTable dt = DB.Database.getDataTable("select * from s_employee");
            //int i = dt.Rows.Count;
            //IResult res = new LEResult();
            //res.Result = true;
            //res.ExtMessage = i.ToString();
            return null;
        }
    }
}
