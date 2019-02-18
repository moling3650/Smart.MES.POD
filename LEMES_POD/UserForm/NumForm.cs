using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using LEDAO;

namespace LEMES_POD.UserForm
{
    public partial class NumForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public NumForm()
        {
            InitializeComponent();
            textBox1.Focus();
        }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Num { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int State { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            NumOK();
            State = 1;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

                NumOK();
                State = 1;
            }
        }

        private void NumOK()
        {
            decimal number = 0;
            if (!decimal.TryParse(textBox1.Text, out number))
            {
                MessageBox.Show("格式不对");
                return;
            }
            Num = decimal.Parse(textBox1.Text);
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            State = 2;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            State = 0;
            this.Close();
        }
        //关闭
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        public static int judestate { get; set; }
        //单纯的弹出框体（待判码）
        //public static void BombBox(string sfc, List<P_SFC_Jude> ListJude)
        //{
        //    //(new UserForm.JudePrompt(sfc, ListJude)).ShowDialog();
        //    UserForm.JudePrompt Jude = new UserForm.JudePrompt(sfc, ListJude);
        //    Jude.ShowDialog();
        //    if (Jude.State == 0)
        //    {
        //        CloseBox(sfc, ListJude);
        //        judestate = 0;
        //    }
        //    else
        //    {
        //        judestate = 1;
        //    }
        //}
        //public static void CloseBox(string sfc, List<P_SFC_Jude> ListJude)
        //{
        //    UserForm.JudePrompt jude = new UserForm.JudePrompt(sfc, ListJude);
        //    jude.Close();
        //}

        public static void BombBox_CloseJude(string sfc, List<P_SFC_Jude> ListJude,decimal qty)
        {
            UserForm.CloseJude Jude = new UserForm.CloseJude(sfc, ListJude,qty);
            Jude.ShowDialog();
            //if (Jude.State == 0)
            //{
            //    CloseBox(sfc, ListJude);
            //    judestate = 0;
            //}
            //else
            //{
            //    judestate = 1;
            //}
        }
    }
}