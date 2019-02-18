using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using ILE;
using ILE.Model;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using LEMES_POD.Voice;
using System.Threading;
using System.Reflection;
using LEMES_POD.Component;
using System.Timers;
using LEMES_POD.Tools;
using DevExpress;

namespace LEMES_POD
{
    public partial class Main : KryptonForm
    {
        Dictionary<String, String> SaveProcess = new Dictionary<string, string>(); //保存工位
        Dictionary<String, String> SaveState = new Dictionary<string, string>(); //保存工位
        string ip = ""; //IP地址
        string strTriger;
        public bool suspend = false;           //暂停工步
        //private LEDAO.B_ProcessList process;      //工序
        private LEDAO.V_ProcessList_Workshop process;      //工序
        private IJob job = new Job();      //工作流
        private B_Machine machine;        //当前工位对应的设备对象
        int time = 5000;     //响应时间
        Stopwatch sw = new Stopwatch();        //计时器
        Stopwatch sw1 = new Stopwatch();        //计时器

        int g_ilastpid = -1;
        public delegate void AsyncDlgt(bool bl);
        AsyncDlgt adlt;

        

        public Main()
        {
            InitializeComponent();
        }
        #region 初始化页面
        private void Main_Load(object sender, EventArgs e)
        {
            //版本号说明：主版本号、次版本号、内部版本号和修订号。主版本号和次版本号是必选的；
            //内部版本号和修订号是可选的，但是如果定义了修订号部分，则内部版本号就是必选的。所有定义的部分都必须是大于或等于 0 的整数
            //Thread to show splash window
            Thread splashUI = new Thread(new ThreadStart(ShowSplashWindow));
            splashUI.Name = "Splash UI";
            splashUI.Priority = ThreadPriority.Normal;
            splashUI.IsBackground = true;
            splashUI.Start();

            //Thread to load time-consuming resources.
            Thread resourceUI = new Thread(new ThreadStart(LoadResources));
            resourceUI.Name = "Resource Loader";
            resourceUI.Priority = ThreadPriority.Highest;
            resourceUI.Start();

            #region 初始化过程
            this.Text = "SMART_MES 制造执行系统 Ver:" + Assembly.GetExecutingAssembly().GetName().Version.ToString();  // 显示版本号
            LEDAO.LogClass.WriteLogFile(this.Text + " : 开始启动");
            kTextBox_CurrCount.Text = "";
            flowLayoutPanel2.Controls.Clear();//工位加载面板
            kptxtOrder.Text = "";   //工单
            kptxtQty.Text = "";   //数量
            kptxtCurrCount.Text = ""; //当前过站数量
            kptxtSFC.Text = "";  //批次
            tabControl1.SelectedIndex = 0; //tabControl选择项索引
            kryptonTextBox2.Text = ""; //员工号
            IPAddress[] ipAdds = Dns.Resolve(Dns.GetHostName()).AddressList;//获得当前IP地址
            this.kcb_ipadd.Items.Clear(); //清空ip列表

            adlt = AsyncJob;

            foreach (IPAddress ip in ipAdds)
            {
                this.kcb_ipadd.Items.Add(ip.ToString()); //添加ip到下拉框里面
            }
            if (kcb_ipadd.Items.Count > 0)
            {
                kcb_ipadd.SelectedIndex = 0;  //默认ip地址是第一个
            }

            #endregion

            resourceUI.Join();  //资源加载过程

            if (SplashForm != null)
            {
                SplashForm.Invoke(new MethodInvoker(delegate { SplashForm.Close(); }));
            }
            splashUI.Join();
        }
        #endregion

        public static SplashForm SplashForm
        {
            get;
            set;
        }

        private  void LoadResources()
        {
            Thread.Sleep(800);
            SplashForm.Invoke(new MethodInvoker(delegate { SplashForm.lblStatus.Text = "Done. " + DateTime.Now.ToString(); }));
        }

        private  void ShowSplashWindow()
        {
            SplashForm = new SplashForm();
            //Application.Run(SplashForm);
            SplashForm.ShowDialog();
        }

        /// <summary>
        /// 设备初始化
        /// </summary>
        /// <param name="Listmachine"></param>
        /// <param name="listStation"></param>
        public void initMachine(string station_code)
        {
            string Json = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineInfo", station_code);
            V_Station_Machine station_machine= JsonConvert.DeserializeObject<V_Station_Machine>(Json);
            if (station_machine != null)
            {
                //System.IO.MemoryStream ms = new System.IO.MemoryStream(station_machine.img);
                //System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
                //this.pb_machine.Image = img;
                if (station_machine.img != "" & station_machine.img != null)
                { 
                    ///此处加载服务器端图片
                }
                this.machine = new B_Machine();  //设备缓存赋值
                this.machine.machine_code = station_machine.machine_code;
                this.machine.machine_name = station_machine.machine_name;
                this.machine.state = station_machine.state;
                if(this.machine.state==-1)
                {
                    this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_white;
                }
                else if (this.machine.state == 0)
                {
                    this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_y;
                }
                else if (this.machine.state == 1)
                {
                    this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_green;
                }
                else if (this.machine.state == 2)
                {
                    this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_red;
                }
                this.lb_machine_code.Text = station_machine.machine_code;
                this.lb_machine_name.Text = station_machine.machine_name;
            }
            else
            {
                this.machine = null;
                this.flp_machine.Visible = false;  //如果没有设备，则右下角设备栏不显示
            }
        }

        /// <summary>
        /// 工装初始化
        /// </summary>
        /// <param name="Listmachine"></param>
        /// <param name="listStation"></param>
        public void initMachineMoulds()
        {
            if (job.reload_code != null)
            { 
                if(job.reload_code.IndexOf("mould/") < 0)
                    return;
            }
            this.flp_mould.Controls.Clear();
            if (this.lb_machine_code.Text == "")
            {
                return;   //如果设备编号为空，则不需要安装模具
            }
            ServiceReference.ServiceClient client = Tools.ServiceReferenceManager.GetClient();

            string mouldkinds = client.RunServerAPI("BLL.Machine", "GetMachineMouldKinds", this.lb_machine_code.Text);

            if (mouldkinds == "")
            {
                return;
            }

            List<dynamic> kinds = JsonConvert.DeserializeObject<List<dynamic>>(mouldkinds); //获取设备需要安装的全部磨具类型

            string ists = client.RunServerAPI("BLL.Machine", "GetMachineInstallMoulds", this.lb_machine_code.Text);
            List<dynamic> istMoulds = JsonConvert.DeserializeObject<List<dynamic>>(ists);  //获取设备已安装的全部磨具清单

            foreach (dynamic kind in kinds)
            { 
                Label lb = new Label();
                lb.AutoSize = true;
                lb.Location = new System.Drawing.Point(31, 8);
                lb.Margin = new System.Windows.Forms.Padding(6, 8, 3, 0);
                lb.Name = "lb_" + kind.model_code;
                lb.Size = new System.Drawing.Size(71, 12);
                lb.TabIndex = 3;
                lb.Text = kind.manufacturer+"-"+kind.model_code;
                this.flp_mould.Controls.Add(lb);

                if (istMoulds == null)
                {
                    lb.Text += "(0/" + kind.qty.ToString()+")";
                    continue;
                }
                var res = istMoulds.Where(x => x.kind_id == kind.mould_kind_id); //查询当前模具类型下安装的模具
                if (res.Count() == 0)
                {
                    lb.Text += "(0/" + kind.qty.ToString() + ")";
                    continue;
                }
                lb.Text += "("+res.Count().ToString()+"/" + kind.qty.ToString() + ")";
                foreach (dynamic a in res)
                {
                    this.flp_mould.Controls.Add(Tool.GetMouldPanel(a.mould_name.ToString(), a.mould_code.ToString()));
                }
            }
            if (job.reload_code != null) //处理完更新后，把更新码改掉，避免重复更新
            {
                job.reload_code=job.reload_code.Replace("mould/", "");
            }
            else
            {
                job.reload_code = "";  //第一次过后，要给他赋值，否则每次都会执行
            }
        }


        #region  通过ip获取工序
        private void kcb_ipadd_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                SaveProcess.Clear();
                this.kcb_process.Items.Clear();
                ip = kcb_ipadd.Text.ToString().Trim();//选择IP地址
                string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Process", "GetProcessList", ip); //通过ip获取工序
                if (string.IsNullOrEmpty(dt))
                {
                    BeginInvoke(new Action(() =>
                            {
                                Component.Tool.CtrlMsgPanel(tb_res, 3, "未获取到当前工序");  //当前IP找不到任何工序
                            }));
                    this.kl_msg.Text = "";  //清空提示,
                    this.ktb_input.Enabled = false; //按钮不能点击
                    return;
                }
                List<B_ProcessList> proc = JsonConvert.DeserializeObject<List<B_ProcessList>>(dt);    //工序信息
                sw.Restart(); sw.Start();
                foreach (var Process in proc)
                {
                    Tools.ComboboxItem item = new Tools.ComboboxItem();
                    item.Value = Process.process_code;
                    item.Text = Process.process_name;
                    this.kcb_process.Items.Add(item);
                    //SaveProcess.Add(Process.process_name, Process.process_code);//保存键值
                    //this.kcb_process.Items.Add(Process.process_name);
                    //SaveProcess.Add(Process.process_name, Process.process_code);//保存键值
                }
                this.kcb_process.SelectedIndex = this.kcb_process.Items.Count > 0 ? 0 : -1;
                this.kcb_station.SelectedIndex = this.kcb_station.Items.Count > 0 ? 0 : -1; ;  // 确定工位信息

                //string Station_code = "";
                //GetMachineInfo(ip, Station_code);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "错误");
            }
        }
        #endregion

        #region  选择工序
        private void kcb_process_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.job.RunState = false;
            System.Threading.Thread.Sleep(300);

            SaveState.Clear();//清空工位信息 
            this.kcb_station.Items.Clear();//清空下拉框
            string ProcessName = ((Tools.ComboboxItem)this.kcb_process.SelectedItem).Value.ToString();// 选中的工序名称
            if (string.IsNullOrEmpty(ProcessName))
            {

                BeginInvoke(new Action(() =>
                        {
                            Component.Tool.CtrlMsgPanel(tb_res, 3, "工序不能为空!");
                        }));
                return;
            }
            string processJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Process", "GetProcess", ProcessName); // 获得当前工序的信息
            this.process = JsonConvert.DeserializeObject<V_ProcessList_Workshop>(processJson); //通过工序获取工序的名称

            this.lb_orderModel.Text = process.task_mode == 0 ? "工单作业" : "派工单作业";
            this.lb_workModel.Text = process.production_mode == 0 ? "批次生产" : "连续生产";

            string cds = ip + "," + ProcessName;
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Station", "GetStationList", cds);//通过ip和工序获取工位信息
            List<B_StationList> stationList = JsonConvert.DeserializeObject<List<B_StationList>>(dt);
            if (stationList != null)
            {
                foreach (var Station in stationList)
                {
                    Tools.ComboboxItem item = new Tools.ComboboxItem();
                    item.Value = Station.station_code;
                    item.Text = Station.station_name;
                    this.kcb_station.Items.Add(item);
                    //this.kcb_station.Items.Add(Station.station_name);
                    SaveState.Add(Station.station_name, Station.station_code);//保存键值
                }
                this.kcb_station.SelectedIndex = this.kcb_station.Items.Count > 0 ? 0 : -1;
                //string ip1 = kcb_ipadd.Text.ToString().Trim();//选择IP地址
                string Station_code = "";
                //GetMachineInfo(ip, Station_code);
            }
            else
            {
                MessageBox.Show("该工序没有匹配工位", "提示");
                return;
            }
            //Timer();
            //切换工序，面板清空，切换到SOP页面
            flowLayoutPanel2.Controls.Clear();
            tabControl1.SelectedIndex = 0;
            
        }
        #endregion

        #region  选择工位信息
        private void kcb_station_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                GetJob(); //获得任务模型
                kbnt_stat1.Visible = false;
                suspend = false;
                StartJob();
                DataGridRef();
                //切换工序，面板清空，切换到SOP页面
                flowLayoutPanel2.Controls.Clear();
                tabControl1.SelectedIndex = 0;
                string ip1 = "";
                string Station_code = ((Tools.ComboboxItem)this.kcb_station.SelectedItem).Value.ToString();// 选中的工序名称
                //GetMachineInfo(ip1, Station_code);
                ///获取设备信息，并绑定到右下角
                initMachine(this.job.StationCode);
                initMachineMoulds();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        #endregion

        #region 获取工步
        private void GetJob()
        {
            BuffPid = 0;
            kryptonTextBox2.Text = ""; //员工编号
            this.job = null;
            this.job = Component.JobConstructor.GetJob(this.process); //获取工步
            this.job.StationCode = SaveState[this.kcb_station.Text.Trim()].ToString().Trim(); //工位编号
            this.job.ProcessCode = this.process.process_code;//工序编号
            if (this.job != null)
            {
                //获取到JOB之后,把指针复位.
                //Component.JobProcessor.InitJob(this.job);
            }
        }
        #endregion

        #region   JOB启动
        internal void StartJob()
        {
            if (job.StepList.Count <= 0) return;
            if (job.Completed)
            {   //
                if (process.production_mode == 0) //如果是按批次生产，则直接提交数据，不需要跳转到设备页
                {
                    BeginInvoke(new Action(() =>
                            {
                                Component.Tool.CtrlMsgPanel(tb_res, 1, "正在提交数据");
                            }));
                    bool b = Submit(); //提交数据,在函数里会把job初始化
                    if (!b)
                    {
                        return; 
                    }
                    else
                    { 
                        string strfilepath = Directory.GetCurrentDirectory() + "\\SFCBUFF\\";
                        string fname = strfilepath + job.OrderNO + "_" + job.SFC + ".txt";
                        FileInfo finfo = new FileInfo(fname);
                        if (finfo.Exists)
                        {
                            File.Delete(fname);
                        }
                    }
                }
                else //如果是连续生产，则跳转到设备页，不需要在此提交工艺数据
                {
                    if (this.job.DispatchNO == null)
                    {
                        Component.Tool.CtrlMsgPanel(tb_res, 3, "未获取到任务派工单，无法启动连续生产!");
                        //开始初始化连续生产面板
                        return;
                    }
                    else
                    {
                        BeginInvoke(new Action(() =>
                        {
                            this.tabControl1.SelectedIndex = 2;
                            MachinePageInit(ktb_Dispatch.Text);
                            //MachinePointInit();
                        }));
                        return;
                    }
                }

            }
            this.kl_msg.Text = job.StepList[job.StepIdx].StepName; //获得工步的名字
            strTriger = job.StepList[job.StepIdx].Triger; //弹窗

            if (job.StepList[job.StepIdx].AutoRun == 1) //是否允许自动
            {
                BeginInvoke(new Action(() =>
                        {
                            this.ktb_input.Enabled = false; //禁止控件对用户做出交互式响应
                        }));
                if (suspend)  //暂停工步
                {
                    BeginInvoke(new Action(() =>
                        {
                            kbnt_stat1.Visible = true;
                            kbnt_Click.Visible = false;
                            Component.Tool.CtrlMsgPanel(tb_res, 1, "暂停");
                        }));
                    return;
                }

                BeginInvoke(new Action(() =>
                        {
                            Component.Tool.CtrlMsgPanel(tb_res, 1, "等待输入" + job.StepList[job.StepIdx].StepName);
                        }));
                adlt.BeginInvoke(true, null, null);
                //AsyncJob(true); //采用异步执行
            }
            else
            {
                BeginInvoke(new Action(() =>
                        {
                            Component.Tool.CtrlMsgPanel(tb_res, 1, "正在采集" + job.StepList[job.StepIdx].StepName);
                            this.ktb_input.Enabled = true;
                        }));
            }

            //工步按钮事件功能
            if (string.IsNullOrEmpty(strTriger) || strTriger.Split(',').Length != 3)
            {
                BeginInvoke(new Action(() =>
                {
                    kbnt_Click.Visible = false;
                }));
            }
            else
            {
                BeginInvoke(new Action(() =>
                        {
                            kbnt_Click.Visible = true;
                            kbnt_Click.Text = strTriger.Split(',')[0];
                        }));
                if (strTriger.Split(',')[2] == "1")
                {
                    kbnt_Click_Click_1(null, null); //调用按钮方法
                }
            }
            BeginInvoke(new Action(() =>
                        {
                            this.ktb_input.Focus();
                        }));
        }
        #endregion

        #region 文本输入
        private void ktb_input_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            //ktb_input.Enabled = false;
            if (this.job == null) return;
            ktb_input.Text = ktb_input.Text.Replace("#", "").ToUpper();
            ktb_input.Text = ktb_input.Text.ToString().Trim(',');
            ktb_input.Text = ktb_input.Text.ToString().Trim('，');
            ktb_input.Text = ktb_input.Text.ToString().Trim('.');
            ktb_input.Text = ktb_input.Text.ToString().Trim('。');
            ktb_input.Text = ktb_input.Text.ToString().Trim();

            string input = ktb_input.Text.Replace("#", "").ToUpper();
            //if (string.IsNullOrEmpty(input))
            //{
            //    BeginInvoke(new Action(() =>
            //                {
            //                    Voice.voice.ShowVoice("ng.wav");
            //                    Component.Tool.CtrlMsgPanel(tb_res, 3, "文本框不能为空");
            //                }));
            //    //ktb_input.Enabled = true;
            //    return;
            //}
            #region  正则表达式
            if (!string.IsNullOrEmpty(job.StepList[job.StepIdx].Format))
            {
                //输入的文本匹配正则表达式
                Regex reg = new Regex(job.StepList[job.StepIdx].Format);
                Match ma = reg.Match(input);
                if (!ma.Success)
                {
                    BeginInvoke(new Action(() =>
                            {
                                Component.Tool.CtrlMsgPanel(tb_res, 3, "文本格式错误,正则表达式: " + job.StepList[job.StepIdx].Format + "");
                            }));
                    ktb_input.Text = "";
                    //ktb_input.Enabled = true;
                    return;
                }
            }
            #endregion
            sw.Reset();//停止时间间隔测量，并将运行时间重置为零。
            sw.Start(); //开始或继续测量某个时间间隔的运行时间。
            adlt.BeginInvoke(false, null, null);
            //AsyncJob(false); //异步执行job 
        }
        #endregion

        ManualResetEventSlim mre1 = new ManualResetEventSlim(false);
        ManualResetEventSlim mre3 = new ManualResetEventSlim(false);
        // public bool isLoadcache = false;
        int BuffPid = 0;
        #region  异步跑job ADLT
        private void AsyncJob(bool autoRun)
        {
            string input = null;
            if (!autoRun) { input = ktb_input.Text; } //获得文本框的值
            Func<IJob, string, IResult> fun = Component.JobProcessor.HandleJob; //动态调用dll
            BeginInvoke(new Action(() =>
                            {
                                this.picOKImg.Visible = false;
                            }));
            IAsyncResult asyncResult = fun.BeginInvoke(this.job, input, AsyncCallBack, this.job);
            this.job.RunState = true;
            int idx = job.StepIdx;
            if (this.job.StepList[idx].TimeOut == null)
            {
                this.job.StepList[idx].TimeOut = "-1";
            }
            //else
            //{
            //    this.job.StepList[idx].TimeOut = (Convert.ToInt32(this.job.StepList[idx].TimeOut) * 1000).ToString(); //工步超时
            //}

            if (asyncResult.AsyncWaitHandle.WaitOne(int.Parse(this.job.StepList[idx].TimeOut), true))
            {
                IResult result = fun.EndInvoke(asyncResult);
                if (idx != job.StepIdx)
                {
                    return;
                }
                if (!result.Result)  //工步执行返回false
                {
                    BeginInvoke(new Action(() =>
                            {
                                Voice.voice.ShowVoice("ng.wav");
                                Component.Tool.CtrlMsgPanel(tb_res, 3, result.ExtMessage);
                                if (this.job.StepList[idx].AutoRun == 1)
                                {
                                    kbnt_stat1.Visible = true;
                                }
                            }));
                    return;
                }
                else
                {
                    if (job.StepList[job.StepIdx].KeyStep && job.StepList[job.StepIdx].Completed)
                    {
                        #region 显示ESOP，且切换tab
                        if (job.Pid != 0)
                        {
                            string strurl = ConfigurationSettings.AppSettings["UrlESOP"] + "?Pid=" + job.Pid.ToString();
                            Uri u = new Uri(@strurl);
                            BeginInvoke(new Action(() =>
                            {
                                webBrowser1.Url = u;
                            }));
                        }
                        BeginInvoke(new Action(() =>
                            {
                                tabControl1.SelectedIndex = 1;
                            }));
                        #endregion

                        #region 获取缓存
                        ////获取缓存文件路径
                        //string strfilepath = Directory.GetCurrentDirectory() + "\\SFCBUFF\\";
                        //string fname = strfilepath + job.OrderNO + "_" + job.SFC + ".txt";
                        //FileInfo finfo = new FileInfo(fname);
                        //if (finfo.Exists)
                        //{
                        //    //加载缓存文件全部内容并赋值给字符串变量
                        //    string jobJson = System.IO.File.ReadAllText(fname);
                        //    job = JsonConvert.DeserializeObject<Job>(jobJson);
                        //    //isLoadcache = true;
                        //}
                        #endregion

                        #region 加载后工步
                        DataJobStep(job);
                        #endregion

                        int stp = job.StepIdx;
                        BeginInvoke(new Action(() =>
                            {
                                DataGridRef();
                                this.button2.Enabled = true;
                                //button2_Click(null, null);
                                iscache = true;
                                this.button2.Text = "缓存中...";
                                //this.button2.BackColor = Color.Red;
                            }));
                        if (this.lb_machine_code.Text != "")
                        {
                            AddMachineOrder(this.lb_machine_code.Text);
                        }
                        AddStationOrder();
                    }

                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    int nowIdx = job.StepIdx;
                    int i = Component.JobProcessor.NextStep(this.job); //返回工步索引加1

                    if (i == job.StepIdx) //说明当前的工步已经是最后可执行的工步了
                    {
                        this.job.Completed = true;
                        CustomControl.StepPanel panel2 = null;
                        for (int index = job.IndexBack; index < i; index++)
                        {
                            CustomControl.StepPanel panel3 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[index - job.IndexBack];
                            panel3.FunCompleted();
                        }
                        for (int id = nowIdx; id < job.StepList.Count; id++)
                        {
                            if (job.StepIdx - job.IndexBack >= 0)
                            {
                                panel2 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[id - job.IndexBack];
                                mre3.Reset();
                                BeginInvoke(new Action(() =>
                                {
                                    if (panel2 != null)
                                        panel2.FunCompletedAll(job.StepList[id].StepValue, job.StepList[id].StepCode, job, id, this); //后面工序全部都是重复利用的情况下，不弹出后工步的子工步
                                    mre3.Set();
                                }));
                                mre3.Wait();
                                if (iscache)
                                {
                                    //Job写入缓存文件
                                    string Jobjson = JsonConvert.SerializeObject(job);
                                    LEDAO.LogClass.WriteCacheFile(Jobjson, job.SFC, job.OrderNO);
                                }
                            }
                        }
                    }
                    else
                    {
                        job.StepIdx = i;
                        if (job.StepIdx >= job.IndexBack && job.IndexBack > 0) //表示是后工步
                        {
                            //JobProcessor.JumpJob(job);
                            CustomControl.StepPanel panel1 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack];
                            BeginInvoke(new Action(() =>
                            {
                                panel1.FunWait();
                            }));
                            if (job.StepIdx != job.IndexBack)
                            {
                                for (int index = job.IndexBack; index < i; index++)
                                {
                                    CustomControl.StepPanel panel3 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[index - job.IndexBack];
                                    panel3.FunCompleted();
                                }
                                #region
                                //for (int j = 1; j <= i - nowIdx; j++)
                                //{
                                //    if (job.StepList[job.StepIdx - j].Completed)
                                //    {
                                //        if (job.StepList[job.StepIdx - j].StepDetail != null)
                                //        {
                                //            if (job.StepIdx - job.IndexBack - j >= 0)
                                //            {
                                //                CustomControl.StepPanel panel2 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack - j];
                                //                mre1.Reset();
                                //                BeginInvoke(new Action(() =>
                                //                {
                                //                    panel2.FunCompleted(job.StepList[job.StepIdx - j].StepValue, job.StepList[job.StepIdx - j].StepCode, job, job.StepIdx - j, this);
                                //                    mre1.Set();
                                //                }));
                                //                mre1.Wait();
                                //            }
                                //        }
                                //        if (iscache)
                                //        {
                                //            //Job写入缓存文件
                                //            string Jobjson = JsonConvert.SerializeObject(job);
                                //            LEDAO.LogClass.WriteCacheFile(Jobjson, job.SFC, job.OrderNO);
                                //        }
                                //    }
                                //}
                                #endregion

                                for (int j = 0; j < i - nowIdx; j++)
                                {
                                    if (job.StepList[nowIdx + j].Completed)
                                    {
                                        if (job.StepList[nowIdx + j].StepDetail != null)
                                        {
                                            if (nowIdx + j > job.IndexBack - 1)
                                            {
                                                CustomControl.StepPanel panel2 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[nowIdx + j - job.IndexBack];
                                                mre1.Reset();
                                                BeginInvoke(new Action(() =>
                                                {
                                                    panel2.FunCompleted(job.StepList[nowIdx + j].StepValue, job.StepList[nowIdx + j].StepCode, job, nowIdx + j, this, nowIdx);
                                                    mre1.Set();
                                                }));
                                                mre1.Wait();
                                            }
                                        }
                                        if (iscache)
                                        {
                                            //Job写入缓存文件
                                            string Jobjson = JsonConvert.SerializeObject(job);
                                            LEDAO.LogClass.WriteCacheFile(Jobjson, job.SFC, job.OrderNO);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    BeginInvoke(new Action(() =>
                    {
                        int t = i - job.IndexBack;
                        try
                        {
                            flowLayoutPanel2.ScrollControlIntoView(flowLayoutPanel2.Controls[t + 1]);
                        }
                        catch
                        { }
                    }));

                    /////////////////////////////////////////////////////////////////////////////////////
                    Voice.voice.ShowVoice("pass.wav");
                    Thread.Sleep(200);
                    StartJob();
                }
            }
            else
            {
                job.RunState = false;
                System.Threading.Thread.Sleep(200);
                BeginInvoke(new Action(() =>
                                        {
                                            tb_res.BackColor = Color.Red;
                                            tb_res.Text = "任务处理超时";
                                            kbnt_stat1.Visible = true;
                                        }));
                Voice.voice.ShowVoice("ng.wav");
                if (job.StepList[job.StepIdx].AutoRestart == 1)
                {
                    StartJob();
                    return;
                }
            }
        }
        #endregion

        #region  异步处理回调
        private void AsyncCallBack(IAsyncResult result)
        {
            sw.Stop(); //停止计时
            BeginInvoke(new Action(() =>
            {

                this.ktb_input.Text = ""; //清空文本
                //this.ktb_input.Enabled = true;
                this.lbt.Text = sw.ElapsedMilliseconds.ToString(); // 获取当前实例测量得出的总运行时间（以毫秒为单位）。
                this.kptxtOrder.Text = job.OrderNO; //当前的工单号
                this.ktb_Dispatch.Text = job.DispatchNO;
                this.kptxtSFC.Text = job.SFC;  //当前的批次
                this.kptxtQty.Text = job.QTY.ToString();//批次的数量
                //20161015,xlf:如果为关键步骤，需要查询当前工单已经完成了多少sfc
                {
                    if (job.StepList[job.StepIdx].DriveName == "DP002") //输入工单，清理上一个工单数量信息
                    {
                        kTextBox_CurrCount.Text = "";   // 清空上一个工单的已过站信息
                    }
                    if ((job.StepList[job.StepIdx].KeyStep || job.StepList[job.StepIdx].DriveName == "DP002")  //输入工单，或者为关键工步都需要获取工单已做sfc数量信息
                        && !string.IsNullOrWhiteSpace(job.OrderNO)
                        && !string.IsNullOrWhiteSpace(job.ProcessCode)
                        && !string.IsNullOrWhiteSpace(job.StationCode))
                    {
                        string strpar = job.OrderNO + "," + job.ProcessCode + ",00";
                        string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WorkOrder", "GetSFCCountByOrder", strpar); //通过工单，工序，工位获取sfc过站记录
                        if (string.IsNullOrWhiteSpace(dt))
                        {
                            kTextBox_CurrCount.Text = "0";
                        }
                        else
                        {
                            List<P_SFC_Process_IOLog> proc = JsonConvert.DeserializeObject<List<P_SFC_Process_IOLog>>(dt);    //sfc过站记录
                            if (proc == null)
                            {
                                kTextBox_CurrCount.Text = "0";
                            }
                            else
                            {
                                int icount = 0;
                                for (int i = 0; i < proc.Count; i++)
                                {
                                    icount += (int)proc[i].qty;
                                }
                                kTextBox_CurrCount.Text = icount.ToString();
                                //kTextBox_CurrCount.Text = proc.Count.ToString();
                            }
                        }
                    }
                }
                if (string.IsNullOrWhiteSpace(kTextBox_CurrCount.Text.ToString()))
                {
                    this.kptxtCurrCount.Text = "0/" + job.MaxQTYOrder.ToString("#0"); //工单下产品数量数量(取整)
                }
                else
                {
                    try
                    {
                        this.kptxtCurrCount.Text = kTextBox_CurrCount.Text + "/" + job.MaxQTYOrder.ToString("#0"); //当前已过站数量 / 工单下产品数量(取整)
                        int icurrcount = int.Parse(kTextBox_CurrCount.Text);
                        int iMaxqtyorder = int.Parse(job.MaxQTYOrder.ToString("#0"));
                        if (icurrcount >= iMaxqtyorder
                            && iMaxqtyorder != 0)
                        {
                            Voice.voice.ShowVoice("process.wav");
                        }
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message.ToString());
                    }
                }

                this.kryptonTextBox2.Text = job.EmpCode; //当前的员工
                initMachineMoulds();
                if (job.Pid > 0)
                {
                    if (g_ilastpid != job.Pid)
                    {
                        string strurl = ConfigurationSettings.AppSettings["UrlESOP"] + "?Pid=" + job.Pid;
                        Uri u = new Uri(strurl);
                        webBrowser1.Url = u;
                        g_ilastpid = job.Pid;
                    }
                }
            }));
            //
        }
        #endregion

        #region  投料管理
        private void buttonSpecHeaderGroup3_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.job.EmpCode))
            {
                MessageBox.Show("请输入员工号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            (new FeedForm(this.kcb_ipadd.Text, job.ProcessCode, job.StationCode, job.EmpCode, job.OrderNO)).ShowDialog(this);
            DataGridRef();
        }
        #endregion

        #region  物料信息刷新
        public void DataGridRef()
        {
            string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetWIPState", this.job.StationCode + "," + this.process.process_code); //通过工站编号去找物料信息
            kryptonDataGridView1.DataSource = JsonConvert.DeserializeObject<List<LEDAO.V_WIP_Seed>>(str).Select(c => new { c.lot_no, c.lot_qty, mat = c.p_name + "/" + c.mat_code, c.order_no }).ToList();
        }
        #endregion

        #region  工步刷新
        ManualResetEventSlim mre2 = new ManualResetEventSlim(false);
        public void DataJobStep(IJob step)
        {
            if (BuffPid != step.Pid)
            {
                flowLayoutPanel2.Controls.Clear(); //清空工步面板
                for (int i = step.IndexBack; i < step.StepList.Count; i++)
                {
                    mre2.Reset();
                    BeginInvoke(new Action(() =>
                    {
                        CustomControl.StepPanel control = new CustomControl.StepPanel(step.StepList[i], this);
                        this.flowLayoutPanel2.Controls.Add(control);
                        BuffPid = step.Pid;
                        mre2.Set();
                    }));
                    mre2.Wait();
                }
            }
            else
            {
                foreach (Control ctl in flowLayoutPanel2.Controls)
                {
                    mre2.Reset();
                    BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            flowLayoutPanel2.ScrollControlIntoView(flowLayoutPanel2.Controls[0]);
                        }
                        catch
                        { }
                        CustomControl.StepPanel control = (CustomControl.StepPanel)ctl;
                        control.FunInit();
                        mre2.Set();
                    }));
                    mre2.Wait();
                }
            }


        }
        #endregion

        #region 关闭子工步
        //关闭子工步
        public void CloseSonStep()
        {
            int i = Component.JobProcessor.NextStep(this.job); //返回工步索引加1
            if (job.StepIdx - job.IndexBack > -1 && job.IndexBack != 0)
            {
                if (job.StepIdx - job.IndexBack - 1 > -1)
                {
                    
                    if (i == job.StepIdx) //说明当前的工步已经是最后可执行的工步了
                    {
                        CustomControl.StepPanel panel3 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack];
                        panel3.FunWait();
                    }
                    else
                    {
                        CustomControl.StepPanel panel2 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack - 1];
                        panel2.FunWait();
                        CustomControl.StepPanel panel1 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack];
                        panel1.FunStop();
                    }
                }
            }
            job.StepList[job.StepIdx].Completed = false;
            if (job.StepIdx > 0)
            {
                if (i == job.StepIdx)
                {
                    job.StepIdx = job.StepIdx;
                }
                else
                {
                    job.StepIdx = job.StepIdx - 1;
                }
            }
            if (job.StepList[job.StepIdx].AutoRun == 1)
            {
                this.job.RunState = false;
                suspend = true;
                //kbnt_stat.Visible = true;
            }
            else
            {
                suspend = false;
                kbnt_stat1.Visible = false;
            }
            job.RunState = false;
            job.Completed = false;
            i = Component.JobProcessor.NextStep(this.job);
            if (iscache && job.StepIdx > job.IndexBack)
            {
                //Job写入缓存文件
                string Jobjson = JsonConvert.SerializeObject(job);
                LEDAO.LogClass.WriteCacheFile(Jobjson, job.SFC, job.OrderNO);
            }
            // iscache = false;
            System.Threading.Thread.Sleep(200);
            StartJob();
        }
        #endregion

        #region 后退按钮
        public void buttonSpecHeaderGroup6_Click_1(object sender, EventArgs e)
        {
            if (job.StepIdx - job.IndexBack > -1 && job.IndexBack != 0)
            {
                if (job.StepIdx - job.IndexBack - 1 > -1)
                {
                    CustomControl.StepPanel panel2 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack - 1];
                    panel2.FunWait();
                }
                CustomControl.StepPanel panel1 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack];
                panel1.FunStop();
            }
            job.StepList[job.StepIdx].Completed = false;
            if (job.StepIdx > 0)
            {
                job.StepIdx = job.StepIdx - 1;
            }
            if (job.StepList[job.StepIdx].AutoRun == 1)
            {
                this.job.RunState = false;
                suspend = true;
                //kbnt_stat.Visible = true;
            }
            else
            {
                suspend = false;
                kbnt_stat1.Visible = false;
            }
            job.RunState = false;
            job.Completed = false;
            if (iscache && job.StepIdx > job.IndexBack)
            {
                //Job写入缓存文件
                string Jobjson = JsonConvert.SerializeObject(job);
                LEDAO.LogClass.WriteCacheFile(Jobjson, job.SFC, job.OrderNO);
            }
            // iscache = false;
            System.Threading.Thread.Sleep(600);
            StartJob();
        }
        #endregion

        #region  点击按钮
        private void kbnt_Click_Click_1(object sender, EventArgs e)
        {
            ILE.IFormProperty pro = Tools.DynamicPopUp.PopUpLoad(job, null);
            if (!pro.Cancel)
            {
                ktb_input.Text = pro.Val;
                sw.Reset();
                sw.Start();
                adlt.BeginInvoke(false, null, null);
                //AsyncJob(false);
            }
        }
        #endregion

        #region   后退工步
        private void kryptonButton2_Click_1(object sender, EventArgs e)
        {
            if (job.StepIdx - job.IndexBack > -1 && job.IndexBack != 0)
            {
                if (job.StepIdx - job.IndexBack - 1 > -1)
                {
                    CustomControl.StepPanel panel2 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack - 1];
                    panel2.FunWait();

                }
                CustomControl.StepPanel panel1 = (CustomControl.StepPanel)flowLayoutPanel2.Controls[job.StepIdx - job.IndexBack];
                panel1.FunStop();
            }
            job.StepList[job.StepIdx].Completed = false;
            if (job.StepIdx > 0)
            {
                job.StepIdx = job.StepIdx - 1;
            }

            if (job.StepList[job.StepIdx].AutoRun == 1)
            {
                this.job.RunState = false;
                suspend = true;
                //kbnt_stat.Visible = true;
            }
            else
            {
                suspend = false;
                kbnt_stat1.Visible = false;
            }
            job.RunState = false;
            job.Completed = false;
            if (iscache && job.StepIdx > job.IndexBack)
            {
                //Job写入缓存文件
                string Jobjson = JsonConvert.SerializeObject(job);
                LEDAO.LogClass.WriteCacheFile(Jobjson, job.SFC, job.OrderNO);
            }
            //iscache = false;
            System.Threading.Thread.Sleep(600);
            StartJob();
        }
        #endregion

        #region 关闭按钮
        private void buttonSpecHeaderGroup1_Click_1(object sender, EventArgs e)
        {
            //isCloseClient = true;
            //buttonSpecHeaderGroup8_Click(sender, e);
            //Application.Exit();
            this.Close();
        }
        #endregion

        #region  提交数据
        private bool Submit()
        {
            this.job.Completed = false;
            sw.Reset();//停止时间测量,将运行时间重置为零
            sw.Start();
            Func<IJob, IResult> fun = Component.JobSubmit.JobUpload;
            IAsyncResult asyncResult = fun.BeginInvoke(this.job, AsyncCallBack, this.job);

            //阻止当前线程，直到当前的 System.Threading.WaitHandle 收到信号为止，同时使用 32 位带符号整数测量时间间隔，并指定是否在等待之前退出同步域。
            if (asyncResult.AsyncWaitHandle.WaitOne(15000, true))
            {
                IResult result = fun.EndInvoke(asyncResult);
                if (!result.Result)
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        Component.Tool.CtrlMsgPanel(tb_res, 3, result.ExtMessage);
                    }));

                }
                else
                {
                    this.job.Completed = false;
                    Component.JobProcessor.InitJob(this.job);
                    BeginInvoke(new Action(() =>
                    {
                        this.picOKImg.Visible = true;
                        DataGridRef();
                    }));
                }
                return result.Result;
            }
            else
            {
                BeginInvoke(new Action(() =>
                           {
                               tb_res.BackColor = Color.Red;
                               tb_res.Text = "任务处理超时";
                           }));
                return false;
            }
        }
        #endregion

        #region   重新刷新页面
        private void hb_flash_Click_1(object sender, EventArgs e)
        {
            Main_Load(null, null); //重新加载页面
        }
        #endregion

        #region  暂停后启动按钮
        private void kbnt_stat1_Click(object sender, EventArgs e)
        {
            suspend = false;
            adlt.BeginInvoke(true, null, null);
            //AsyncJob(true);
            kbnt_stat1.Visible = false;
            //iscache = false;
        }
        #endregion

        #region 测试
        //测试
        private void button1_Click(object sender, EventArgs e)
        {
            sw.Restart();
            sw.Start();
            try
            {
                //ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Employee", "GetStaff", "10000");
                //List<S_Employee> emp ;

                //emp = JsonConvert.DeserializeObject<List<S_Employee>>(dt);
                S_Employee emp = JsonConvert.DeserializeObject<S_Employee>(dt);
                this.Text = emp.emp_name;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            sw.Stop();
            BeginInvoke(new Action(() =>
                           {
                               tb_res.Text = sw.ElapsedMilliseconds.ToString();
                           }));
        }
        #endregion

        #region 点击生产指令按钮
        //点击选择工单
        private void button_workeOrder_Click(object sender, EventArgs e)
        {
            if (job.EmpCode != null)
            {
                string ProcessCode = job.ProcessCode;
                string flow_code = job.FlowCode;
                (new WorkerOrderFrom(this, ProcessCode, flow_code)).ShowDialog(this);
            }
            else
            {
                MessageBox.Show("请输入员工号", "提示");
                return;
            }
        }
        #endregion

        #region Closing事件
        //关闭事件，无论系统关闭，电脑关闭，都会执行此事件
        // bool isCloseClient;
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            //isCloseClient = true;
            //buttonSpecHeaderGroup8_Click(sender, e);
        }
        #endregion

        #region  网络测试
        //网络测试
        private void button_pingtest_Click(object sender, EventArgs e)
        {
            var form = new MyForm();
            form.ButtonOnClick(null, null);
            form.ShowDialog();
        }
        #endregion

        #region 维修
        //维修
        private void button_RepairTool_Click(object sender, EventArgs e)
        {
            if (job == null
                || string.IsNullOrWhiteSpace(job.EmpCode)
                || string.IsNullOrWhiteSpace(job.workshop))
            {
                MessageBox.Show("请先登录!", "错误");
                return;
            }

            var form = new FrmRepairTool();

            //检查用户权限
            FrmLogin frmlogin = new FrmLogin();
            frmlogin.g_strFuncCode = form.Name;
            frmlogin.ShowDialog();
            if (!frmlogin.g_bLoginResult)
            {
                MessageBox.Show("验证用户权限失败", "错误");
                return;
            }

            form.g_strWorkShopCode = job.workshop;
            form.g_stremp = job.EmpCode;
            form.ShowDialog();
        }
        #endregion

        #region 补料管理
        //补料管理
        private void fedbatchM_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.job.EmpCode))
            {
                MessageBox.Show("请输入员工号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(this.job.OrderNO))
            {
                MessageBox.Show("请输入工单号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            (new UserForm.FedBatchManage(job, this)).ShowDialog();
        }
        #endregion

        #region 输入工具编号
        //输入工具编号
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;


        }
        #endregion

        #region 切换工具选项卡时获取焦点
        //切换工具选项卡时获取焦点
        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl2.SelectedTab == tabPage4)
            {
                //textBox1.Focus();
                //textBox1.TabIndex = 0;
            }
        }
        #endregion

        #region 物料刷新
        //刷新物料
        private void buttonSpecHeaderGroup10_Click(object sender, EventArgs e)
        {
            DataGridRef();
        }
        #endregion

        #region 缓存
        //缓存
        bool iscache = false;
        private void button2_Click(object sender, EventArgs e)
        {
            if (!iscache)
            {
                iscache = true;
                this.button2.Text = "缓存中...";
                //this.button2.BackColor = Color.Red;
            }
            else
            {
                iscache = false;
                this.button2.Text = "缓存";
                this.button2.BackColor = Color.WhiteSmoke;
            }
        }
        #endregion

        #region 添加设备正在进行的工单
        //添加设备正在进行的工单
        public void AddMachineOrder(string machine_code)
        {
            string strOrder = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WorkOrder", "GetOrderByID", job.OrderNO);
            List<P_WorkOrder> ListOrder = JsonConvert.DeserializeObject<List<P_WorkOrder>>(strOrder);
            if (machine_code != null)
            {
                P_Order_Machine Ordermachine = new P_Order_Machine
                {
                    order_no = job.OrderNO,
                    father_order = ListOrder[0].parent_order,
                    main_order = ListOrder[0].main_order,
                    sfc = job.SFC,
                    product_code = job.Product,
                    machine_code =machine_code,
                    inputdate = DateTime.Now,
                    input_time = DateTime.Now,
                    emp_code=job.EmpCode,
                    diispatching_no=job.DispatchNO
                };
                string strJson=JsonConvert.SerializeObject(Ordermachine);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineOrder", strJson);

            }
        }
        #endregion

        #region 添加工位正在进行的工单
        //添加工位正在进行的工单
        private void AddStationOrder()
        {
            string strOrder = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WorkOrder", "GetOrderByID", job.OrderNO);
            List<P_WorkOrder> ListOrder = JsonConvert.DeserializeObject<List<P_WorkOrder>>(strOrder);
            P_Order_Station p_station = new P_Order_Station
            {
                order_no = job.OrderNO,
                parent_order = ListOrder[0].parent_order,
                main_order = ListOrder[0].main_order,
                sfc = job.SFC,
                station_code = job.StationCode,
                emp_code = job.EmpCode,
                input_time = DateTime.Now
            };
            string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Station", "GetStationOrder", job.StationCode);
            List<P_Order_Station> ListStationOrder = JsonConvert.DeserializeObject<List<P_Order_Station>>(str);
            if (ListStationOrder != null)
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Station", "DeleteStationOrder", job.StationCode);
            }
            string strJson = JsonToolsNet.ObjectToJson(p_station);
            Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Station", "AddStationOrder", strJson);
        }
        #endregion

        ///========================================================设备集成相关====================================================================================

        ILE.Model.StandardDataPoint[] sdp = null;  //标准点位缓存
        ILE.Model.AnalogDataPoint[] adp = null;    //模拟点位缓存

        List<ILE.IDCO> BUS_DCOList = new List<IDCO>(); //用于总线式采集的DCO驱动，全部放这里，
        List<ILE.IDCO> OWN_DCOList = new List<IDCO>(); //用于独立式采集的DCO驱动，全部放这里，

        private string MachinePageResult;

        #region 设备页初始化
        //
        private void MachinePageInit(string DispatchNo)
        {
            string entityDsp = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WorkDispatching", "GetOrderByNO", DispatchNo); //通过派工单编号获取派工单
            P_WorkDispatching dsp = JsonConvert.DeserializeObject<P_WorkDispatching>(entityDsp);    

            job.InitCpltQty = dsp.cplt_qty.Value; //把派工单的已完成数缓存到JOB，用于后续部分计算
            job.InitNGQty = dsp.ng_qty.Value;     //把派工单的已不良数缓存到JOB，用于后续部分计算

            string entityPdct = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetproductInfoByCode", dsp.product_code); //通过成品编号获取成品
            B_Product pdct = JsonConvert.DeserializeObject<B_Product>(entityPdct);    //sfc过站记录

            this.lb_mo.Text = dsp.dispatching_no;
            this.lb_plan.Text = dsp.qty.ToString();
            this.lb_product.Text = pdct.product_name;

            this.lb_cplt.Text = dsp.cplt_qty.ToString();
            this.lb_cpltPct.Text = (dsp.cplt_qty.Value/dsp.qty.Value * 100).ToString("F2") + "%";
            int n = (int)(dsp.cplt_qty.Value / dsp.qty.Value * 100 > 100 ? 100 : dsp.cplt_qty.Value / dsp.qty.Value * 100);
            this.pbc_cplt.Text = ((int)(dsp.cplt_qty.Value / dsp.qty.Value * 100)).ToString();

            this.lb_ng.Text = job.NGQty.ToString();
            this.lb_ngPct.Text = "0%";
            this.pbc_ng.Text = "0";
            if (dsp.cplt_qty.Value == 0)
            {
                this.lb_ngPctAll.Text = "0%";
            }
            else
            {
                this.lb_ngPctAll.Text = (dsp.ng_qty.Value / dsp.cplt_qty.Value * 100).ToString("F2") + "%";
            }

            
            //Tool.GetChart("温度",12,12);

            

        }
        #endregion

        #region 数据点点初始化
        private void MachinePointInit()
        {
            try
            {
                string standarPoints = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.DataPoint", "GetStandardPoint", this.machine.machine_code); //通过派工单编号获取派工单
                sdp = JsonConvert.DeserializeObject<ILE.Model.StandardDataPoint[]>(standarPoints);

                string analogPoints = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.DataPoint", "GetAnalogPoint", this.machine.machine_code); //通过派工单编号获取派工单
                adp = JsonConvert.DeserializeObject<ILE.Model.AnalogDataPoint[]>(analogPoints);
            }
            catch (Exception exc)
            { }

            this.MachinePageResult = "";
            this.lvBind.Items.Clear();
            this.treeList1.Nodes[0].Nodes.Clear();
            this.treeList1.Nodes[1].Nodes.Clear();
            this.BUS_DCOList.Clear();
            ILE.IResult rec = new LEResult();
            //根据采集模式，创建公共的（BUS）采集驱动,和独立的（OWN）采集驱动  标准量
            foreach (ILE.IDataPoint DP in sdp)
            {
                this.treeList1.AppendNode(new object[] { DP.dataPoint_name, DP.dc_drive_code }, this.treeList1.Nodes[0]);
                DP.counter = 0;
                if (DP.dc_type == 0)  //如果是总线式采集
                {
                    var dc = BUS_DCOList.Where(x => x.Driver_code == DP.dc_drive_code).FirstOrDefault(); //在LIST中查询，如果没有
                    if (dc == null)
                    {
                        rec = DriveFactory.GetDCO(DP.dc_drive_code,DP.parameter); //如果没有创建，则创建一个，否则跳过
                        IDCO dco = (IDCO)rec.obj;
                        dco.OnDataReceiveEvent += new DataReceiveEvent(dco_OnDataReceiveEvent);
                        rec= dco.AddItem(DP);

                        BUS_DCOList.Add(dco);
                    }
                    else
                    {
                        rec=dc.AddItem(DP);
                    }
                }
                else  //如果是独立采集
                {
                    rec = DriveFactory.GetDCO(DP.dc_drive_code,DP.parameter); //如果没有创建，则创建一个，否则跳过
                    IDCO dco = (IDCO)rec.obj;
                    dco.OnDataReceiveEvent += new DataReceiveEvent(dco_OnDataReceiveEvent);
                    rec=dco.AddItem(DP);
                    OWN_DCOList.Add(dco);
                }

                if (rec!=null & !rec.Result)  //如果添加数据点到驱动中失败,把数据点标识为断开
                {
                    this.MachinePageResult += rec.ExtMessage+"\r\n";
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node1 in treeList1.Nodes[0].Nodes)
                    {
                        if (node1[0] == DP.dataPoint_name)
                        {
                            node1.ImageIndex = 1;
                            break;
                        }
                    }
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node1 in treeList1.Nodes[1].Nodes)
                    {
                        if (node1[0] == DP.dataPoint_name)
                        {
                            node1.ImageIndex = 1;
                            break;
                        }
                    }
                }
            }

            //根据采集模式，创建公共的（BUS）采集驱动,和独立的（OWN）采集驱动  模拟量
            foreach (ILE.IDataPoint DP in adp)
            {
                this.treeList1.AppendNode(new object[] { DP.dataPoint_name, DP.dc_drive_code }, this.treeList1.Nodes[1]);
                DP.counter = 0;
                if (DP.dc_type == 0)  //如果是总线式采集
                {
                    var dc = BUS_DCOList.Where(x => x.Driver_code == DP.dc_drive_code).FirstOrDefault(); //在LIST中查询，如果没有
                    if (dc == null)
                    {
                        rec = DriveFactory.GetDCO(DP.dc_drive_code,DP.parameter); //如果没有创建，则创建一个，否则跳过
                        IDCO dco = (IDCO)rec.obj;
                        dco.OnDataReceiveEvent += new DataReceiveEvent(dco_OnDataReceiveEvent);
                        rec=dco.AddItem(DP);
                        BUS_DCOList.Add(dco);
                    }
                    else
                    {
                        rec=dc.AddItem(DP);
                    }
                }
                else
                {
                    rec = DriveFactory.GetDCO(DP.dc_drive_code,DP.parameter); //如果没有创建，则创建一个，否则跳过
                    IDCO dco = (IDCO)rec.obj;
                    dco.OnDataReceiveEvent += new DataReceiveEvent(dco_OnDataReceiveEvent);
                    rec=dco.AddItem(DP);
                    OWN_DCOList.Add(dco);
                }

                if (rec != null & !rec.Result)  //如果添加数据点到驱动中失败,把数据点标识为断开
                {
                    this.MachinePageResult += rec.ExtMessage+"\r\n";
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node1 in treeList1.Nodes[0].Nodes)
                    {
                        if (node1[0].ToString() == DP.dataPoint_name)
                        {
                            node1.ImageIndex = 1;
                            break;
                        }
                    }
                    foreach (DevExpress.XtraTreeList.Nodes.TreeListNode node1 in treeList1.Nodes[1].Nodes)
                    {
                        if (node1[0].ToString() == DP.dataPoint_name)
                        {
                            node1.ImageIndex = 1;
                            break;
                        }
                    }
                }
            }

            this.treeList1.ExpandAll();

            ///给listView和chart进行初始化
            int i = 0;
            foreach (AnalogDataPoint dp in adp)
            {
                ListViewItem item = new ListViewItem(dp.dataPoint_name);
                item.SubItems.Add(dp.lcl.ToString());
                item.SubItems.Add(dp.ucl.ToString());
                item.SubItems.Add("-");
                item.SubItems.Add("OK");
                lvBind.Items.Add(item);

                if (dp.to_monitor == 1)
                {
                    this.xtraTabControl1.TabPages[i].PageVisible = true;
                    this.xtraTabControl1.TabPages[i].Text = "  " + dp.dataPoint_name + "  ";
                    this.xtraTabControl1.TabPages[i].Name = dp.point_id.ToString();

                    int j = this.xtraTabControl1.TabPages[0].Controls.Count;
                    DevExpress.XtraCharts.ChartControl chart = ((DevExpress.XtraCharts.ChartControl)this.xtraTabControl1.TabPages[0].Controls[0]);
                    //chart.Series[0].Name = dp.dataPoint_name;
                    try
                    {
                        DevExpress.XtraCharts.XYDiagram xyDiagram1 = (DevExpress.XtraCharts.XYDiagram)chart.Diagram;
                        xyDiagram1.AxisY.ConstantLines[0].AxisValue = dp.ucl;
                        xyDiagram1.AxisY.ConstantLines[1].AxisValue = dp.lcl;
                    }
                    catch (Exception exc)
                    { }
                }
                i++;
            }

            ///目前如果有任何数据点无法启动，则不允许继续执行
            if (MachinePageResult != "")
            {
                this.flp_machinePageResult.Visible = true;
                this.lb_machinePageResult.Text = MachinePageResult;
                this.kb_start.Values.Image = global::LEMES_POD.Properties.Resources.resultset_next;
                this.kb_start.Text = "启动";
                return;
            }

            this.flp_machinePageResult.Visible = false;
            this.kb_start.Values.Image = global::LEMES_POD.Properties.Resources.pause_blue;
            this.kb_start.Text = "停止";

            foreach (ILE.IDCO dco in BUS_DCOList)  //启动总线驱动列表
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(LocationStart), dco);
            }

            foreach (ILE.IDCO dco in OWN_DCOList)  //启动独占驱动列表
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(LocationStart), dco);
            }
        }
        #endregion

        /// <summary>
        /// 此处开始执行数据驱动
        /// </summary>
        /// <param name="dco"></param>
        private void LocationStart(object dco)
        {
            ILE.IDCO location = (ILE.IDCO)dco;
            IResult rec=location.init();
        }

        #region 接收设备数据返回
        //添加设备正在进行的工单
        private void dco_OnDataReceiveEvent(IDataPoint dp)
        {
            if (dp.business_code != null)
            {
                try
                {
                    switch (dp.business_code)
                    {
                        case "01":   ///完成数
                            business001((StandardDataPoint)dp);
                            break;
                        case "02":   ///异常数
                            business002((StandardDataPoint)dp);
                            break;
                        case "03":   ///设备状态
                            string ss = dp.val;
                            business003((StandardDataPoint)dp);
                            break;
                        case "00":
                            AnalogQTYReceived((AnalogDataPoint)dp);  //00数据点为模拟量，只能接收AnalogDataPoint
                            break;
                    }
                }
                catch (Exception exc)
                { 
                    
                }
            }
            else
            {
                AnalogQTYReceived((AnalogDataPoint)dp);  //00数据点为模拟量，只能接收AnalogDataPoint
            }
        }


        /// <summary>
        /// 完成数的数据点处理业务
        /// </summary>
        /// <param name="dp"></param>
        private void business001(StandardDataPoint idp)
        {
            StandardDataPoint dp = (StandardDataPoint)idp;   //此处默认只能使用标准数据点,做转换
                this.BeginInvoke(
                         new Action(() =>
                                   {
                                        try
                                        {
                                            if (dp.InitVal == null)
                                                dp.InitVal = "0";
                                            if (dp.LastPoint == null)  //如果没有上一数据点，说明是刚启动
                                            {
                                                dp.TaskSum = decimal.Parse(dp.val)/5 - decimal.Parse(dp.InitVal)/5;
                                                
                                            }
                                            else
                                            {
                                                dp.TaskSum += decimal.Parse(dp.val)/5 - decimal.Parse(dp.LastPoint.val)/5;
                                            }
                                            
                                            if (dp.LastPoint == null)
                                            {
                                                this.lb_cplt.Text = (float.Parse(lb_cplt.Text) + float.Parse(dp.val) / 5 - float.Parse(dp.InitVal) / 5).ToString();
                                            }
                                            else
                                            {
                                                this.lb_cplt.Text = (float.Parse(lb_cplt.Text) + float.Parse(dp.val) / 5 - float.Parse(dp.LastPoint.val) / 5).ToString();
                                            }

                                            StandardDataPoint ldp = new StandardDataPoint();
                                            ldp.val = dp.val;
                                            dp.LastPoint = ldp;

                                            int n = (int)(float.Parse(lb_cplt.Text) / float.Parse(lb_plan.Text) * 100);
                                            this.lb_cpltPct.Text = (float.Parse(lb_cplt.Text) / float.Parse(lb_plan.Text) * 100 > 100 ? 100 : n).ToString()+"%";
                                            this.pbc_cplt.Text = (float.Parse(lb_cplt.Text) / float.Parse(lb_plan.Text) * 100 > 100 ? 100 : n).ToString();
                                            dp.AddPointVal(dp.val, DateTime.Now);  //将当前l值加入缓存中
                                            if (dp.TaskVals.Count > 5)  //默认只缓存5组数据
                                                dp.TaskVals.RemoveAt(0);
                                            dp.counter++;
                                            
                                            //如果满足了任务驱动的触发条件，会在这里触发任务，一般是报工
                                            if (dp.task_drive_code != null & dp.trigger_condition > 0 & dp.counter == dp.trigger_condition) //触发任务驱动的条件
                                            {
                                                decimal qty=dp.TaskSum;
                                                lock (dp) //重新赋值的时候不允许操作DP对象
                                                {
                                                    dp.TaskSum = 0;
                                                }
                                                ILE.IResult rec = DriveFactory.GetTPO(dp.task_drive_code, dp.task_parameter); //如果没有创建，则创建一个，否则跳过
                                                ITPO dco = (ITPO)rec.obj;
                                                //decimal qty= dp.TaskVals.Sum(x=>decimal.Parse(x.Va));
                                                
                                                rec= dco.DoWork(this.job, qty.ToString());

                                                dp.counter = 0;
                                            }
                                        }
                                        catch (Exception exc)
                                        {
                                            dp.counter = 0;
                                        }
                                   })
                         );
            
        }

        private void business002(StandardDataPoint dp)
        {
            this.BeginInvoke(
                    new Action(() =>
                    {
                        //this.lb_ng.Text = (float.Parse(dp.val) - float.Parse(dp.InitVal)).ToString();
                    })
                );
        }

        private void business003(StandardDataPoint dp)
        {
            int state = 0;
            try
            {
                state = bool.Parse(dp.val) ? 1 : 0;
            }
            catch
            {
                state = int.Parse(dp.val);
            }
            if (this.machine.state != state)
            {
                string jsonData = dp.machine_code + "," + state.ToString();
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineState", jsonData); //通过派工单编号获取派工单
                this.machine.state = state;
                this.BeginInvoke(
                    new Action(() =>
                    {
                        if (this.machine.state == -1)       //关机断电
                        {
                            this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_white;
                        }
                        else if (this.machine.state == 0)   //停机待机
                        {
                            this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_y;
                        }
                        else if (this.machine.state == 1)  //运行
                        {
                            //将设备状态履历表改成运行状态
                            P_Machine_State_Record record = new P_Machine_State_Record();
                            record.machine_code = this.machine.machine_code;
                            record.be_current = 1;
                            record.state = 1;
                            record.start_time = DateTime.Now;
                            //AddMachineStateRecord
                            string json = JsonConvert.SerializeObject(record);
                            string StopreasonJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineStateRecord", json);
                            this.pictureBox3.Image = global::LEMES_POD.Properties.Resources.ball_green;
                        }
                    })
                );
                if (state == 0 | state==-1)
                {
                    (new UserForm.MachineOFF(machine.machine_code, state, null, job.EmpCode, false)).ShowDialog();
                    //UserForm.MachineOFF mo=new UserForm.MachineOFF(
                }
                else if (state == -1 & machine.state == 1)
                { 
                    //设备从运转直接关机，这种情况可能是停电，一般不需要特殊处理
                }
            }
        }

        /// <summary>
        /// 模拟量处理
        /// </summary>
        /// <param name="dp"></param>
        private void AnalogQTYReceived(AnalogDataPoint dp)
        {
            dp.AddPointVal(dp.val, DateTime.Now);
            if (dp.PointVals.Count > dp.group_count) //如果缓存的数据超过了组距，移除第一个
            {
                dp.PointVals.RemoveAt(0);
            }

            this.BeginInvoke(new Action(() =>
            {
                try
                {
                    for (int i = 0; i < lvBind.Items.Count; ++i)
                    {
                        if (lvBind.Items[i].Text == dp.dataPoint_name)
                        {
                            lvBind.Items[i].SubItems[3].Text = dp.val;
                            if (checkEdit1.Checked)  //如果启动了监听，才接收数据
                            {
                                DevExpress.XtraCharts.SeriesPoint seriesPoint = new DevExpress.XtraCharts.SeriesPoint(DateTime.Now, new object[] {
                                ((object)(dp.val))});

                                var res = adp.Where(x => x.dataPoint_name == dp.dataPoint_name).FirstOrDefault();
                                if (res == null)
                                    break;
                                ///此处通过point_name获取tabpage

                                for (int n = 0; n < 6; n++)  //此处无法使用foreach,原因不明
                                {
                                    if (this.xtraTabControl1.TabPages[n].Text.Trim() == dp.dataPoint_name)
                                    {
                                        DevExpress.XtraCharts.ChartControl chart = ((DevExpress.XtraCharts.ChartControl)this.xtraTabControl1.TabPages[n].Controls[0]);
                                        if (chart.Series[0].Points.Count() > res.group_count)
                                        {
                                            chart.Series[0].Points.RemoveAt(0);
                                        }
                                        chart.Series[0].Points.Add(seriesPoint);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception exc)
                { }
                })
            );
        }
        #endregion

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.tabControl1.SelectedIndex = 1;
            
        }

        private void kb_start_Click(object sender, EventArgs e)
        {
            if (kb_start.Text == "启动")
            {
                if (this.ktb_Dispatch.Text == "")
                {
                    this.lb_machinePageResult.Text = "未启动任何派工单";
                    return;
                }
                //this.MachinePageInit(this.ktb_Dispatch.Text);
                this.MachinePointInit();  //函数内会修改按钮状态
            }
            else
            {
                for (int i = 0; i < BUS_DCOList.Count;i++)
                {
                    BUS_DCOList[i].Abort();
                    BUS_DCOList.RemoveAt(0);
                }
                this.kb_start.Text = "启动";
                this.kb_start.Values.Image = global::LEMES_POD.Properties.Resources.resultset_next;
            }
            //if()
            
        }


    }
}
