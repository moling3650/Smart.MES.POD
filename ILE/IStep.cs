using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public interface IStep
    {
        /// <summary>
        /// 工步明细
        /// </summary>
        List<StepData> StepDetail
        {
            get;
            set;
        }
        /// <summary>
        /// 子工步明细
        /// </summary>
        List<StepData> StepSonDetail
        {
            get;
            set;
        }

        #region 工步

        /// <summary>
        /// 步骤编号 ,自增
        /// </summary>
        int StepID
        {
            get;
            set;
        }
        
        string StepCode
        {
            get;
            set;
        }

        /// <summary>
        /// 请输入:XXX
        /// </summary>
        string StepName
        {
            get;
            set;
        }

        /// <summary>
        /// 工步类型
        /// </summary>
        string StepType
        {
            get;
            set;
        }

        /// <summary>
        /// 驱动类型
        /// </summary>
        int TypeID
        {
            get;
            set;
        }


        /// <summary>
        /// 驱动名
        /// </summary>
        string DriveCode
        {
            get;
            set;
        }
        /// <summary>
        /// 驱动名
        /// </summary>
        string DriveName
        {
            get;
            set;
        }

        /// <summary>
        /// 参数
        /// </summary>
        string Parameter
        {
            get;
            set;
        }

        /// <summary>
        /// 物料
        /// </summary>
        string Matcode
        {
            get;
            set;
        }

        /// <summary>
        /// 消耗  1:只判断掩码 2:自动消耗 3:手动消耗 4:装料并手动消耗
        /// </summary>
        int CtrlType
        {
            get;
            set;
        }

        /// <summary>
        /// 工步超时
        /// </summary>
        string TimeOut
        {
            get;
            set;
        }

        /// <summary>
        /// 正则表达式
        /// </summary>
        string Format
        {
            get;
            set;
        }

        /// <summary>
        /// 允许重复
        /// </summary>
        int AllowReuse
        {
            get;
            set;
        }

        /// <summary>
        /// 允许自动
        /// </summary>
        int AutoRun
        {
            get;
            set;
        }

        /// <summary>
        /// 允许重启工步
        /// </summary>
        int AutoRestart
        {
            get;
            set;
        }

        /// <summary>
        /// 顺序
        /// </summary>
        int Idx
        {
            get;
            set;
        }

        /// <summary>
        /// 允许工步提交
        /// </summary>
        int IsRecord
        {
            get;
            set;
        }

        /// <summary>
        /// 弹窗
        /// </summary>
        string Triger
        {
            get;
            set;
        }

        /// <summary>
        /// 弹窗2
        /// </summary>
        string Parameter2
        {
            get;
            set;
        }

        /// <summary>
        /// 单位  
        /// </summary>
        string Unit
        {
            get;
            set;
        }
        /// <summary>
        /// 物料消耗类型
        /// </summary>
        int consume_type
        {
            get;
            set;
        }
        /// <summary>
        /// 耗料百分比
        /// </summary>
        float consume_percent
        {
            get;
            set;
        }
        #endregion







        
    }
}
