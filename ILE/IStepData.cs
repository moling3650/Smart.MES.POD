using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public interface IStepData
    {

        /// <summary>
        /// GUID唯一标示 sid
        /// </summary>
        string GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 存入值
        /// </summary>
        string StepVal
        {
            get;
            set;
        }

        /// <summary>
        /// 写入时间
        /// </summary>
        DateTime InPutDate
        {
            get;
            set;
        }

        /// <summary>
        /// 数量
        /// </summary>
        decimal qty
        {
            get;
            set;
        }
        /// <summary>
        /// 物料批次数量
        /// </summary>
        decimal ? lotqty
        {
            get;
            set;
        }
        /// <summary>
        /// seed_id
        /// </summary>
       string seed_id
        {
            get;
            set;
        }
       /// <summary>
       /// wip_id
       /// </summary>
       string wip_id
       {
           get;
           set;
       }
        /// <summary>
        /// 物料
        /// </summary>
        string matCode
        {
            get;
            set;
        }
        /// <summary>
        /// stepname
        /// </summary>
        string stepname
        {
            get;
            set;
        }
        /// <summary>
        /// stepSonPre
        /// </summary>
        string stepSonPre
        {
            get;
            set;
        }
        /// <summary>
        /// 子工步编码
        /// </summary>
        string stepsoncode
        {
            get;
            set;
        }
        /// <summary>
        /// 结论
        /// </summary>
        string stepconclude
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
        /// 设备编号
        /// </summary>
        string machine_code
        {
            get;
            set;
        }
        /// <summary>
        /// 备件编号
        /// </summary>
        string sprepart_code
        {
            get;
            set;
        }
    }
}
