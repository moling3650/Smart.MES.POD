using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Xml;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using NV_SNP.Toos;
using System.Linq;
using ILE;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.IO;

namespace NV_SNP
{
    public partial class Main : KryptonForm
    {
        public Main()
        {
            InitializeComponent();

        }

        private void kcb_custom_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string cid = this.kcb_custom.SelectedValue.ToString();
            //string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderInfo", cid);
            //List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            //this.kryptonDataGridView1.DataSource = proc;
            //this.kryptonDataGridView1.DataSource = proc.Select(c => new {c.order_no,c.product_code,c.qty,c.state,c.input_time,c.indent });
        }
        //绑定产品编号下拉选
        void BindCustomer()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductCode", "cid");
            List<B_Product> proc = JsonConvert.DeserializeObject<List<B_Product>>(dt);
            this.kcb_custom.ValueMember = "product_code";
            this.kcb_custom.DisplayMember = "Product_Name";
            this.kcb_custom.DataSource = proc;
        }

        private void ktb_print_Click(object sender, EventArgs e)
        {

            if (this.kryptonDataGridView1.Rows.Count > 0)
            {
                string orderno = this.kryptonDataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
                Print md = new Print(orderno);
                md.ShowDialog();
                if (md.DialogResult == DialogResult.Cancel)
                {
                    if (kryptonRadioButton1.Checked)
                    {
                        GetOrder();
                    }
                    if (kryptonRadioButton2.Checked)
                    {
                        GetIndent();
                    }
                }
            }
            else
            {
                MessageBox.Show("暂无工单，请选择即将打印的工单！");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            (new CustomerModify()).ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            (new ModuleModify()).ShowDialog();
        }

        #region
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //(new SpecialModify()).ShowDialog();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            //(new OrderModify()).ShowDialog();
            //string cid = this.kcb_custom.SelectedValue.ToString();
            //DataTable dt = Objs.Order.GetOnLineOrdersByCustomer(cid);
            //this.kryptonDataGridView1.DataSource = dt;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //(new OrderModify()).ShowDialog();
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            //if (this.kryptonDataGridView1.SelectedRows.Count < 1)
            //    return;
            //if (MessageBox.Show("结案后工单将不能继续生产，是否确定结案", "结案提示:", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
            //{
            //    return;
            //}
            //string orderNO = kryptonDataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            //try
            //{
            //    Objs.Order.FinishOrder(orderNO);
            //    kryptonDataGridView1.Rows.RemoveAt(kryptonDataGridView1.SelectedRows[0].Index);
            //}
            //catch (Exception exc)
            //{
            //    MessageBox.Show("结案失败:" + exc.Message);
            //}
        }

        private void kbt_delete_Click(object sender, EventArgs e)
        {
            //if (MessageBox.Show("只有未作业的工单才能删除，确认要删除吗?", "提示:", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            //{
            //    MessageBox.Show("yes");
            //}
            //else { MessageBox.Show("no"); }
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //(new OrderReport()).ShowDialog();
        }

        private void kryptonDataGridView1_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void kbt_onLine_Click(object sender, EventArgs e)
        {
            //if (kryptonDataGridView1.SelectedRows.Count < 1)
            //    return;
            //string odno = kryptonDataGridView1.SelectedRows[0].Cells[0].Value.ToString();

            //(new OnLineReport(odno)).ShowDialog();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //(new OnLineReport()).ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //XmlDocument xd = new XmlDocument();
            //xd.Load("EntersConfig.xml");
            //XmlNode node = xd.SelectSingleNode("Enters");
            //XmlNodeList nlst = node.ChildNodes;

            ////foreach (XmlNode n in nlst)
            ////{
            ////    XmlElement ne = (XmlElement)n;
            ////    string name = ne.GetAttribute("name");
            ////    string val = ne.GetAttribute("value");
            ////}
            //string EntName = nlst[1].Name;
            //string val = ((XmlElement)nlst[1]).GetAttribute("value");
            //SerialManager.Enters ent = (SerialManager.Enters)System.Enum.Parse(typeof(SerialManager.Enters), EntName);
            //SerialManager sm = new SerialManager(ent, val);
            //this.textBox1.Text = sm.GetEnterString(int.Parse(textBox1.Text));
            ////string[] strs=sm.GetBitsByDecimal("100100010000000000",NVBarcode.SerialManager.SerialManager.Enters.Enter32);
        }
        #endregion

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            (new RePrint()).ShowDialog();
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            GetOrder();
        }
        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            #region
            //string Mid = this.kryptonDataGridView1.SelectedRows[0].Cells["Mid"].Value.ToString();
            //string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateByTemplate_id", Mid);
            //List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            //string rule = proc[0].RuleStr.ToString();
            //string[] ruleArr = rule.Split('&');
            //if (ruleArr[1] != "")
            //{
            //    string variable = ruleArr[1].ToString();
            //    (new PrintVariable(variable)).ShowDialog();
            //}
            //else
            //{
            //    string order_no = this.kryptonDataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
            //    string dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorder", order_no);
            //    List<P_WorkOrder> proc1 = JsonConvert.DeserializeObject<List<P_WorkOrder>>(dt1);
            //    string Main_order = string.Empty;
            //    if (proc != null)
            //    {
            //        Main_order = proc1[0].main_order.ToString();
            //    }
            //    (new PrintAll(Main_order)).ShowDialog();
            //}
            #endregion
        }

        private void Main_Load(object sender, EventArgs e)
        {
            BindCustomer();
            BingWorkOrderByTime();
            dateTimePicker1.Value = new DateTime(this.dateTimePicker1.Value.Year, this.dateTimePicker1.Value.Month, this.dateTimePicker1.Value.Day - 3, 00, 00, 00);
        }

        //获取最近三天的工单
        public void BingWorkOrderByTime()
        {

            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderBytime", "");
            List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
        }
        //搜索
        private void button2_Click(object sender, EventArgs e)
        {
            if (kryptonRadioButton1.Checked)
            {
                GetOrder();
            }
            if (kryptonRadioButton2.Checked)
            {
                GetIndent();
            }

        }
        //根据订单获取所有需打印的工单
        void GetIndent()
        {
            string indent = this.kryptonTextBox1.Text.ToString();
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetIndent", indent);
            List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            if (proc.Count == 0)
            {
                MessageBox.Show("该订单暂无数据，请核对订单号！", "提示");
                return;
            }
            this.kryptonDataGridView1.DataSource = proc;
        }
        //绑定gridview列表
        void BindView()
        {
            string cid = this.kryptonTextBox1.Text.ToString();
            string Stime = this.dateTimePicker1.Value.Date.ToLongDateString();
            string Etime = this.dateTimePicker2.Value.Date.ToLongDateString();
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderInfo", cid + "," + Stime + "," + Etime);
            List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
        }
        //递归获取工单所关联的所有工单
        private void GetOrder()
        {
            #region 没有main_order情况下
            //string Txt_Order_id = this.kryptonTextBox1.Text.ToString();
            //string dt0 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorder", Txt_Order_id);
            //List<P_WorkOrder> proc0 = JsonConvert.DeserializeObject<List<P_WorkOrder>>(dt0);
            //if (proc0 == null)
            //{
            //    MessageBox.Show("该工单暂无数据，请核对工单号！", "提示");
            //    return;
            //}

            //if (proc0[0].parent_order == "")
            //{
            //    Order_id = proc0[0].order_no.ToString();
            //}
            //else
            //{
            //    string P_order = proc0[0].parent_order.ToString();
            //    GetPID(P_order);
            //}
            //string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderByOrder_id", Order_id);
            //List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            //if (proc == null)
            //{
            //    MessageBox.Show("该工单暂无数据，请核对工单号！", "提示");
            //    return;
            //}
            //this.kryptonDataGridView1.DataSource = proc;
            //string dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderByParentOrder_id", Order_id);
            //List<P_SSW_PrintList> proc1 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt1);
            //if (proc1 != null)
            //{
            //    for (int i = 0; i < proc1.Count; i++)
            //    {
            //        string Order_code = proc1[i].order_no.ToString();
            //        GetSonOrder(Order_code, ref proc);
            //    }
            //}

            #endregion
            string Txt_Order_id = this.kryptonTextBox1.Text.ToString();
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderByMainOrder_id", Txt_Order_id);
            List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
        }
        //
        string Order_id = string.Empty;
        private string GetPID(string P_order)
        {
            string dt0 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorder", P_order);
            List<P_WorkOrder> proc0 = JsonConvert.DeserializeObject<List<P_WorkOrder>>(dt0);
            if (proc0[0].parent_order == "")
            {
                Order_id = proc0[0].order_no.ToString();
            }
            else
            {
                string P = proc0[0].parent_order.ToString();
                GetPID(P);
            }
            return Order_id;
        }
        //
        List<P_SSW_PrintList> list;
        private void GetSonOrder(string Order_code, ref List<P_SSW_PrintList> proc)
        {
            string dt2 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderByOrder_id", Order_code);
            List<P_SSW_PrintList> proc2 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt2);
            list = proc.Concat(proc2).ToList();
            proc = list;
            this.kryptonDataGridView1.DataSource = proc;
            string dt3 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderByParentOrder_id", Order_code);
            List<P_SSW_PrintList> proc3 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt3);
            if (proc3 != null)
            {
                for (int j = 0; j < proc3.Count; j++)
                {
                    string Order = proc3[j].order_no.ToString();
                    GetSonOrder(Order, ref proc);
                }
            }
        }

        private void kryptonButton3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            this.kryptonTextBox1.Text = "";
        }

        private void kryptonRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            this.kryptonTextBox1.Text = "";
        }
        //福斯特条码打印
        private void kryptonButton1_Click_1(object sender, EventArgs e)
        {
            (new PrintWorkOrder()).ShowDialog();
        }
        //回车
        private void kryptonTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            if (kryptonRadioButton1.Checked)
            {
                GetOrder();
            }
            if (kryptonRadioButton2.Checked)
            {
                GetIndent();
            }
        }
    }
}
