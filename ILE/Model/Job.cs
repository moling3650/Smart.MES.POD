using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;

namespace ILE.Model
{
    public class Job:IJob
    {
        public Job()
        {
            this.Result = true;
            StepLoad = false;
        }
        public List<Step> StepList
        {
            get;
            set;
        }

        public string FlowCode
        {
            get;
            set;
        }
        public int baseqty
        {
            get;
            set;
        }
        public string ProcessCode
        {
            get;
            set;
        }

        public string StationCode
        {
            get;
            set;
        }

        public string OrderNO
        {
            get;
            set;
        }

        public string DispatchNO
        {
            get;
            set;
        }

        public string Mould_Code
        {
            get;
            set;
        }

        public string SFC
        {
            get;
            set;
        }

        public int IsPack
        {
            get;
            set;
        }

        public decimal MaxQTY
        {
            get;
            set;
        }

        public decimal QTY
        {
            get;
            set;
        }

        /// <summary>
        /// 任务开始时的完成数
        /// </summary>
        public decimal InitCpltQty
        {
            get;
            set;
        }

        /// <summary>
        /// 任务当前完成数
        /// </summary>
        public decimal CpltQty
        {
            get;
            set;
        }

        /// <summary>
        /// 任务开始时的不良数
        /// </summary>
        public decimal InitNGQty
        {
            get;
            set;
        }
        /// <summary>
        /// 任务当前的不良数
        /// </summary>
        public decimal NGQty
        {
            get;
            set;
        }

        public string EmpCode
        {
            get;
            set;
        }

        public int StepIdx
        {
            get;
            set;
        }

        public string RouteType
        {
            get;
            set;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }

        public bool Completed
        {
            get;
            set;
        }

        public int Pid
        {
            get;
            set;
        }

        public int IndexBack
        {
            get;
            set;
        }

        public bool Result
        {
            get;
            set;
        }

        public List<Model.NGCode> NGCodes
        {
            get;
            set;
        }

        public string Product
        {
            get;
            set;
        }


        public bool StepLoad
        {
            get;
            set;
        }


        public string FatherOrderNO
        {
            get;
            set;
        }
        public decimal MaxQTYOrder
        {
            get;
            set;
        }

        public decimal QTYOrder
        {
            get;
            set;
        }
        //车间别
        public string workshop
        {
            get;
            set;
        }
        //工序组编码
        public string group_code
        {
            get;
            set;
        }
        /// <summary>
        /// 前工步是否存在可配
        /// </summary>
        public bool IsExiseStep { get; set; }
        /// <summary>
        /// 线程状态
        /// </summary>
        public bool RunState { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 任务模式，0工单，1派工单
        /// </summary>
        public int task_mode { get; set; }

        /// <summary>
        /// 作业模式，0批次生产，1连续生产
        /// </summary>
        public int production_mode { get; set; }

        /// <summary>
        /// job中的内容变化后，如果需要更新主界面的内容，则把内容编码拼进去如：machine/material/mould/,则说明主界面需要更新物料和磨具
        /// </summary>
        public string reload_code { get; set; }
    }
}
