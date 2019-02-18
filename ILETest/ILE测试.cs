using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ILETest
{
    public partial class ILE测试 : Form
    {
        //计时器
        System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
        public ILE测试()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sw.Restart();
            sw.Start();
            try
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();

                textBox2.Text=client.RunServerAPI(txtClass.Text, txtFun.Text, textBox1.Text);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
            sw.Stop();
            lbTime.Text = sw.ElapsedMilliseconds.ToString();
     
        }

      
    }
}
