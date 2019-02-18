using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LEMES_POD.Component
{
    internal class JobSubmit
    {
        public static ILE.IResult JobUpload(ILE.IJob job)
        {
            string Pid = job.Pid.ToString();
            string Strict =Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Process", "GetStrict", Pid);
            string strJob=JsonConvert.SerializeObject(job);
            try
            {
                string strResult = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.JobSubmit", "JobUpload", strJob + "|" + Strict);
                ILE.LEResult result = JsonConvert.DeserializeObject<ILE.LEResult>(strResult);                
                return result;
            }
            catch (Exception)
            {
                //throw;
                ILE.IResult res = new ILE.LEResult();
                res.ExtMessage = "";
                res.Result = false;
                return res;
            }

        }
    }
}
