using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LEDAO
{
    public class RMessage
    {
        //判断是否为真
        public bool Result { set; get; }

        //返回消息
        public string MessageStr { set; get; }

        //返回值
        public object JsonData { set; get; }
    }
}
