using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using ILE.Model;
using System.Data;
using LEDAO;
using Newtonsoft.Json;

namespace LEMES_POD.Component
{
    class JobConstructor
    {
        public static IJob GetJob(V_ProcessList_Workshop process )//B_ProcessList process)
        {
            IResult res = BindPreStep(process.process_code);
            ILE.IJob job = new Job();
            List<Step> steplist = new List<Step>();
            job.StepList = steplist;
            if (res.Result)  //存在前置工步的情况下
            {
                List<dynamic> pre = JsonConvert.DeserializeObject<List<dynamic>>(res.obj.ToString());
                foreach (var tstep in pre)
                {
                    Step step = new Step();
                    step.StepID = tstep.pre_id;
                    step.StepName = tstep.step_name;
                    step.StepCode = tstep.step_code;
                    step.StepType = tstep.step_type;
                    step.TypeID = tstep.type_id.Value;
                    step.DriveCode = tstep.drive_code;
                    step.FileName = tstep.file_name;
                    step.Parameter = tstep.parameter;
                    step.TimeOut = tstep.time_out;
                    step.Format = tstep.format;
                    step.KeyStep = tstep.IsKeySteps == 1 ? true : false;
                    step.AllowReuse = tstep.allow_reuse==null ?0:tstep.allow_reuse.Value;
                    step.AutoRun = tstep.autorun == null ? 0 : tstep.autorun.Value; 
                    step.AutoRestart = (int)tstep.auto_restart;
                    step.Idx = tstep.idx;
                    step.IsRecord = tstep.is_record;
                    step.Triger = tstep.triger;
                    step.Parameter2 = tstep.parameter2;
                    job.StepList.Add(step);
                }
                job.StepIdx = 0;
                job.IsExiseStep = true;//存在可配
                job.RouteType = process.route_type;
                job.workshop = process.ws_code;
                job.group_code = process.group_code;
                job.RunState = true;
                return job;
            }
            Step sp1 = new Step();
            sp1.StepName = "员工号";
            sp1.DriveCode = "DP001";
            sp1.FileName = "DP001.dll";
            sp1.AllowReuse = 1;
            sp1.Completed = false;
            sp1.IsRecord = 0;
            job.StepList.Add(sp1);

            if (process.task_mode == 0)  //工单作业模式下，只有首站需要录工单
            {
                if (process.route_type == "首工站")
                {
                    Step sp2 = new Step();
                    sp2.StepName = "工单号";
                    sp2.DriveCode = "DP002";
                    sp2.FileName = "DP002.dll";
                    sp2.AllowReuse = 1;
                    sp2.Completed = false;
                    sp2.IsRecord = 0;
                    job.StepList.Add(sp2);
                }
            }
            else //派工单作业模式下，所有工序都要输工单
            {
                Step sp2 = new Step();
                sp2.StepName = "派工单号";
                sp2.DriveCode = "DP005";
                sp2.FileName = "DP005.dll";
                sp2.AllowReuse = 1;
                sp2.Completed = false;
                if (process.production_mode == 1)  //连续生产模式下，派工单号必须变为关键工步,否则无法加载后工步
                {
                    sp2.KeyStep = true;
                }
                sp2.AutoRun = 1;
                sp2.IsRecord = 0;
                job.StepList.Add(sp2);
            }

            if (process.production_mode == 0)
            {
                Step sp3 = new Step();
                sp3.StepName = "成品批次";
                sp3.DriveCode = "DP003";
                sp3.FileName = "DP003.dll";
                sp3.Completed = false;
                sp3.KeyStep = true;
                sp3.AllowReuse = 0;
                sp3.IsRecord = 0;
                job.StepList.Add(sp3);
            }
            //string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetProductLotType", "10000");
            job.StepIdx = 0;
            job.QTY = 1;
            job.RouteType = process.route_type;
            job.RunState = true;
            job.workshop = process.ws_code;
            job.group_code = process.group_code;
            return job;
        }

        /// <summary>
        /// 关键工序采集完成后,加载后续的工序,比如零件,录入,设备,从processStep表
        /// </summary>
        /// <param name="job"></param>
        public static ILE.IResult FillJobStep(IJob job)
        {
            int i = 0;
            List<Step> liststep = new List<Step>();
            if (!job.IsExiseStep&&job.MaxQTY > 1 & job.RouteType == "首工站" &job.production_mode==0)
            {
                Step sp4=new Step();
                sp4.StepName = "批次数量";
                sp4.StepCode = "LOT QTY";
                sp4.TypeID = 0;
                sp4.DriveCode = "DP004";
                sp4.FileName = "DP004.dll";
                sp4.IsRecord = 0;
                sp4.AllowReuse = 0;
                sp4.Format = "^[0-9]+$";
                sp4.Completed = false;
                liststep.Add(sp4);
                i++;
            }
            //job.IndexBack = job.StepList.Count();
            ILE.IResult res = BindStep(job.Pid.ToString());
            if (!res.Result)
            {  
                res.obj = null; 
                return res; 
            }

            if (res.obj == null)  //无工步的情况
            {
                job.IndexBack = job.StepList.Count + i;
                job.StepList.AddRange(liststep);
                job.StepLoad = false;
                return res;
            }

            List<dynamic> steplist = JsonConvert.DeserializeObject<List<dynamic>>(res.obj.ToString());
            foreach (dynamic tstep in steplist)
            {
                Step step=new Step();
                step.StepID = tstep.step_id;
                step.StepName = tstep.step_name;
                step.StepCode = tstep.step_code;
                step.StepType = tstep.step_type;
                step.TypeID = tstep.type_id;
                step.DriveCode = tstep.drive_code;
                step.FileName = tstep.file_name;
                step.Parameter = tstep.parameter;
                step.Matcode = tstep.mat_code;
                step.CtrlType = tstep.ctrl_type;
                step.TimeOut = tstep.time_out;
                step.Format = tstep.format;
                step.AllowReuse = tstep.allow_reuse;
                step.AutoRun = tstep.autorun;
                step.AutoRestart = (int)tstep.auto_restart;
                step.Idx = tstep.idx;
                step.IsRecord = tstep.is_record;
                step.Triger = tstep.triger;
                step.Parameter2 = tstep.parameter2;
                step.consume_type =(int)tstep.consume_type;
                step.consume_percent = (float)tstep.consume_percent;
                liststep.Add(step);             
            }
            //代表前工步的数量
            job.IndexBack =job.StepList.Count + i;
            job.StepList.AddRange(liststep);
            job.StepLoad = true;
            return res;
        }


        public static IResult BindStep(string pid)
        {
            return BLL.ServiceReference.DISResult("BLL.Step","GetStepAll",pid);
        }

        public static IResult BindPreStep(string process)
        {
            string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Step", "GetPreStepAll", process);
            ILE.IResult res = JsonConvert.DeserializeObject<LEResult>(strRes);
            return res;
        }

    }
}
