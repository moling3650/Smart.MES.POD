using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public interface IDataPoint
    {
        int point_id
        {
            get;
            set;
        }

        string Item_name
        {
            get;
            set;
        }
        string dataPoint_name
        {
            get;
            set;
        }
        string machine_code
        {
            get;
            set;
        }
        int point_type
        {
            get;
            set;
        }

        int dc_type
        {
            get;
            set;
        }

        string dc_drive_code
        {
            get;
            set;
        }

        string task_drive_code
        {
            get;
            set;
        }

        string parameter
        {
            get;
            set;
        }

        int rate
        {
            get;
            set;
        }

        string val
        {
            get;
            set;
        }

        DateTime TimeStamp
        {
            get;
            set;
        }

        string InitVal
        {
            get;
            set;
        }

        string business_code
        {
            get;
            set;
        }

        /// <summary>
        /// 触发条件
        /// </summary>
        int? trigger_condition
        {
            get;
            set;
        }

        int? trigger_type
        {
            get;
            set;
        }

        int? counter
        {
            get;
            set;
        }
    }


    public class PointVal
    {
        public PointVal()
        { }

        public DateTime ValTime
        {
            get;
            set;
        }

        public string Val
        {
            get;
            set;
        }
    }
}
