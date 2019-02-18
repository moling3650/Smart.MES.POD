using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace LEMES_POD.UserForm
{
    public partial class CheckPassWord : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public CheckPassWord()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            string Pass = string.Empty;
            Pass = System.Configuration.ConfigurationSettings.AppSettings["Pass"].ToString();
            
            if (Pass == tb1.Text.ToString().Trim())
            {
                this.Close();
            }
            else
            {
                DialogResult result = KryptonMessageBox.Show("密码输入不正确", "警告", MessageBoxButtons.OK);
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
            }
        }
    }
}
 