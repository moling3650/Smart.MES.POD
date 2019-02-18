using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Threading;

namespace LEMES_POD.UserForm
{
    public partial class Downloadpackage : Form
    {

        string[] filepath;
        string filename = string.Empty;
        public Downloadpackage()
        {
            string targetLoginName = "administrator";
            string targetPassword = "88888888";
            string targetIP = "192.168.1.12";
            using (BLL.IdentityScope iss = new BLL.IdentityScope(targetLoginName, targetPassword, targetIP))
            {
                filepath = Directory.GetFiles("\\\\192.168.1.12\\力朗共享文件\\POD安装包");
                filename = Path.GetFileName(filepath[0]);
            }
            InitializeComponent();
            label4.Visible = false;
            this.textBox1.Text = filename;
        }
        //下载
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请选择下载地址", "提示");
                return;
            }
            try
            {
                string targetLoginName = "administrator";
                string targetPassword = "88888888";
                string targetIP = "192.168.1.12";
                using (BLL.IdentityScope iss = new BLL.IdentityScope(targetLoginName, targetPassword, targetIP))
                {
                    filepath = Directory.GetFiles("\\\\192.168.1.12\\力朗共享文件\\POD安装包");
                    filename = Path.GetFileName(filepath[0]);
                    System.Net.WebClient myWebClient = new System.Net.WebClient();
                    BeginInvoke(new Action(() =>
                    {
                        myWebClient.DownloadFile(filepath[0], textBox2.Text + "\\" + filename);
                    }));
                    //下载进度条
                    string path = filepath[0];
                    float percent = 0;
                    System.Net.FileWebRequest Myrq = (System.Net.FileWebRequest)System.Net.FileWebRequest.Create(path);
                    System.Net.FileWebResponse myrp = (System.Net.FileWebResponse)Myrq.GetResponse();
                    long totalBytes = myrp.ContentLength;
                    if (prog != null)
                    {
                        prog.Maximum = (int)totalBytes;
                    }
                    if (prog != null)
                    {
                        prog.Maximum = (int)totalBytes;
                    }
                    System.IO.Stream st = myrp.GetResponseStream();
                    System.IO.Stream so = new System.IO.FileStream(filename, System.IO.FileMode.Create);
                    long totalDownloadedByte = 0;
                    byte[] by = new byte[1024];
                    int osize = st.Read(by, 0, (int)by.Length);
                    while (osize > 0)
                    {
                        totalDownloadedByte = osize + totalDownloadedByte;
                        System.Windows.Forms.Application.DoEvents();
                        so.Write(by, 0, osize);
                        if (prog != null)
                        {
                            prog.Value = (int)totalDownloadedByte;
                        }
                        osize = st.Read(by, 0, (int)by.Length);
                        percent = (float)totalDownloadedByte / (float)totalBytes * 100;
                        label4.Visible = true;
                        label4.Text = "下载进度：" + percent.ToString("#0.00") + "%";
                        System.Windows.Forms.Application.DoEvents(); //必须加注这句代码，否则label4将因为循环执行太快而来不及显示信息
                    }
                    so.Close();
                    st.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "警告");
            }
            this.Close();
            MessageBox.Show("下载成功，请安装", "提示");

        }
        //选择下载位置
        private void button1_Click(object sender, EventArgs e)
        {
            System.Threading.Thread t = new
            System.Threading.Thread(new System.Threading.ThreadStart(SelectPosition));
            // 解决单线程无法调用选择文件对话框问题
            t.SetApartmentState(System.Threading.ApartmentState.STA);
            t.Start();
        }
        public void SelectPosition()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择一个文件夹";
            dialog.ShowNewFolderButton = false;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BeginInvoke(new Action(() =>
                {
                    this.textBox2.Text = dialog.SelectedPath;
                }));

            }
        }
    }
}
