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
    public partial class ReturnMateralNumForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        decimal _qty;
        public ReturnMateralNumForm(string qty)
        {
            InitializeComponent();
            textBox1.Focus();
            _qty = Convert.ToDecimal(qty);
        }
        /// <summary>
        /// ����
        /// </summary>
        public decimal Num { get; set; }
        /// <summary>
        /// ״̬
        /// </summary>
        public int State { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            NumOK();
            State = 1;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if ("1234567890.".IndexOf(e.KeyChar) != -1
            //    && e.KeyChar != (char)8) 
            //{
            //    e.Handled = true;
            //}
            //State = 1;

        }

        private void NumOK()
        {

            decimal number = 0;
            if (!decimal.TryParse(textBox1.Text, out number))
            {
                MessageBox.Show("��ʽ���ԣ�", "��ʾ");
                return;
            }
            Num = decimal.Parse(textBox1.Text);
            if (Num > _qty)
            {
                MessageBox.Show("ж���������ܴ���Ͷ��������", "��ʾ");
                return;
            }
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
            this.DialogResult = DialogResult.OK;
        }
    }
}