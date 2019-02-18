using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public interface IJob
    {
        List<Step> StepList
        {
            get;
            set;
        }
       
        string FlowCode
        {
            get;
            set;
        }

        int baseqty
        {
            get;
            set;
        }

        string ProcessCode
        {
            get;
            set;
        }

        string StationCode
        {
            get;
            set;
        }

        /// <summary>
        /// 工单
        /// </summary>
        string OrderNO
        {
            get;
            set;
        }

        /// <summary>
        /// 派工单
        /// </summary>
        string DispatchNO
        {
            get;
            set;
        }

        string Mould_Code
        {
            get;
            set;
        }

        string SFC
        {
            get;
            set;
        }

        /// <summary>
        /// 当前JOB是PACK方式还是单批次方式
        /// </summary>
        int IsPack
        {
            get;
            set;
        }

        decimal MaxQTY
        {
            get;
            set;
        }

        /// <summary>
        /// 计划数
        /// </summary>
        decimal QTY
        {
            get;
            set;
        }

        /// <summary>
        /// 任务开始时的完成数
        /// </summary>
        decimal InitCpltQty
        {
            get;
            set;
        }

        /// <summary>
        /// 任务当前完成数
        /// </summary>
        decimal CpltQty
        {
            get;
            set;
        }


        /// <summary>
        /// 任务开始时的不良数
        /// </summary>
        decimal InitNGQty
        {
            get;
            set;
        }

        /// <summary>
        /// 任务当前的不良数
        /// </summary>
        decimal NGQty
        {
            get;
            set;
        }

        decimal MaxQTYOrder
        {
            get;
            set;
        }

        decimal QTYOrder
        {
            get;
            set;
        }
        //车间别
        string workshop
        {
            get;
            set;
        }
        string group_code
        {
            get;
            set;
        }
        /// <summary>
        /// 员工号
        /// </summary>
        string EmpCode{get; set;}
        /// <summary>
        /// 工步索引
        /// </summary>
        int StepIdx{get; set;}
        /// <summary>
        /// 
        /// </summary>
        string RouteType{get;set;}

        void Run();

        bool Completed
        {
            get;
            set;
        }

        int Pid
        {
            get;
            set;
        }

        int IndexBack
        {
            get;
            set;
        }

        /// <summary>
        /// 返回结果NG OK
        /// </summary>
        bool Result
        {
            get;
            set;
        }

        List<Model.NGCode> NGCodes
        {
            get;
            set;
        }

        string Product{get; set;}

        bool StepLoad { get; set; }

        string FatherOrderNO{ get; set; }

        bool IsExiseStep { get; set; }
        /// <summary>
        /// 线程状态
        /// </summary>
        bool RunState { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        DateTime start_time { get; set; }

        int task_mode { get; set; }

        int production_mode { get; set; }

        /// <summary>
        /// job中的内容变化后，如果需要更新主界面的内容，则把内容编码拼进去如：machine/material/mould/,则说明主界面需要更新物料和磨具
        /// </summary>
        string reload_code { get; set; }
    }
}
