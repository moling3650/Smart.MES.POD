using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public interface IResult
    {
        bool Result{ get;set; }

        string ExtMessage{ get;set;}

        object obj{get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LEResult : IResult
    {
        public LEResult(){Result = false;}
        /// <summary>
        /// 结果
        /// </summary>
        public bool Result{ get;set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string ExtMessage{get; set;}
        /// <summary>
        /// 对象值
        /// </summary>
        public object obj{get;set; }
    }
}
