using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using LEDAO;
using Newtonsoft.Json;
using JsonTools;

namespace LEMES_LEOQC
{
    public partial class OQCWin : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        private string maxNGValue, oqcno, orderno;//不良最大数，检验单号，工单号

        public OQCWin(string _maxNGValue, string _oqcno, string _orderno)
        {
            InitializeComponent();
            this.maxNGValue = _maxNGValue;
            this.oqcno = _oqcno;
            this.orderno = _orderno;
        }
        private void OQCWin_Load(object sender, EventArgs e)
        {
            ktb_oqcoreder.Text = oqcno;//检验单号
            ktb_orderNo.Text = orderno;//工单号
            LoadGridView();
        }
        //绑定数据
        private void LoadGridView()
        {
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetNGList", oqcno + "," + orderno);
            List<P_OQC_NGList> source = JsonConvert.DeserializeObject<List<P_OQC_NGList>>(dt);
            if (source == null)
            {
                List<P_OQC_NGList> emptySource = new List<P_OQC_NGList>();
                kryptonDataGridView1.DataSource = emptySource;
            }
            else
            {
                kryptonDataGridView1.DataSource = source;
            }
        }
        //删除选中
        private void ktbn_del_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请先选中需要删除的列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = KryptonMessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            try
            {
                string delID = kryptonDataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "DelNGList", delID);
            }
            catch (Exception)
            {
                MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadGridView();
        }
        //向OQCOrder表提交本次检验结果
        private void ResultToOQCOrder(string oqcNo, string orderNo, string result)
        {
            string datetime = DateTime.Now.ToString();
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "UpDataOQCOrder", oqcNo + "," + orderNo + "," + result + "," + datetime);
        }
        //完成检验按钮
        private void ktbn_end_Click(object sender, EventArgs e)
        {
            DialogResult result = KryptonMessageBox.Show("确定提交检验结果吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetNGList", oqcno + "," + orderno);
            List<P_OQC_NGList> oqcList = JsonConvert.DeserializeObject<List<P_OQC_NGList>>(dt);
            int NGNum = 0;
            if (oqcList == null)//没有不良
            {
                NGNum = 0;
            }
            else
            {
                foreach (var item in oqcList)
                {
                    int num = Convert.ToInt32(item.ng_qty);
                    NGNum += num;
                }
            }
            if (NGNum >= Convert.ToInt32(maxNGValue))//记录的不良数大于检验标准不良拒收值
            {
                string oqcResult = "1";//1代表检验结果为NG
                ResultToOQCOrder(oqcno, orderno, oqcResult);
                MessageBox.Show("本批次检验结果为NG", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string oqcResult = "0";//0代表检验结果为OK
                ResultToOQCOrder(oqcno, orderno, oqcResult);
                MessageBox.Show("本批次检验结果为OK", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //窗体关闭提示
        private void OQCWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = KryptonMessageBox.Show("确定要关闭吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
                this.Close();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }
        //验证员工号
        private void text_empcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)9)
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string jsdata = client.RunServerAPI("BLL.Employee", "GetStaff", text_empcode.Text);
                if (jsdata == "")
                {
                    MessageBox.Show("员工号不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    text_empcode.Text = null;
                    text_empcode.Focus();
                    return;
                }
                text_empcode.ReadOnly = true;
                ktb_NGcoude.Focus();
            }
        }
        //员工退出
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            text_empcode.Text = null;
            text_empcode.ReadOnly = false;
            text_empcode.Focus();
        }
        private bool checkNGCode()
        {
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string jsdata = client.RunServerAPI("BLL.NGCode", "GetNGCode", ktb_NGcoude.Text);
            if (jsdata == "")
            {
                MessageBox.Show("无效的不良代码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        //不良代码输入
        private void ktb_NGcoude_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)9)
            {
                if (checkNGCode())
                {
                    ktb_NGnum.Focus();
                }
                else
                {
                    ktb_NGcoude.Text = null;
                    ktb_NGcoude.Focus();
                    return;
                }
            }
        }
        //不良数输完，添加不良
        private void ktb_NGnum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (text_empcode.Text == "")
                {
                    MessageBox.Show("请输入工号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    text_empcode.Focus();
                    return;
                }
                if (ktb_NGcoude.Text == "")
                {
                    MessageBox.Show("请输入不良代码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ktb_NGcoude.Focus();
                    return;
                }
                if (ktb_NGnum.Text == "")
                {
                    MessageBox.Show("请输入不良数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (checkNGCode())
                {
                    ktb_NGnum.Focus();
                }
                else
                {
                    ktb_NGcoude.Text = null;
                    ktb_NGcoude.Focus();
                    return;
                }
                try
                {
                    P_OQC_NGList ngList = new P_OQC_NGList()
                    {
                        oqc_order_no = ktb_oqcoreder.Text.Trim(),
                        order_no = ktb_orderNo.Text.Trim(),
                        ng_code = ktb_NGcoude.Text.Trim(),
                        ng_qty = Convert.ToDecimal(ktb_NGnum.Text.Trim()),
                        input_time = DateTime.Now,
                        emp_code = text_empcode.Text.Trim()
                    };
                    string strJson = JsonToolsNet.ObjectToJson(ngList);
                    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "CreateNGList", strJson);
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    LoadGridView();
                    ktb_NGcoude.Text = null;
                    ktb_NGnum.Text = null;
                    ktb_NGcoude.Focus();
                }
                catch (Exception)
                {
                    MessageBox.Show("请输入正确的不良数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ktb_NGnum.Text = null;
                    ktb_NGnum.Focus();
                    return;
                }
            }
        }

        private void text_empcode_Leave(object sender, EventArgs e)
        {
            if (text_empcode.Text == "")
            {
                MessageBox.Show("请先输入工号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                text_empcode.Text = null;
                text_empcode.Focus();
                return;
            }
            else
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string jsdata = client.RunServerAPI("BLL.Employee", "GetStaff", text_empcode.Text);
                if (jsdata == "")
                {
                    MessageBox.Show("员工号不存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    text_empcode.Text = null;
                    text_empcode.Focus();
                    return;
                }
                text_empcode.ReadOnly = true;
                ktb_NGcoude.Focus();
            }
        }

        //private void ktb_NGcoude_Leave(object sender, EventArgs e)
        //{
        //    if (ktb_NGcoude.Text == "")
        //    {
        //        MessageBox.Show("请先输入不良代码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        ktb_NGcoude.Text = null;
        //        ktb_NGcoude.Focus();
        //        return;
        //    }
        //    else
        //    {
        //        ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
        //        string jsdata = client.RunServerAPI("BLL.NGCode", "GetNGCode", ktb_NGcoude.Text);
        //        if (jsdata == "")
        //        {
        //            MessageBox.Show("无效的不良代码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            ktb_NGcoude.Text = null;
        //            ktb_NGcoude.Focus();
        //            return;
        //        }
        //    }
        //}
    }
}