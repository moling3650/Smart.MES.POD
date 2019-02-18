using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RepairTool
{
    public partial class FrmLogin : Form
    {
        public bool g_bLoginResult = false;
        public string g_strFuncCode = "";
        public FrmLogin()
        {
            InitializeComponent();
            g_bLoginResult = false;
        }
        
        private void btn_ok_Click(object sender, EventArgs e)
        {
            g_bLoginResult = false;
            if(string.IsNullOrWhiteSpace(textBox_Userid.Text.ToString().Trim())
                || string.IsNullOrWhiteSpace(textBox_password.ToString().Trim()))
            {
                MessageBox.Show("请输入用户编号或密码!", "提示");
                return;
            }
            if (string.IsNullOrWhiteSpace(g_strFuncCode))
            {
                MessageBox.Show("工单代码为空!", "提示");
                return;
            }
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string str = client.RunServerAPI("BLL.Employee", "CheckLogin", textBox_Userid.Text.ToString().Trim() + ";" + textBox_password.Text.ToString().Trim());
            if (str.Contains("OK"))
            {
                //登录验证成功
                str = "";
                
                str = client.RunServerAPI("BLL.Employee", "CheckFuncCode", textBox_Userid.Text.ToString().Trim() + ";" + g_strFuncCode);
                if (str.Contains("OK"))
                {
                    //功能检查成功
                    g_bLoginResult = true;
                    Close();
                }
                else 
                {
                    g_bLoginResult = false;
                    MessageBox.Show(str, "错误");
                    return;
                }
            }
            else
            {
                g_bLoginResult = false;
                MessageBox.Show(str, "错误");
                return;
            }

        }

        private void textBox_password_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok_Click(null,null);
            }
        }
    }
}
