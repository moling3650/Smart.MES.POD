using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Xml;
using NV_SNP.Toos;
using System.Linq;
using ILE;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.IO;
using NVBarcode.Serial;
using JsonTools;

namespace NV_SNP
{
    public partial class PrintFST : KryptonForm
    {
        string orderNO;
        List<P_SSW_TemplateList> proc1;
        List<P_SSW_PrintList> proc2;
        NVBarcode.PrintEngine6 pn;
        NVBarcode.BarCode barcode;
        public Dictionary<string, string> dicRulePar = new Dictionary<string, string>();

        public PrintFST(string orderno)
        {
            InitializeComponent();
            this.orderNO = orderno;
        }

        private void Print_Load(object sender, EventArgs e)
        {
            //bindMuBan();//为模板下拉框赋值
            BindOrder();//为文本框赋值

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
        }
        #region
        //void bindMuBan()
        //{
        //    string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateList", "cid");
        //    List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
        //    this.kryptonComboBox2.ValueMember = "Template_id";
        //    this.kryptonComboBox2.DisplayMember = "Template_name";
        //    this.kryptonComboBox2.DataSource = proc;
        //}
        #endregion
        string Mid = string.Empty;
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
                this.ktb_orderNO.Text = proc2[0].order_no.ToString();
                this.kryptonTextBox1.Text = proc2[0].Template_id.ToString();
                this.ktb_sy.Text = (proc2[0].qty - (proc2[0].completed == null ? 0 : proc2[0].completed)).ToString();
                this.ktb_pNum.Text = Convert.ToDouble(ktb_sy.Text).ToString("#0");
                this.ktb_qty.Text = proc2[0].qty.ToString();
                if (proc2[0].completed != null)
                {
                    this.ktb_complete.Text = proc2[0].completed.ToString();
                }
                else
                {
                    this.ktb_complete.Text = "0";
                }
                string MoudelId = this.kryptonTextBox1.Text.ToString();
                string mdl = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateInfo", MoudelId);
                proc1 = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(mdl);
                Mid = proc1[0].RuleStr.ToString();

                if (Mid.IndexOf("(") > -1)
                {
                    //PrintVari pv = new PrintVari(Mid, "");
                    //pv.ShowDialog();
                    //Mid = pv.result;
                    foreach (var itemkeys in dicRulePar.Keys)
                    {
                        if (Mid.IndexOf("(" + itemkeys.ToString() + ")") > -1)
                        {
                            Mid = Mid.Replace("(" + itemkeys.ToString() + ")", dicRulePar[itemkeys.ToString()]);
                        }
                    }
                }

                NVBarcode.CodeRule coderule = Objs.SpecialRule.TransformRuleStr(Mid);   //将特殊规则转成一般规则
                barcode = coderule.GetCodeByRule();   //将规则转成条码

                prefix = barcode.prefix;
                this.ktb_Pcopies.Text = proc1[0].printCopies.ToString();

                this.klb_barcode.Text += prefix + barcode.snStr + barcode.suffix;
                this.pn = NVBarcode.PrintEngine6.GetPrintEngine(proc1[0].TemplatePath);
                string PrintSn = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrint_currentSn", barcode.prefix + "," + barcode.suffix);
                PrintSnList = JsonConvert.DeserializeObject<List<P_Print_CurrentSN>>(PrintSn);
                if (PrintSnList != null)
                {
                    this.ktb_currentSN.Text = PrintSnList[0].current_sn.ToString();
                }
                else
                {
                    this.ktb_currentSN.Text = "1";
                }
                //if (mdl.CheckCode == 1)
                //    this.klb_barcode.Text += "[C]"; //如果有校验码，则在最后加上[C];
            }
        }
        string prefix = string.Empty;
        List<P_Print_CurrentSN> PrintSnList = null;
        private void ktb_pNum_KeyPress(object sender, KeyPressEventArgs e)
        {
            //实现打印
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
            base.OnKeyPress(e);
            if (e.KeyChar == 13)
            {
                ktb_start_Click(ktb_pNum, new EventArgs());
            }
        }
        private void ktb_start_Click(object sender, EventArgs e)
        {
            if (ktb_pNum.Text == "")
            {
                MessageBox.Show("请输入打印张数", "提示");
                return;
            }
            if (ktb_Pcopies.Text == "")
            {
                MessageBox.Show("请输入打印份数", "提示");
                return;
            }
            string Order_No = this.ktb_orderNO.Text.ToString();
            SerialManager sm = null;
            if (kryptonComboBox1.SelectedIndex > 0)   //如果打印进制不是10进制
            {
                string entName = kryptonComboBox1.SelectedValue.ToString();
                sm = GetSM(entName);   //获取一个序列号管理对象
            }
            string suffix = "";
            List<P_WorkOrder> workList = null;
            for (int it = 0; it < int.Parse(ktb_pNum.Text); it++)
            {
                if (proc1[0].checkCode == 1)
                {
                    barcode.sn = GetSNByEnter(sm, Int32.Parse(this.ktb_currentSN.Text) + it);
                    suffix = barcode.suffix + NVBarcode.CheckCode.ModCheckCode(barcode);
                }
                else
                {
                    suffix = barcode.suffix;
                }
                //根据前缀后缀判断是否存在数据，如果有数据，在当前的sn累加打印，没有数据则从1开始打印
                //string PrintSn = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrint_currentSn", barcode.prefix + "," + suffix);
                //PrintSnList = JsonConvert.DeserializeObject<List<P_Print_CurrentSN>>(PrintSn);
                string sn = "";
                if (PrintSnList != null)
                {
                    sn = NVBarcode.CodeRule.GetCompleteSN(GetSNByEnter(sm, int.Parse(PrintSnList[0].current_sn.ToString()) + it), barcode.snStr.Length);  //获取完整的SN
                }
                else
                {
                    sn = NVBarcode.CodeRule.GetCompleteSN(GetSNByEnter(sm, 1 + it), barcode.snStr.Length);  //获取完整的SN
                }

                ktb_Pcopies.Text = proc1[0].printCopies.ToString();
                for (int i = 0; i < Convert.ToInt32(ktb_Pcopies.Text); i++)
                {
                    pn.printDocument(sn, 1, prefix, suffix, 1, 1, int.Parse(proc1[0].faxType.ToString()));
                }
                string workinfo = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorder", Order_No);
                workList = JsonConvert.DeserializeObject<List<P_WorkOrder>>(workinfo);
                string _product_code = proc2[0].product_code;
                string ProductCode = NV_SNP.Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProduct", _product_code);
                List<B_Product> ProductCodeList = JsonConvert.DeserializeObject<List<B_Product>>(ProductCode);
                if (ProductCodeList[0].print_bind == 1)
                {
                    //打印一个条码，在条码绑定表插入一条数据
                    P_BarCodeBing P_barcode = new P_BarCodeBing()
                    {
                        order = Order_No,
                        parent_order = workList[0].parent_order,
                        main_order = workList[0].main_order,
                        state = 0,
                        barcode = barcode.prefix + sn + suffix,
                        product_code = _product_code,
                        InputTime = DateTime.Now
                    };
                    string strJson = JsonToolsNet.ObjectToJson(P_barcode);
                    Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertBarCode", strJson);
                }
                //timer1.Stop();
            }
            //int planSN = (int.Parse(PrintSnList[0].current_sn.ToString()) - 1) + int.Parse(ktb_pNum.Text);
            //int planSN = (int.Parse(this.ktb_currentSN.Text) - 1) + int.Parse(ktb_pNum.Text);
            //PrintConfirm pconfirm = new PrintConfirm(int.Parse(ktb_pNum.Text), planSN);
            //pconfirm.ShowDialog();
            //if (pconfirm.num > 0)   //如果确认有打印数量
            //{
            //添加或修改P_Print_CurrentSN数据
            if (PrintSnList != null)
            {
                //修改
                string Sn = (PrintSnList[0].current_sn + Convert.ToInt32(decimal.Parse(ktb_pNum.Text))).ToString();
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateP_Print_currentSn", barcode.prefix + "|" + suffix + "|" + Sn);
            }
            else
            {
                //添加
                P_Print_CurrentSN P_Print_CurrentSN = new P_Print_CurrentSN()
                {
                    prefix = barcode.prefix,
                    suffix = suffix,
                    current_sn = Convert.ToInt32(1 + Convert.ToInt32(decimal.Parse(ktb_pNum.Text))),
                    input_time = DateTime.Now
                };
                string strJson = JsonToolsNet.ObjectToJson(P_Print_CurrentSN);
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertPrint_CurrentSn", strJson);

            }


            this.klb_barcode.Text = "条码:";
            //修改P_SSW_TemplateList的currentSN                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           
            string MId = proc2[0].Template_id.ToString();
            int CSN = int.Parse(proc1[0].currentSN.ToString()) + int.Parse(ktb_pNum.Text);
            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateTemplateByMid", MId + "|" + CSN);
            //为P_SSW_PrintList新增或修改
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrint_orderNo", orderNO);
            List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            if (proc != null)
            {
                //修改
                string order_no = proc[0].order_no.ToString();
                int num = int.Parse(proc2[0].completed.ToString()) + int.Parse(ktb_pNum.Text.ToString());
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdatePrintByOrder_no", order_no + "|" + num);
            }
            else
            {
                //添加
                P_SSW_PrintList P_list = new P_SSW_PrintList()
                {
                    completed = Convert.ToInt32(this.ktb_pNum.Text.ToString()),
                    order_no = this.ktb_orderNO.Text.ToString(),
                    Main_order = workList[0].main_order,
                    product_code = proc2[0].product_code,
                    product_name = null,
                    Template_id = proc2[0].Template_id,
                    qty = proc2[0].qty,
                    inputTime = null,
                    finishTime = null,
                    state = null
                };
                string strJson = JsonToolsNet.ObjectToJson(P_list);
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertPrint", strJson);
            }
            //重新绑定工单信息
            BindOrderRu();
            if (Convert.ToDecimal(this.ktb_qty.Text) - Convert.ToDecimal(this.ktb_complete.Text) <= 0)
            {
                //修改工单状态
                string order_no = proc2[0].order_no.ToString();
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateWorkOderState", order_no);
            }
            string Alreadyprint = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetAlreadyPrint", workList[0].main_order);
            List<P_SSW_PrintList> AlreadyprintList = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(Alreadyprint);
            string Mainworkinfo = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetByMain_order", workList[0].main_order);
            int WorkOrderNum = JsonConvert.DeserializeObject<List<P_WorkOrder>>(Mainworkinfo).Count;
            if (AlreadyprintList != null && AlreadyprintList.Count == WorkOrderNum)
            {
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateMainWorkOderState", workList[0].main_order);
            }
            //}
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
        //取消打印
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //删除打印线程
        private void Print_FormClosed(object sender, FormClosedEventArgs e)
        {
            NVBarcode.PrintEngine6.closeApp();
        }
        //刷新
        #region
        void BindOrderRu()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrintInfo", orderNO);
            proc2 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            if (proc2.Count < 1)
            {
                MessageBox.Show("没有找到相应的工单");
            }
            else
            {
                this.ktb_orderNO.Text = proc2[0].order_no.ToString();
                this.kryptonTextBox1.Text = proc2[0].Template_id.ToString();
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
                string MoudelId = this.kryptonTextBox1.Text.ToString();
                string mdl = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateInfo", MoudelId);
                proc1 = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(mdl);

                //this.ktb_currentSN.Text = proc1[0].currentSN.ToString();

                NVBarcode.CodeRule coderule = Objs.SpecialRule.TransformRuleStr(Mid);   //将特殊规则转成一般规则
                barcode = coderule.GetCodeByRule();   //将规则转成条码

                prefix = barcode.prefix;
                this.klb_barcode.Text += prefix + barcode.snStr + barcode.suffix;
                this.pn = NVBarcode.PrintEngine6.GetPrintEngine(proc1[0].TemplatePath);
                string PrintSn = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrint_currentSn", barcode.prefix + "," + barcode.suffix);
                PrintSnList = JsonConvert.DeserializeObject<List<P_Print_CurrentSN>>(PrintSn);
                this.ktb_currentSN.Text = PrintSnList[0].current_sn.ToString();
                //if (mdl.CheckCode == 1)
                //    this.klb_barcode.Text += "[C]"; //如果有校验码，则在最后加上[C];
            }
        }
        #endregion
    }
}
