using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using LEDAO;

namespace DP_Quality
{
    public class SPO:ILE.ISPO
    {
        public IResult DoWork(IJob job, string val)
        {
            return null;
        }
        public IResult DoWork(IJob jobModel)
        {
            ILE.IResult res = new ILE.LEResult();

            QualityTest sf = new QualityTest(jobModel.Product);
            sf.ShowDialog();
            res.Result = true; //这里说明工步完成了

            jobModel.StepList[jobModel.StepIdx].Completed = true;  //当前步骤完成
            jobModel.Result = sf.QltResult;
            //res.ExtMessage = sf.ExtMessage;
            //job.reload_code += "mould/";
            if (sf.QltResult == false)
            {
                jobModel.NGCodes = sf.ng_codes;
            }
            
            return res;
        }
    }
}
