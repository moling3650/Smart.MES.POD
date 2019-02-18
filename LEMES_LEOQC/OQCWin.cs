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
        private string maxNGValue, oqcno, orderno;//��������������鵥�ţ�������

        public OQCWin(string _maxNGValue, string _oqcno, string _orderno)
        {
            InitializeComponent();
            this.maxNGValue = _maxNGValue;
            this.oqcno = _oqcno;
            this.orderno = _orderno;
        }
        private void OQCWin_Load(object sender, EventArgs e)
        {
            ktb_oqcoreder.Text = oqcno;//���鵥��
            ktb_orderNo.Text = orderno;//������
            LoadGridView();
        }
        //������
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
        //ɾ��ѡ��
        private void ktbn_del_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("����ѡ����Ҫɾ������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = KryptonMessageBox.Show("ȷ��ɾ����", "����", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                MessageBox.Show("ɾ��ʧ�ܣ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            LoadGridView();
        }
        //��OQCOrder���ύ���μ�����
        private void ResultToOQCOrder(string oqcNo, string orderNo, string result)
        {
            string datetime = DateTime.Now.ToString();
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "UpDataOQCOrder", oqcNo + "," + orderNo + "," + result + "," + datetime);
        }
        //��ɼ��鰴ť
        private void ktbn_end_Click(object sender, EventArgs e)
        {
            DialogResult result = KryptonMessageBox.Show("ȷ���ύ��������", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.OQC", "GetNGList", oqcno + "," + orderno);
            List<P_OQC_NGList> oqcList = JsonConvert.DeserializeObject<List<P_OQC_NGList>>(dt);
            int NGNum = 0;
            if (oqcList == null)//û�в���
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
            if (NGNum >= Convert.ToInt32(maxNGValue))//��¼�Ĳ��������ڼ����׼��������ֵ
            {
                string oqcResult = "1";//1���������ΪNG
                ResultToOQCOrder(oqcno, orderno, oqcResult);
                MessageBox.Show("�����μ�����ΪNG", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string oqcResult = "0";//0���������ΪOK
                ResultToOQCOrder(oqcno, orderno, oqcResult);
                MessageBox.Show("�����μ�����ΪOK", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }
        //����ر���ʾ
        private void OQCWin_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = KryptonMessageBox.Show("ȷ��Ҫ�ر���", "��ʾ", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
        //��֤Ա����
        private void text_empcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 || e.KeyChar == (char)9)
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string jsdata = client.RunServerAPI("BLL.Employee", "GetStaff", text_empcode.Text);
                if (jsdata == "")
                {
                    MessageBox.Show("Ա���Ų�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    text_empcode.Text = null;
                    text_empcode.Focus();
                    return;
                }
                text_empcode.ReadOnly = true;
                ktb_NGcoude.Focus();
            }
        }
        //Ա���˳�
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
                MessageBox.Show("��Ч�Ĳ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        //������������
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
        //���������꣬��Ӳ���
        private void ktb_NGnum_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (text_empcode.Text == "")
                {
                    MessageBox.Show("�����빤��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    text_empcode.Focus();
                    return;
                }
                if (ktb_NGcoude.Text == "")
                {
                    MessageBox.Show("�����벻������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ktb_NGcoude.Focus();
                    return;
                }
                if (ktb_NGnum.Text == "")
                {
                    MessageBox.Show("�����벻������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("��ӳɹ�", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    LoadGridView();
                    ktb_NGcoude.Text = null;
                    ktb_NGnum.Text = null;
                    ktb_NGcoude.Focus();
                }
                catch (Exception)
                {
                    MessageBox.Show("��������ȷ�Ĳ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("�������빤��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Ա���Ų�����", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        //        MessageBox.Show("�������벻������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        //            MessageBox.Show("��Ч�Ĳ�������", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            ktb_NGcoude.Text = null;
        //            ktb_NGcoude.Focus();
        //            return;
        //        }
        //    }
        //}
    }
}