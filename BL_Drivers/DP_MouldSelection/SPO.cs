using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using LEDAO;
using Newtonsoft.Json;

namespace DP_MouldSelection
{
    public class SPO : ILE.ISPO
    {
        ServiceReference.ServiceClient client;
        //主批次号
        public SPO()
        {

        }

        public IResult DoWork(IJob job, string val)
        {
            return null;
        }

        public IResult DoWork(IJob job)
        {
            ILE.IResult res=new ILE.LEResult();
            client = new ServiceReference.ServiceClient();

            string ResDispatching = client.RunServerAPI("BLL.Machine", "GetMachineInfo", job.StationCode);

            if (ResDispatching == "")
            {
                res.Result = false;
                res.ExtMessage = "该工位未绑定设备，无法使用模具";
                return res;
            }

            B_Machine produc = JsonConvert.DeserializeObject<B_Machine>(ResDispatching);

            SelectionForm sf = new SelectionForm(produc,job.StationCode,job.EmpCode);
            sf.ShowDialog();
            res.Result = sf.res.Result;
            res.ExtMessage = sf.res.ExtMessage;
            job.reload_code += "mould/";
            return res;
        }
    }
}
