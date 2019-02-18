using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;

namespace TP001
{
    /// <summary>
    ///报工驱动
    /// </summary>
    public class TPO : ILE.ITPO
    {
        private string _param
        {
            get;
            set;
        }
        public TPO(string param)
        {
            this._param = param;
        }

        public string Driver_code
        {
            get;
            set;
        }
        /// <summary>
        /// 派工单与工单报工
        /// </summary>
        /// <param name="job"></param>
        /// <param name="cplt"></param>
        /// <returns></returns>
        public IResult DoWork(IJob job, string cplt)
        {

            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string jsdata = client.RunServerAPI("BLL.WorkDispatching", "OrderReport", job.DispatchNO+","+job.OrderNO+","+cplt);

            ILE.LEResult rec = new LEResult();
            if (jsdata != "")
            {
                rec.Result = false;
                rec.ExtMessage = jsdata;
                return rec;
            }
            
            rec.Result = true;
            return rec;
        }
    }


}
