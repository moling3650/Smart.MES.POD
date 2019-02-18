using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;
namespace LEDAO
{
    //[DataContractAttribute(IsReference = true)]
    //public partial class UserDefineData_SFCProcessData : EntityObject
    public partial class UserDefineData_SFCProcessData
    {
        //工单
        public string order_no { set; get; }
        //物料号
        public string mat_code { set; get; }
        //批次号
        public string SFC { set; get; }
        //测试值
        public string val { set; get; }
        //工步类型
        public string step_type { set; get; }
        //工步代码
        public string step_code { set; get; }
        //工步名称
        public string step_name { set; get; }
    }
}
