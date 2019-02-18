using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DP_InPut
{
    public class SPO:ILE.ISPO
    {
        public ILE.IResult DoWork(ILE.IJob jobModel)
        {
            jobModel.StepList[jobModel.StepIdx].StepValue = "OK";
            jobModel.StepList[jobModel.StepIdx].Completed = true;
            jobModel.StepList[jobModel.StepIdx].StepDetail = new List<ILE.StepData>();
            ILE.StepData stepdata = new ILE.StepData();
            stepdata.InPutDate = DateTime.Now;
            stepdata.StepVal = "OK";
            jobModel.StepList[jobModel.StepIdx].StepDetail.Add(stepdata);
            ILE.IResult res = new ILE.LEResult();
            res.Result = true;
            return res;
        }
        public ILE.IResult DoWork(ILE.IJob jobModel, string val)
        {

            jobModel.StepList[jobModel.StepIdx].StepValue = val;
            jobModel.StepList[jobModel.StepIdx].Completed = true;
            jobModel.StepList[jobModel.StepIdx].StepDetail = new List<ILE.StepData>();
            ILE.StepData stepdata = new ILE.StepData();
            stepdata.InPutDate = DateTime.Now;
            stepdata.StepVal = val;
            jobModel.StepList[jobModel.StepIdx].StepDetail.Add(stepdata);
            ILE.IResult res = new ILE.LEResult();
            res.Result = true;
            return res;
        }
    }
}
