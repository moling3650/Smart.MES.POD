using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using NV_SNP.Toos;
using System.Linq;
using NVBarcode.Serial;
using ILE;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.IO;
using JsonTools;

namespace NV_SNP
{
    public partial class PrintAll : KryptonForm
    {
        NVBarcode.PrintEngine6 pn;
        NVBarcode.BarCode barcode;
        string Main_or = string.Empty;
        string clas = string.Empty;
        Form form;
        List<string> lstRulePar = new List<string>(); //加载模板参数
        public PrintAll(string Main_order, string classes, Form fm)
        {
            form = fm;
            clas = classes;
            Main_or = Main_order;
            InitializeComponent();
            this.kryptonDataGridView1.AutoGenerateColumns = false;
        }
        private void PrintAll_Load(object sender, EventArgs e)
        {
            BindWorkerOder();
        }
        private void cb_custom_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetRoot();
            //this.treeView1.ExpandAll();
            //BindWorkerOder();
        }

        #region 废旧代码
        void BindModule()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplate", nodeText);
            List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
        }
        //判断该产品是否有模板
        private bool IsTemplate()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplate", nodeText);
            List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
            if (proc == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //(new ModuleAdd(nodeText,node)).ShowDialog();
            if (IsTemplate())
            {
                ModuleAdd md = new ModuleAdd(nodeText, node);
                md.ShowDialog();
                if (md.DialogResult == DialogResult.OK)
                {
                    BindModule();
                }
            }
            else
            {
                MessageBox.Show("该产品已有模板，不可重复添加！");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplate", nodeText);
            List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            if (IsTemplate())
            {
                MessageBox.Show("暂无模板，不可修改，请添加模板！");
            }
            else
            {
                string mid = proc[0].Template_id.ToString();
                //(new ModuleAdd(mid)).ShowDialog();
                ModuleAdd md = new ModuleAdd(mid);
                md.ShowDialog();
                if (md.DialogResult == DialogResult.OK)
                {
                    BindModule();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTemplate())
                {
                    MessageBox.Show("暂无模板，无法删除，请添加模板！");
                }
                else
                {
                    Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "DeleteTemplate", nodeText);
                    MessageBox.Show("删除成功");
                    BindModule();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("删除失败:" + exc.Message);
            }
        }

        private void kbt_CanBeZero_Click(object sender, EventArgs e)
        {
            //PassWord pw = new PassWord();
            //pw.ShowDialog();
            //if (pw.DialogResult != DialogResult.OK)
            //{
            //    MessageBox.Show("没有通过权限验证");
            //    return;
            //}

            //string mid=kryptonDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //Objs.Module.SetModuleZero(mid);
            //string cid = this.cb_custom.SelectedValue.ToString();
            ////MessageBox.Show(mid);
            //BindModule(cid); 
        }
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }
        private void kryptonDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //if (kryptonDataGridView1.SelectedRows.Count < 1)
            //    return;
            //string mid=kryptonDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //this.kbt_CanBeZero.Enabled = Objs.Module.CanBeSetZero(mid); //判断此规则是否可以归零
            //this.klb1.Visible = kbt_CanBeZero.Enabled;
        }

        string node = null;
        string nodeText = null;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            nodeText = e.Node.Text.ToString();
            node = e.Node.Tag.ToString();
            BindWorkerOder();
        }
        #endregion

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void BindWorkerOder()
        {
            string dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorkOrderByMain_order", Main_or);
            List<P_SSW_PrintList> proc1 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt1);
            this.kryptonDataGridView1.DataSource = proc1;

            //根据工单获取模板
            if (proc1 == null
                || proc1.Count <1)
            {
                kryptonButton1.Enabled = false;  //没有找到工单
                return;
            }
            else
            {
                kryptonButton1.Enabled = true;
            }
            lstRulePar.Clear();
            for (int i = 0; i < proc1.Count; i++)
            {
                GetRulePar(proc1[i].order_no);
            }

            // 在界面上展示参数输入框
            for (int j = 0; j < lstRulePar.Count; j++)
            {
                GetVariPanel(lstRulePar[j], j);
            }
        }
        void BingMainWorkOrder()
        {
            DataGridView gdv = new DataGridView();
            gdv = (DataGridView)form.Controls.Find("kryptonDataGridView1", true)[0];
            string mainorder = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetMainOrder", "");
            List<P_MainWorkOrder> pmwo = JsonConvert.DeserializeObject<List<P_MainWorkOrder>>(mainorder);
            gdv.DataSource = pmwo;
        }
        //工单套打
        private void kryptonButton1_Click(object sender, EventArgs e)
        {

            try
            {
                string Main_order = this.kryptonDataGridView1.SelectedRows[0].Cells["Column9"].Value.ToString();

                //获取模板参数值
                Dictionary<string, string> dicRuleParTmp = new Dictionary<string, string>();
                for (int m = 0; m < lstRulePar.Count; m++)
                {
                    string strParValue = ((TextBox)kPanel_TabPar.Controls[lstRulePar[m]]).Text.ToString();
                    if (string.IsNullOrWhiteSpace(strParValue))
                    {
                        MessageBox.Show("请设置模板参数[" + lstRulePar[m] + "]", "提示");
                        return;
                    }
                    dicRuleParTmp.Add(lstRulePar[m], strParValue);
                }
                //(new PrintFST(Main_order)).ShowDialog();
                PrintFST md = new PrintFST(Main_order);
                md.dicRulePar = dicRuleParTmp;
                md.ShowDialog();
                if (md.DialogResult == DialogResult.Cancel)
                {
                    BindWorkerOder();
                    BingMainWorkOrder();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message,"错误--catch");
            }
            #region 循环打印
            //int RowsCount = this.kryptonDataGridView1.Rows.Count;
            ////循环判断模板不能为空
            //for (int i = 0; i < RowsCount; i++)
            //{
            //    try
            //    {
            //        string M_id = this.kryptonDataGridView1.Rows[i].Cells["Column1"].Value.ToString();
            //    }
            //    catch
            //    {
            //        string P_code = this.kryptonDataGridView1.Rows[i].Cells["Column8"].Value.ToString();
            //        string MessInfo = "产品编号为[" + P_code + "]的产品无打印模板,\n";
            //        MessInfo += "请在模板管理中添加！";
            //        MessageBox.Show(MessInfo, "提示");
            //        return;
            //    }
            //}

            ////循环打印工单
            //for (int i = 0; i < RowsCount; i++)
            //{
            //    string Qty1 = this.kryptonDataGridView1.Rows[i].Cells["Column11"].Value.ToString();
            //    string order_No = this.kryptonDataGridView1.Rows[i].Cells["Column9"].Value.ToString();
            //    string P_Name = this.kryptonDataGridView1.Rows[i].Cells["Column14"].Value.ToString();
            //    string P_code = this.kryptonDataGridView1.Rows[i].Cells["Column8"].Value.ToString();
            //    string M_id = this.kryptonDataGridView1.Rows[i].Cells["Column1"].Value.ToString();
            //    string mdl = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateInfo", M_id);
            //    List<P_SSW_TemplateList> proc1 = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(mdl);

            //    string workinfo = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorder", order_No);
            //    List<P_WorkOrder> proc2 = JsonConvert.DeserializeObject<List<P_WorkOrder>>(workinfo);

            //    if (proc1[0].RuleStr.IndexOf("(") > -1)
            //    {
            //        PrintVari pv = new PrintVari(proc1[0].RuleStr, P_Name);
            //        pv.ShowDialog();
            //        proc1[0].RuleStr = pv.result;
            //    }

            //    NVBarcode.CodeRule coderule = Objs.SpecialRule.TransformRuleStr(proc1[0].RuleStr);   //将特殊规则转成一般规则
            //    barcode = coderule.GetCodeByRule();   //将规则转成条码
            //    this.pn = NVBarcode.PrintEngine6.GetPrintEngine(proc1[0].TemplatePath);
            //    if (this.pn == null)
            //    {
            //        string MessInfo = "产品编号为[" + P_code + "]的产品无打印模板,\n";
            //        MessInfo += "请在模板管理中添加！";
            //        MessageBox.Show(MessInfo, "提示");
            //        return;
            //    }
            //    string suffix = "";
            //    List<P_Print_CurrentSN> PrintSnList = null;
            //    for (int j = 0; j < decimal.Parse(Qty1.ToString()); j++)
            //    {
            //        SerialManager sm = null;
            //        if (proc1[0].checkCode == 1)
            //        {
            //            barcode.sn = GetSNByEnter(sm, j);
            //            suffix = barcode.suffix + NVBarcode.CheckCode.ModCheckCode(barcode);
            //        }
            //        else
            //        {
            //            suffix = barcode.suffix;
            //        }
            //        //根据前缀后缀判断是否存在数据，如果有数据，在当前的sn累加打印，没有数据则从1开始打印
            //        string PrintSn = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrint_currentSn", barcode.prefix + "," + suffix);
            //        PrintSnList = JsonConvert.DeserializeObject<List<P_Print_CurrentSN>>(PrintSn);
            //        string sn = "";
            //        if (PrintSnList != null)
            //        {
            //            sn = NVBarcode.CodeRule.GetCompleteSN(GetSNByEnter(sm, int.Parse(PrintSnList[0].current_sn.ToString()) + j), barcode.snStr.Length);  //获取完整的SN
            //        }
            //        else
            //        {
            //            sn = NVBarcode.CodeRule.GetCompleteSN(GetSNByEnter(sm, 1 + j), barcode.snStr.Length);  //获取完整的SN
            //        }
            //        for (int m = 0; m < proc1[0].printCopies; m++)
            //        {
            //            pn.printDocument(sn, 1, barcode.prefix, suffix, 1, 1, int.Parse(proc1[0].faxType.ToString()));
            //        }
            //        string Productinfo = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProduct", P_code);
            //        List<B_Product> ProductList = JsonConvert.DeserializeObject<List<B_Product>>(Productinfo);
            //        if (ProductList[0].print_bind == 1)
            //        {
            //            //打印一个条码，在条码绑定表插入一条数据
            //            P_BarCodeBing P_barcode = new P_BarCodeBing()
            //            {
            //                order = order_No,
            //                parent_order = proc2[0].parent_order,
            //                main_order = proc2[0].main_order,
            //                state = 0,
            //                barcode = barcode.prefix + sn + suffix,
            //                product_code = P_code,
            //                InputTime = DateTime.Now
            //            };
            //            string strJson = JsonToolsNet.ObjectToJson(P_barcode);
            //            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertBarCode", strJson);
            //        }

            //    }

            //    //套打完成修改一系列状态
            //    if (decimal.Parse(Qty1) > 0)
            //    {
            //        //修改p_MainworkOrder的状态
            //        if (i == RowsCount - 1)
            //        {
            //            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateMainWorkOderState", Main_or);
            //        }
            //        //添加或修改P_Print_CurrentSN数据
            //        if (PrintSnList != null)
            //        {
            //            //修改
            //            string Sn = (PrintSnList[0].current_sn + Convert.ToInt32(decimal.Parse(Qty1))).ToString();
            //            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateP_Print_currentSn", barcode.prefix + "|" + suffix + "|" + Sn);
            //        }
            //        else
            //        {
            //            //添加
            //            P_Print_CurrentSN P_Print_CurrentSN = new P_Print_CurrentSN()
            //            {
            //                prefix = barcode.prefix,
            //                suffix = suffix,
            //                current_sn = Convert.ToInt32(1 + Convert.ToInt32(decimal.Parse(Qty1))),
            //                input_time = DateTime.Now
            //            };
            //            string strJson = JsonToolsNet.ObjectToJson(P_Print_CurrentSN);
            //            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertPrint_CurrentSn", strJson);

            //        }
            //        //套打完成添加或修改完成数  --P_SSW_PrintList
            //        string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrint_orderNo", order_No);
            //        List<P_SSW_PrintList> proc = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            //        if (proc != null)
            //        {
            //            //修改 
            //            string F_Num = (decimal.Parse(Qty1)).ToString();
            //            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdatePrintByOrder_no", order_No + "|" + F_Num);
            //        }
            //        else
            //        {
            //            //添加
            //            P_SSW_PrintList P_list = new P_SSW_PrintList()
            //            {
            //                order_no = order_No.ToString(),
            //                product_code = P_code.ToString(),
            //                product_name = "",
            //                Template_id = M_id.ToString(),
            //                qty = decimal.Parse(Qty1),
            //                completed = Convert.ToInt32(decimal.Parse(Qty1)),
            //                inputTime = null,
            //                finishTime = null,
            //                state = ""
            //            };
            //            string strJson = JsonToolsNet.ObjectToJson(P_list);
            //            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertPrint", strJson);
            //        }

            //        //套打完成修改sn
            //        string C_Sn = (proc1[0].currentSN + Convert.ToInt32(decimal.Parse(Qty1))).ToString();
            //        Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateTemplateByMid", M_id + "|" + C_Sn);
            //        //如果完成数和总数量相同时，更改状态
            //        string S_No = "1";
            //        Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateWorkorder_State", order_No + "|" + S_No);

            //    }
            //}
            //MessageBox.Show("打印完成", "提示");
            //this.Close();
            ////刷新当前工单页面
            //BindWorkerOder();
            ////打印完成确定后刷新主工单页面
            //BingMainWorkOrder();
            #endregion

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
        private void GetRulePar(string parstrOrderNo)
        {
            //加载模板参数
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetPrintInfo", parstrOrderNo);
             List<P_SSW_PrintList>  proc2 = JsonConvert.DeserializeObject<List<P_SSW_PrintList>>(dt);
            if (proc2.Count < 1)
            {
                MessageBox.Show("存在无效工单[" + parstrOrderNo+"]");
                return;
            }
            else
            {
                string mdl = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateInfo", proc2[0].Template_id.ToString());
                List<P_SSW_TemplateList>  proc1 = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(mdl);
                string strRuleMid = proc1[0].RuleStr.ToString();
                GetVariList(lstRulePar, strRuleMid);
            }
            //for (int i = 0; i < 10; i++)
            //{
            //    int a = 2;
            //    string strRule = "FST-[Y:yy][M:MM][D:dd](班次" + i.ToString() + ")-A1-A[SN:0001]";
            //    GetVariList(lstRulePar, strRule);
            //}
            
        }
        //获取工单下模板信息
        private void GetVariList(List<string> lstRulePar, string rule)
        {
            for (int i = 0; i < rule.Length; i++)
            {
                if (rule[i] == '(')
                {
                    for (int j = ++i; j < rule.Length; j++)
                    {
                        if (rule[j] == ')')
                        {
                            if (lstRulePar.IndexOf(rule.Substring(i, j - i)) >= 0)
                            {
                                //如果列表中已经存在
                            }
                            else
                            {
                                lstRulePar.Add(rule.Substring(i, j - i));
                            }
                            i = j;
                            break;
                        }
                    }
                }
            }
        }


        
        
        //产生模板中的参数输入框
        private void GetVariPanel(string variName,int pariindex)
        {
            // 
            // label1
            // 
            Label label1 = new Label();
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("宋体", 13F);
            label1.Location = new System.Drawing.Point(2, 5+pariindex*32);
            label1.Name = "label1";
            label1.Text = variName;
            label1.Size = new System.Drawing.Size(56, 16);
            label1.TabIndex = 0;
            // 
            // textBox1
            // 
            TextBox textBox1 = new TextBox();
            textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox1.Font = new System.Drawing.Font("宋体", 13F);
            textBox1.Location = new System.Drawing.Point(89, 2 + pariindex * 32);
            textBox1.Name = variName;
            textBox1.Size = new System.Drawing.Size(150, 26);
            textBox1.TabIndex = 1;
            if (pariindex == 0)
            {
                textBox1.TabIndex = 0;
                textBox1.Focus();
            }

            //加入到界面指定位置中
            kPanel_TabPar.Controls.Add(textBox1);
            kPanel_TabPar.Controls.Add(label1);
            
            

        }
    }
}
