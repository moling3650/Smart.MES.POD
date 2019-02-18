using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using Newtonsoft.Json;
using LEDAO;

namespace DP005
{
    public class SPO : ILE.ISPO
    {
        //主批次号
        public SPO()
        {

        }

        public IResult DoWork(IJob job, string val)
        {
            DispatchForm form = new DispatchForm(job.StationCode);
            form.ShowDialog();
            string dispatching = form.Dispetching;
            string orderNO = form.OrderNO;
            string product = form.Product;
            IResult result = new LEResult();
            if (dispatching == null)
            {
                result.Result = false;
                result.ExtMessage = "未选择";
            }
            else
            {
                job.DispatchNO = dispatching;
                job.OrderNO = orderNO;
                job.Product=product;
                result.Result = true;

                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string ResProduct = client.RunServerAPI("BLL.Product", "GetProductFlow", orderNO);
                IResult res = new LEResult();
                res = JsonConvert.DeserializeObject<ILE.LEResult>(ResProduct);
                job.Pid = Convert.ToInt32(res.obj);

            }
            return result;
        }

        public IResult DoWork(ILE.IJob job)
        {
            
            DispatchForm form = new DispatchForm(job.StationCode);

            form.ShowDialog();

            string dispatching = form.Dispetching;
            string orderNO = form.OrderNO;
            IResult result = new LEResult();
            if (dispatching == null)
            {
                result.Result = false;
                result.ExtMessage = "未选择";
            }
            else
            {
                job.DispatchNO = dispatching;
                job.Product = form.Product;
                job.OrderNO = orderNO;
                result.Result = true;

                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                ILE.LEResult res = new LEResult();
                string ResProduct1 = client.RunServerAPI("BLL.Product", "GetProductFlow", job.OrderNO);
                res = JsonConvert.DeserializeObject<ILE.LEResult>(ResProduct1);
                V_ProductFlow produc1 = JsonConvert.DeserializeObject<V_ProductFlow>(res.obj.ToString());

                string strRes1 = client.RunServerAPI("BLL.Process", "GetFlowDetailOK", produc1.flow_code + "," + job.ProcessCode);
                res = JsonConvert.DeserializeObject<ILE.LEResult>(strRes1);
                job.Pid = Convert.ToInt32(res.obj);
                job.StepList[job.StepIdx].Completed = true;
            }
            return result;
        }
    }

}
