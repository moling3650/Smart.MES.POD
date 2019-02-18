using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public class StepData : IStepData
    {
        /// <summary>
        /// GUID唯一标示 sid
        /// </summary>
        public string GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 存入值
        /// </summary>
        public string StepVal
        {
            get;
            set;
        }

        /// <summary>
        /// 写入时间
        /// </summary>
        public DateTime InPutDate
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal qty
        {
            get;
            set;
        }
        /// <summary>
        /// 物料数量
        /// </summary>
        public decimal ? lotqty
        {
            get;
            set;
        }
        /// <summary>
        /// ssed_id
        /// </summary>
        public string seed_id
        {
            get;
            set;
        }
        /// <summary>
        /// wip_id
        /// </summary>
        public string wip_id
        {
            get;
            set;
        }
        /// <summary>
        /// matcode
        /// </summary>
        public string matCode
        {
            get;
            set;
        }
        /// <summary>
        /// stepname
        /// </summary>
        public string stepname
        {
            get;
            set;
        }
        /// <summary>
        /// 子工步工艺要求
        /// </summary>
        public string stepSonPre
        {
            get;
            set;
        }
        /// <summary>
        /// 子工步编码
        /// </summary>
        public string stepsoncode
        {
            set;
            get;
        }
        /// <summary>
        /// 结论
        /// </summary>
        public string stepconclude
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
        //设备号
        public string machine_code
        {
            get;
            set;
        }
        public string sprepart_code
        {
            get;
            set;
        }
    }
}
