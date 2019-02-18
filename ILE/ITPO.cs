using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    /// <summary>
    /// 业务驱动接口
    /// </summary>
    public interface ITPO
    {
        string Driver_code
        {
            get;
            set;
        }
        IResult DoWork(IJob jobModel, string val);

    }
}
