using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LEDAO;
using Newtonsoft.Json;
using LEMES_POD;

namespace LEMES_POD.UserForm
{
    public partial class CloseJude : Form
    {
        bool iscloseClient = true;
        public CloseJude(string sfc, List<P_SFC_Jude> ListJude, decimal qty)
        {
            InitializeComponent();
            InitFrom(sfc, ListJude, qty);
        }
        //初始化页面
        public void InitFrom(string sfc, List<P_SFC_Jude> ListJude, decimal qty)
        {
            this.textBox1.Text = sfc;
            this.dataGridView1.DataSource = ListJude;
            this.textBox2.Text = qty.ToString();
            string gradeJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SFC", "GetSfcGrade", sfc);
            List<V_SFC_Grade> listGrade = JsonConvert.DeserializeObject<List<V_SFC_Grade>>(gradeJson);
            if (listGrade != null)
            {
                foreach (var grade in listGrade)
                {
                    Tools.ComboboxItem item = new Tools.ComboboxItem();
                    item.Value = grade.grade_code;
                    item.Text = grade.grade_name;
                    this.comboBox1.Items.Add(item);
                }
                this.comboBox1.SelectedIndex = this.comboBox1.Items.Count > 0 ? 0 : -1;
            }
        }
        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            string SFC = textBox1.Text.ToString();
            if (radioButton1.Checked == true)
            {
                //string gradecode = comboBox1.SelectedValue.ToString();
                string gradecode = ((Tools.ComboboxItem)this.comboBox1.SelectedItem).Value.ToString();// 选中的级别
                //根据批次更改P_sfc_State的grade_id
                try
                {
                    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SFC", "UpdateSFCGrade", SFC + "," + gradecode);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("降级失败", "提示");
                    return;
                }
            }
            if (radioButton2.Checked == true)
            {
                //取消待判，直接删除
                try
                {
                    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SFC", "DeleteJude", SFC);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("取消待判失败","提示");
                    return;
                }
            }
            iscloseClient = true;
            this.Close();
        }

        //关闭窗体
        private void CloseJude_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!iscloseClient)
            {
                //LEMES_POD.Main.buttonSpecHeaderGroup6_Click_1(sender, e);
            }
        }
        //选中
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = true;
        }

    }
}
