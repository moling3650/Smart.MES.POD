using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using ILE.Model;
using HaiGrang.Package.OpcNetApiChs;
using HaiGrang.Package.OpcNetApiChs.Opc;
using HaiGrang.Package.OpcNetApiChs.Da;
using HaiGrang.Package.OpcNetApiChs.DaNet;
using HaiGrang.Package.OpcNetApiChs.Common;

namespace DC_OpcMonitor
{
    public class DCO:IDCO
    {
        /// <summary>
        /// 设备的状态开关
        /// </summary>
        public ILE.IDataPoint statusPoint
        {
            get;
            set;
        }

        public string Driver_code
        {
            get;
            set;
        }

        /// <summary>
        /// 除状态以外全部数据点都放这里，通过AddItem添加
        /// </summary>
        public List<ILE.IDataPoint> ItemPoints
        {
            get;
            set;
        }

        
        
        /// <summary>
        /// 添加状态点到ItemPoints
        /// </summary>
        /// <param name="point"></param>
        public IResult AddItem(ILE.IDataPoint point)
        {
            //此处把配置文件中的数据点名拆出来，放进模型
            string[] pms = point.parameter.Split(',');
            point.Item_name = pms[2];

            ILE.LEResult res = new LEResult();
            try
            {
                HaiGrang.Package.OpcNetApiChs.DaNet.RefreshEventHandler reh = new HaiGrang.Package.OpcNetApiChs.DaNet.RefreshEventHandler(this.DataChangeHandler);
                myRefreshGroup = new RefreshGroup(server, 500, reh);

                mySyncIOGroup = new SyncIOGroup(this.server);

                int i = myRefreshGroup.Add(point.Item_name);
                if (!HRESULTS.Succeeded(i))
                {
                    res.Result = false;
                    res.ExtMessage = "新增监测点["+point.dataPoint_name+"]失败";
                    this.server.Disconnect();
                    this.server = null;
                    return res;
                }
                myRefreshGroup.Remove(point.Item_name);  //如果成功添加了，说明数据点可用，此时要拿出来，不能直接启动数据点。

                this.ItemPoints.Add(point);
                res.Result = true;
                
            }
            catch (Exception exc)
            {
                res.Result = false;
                res.ExtMessage = "新增监测点失败";
            }
            return res;
        }

        /// <summary>
        /// 数据接收到时需要启动的事件
        /// </summary>
        public event DataReceiveEvent OnDataReceiveEvent;
        /// <summary>
        /// 设备断线时触发
        /// </summary>
        public event DataReceiveEvent OnMachineDisconn;

        OpcServer server;           //服务对象
        SyncIOGroup mySyncIOGroup;  //操作对象
        RefreshGroup myRefreshGroup;  //监听对象
        Guid SrvGuid;
        string strMachine;
        System.Threading.Thread thread;

        public DCO(string pamater)
        {
            this.ItemPoints = new List<IDataPoint>();
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(Monitor));

            ILE.LEResult res = new LEResult();
            //用这个数据点的配置，启动OPC连接，正因如此，此处注意，无法支持一台设备存在两个OPC_server地址
            string[] pmts = pamater.Split(',');
            strMachine = pmts[0];   //服务PC的IP地址
            string strServerName = pmts[1]; //服务名
            string strPointName = pmts[2];  //数据点地址名

            OpcServerBrowser myBrowser = new OpcServerBrowser(strMachine);
            
            try
            {
                myBrowser.CLSIDFromProgID(strServerName, out SrvGuid);  //获取OPC服务组件的注册ID，获取不到会直接报错
            }
            catch
            {
                res.ExtMessage = "服务不存在或无法访问!";
                res.Result = false;
                //return res;
                throw new Exception("服务不存在或无法访问");
            }
            Host host = new Host(strMachine);

            server = new OpcServer();
            host.Domain = strMachine.ToUpper();
            host.HostName = strMachine;
            host.UserName = "";
            host.Password = "";
            int rtc = server.Connect(strMachine, SrvGuid);
            try
            {
                HRESULTS.Succeeded(rtc);
                //this.label1.Text = "连接成功!";
                //守护线程
            }
            catch (Exception exc)
            {
                res.Result = false;
                res.ExtMessage = exc.Message;
                this.server.Disconnect();
                this.server = null;
                throw new Exception("服务连接失败");
            }
        }

        

        /// <summary>
        /// 启动函数
        /// </summary>
        /// <returns></returns>
        public IResult init()
        {
            OPCItemState rslt;
            OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
            ILE.LEResult res = new LEResult();
            ///将所有数据点放进RefreshGroup容器
            foreach (ILE.IDataPoint po in this.ItemPoints)
            {
                if (po.business_code == "01" | po.business_code=="02")  //如果是采集完成数与异常数，要把初始值拿到用于扣减
                {
                    int j = mySyncIOGroup.Read(dsrc, po.Item_name, out rslt);
                    po.InitVal = rslt.DataValue.ToString();
                }

                int i = myRefreshGroup.Add(po.Item_name);
                if (!HRESULTS.Succeeded(i))
                {
                    res.Result = false;
                    res.ExtMessage = "新增监测点失败";
                    this.server.Disconnect();
                    this.server = null;
                    return res;
                }
            }

            //Action action = new Action(Monitor);
            //action.BeginInvoke(null, null);
            thread.Start();
            res.Result = true;
            return res;
        }

        /// <summary>
        /// 监听测点数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataChangeHandler(object sender, HaiGrang.Package.OpcNetApiChs.DaNet.RefreshEventArguments e)
        {
            try
            {
                for (int i = 0; i < e.items.Length; ++i)
                {
                    int hnd = e.items[i].OpcIDef.HandleClient;
                    object val = e.items[i].OpcIRslt.DataValue;
                    string qt = myRefreshGroup.GetQualityString(e.items[i].OpcIRslt.Quality);
                    DateTime dt = DateTime.FromFileTime(e.items[i].OpcIRslt.TimeStamp);

                    ItemDef item = myRefreshGroup.FindClientHandle(hnd);
                    string name = item.OpcIDef.ItemID;

                    IDataPoint p = (from points in ItemPoints
                                    where points.Item_name == name
                                    select points).LastOrDefault();

                    ///将结果封装到一个POINT对象中，触发事件

                    p.val = val.ToString();
                    OnDataReceiveEvent(p);
                }
            }
            catch (Exception exc)
            {
                
            }
        }

        public void Abort()
        {
            thread.Abort();
            myRefreshGroup.Dispose();
            server.Disconnect();
        }

        private void Monitor()
        {
            int i=0;
            var dc = this.ItemPoints.Where(x => x.business_code == "03").FirstOrDefault();
            if (dc == null) //没有配置状态点
                return;
            while (true)
            {
                try
                {
                    OPCItemState rslt;
                    OPCDATASOURCE dsrc = OPCDATASOURCE.OPC_DS_DEVICE;
                    int j = mySyncIOGroup.Read(dsrc, dc.Item_name, out rslt);
                }
                catch(Exception exc)
                {
                    dc.val = "-1";
                    OnDataReceiveEvent(dc);
                }
                System.Threading.Thread.Sleep(3000);
            }
        }
    }
}
