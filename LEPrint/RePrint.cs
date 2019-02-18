using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using NVBarcode.Serial;
using NV_SNP.Toos;
using System.Linq;
using ILE;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.IO;
using JsonTools;

namespace NV_SNP
{
    public partial class RePrint : Form
    {
        string orderNO;
        //DataTable orderDT;
        //Objs.Module mdl;
        List<P_SSW_TemplateList> proc1;
        List<P_SSW_PrintList> proc2;
        NVBarcode.PrintEngine6 pn;
        NVBarcode.BarCode barcode;

        public RePrint()
        {
            InitializeComponent();
        }

        private void RePrint_Load(object sender, EventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("EntersConfig.xml");
            XmlNode node = xd.SelectSingleNode("Enters");
            XmlNodeList nlst = node.ChildNodes;

            DataTable dt = new DataTable();
            dt.Columns.Add("name");
            dt.Columns.Add("value");
            DataRow row = dt.NewRow();
            row["name"] = "10进制";
            row["value"] = "10";
            dt.Rows.Add(row);
            foreach (XmlNode nd in nlst)
            {
                string name = ((XmlElement)nd).GetAttribute("name");
                string val = nd.Name;
                DataRow row1 = dt.NewRow();
                row1["name"] = name;
                row1["value"] = val;
                dt.Rows.Add(row1);
            }
            this.kryptonComboBox1.ValueMember = "value";
            this.kryptonComboBox1.DisplayMember = "name";
            this.kryptonComboBox1.DataSource = dt;
            xd = null;

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.orderNO = textBox1.Text.Trim();
                BindOrder();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.orderNO = textBox1.Text.Trim();
            BindOrder();
        }

        void BindOrder()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrintInfo", orderNO);
            proc2 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            if (proc2.Count < 1)
            {
                MessageBox.Show("没有找到相应的工单");
            }
            else
            {
                if (proc2[0].Template_id != null)
                {
                    this.ktb_moduleName.Text = proc2[0].Template_id.ToString();
                }
                else
                {
                    MessageBox.Show("暂无打印模板，请添加模板", "提示");
                    return;
                }
                this.ktb_orderNO.Text = proc2[0].order_no.ToString();
                //this.ktb_currentSN.Text = ((proc2[0].completed == null ? 0 : proc2[0].completed) + 1).ToString();
                this.ktb_sy.Text = (proc2[0].qty - (proc2[0].completed == null ? 0 : proc2[0].completed)).ToString();
                this.ktb_qty.Text = proc2[0].qty.ToString();
                if (proc2[0].completed != null)
                {
                    this.ktb_complete.Text = proc2[0].completed.ToString();
                }
                else
                {
                    this.ktb_complete.Text = "0";
                }
                string MoudelId = this.ktb_moduleName.Text.ToString();
                string mdl = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateInfo", MoudelId);
                proc1 = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(mdl);
                this.ktb_currentSN.Text = proc1[0].currentSN.ToString();
                this.pn = NVBarcode.PrintEngine6.GetPrintEngine(proc1[0].TemplatePath);
                string Mid = proc1[0].RuleStr.ToString();
                NVBarcode.CodeRule coderule = Objs.SpecialRule.TransformRuleStr(Mid);   //将特殊规则转成一般规则
                barcode = coderule.GetCodeByRule();   //将规则转成条码
                this.klb_barcode.Text += barcode.prefix + barcode.snStr + barcode.suffix;

                //if (mdl.CheckCode == 1)
                //    this.klb_barcode.Text += "[C]"; //如果有校验码，则在最后加上[C]； 
                //this.ktb_currentSN.Text = mdl.CurrentSN.ToString();
            }
        }

        private void ktb_start_Click(object sender, EventArgs e)
        {
            if (this.proc2 == null)
            {
                MessageBox.Show("没有获取到工单，打印终止");
                return;
            }
            if (tb_1.Text.Trim() == "" | tb_2.Text.Trim() == "")
            {
                MessageBox.Show("请输入补打范围区域");
                return;
            }
            if (int.Parse(tb_2.Text) < int.Parse(tb_1.Text))
            {
                MessageBox.Show("范围输入错误");
                return;
            }
            PrintEngine.Reprint.Print("", "", this, proc1, "");

        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
            base.OnKeyPress(e);
            if (e.KeyChar == 13)
            {
                ktb_start_Click(tb_2, new EventArgs());
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
            base.OnKeyPress(e);

        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private SerialManager GetSM(string EntName)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("EntersConfig.xml");
            XmlNode node = xd.SelectSingleNode("Enters");
            if (node.ChildNodes.Count == 0)  //如果没有找到下属节点，说明配置错误
                return null;
            XmlNodeList nl = node.ChildNodes;
            SerialManager sm = null;
            foreach (XmlNode nod in nl)
            {
                if (nod.Name == EntName)
                {
                    SerialManager.Enters ent = (SerialManager.Enters)System.Enum.Parse(typeof(SerialManager.Enters), EntName);
                    string val = ((XmlElement)nod).GetAttribute("value");
                    sm = new SerialManager(ent, val);
                    return sm;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取对应进制下的序列号
        /// </summary>
        /// <returns></returns>
        private string GetSNByEnter(SerialManager sm, int sn)
        {
            if (sm == null)
                return sn.ToString();
            return sm.GetEnterString(sn);

        }
    }
}
