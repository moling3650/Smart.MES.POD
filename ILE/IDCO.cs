using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ILE
{
    public delegate void DataReceiveEvent(ILE.IDataPoint dp);
    /// <summary>
    /// 设备集成数据点采集通用接口
    /// </summary>
    public interface IDCO
    {
        event DataReceiveEvent OnDataReceiveEvent;
        event DataReceiveEvent OnMachineDisconn;

        List<ILE.IDataPoint> ItemPoints
        {
            get;
            set;
        }


        IResult init();

        void Abort();

        IResult AddItem(IDataPoint point);

        string Driver_code
        {
            get;
            set;
        }
    }
}
