using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using LEDAO;

namespace LL_MaterialNum
{
    public class SPO:ISPO
    {
        public IResult DoWork(IJob job)
        {
            ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
            IResult res = new LEResult();
            try
            {
                int Lot_Qty = int.Parse(job.QTY.ToString());
                string Mat_code = job.StepList[job.StepIdx].Matcode.ToString();
                string Product_code = job.Product.ToString();
                decimal qty = decimal.Parse(clien.RunServerAPI("BLL.Product", "GetMaterialQty", Mat_code + "," + Product_code));
                decimal MulQty = Lot_Qty * qty;
                job.StepList[job.StepIdx].StepValue = MulQty.ToString();
                job.StepList[job.StepIdx].StepDetail = new List<ILE.StepData>();
                ILE.StepData stepdata = new ILE.StepData();
                stepdata.InPutDate = DateTime.Now;
                stepdata.StepVal = MulQty.ToString();
                job.StepList[job.StepIdx].StepDetail.Add(stepdata);
                job.StepList[job.StepIdx].Completed = true;
                res.Result = true;

               
            }
            catch(Exception ex)
            {
                res.ExtMessage ="驱动加载失败，请检查工步配置";
                res.Result = false;
                return res;
            }
            return res;
        }
        //
        public IResult DoWork(ILE.IJob job, string val)
        {
            return null;
        }
    }
}
