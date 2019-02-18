using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE.Model
{
    public class AnalogDataPoint : IDataPoint
    {
        public AnalogDataPoint()
        {
            this.PointVals = new List<PointVal>();
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

        public string InitVal
        {
            get;
            set;
        }

        public string business_code
        {
            get;
            set;
        }

        public string business_name
        {
            get;
            set;
        }


        public int to_monitor
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

        /// <summary>
        /// 触发条件
        /// </summary>
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

        public decimal? lcl
        {
            get;
            set;
        }

        public decimal? ucl
        {
            get;
            set;
        }

        public int? group_count
        {
            get;
            set;
        }

        public int? run_at
        {
            get;
            set;
        }

        /// <summary>
        /// 用于保存最近的一批数据
        /// </summary>
        public List<PointVal> PointVals
        {
            get;
            set;
        }

        public void AddPointVal(string val,DateTime time)
        { 
            PointVal pv=new PointVal();
            pv.ValTime=time;
            pv.Val=val;
            PointVals.Add(pv);
        }
    }
}
