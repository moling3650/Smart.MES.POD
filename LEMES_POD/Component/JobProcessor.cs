using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using ILE;
using LEDAO;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace LEMES_POD.Component
{
    public class JobProcessor
    {
        #region 动态调用dll驱动
        public static ILE.IResult HandleJob(ILE.IJob job, string val)
        {

            string driveName = job.StepList[job.StepIdx].FileName;
            
            string path = System.IO.Directory.GetCurrentDirectory() + "\\DriveList\\" + driveName;
            IResult rec = new LEResult();
            try
            {
                Assembly ass = Assembly.LoadFrom(path);
                Type type = ass.GetType(job.StepList[job.StepIdx].DriveCode + ".SPO");   //drive_code作为默认的驱动内命名空间
                Object obj = Activator.CreateInstance(type);
                ISPO SpoObj = (ISPO)obj;
                if (val == null) //自动耗料
                {
                    rec = SpoObj.DoWork(job);
                }
                else
                {
                    rec = SpoObj.DoWork(job, val);
                }
            }
            catch (Exception exp)
            {
                rec.Result = false;
                rec.ExtMessage = "加载工步驱动" + "[" + driveName + "]失败:" + exp.Message;
                return rec;
            }

            ///如果当前被完成的STEP是关键步骤
            if (rec.Result && job.Pid != 0 && job.StepList[job.StepIdx].KeyStep && !job.StepLoad)
            {
                return Component.JobConstructor.FillJobStep(job);
            }
            string i = rec.ExtMessage;
            return rec;
        }
        #endregion


        public static int NextStep(ILE.IJob job)
        {
            if (job.StepIdx == job.StepList.Count)
            {
                return job.StepIdx;//最后工步
            }
            for (int i = job.StepIdx + 1; i < job.StepList.Count; i++)
            {
                if (!job.StepList[i].Completed)
                {
                    //job.StepIdx = i;
                    return i; //
                }
            }
            return job.StepIdx;
        }
        public static int NextStep1(ILE.IJob job)
        {
            if (job.StepIdx - 1 == job.StepList.Count)
            {
                return job.StepIdx;//最后工步
            }
            for (int i = job.StepIdx + 1; i < job.StepList.Count; i++)
            {
                if (!job.StepList[i].Completed)
                {
                    //job.StepIdx = i;
                    return i; //
                }
            }
            return job.StepIdx - 1;
        }

        //初始化当前的JOB,把指针还原
        public static void InitJob(IJob job)
        {
            //0： 否 1： 是
            bool key = false;
            for (int i = job.StepList.Count - 1; i >= 0; i--)
            {
                if (job.StepList[i].KeyStep)
                { 
                    break; 
                }
                else
                {
                    if (job.StepList[i].AllowReuse == 0 & job.StepList[i].StepValue != null)
                    {
                        //job.StepList.RemoveAt(i);//清除数据
                        job.StepList[i].StepValue = null;
                        job.StepList[i].Completed = false;
                    }
                }
            }

            for (int idx = 0; idx < job.StepList.Count; idx++)
            {
                if (job.StepList[idx].AllowReuse == 0 & job.StepList[idx].StepValue != null)
                {
                    job.StepIdx = idx;
                    //job.IndexBack = 0;
                }
            }
            //job.StepLoad = false;
        }
        //重复利用时跳出此工步
        public static void JumpJob(IJob job)
        {
            for (int idx = job.IndexBack; idx < job.StepList.Count; idx++)
            {
                if (job.StepList[idx].AllowReuse == 2 & job.StepList[idx].StepValue != null)
                {
                    job.StepIdx = idx + 1;
                }
            }
        }
    }
}
