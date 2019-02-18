#region


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

#endregion
namespace LEMES_POD
{
    //internal class Script
    //{
        //[STAThread]
        //public static void Main(string[] args)
        //{
        //    var form = new MyForm();
        //    form.ShowDialog();
        //}

        public class MyForm : Form
        {
            private readonly BackgroundWorker _backgroundWorker;
            private readonly TextBox _replyInfoList;
            private Button button1;
            private readonly TextBox _textBox;


            public MyForm()
            {
                this.Text = "网络测试";
                this.Font = new Font("宋体", 13, FontStyle.Bold); 
                this.Size = new Size(698, 515); 
                this.StartPosition = FormStartPosition.CenterScreen;
                _textBox = new TextBox();
                //_textBox.Text = "172.25.77.10";
                _textBox.Text = "192.168.1.11";
                _textBox.Dock = DockStyle.Top;

                _replyInfoList = new TextBox();
                _replyInfoList.ReadOnly = true;
                _replyInfoList.Multiline = true;
                _replyInfoList.ScrollBars = ScrollBars.Both;
                _replyInfoList.Dock = DockStyle.Fill;

                var button = new Button();
                button.Text = "启动/停止";
                button.Dock = DockStyle.Bottom;

                Controls.Add(_replyInfoList);
                Controls.Add(_textBox);
                Controls.Add(button);



                _backgroundWorker = new BackgroundWorker();
                _backgroundWorker.WorkerSupportsCancellation = true;
                _backgroundWorker.DoWork += BackgroundWorkerOnDoWork;

                button.Click += ButtonOnClick;
            }

            private void BackgroundWorkerOnDoWork(object sender, DoWorkEventArgs e)
            {
                while (!_backgroundWorker.CancellationPending)
                {
                    Thread.Sleep(500);

                    var pingReply = Ping((string)e.Argument);
                    ShowPingReplyInfo(pingReply);
                }
            }

            public void ButtonOnClick(object sender, EventArgs eventArgs)
            {
                if (_backgroundWorker.IsBusy)
                    _backgroundWorker.CancelAsync();
                else
                    _backgroundWorker.RunWorkerAsync(_textBox.Text);
            }

            private static PingReply Ping(string address)
            {
                Ping ping = null;
                try
                {
                    ping = new Ping();

                    return ping.Send(address);
                }
                finally
                {
                    if (ping != null)
                    {
                        // 2.0 下ping 的一个bug，需要显示转型后释放
                        IDisposable disposable = ping;
                        disposable.Dispose();

                        ping.Dispose();
                    }
                }
            }

            private void ShowPingReplyInfo(PingReply pingReply)
            {
                try
                {
                    var pingReplyInfo = string.Format("");
                    if (pingReply.Address == null)
                    {
                        pingReplyInfo = string.Format("链接失败");
                        Debug.WriteLine(pingReplyInfo);
                    }
                    else
                    {
                        pingReplyInfo = string.Format("来自 {0} 的回复：字节={1} 时间={2} TTL={3}",
    pingReply.Address, pingReply.Buffer.Length, pingReply.RoundtripTime, pingReply.Options.Ttl);
                        Debug.WriteLine(pingReplyInfo);
                    }

                    MethodInvoker invoker = () =>
                    {
                        var infos = new List<string>(_replyInfoList.Lines);
                        infos.Insert(0, pingReplyInfo);
                        _replyInfoList.Lines = infos.ToArray();
                    };

                    if (_replyInfoList.InvokeRequired)
                        _replyInfoList.BeginInvoke(invoker);
                    else
                        invoker();
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }

            private void InitializeComponent()
            {
                this.button1 = new System.Windows.Forms.Button();
                this.SuspendLayout();
                // 
                // button1
                // 
                this.button1.Image = global::LEMES_POD.Properties.Resources.shopping_cart_32;
                this.button1.Location = new System.Drawing.Point(87, 12);
                this.button1.Name = "button1";
                this.button1.Size = new System.Drawing.Size(115, 86);
                this.button1.TabIndex = 0;
                this.button1.Text = "button1";
                this.button1.UseVisualStyleBackColor = true;
                // 
                // MyForm
                // 
                this.ClientSize = new System.Drawing.Size(624, 345);
                this.Controls.Add(this.button1);
                this.Name = "MyForm";
                this.ResumeLayout(false);

            }
        }
    //}
}