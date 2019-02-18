using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{

    /// <summary>
    /// 业务驱动接口
    /// </summary>
    public interface ISPO
    {
        IResult DoWork(IJob jobModel);

        IResult DoWork(IJob jobModel,string val);

        
    }
}
