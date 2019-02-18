using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using ILE;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.IO;
using LEMES_LEOQC.Tools;
namespace LEMES_LEOQC
{
    public partial class Main : KryptonForm
    {
        public Main()
        {
            InitializeComponent();
        }
        //删除批次
        private void kbtn_del_Click(object sender, EventArgs e)
        {
            if (kckl_lot.SelectedItems.Count <= 0)
            {
                MessageBox.Show("请选中后再试", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            kckl_lot.Items.Remove(kckl_lot.SelectedItem);
            for (int i = 0; i < kckl_lot.Items.Count; i++)//默认选中
            {
                kckl_lot.SetItemChecked(i, true);
            }
        }
        //检验水准
        private void ktl_total_TextChanged(object sender, EventArgs e)
        {
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetOQCType", "asd");
            List<B_OQC_Type> proc = JsonConvert.DeserializeObject<List<B_OQC_Type>>(dt);
            kryptonComboBox2.ValueMember = "oqc_type_id";
            kryptonComboBox2.DisplayMember = "oqc_type_item";
            kryptonComboBox2.DataSource = proc;
        }
        //水准下属等级和抽样等级
        private void kryptonComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string num = ktl_total.Text;
            string tpid = kryptonComboBox2.SelectedValue.ToString();
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetStandardCode", num + "," + tpid);
            List<V_OQC_Index_Detail> oqcList = JsonConvert.DeserializeObject<List<V_OQC_Index_Detail>>(dt);
            kryptonComboBox1.ValueMember = "oqcid";  //水准下属等级
            kryptonComboBox1.DisplayMember = "item_char";
            kryptonComboBox1.DataSource = oqcList;
            kcb_Level.ValueMember = "id";     //抽样等级
            kcb_Level.DisplayMember = "idx_type";
            kcb_Level.DataSource = oqcList;
        }
        //生成抽检按钮
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            if (kcb_order.Text == "" || ktl_total.Text == "0")
            {
                MessageBox.Show("请将信息填写完整后再试", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string oqcorder = "Q" + DateTime.Now.ToString("yyyyMMddHHmm") + kcb_order.Text;//动态生成检验单据号
            B_OQC_Order oqc_order = new B_OQC_Order()
            {
                order_no = kcb_order.Text,
                qty = Convert.ToDecimal(ktl_total.Text),
                type_id = Convert.ToInt32(kryptonComboBox2.SelectedValue),
                oqc_id = Convert.ToInt32(kryptonComboBox1.SelectedValue),
                idx_id = Convert.ToInt32(kcb_Level.SelectedValue),
                state = 2,
                input_time = DateTime.Now,
                oqc_order_no = oqcorder,
                oqc_result = null,
                end_time = null
            };
            try
            {
                string strJson = JsonToolsNet.ObjectToJson(oqc_order);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "CreateOQC_Order", strJson);
            }
            catch (Exception)
            {
                MessageBox.Show("生成失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                throw;
            }

            MessageBox.Show("生成成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            string num = ktl_total.Text;
            string tpid = kryptonComboBox2.SelectedValue.ToString();
            string id = kcb_Level.SelectedValue.ToString();
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetIndex", num + "," + tpid + "," + id);
            List<V_OQC_Index_Detail> oqcList = JsonConvert.DeserializeObject<List<V_OQC_Index_Detail>>(dt);
            string NGValue = oqcList.FirstOrDefault().idx_value.ToString();
            string oqcno = oqcorder;
            string orderNo = kcb_order.Text;
            new OQCWin(NGValue, oqcno, orderNo).Show();
            this.Hide();
        }
        //显示批次数量
        private void kckl_lot_Leave(object sender, EventArgs e)
        {
            int total = 0, sum = 0;
            foreach (var tmp in kckl_lot.CheckedItems)
            {
                string chemsg = tmp.ToString();
                int i = chemsg.IndexOf(":");
                int t = chemsg.Length - i - 5;
                total = Convert.ToInt32(chemsg.Substring(i + 1, t));
                sum += total;
            }
            ktl_total.Text = sum.ToString();
        }
        private void kcb_order_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                ktb_lotNo.Focus();
            }
        }
        //输完批次空格键添加
        private void ktb_lotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            string orderNo = kcb_order.Text;
            string sfc = ktb_lotNo.Text;
            if (e.KeyChar == (char)13)
            {
                if (orderNo == "" || ktb_lotNo.Text == "")
                {
                    MessageBox.Show("请输入工单和批次单号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetSFCByorder", orderNo + "," + sfc);
                List<P_SFC_State> proc = JsonConvert.DeserializeObject<List<P_SFC_State>>(dt);
                if (proc == null)
                {
                    MessageBox.Show("未找到该批次,请确认工单号和批次号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ktb_lotNo.Text = null;
                    ktb_lotNo.Focus();
                    return;
                }
                foreach (var item in proc)//添加批次数量
                {
                    kckl_lot.Items.Add(item.SFC + "       " + "数量:" + item.qty);
                }
                for (int i = 0; i < kckl_lot.Items.Count; i++)//默认选中
                {
                    kckl_lot.SetItemChecked(i, true);
                }
                ktb_lotNo.Text = null;
                ktb_lotNo.Focus();
                int total = 0, sum = 0;
                foreach (var tmp in kckl_lot.CheckedItems)//显示总数量
                {
                    string chemsg = tmp.ToString();
                    int i = chemsg.IndexOf(":");
                    int t = chemsg.Length - i - 5;
                    total = Convert.ToInt32(chemsg.Substring(i + 1, t));
                    sum += total;
                }
                ktl_total.Text = sum.ToString();
            }
        }
    }
}
