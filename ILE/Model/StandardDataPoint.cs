using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE.Model
{
    
    //数据驱动使用的初始化参数
    public class StandardDataPoint:IDataPoint
    {
        public StandardDataPoint()
        {
            this.TaskVals = new List<PointVal>();
            this.TaskSum = 0;
        }
        public int point_id
        {
            get;
            set;
        }

        public string Item_name
        {
            get;
            set;
        }

        public string dataPoint_name
        {
            get;
            set;
        }

        public string machine_code
        {
            get;
            set;
        }

        public int point_type
        {
            get;
            set;
        }

        public int dc_type
        {
            get;
            set;
        }

        public string dc_drive_code
        {
            get;
            set;
        }

        public string task_drive_code
        {
            get;
            set;
        }

        public string parameter
        {
            get;
            set;
        }
        public string task_parameter
        {
            get;
            set;
        }

        public int rate
        {
            get;
            set;
        }

        public string val
        {
            get;
            set;
        }

        public DateTime TimeStamp
        {
            get;
            set;
        }

        /// <summary>
        /// 初始值  用于计算完成数扣减前值
        /// </summary>
        public string InitVal
        {
            get;
            set;
        }

        /// <summary>
        /// 业务代码
        /// </summary>
        public string business_code
        {
            get;
            set;
        }

        /// <summary>
        /// 驱动编号
        /// </summary>
        public string business_drive_code
        {
            get;
            set;
        }


        /// <summary>
        /// 触发条件
        /// </summary>
        public int? trigger_condition
        {
            get;
            set;
        }

        public int? trigger_type
        {
            get;
            set;
        }

        public int? counter
        {
            get;
            set;
        }

        /// <summary>
        /// 业务参数
        /// </summary>
        public string business_parameter
        {
            get;
            set;
        }

        public int run_at
        {
            get;
            set;
        }

        /// <summary>
        /// 用于保存未提交的任务数据，任务提交后清除
        /// </summary>
        public List<PointVal> TaskVals
        {
            get;
            set;
        }

        public StandardDataPoint LastPoint
        {
            get;
            set;
        }

        /// <summary>
        /// 一个任务循环（例如5个信号周期）的总完成数
        /// </summary>
        public decimal TaskSum
        {
            get;
            set;
        }

        public void AddPointVal(string val, DateTime time)
        {
            PointVal pv = new PointVal();
            pv.ValTime = time;
            pv.Val = val;
            TaskVals.Add(pv);
        }
    }
}
