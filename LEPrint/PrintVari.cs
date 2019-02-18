using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NV_SNP
{
    public partial class PrintVari : Form
    {
        private string _rule;
        public string result;
        public string _P_name;
        public string main_order;
        public PrintVari(string rule, string P_Name,string _main_order)
        {
            main_order = _main_order;
            _P_name = P_Name;
            this._rule = rule;
            InitializeComponent();
        }

        private void PrintVari_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            if (_P_name != "")
            {
                kryptonLabel1.Visible = true;
                kryptonLabel1.Text = "当前打印【" + _P_name + "】条码，请录入";
            }
            List<string> list = GetVariList(_rule);

            foreach (string str in list)
            {
                this.flowLayoutPanel1.Controls.Add(GetVariPanel(str));
            }
            
        }

        List<string> GetVariList(string rule)
        {
            List<string> vlist=new List<string>();
            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] == '(')
                {
                    for (int j = ++i; j < rule.Length; j++)
                    {
                        if (rule[j] == ')')
                        { 
                            vlist.Add(rule.Substring(i,j-i));
                            i = j;
                            break;
                        }
                    }
                }
            }
            return vlist;
        }

        Control GetVariPanel(string variName)
        {
            // 
            // label1
            // 
            Label label1 = new Label();
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("宋体", 12F);
            label1.Location = new System.Drawing.Point(17, 11);
            label1.Name = "label1";
            label1.Text = variName;
            label1.Size = new System.Drawing.Size(56, 16);
            label1.TabIndex = 0;
            // 
            // textBox1
            // 
            TextBox textBox1 = new TextBox();
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox1.Font = new System.Drawing.Font("宋体", 12F);
            textBox1.Location = new System.Drawing.Point(89, 6);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(150, 26);
            textBox1.TabIndex = 1;

            textBox1.Text = main_order;

            Panel panel1 = new Panel();
            panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label1);
            panel1.Location = new System.Drawing.Point(3, 3);
            panel1.Name = variName;
            panel1.Size = new System.Drawing.Size(264, 40);
            panel1.TabIndex = 0;

            return panel1;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            { 
                string vari=ctrl.Controls["textBox1"].Text;
                if (vari == "")
                {
                    MessageBox.Show("请赋值","提示");
                    return;
                }
                _rule=_rule.Replace("(" + ctrl.Name + ")", vari);
            }
            this.result = _rule;
            this.Close();
        }
    }
}
