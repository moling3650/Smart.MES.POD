using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Newtonsoft.Json;
using LEDAO;

namespace LEMES_POD
{
    public partial class WorkerOrderFrom1 : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        FeedForm ff;
        string process;
        string _flow_code = string.Empty;
        public WorkerOrderFrom1(FeedForm f, string ProcessCode, string flow_code1)
        {
            process = ProcessCode;
            ff = f;
            _flow_code = flow_code1;
            InitializeComponent();
            BindData();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            BindData();
        }
        public void BindData()
        {
            DateTime time1 = this.kdt_plannedtime.Value.Date;
            DateTime time2 = this.kdt_plannedtime1.Value.Date;
            if (time1 > time2)
            {
                MessageBox.Show("��ʼʱ����ڽ���ʱ��","��ʾ");
                return;
            }
            string planned_time1 = time1.ToString("yyyy-MM-dd");
            string planned_time2 = time2.ToString("yyyy-MM-dd");
            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WorkOrder", "GetOrderByTime", planned_time1 + "," + process + "," + planned_time2);
            List<V_WorkOrder_Product> proc = JsonConvert.DeserializeObject<List<V_WorkOrder_Product>>(dt);
            if (proc == null)
            {
                this.kryptonDataGridView1.Visible = false;
                this.kryptonWrapLabel1.Visible = true;
                this.kryptonGroupBox2.Visible = true;
                this.kryptonWrapLabel1.Text = time1 + "��" + time2 + "����["+process+"]������ָ��";
                return;
            }
            this.kryptonGroupBox2.Visible = false;
            this.kryptonWrapLabel1.Visible = false;
            this.kryptonDataGridView1.Visible = true;
            this.kryptonDataGridView1.DataSource = proc;
            // this.kryptonDataGridView1.ClearSelection();
        }
        /// <summary>
        /// ���ȷ����ť���رյ�ǰҳ�棬������ҳ���TextBOX�ؼ���ֵ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView1.DataSource == null)
            {
                MessageBox.Show("��ѡ�񹤵�", "��ʾ");
                return;
            }
            string Order_no = this.kryptonDataGridView1.SelectedRows[0].Cells["Order_no"].Value.ToString();
            KryptonTextBox text = new KryptonTextBox();
            text = (KryptonTextBox)ff.Controls.Find("ktxtOrder", true)[0];
            text.Text = Order_no.ToString();
            text.Focus();
            text.TabIndex = 0;
            this.Close();

        }
        //˫�����¼�
        private void kryptonDataGridView1_DoubleClick(object sender, EventArgs e)
        {
            string Order_no = this.kryptonDataGridView1.SelectedRows[0].Cells["Order_no"].Value.ToString();
            KryptonTextBox text = new KryptonTextBox();
            text = (KryptonTextBox)ff.Controls.Find("ktxtOrder", true)[0];
            text.Text = Order_no.ToString();
            text.Focus();
            text.TabIndex = 0;
            this.Close();

        }
    }
}