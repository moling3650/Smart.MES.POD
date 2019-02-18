using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace NV_SNP
{
    public partial class PrintConfirm : Form
    {
        public int planNum;
        public int planSN;
        public int num;

        public PrintConfirm(int pnum,int psn)
        {
            InitializeComponent();
            this.planNum = pnum;
            this.planSN = psn;
            
        }

        private void PrintConfirm_Load(object sender, EventArgs e)
        {
            this.ktb_planNum.Text = this.planNum.ToString();
            this.ktb_planSN.Text = this.planSN.ToString();
            this.ktb_num.Text = this.planSN.ToString();
        }

        private void ktb_num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
            base.OnKeyPress(e); 
        }

        private void ktb_ent_Click(object sender, EventArgs e)
        {
            if (ktb_num.Text.Trim() =="")
            {
                MessageBox.Show("请填入确认打印到SN,如果没有打印请点击打印失败");
            }
            if (int.Parse(ktb_num.Text) > int.Parse(ktb_planSN.Text))
            {
                ktb_num.Text = ktb_planSN.Text;
            }

            this.num = int.Parse(ktb_num.Text);
            this.Close();
        }

        private void ktb_cansel_Click(object sender, EventArgs e)
        {
            this.num = 0;
            this.Close();
        }

        
    }
}
