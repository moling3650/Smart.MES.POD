using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public interface IFormProperty
    {
        /// <summary>
        /// 工步
        /// </summary>
        IJob Job
        {
            get;
            set;
        }
        /// <summary>
        /// 值
        /// </summary>
        string Val
        {
            get;
            set;
        }
        /// <summary>
        /// 是否取消
        /// </summary>
        bool Cancel
        {
            get;
            set;
        }

        void Run();
        
    }
}
