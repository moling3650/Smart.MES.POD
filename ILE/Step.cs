using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public class Step : IStep
    {


        /// <summary>
        /// 工步明细
        /// </summary>
        public List<StepData> StepDetail
        {
            get;
            set;
        }

        public List<StepData> StepSonDetail
        {
            get;
            set;
        }


        #region 工步

        /// <summary>
        /// 步骤编号 ,自增
        /// </summary>
        public int StepID
        {
            get;
            set;
        }

        /// <summary>
        /// 工步代码
        /// </summary>
        public string StepCode
        {
            get;
            set;
        }

        /// <summary>
        /// 请输入:XXX
        /// </summary>
        public string StepName
        {
            get;
            set;
        }

        /// <summary>
        /// 工步类型
        /// </summary>
        public string StepType
        {
            get;
            set;
        }

        /// <summary>
        /// 驱动类型
        /// </summary>
        public int TypeID
        {
            get;
            set;
        }

        /// <summary>
        /// 驱动名
        /// </summary>
        public string DriveCode
        {
            get;
            set;
        }
        /// <summary>
        /// 驱动名
        /// </summary>
        public string DriveName
        {
            get;
            set;
        }
        /// <summary>
        /// 驱动文件名
        /// </summary>
        public string FileName
        {
            get;
            set;
        }
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameter
        {
            get;
            set;
        }

        /// <summary>
        /// 物料
        /// </summary>
        public string Matcode
        {
            get;
            set;
        }


        /// <summary>
        /// 消耗  1:只判断掩码 2:自动消耗 3:手动消耗 4:装料并手动消耗
        /// </summary>
        public int CtrlType
        {
            get;
            set;
        }


        /// <summary>
        /// 工步超时
        /// </summary>
        public string TimeOut
        {
            get;
            set;
        }

        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Format
        {
            get;
            set;
        }


        /// <summary>
        /// 允许重复
        /// </summary>
        public int AllowReuse
        {
            get;
            set;
        }

        /// <summary>
        /// 允许自动
        /// </summary>
        public int AutoRun
        {
            get;
            set;
        }

        /// <summary>
        /// 允许重启工步
        /// </summary>
        public int AutoRestart
        {
            get;
            set;
        }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Idx
        {
            get;
            set;
        }

        /// <summary>
        /// 允许工步提交
        /// </summary>
        public int IsRecord
        {
            get;
            set;
        }

        /// <summary>
        /// 弹窗
        /// </summary>
        public string Triger
        {
            get;
            set;
        }

        /// <summary>
        /// 弹窗2
        /// </summary>
        public string Parameter2
        {
            get;
            set;
        }

        public string Unit
        {
            get;
            set;
        }

        #endregion

       
        public bool KeyStep
        {
            get;
            set;
        }
        public bool Completed
        {
            get;
            set;
        }
        //public bool isLoadcache
        //{
        //    get;
        //    set;
        //}
        public string StepValue
        {
            get;
            set;
        }
        //物料消耗类型
        public int consume_type
        {
            get;
            set;
        }
        //耗料百分比
        public float consume_percent
        {
            get;
            set;
        }

    }
}
