using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Reflection;
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
    public partial class ModuleAdd : KryptonForm
    {
        private string moduleID = "";
        private string setzero = "最大值";

        string Node = string.Empty;
        String NodeText = string.Empty;
        public ModuleAdd(string nodeText, string node)
        {
            InitializeComponent();
            this.ktb_module.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Text = "新增模板";
            Node = node;
            NodeText = nodeText;
            CheckList();
        }

        public void CheckList()
        {
            //ComboboxItem item1 = new ComboboxItem("工单", "Order_No");
            ckBox_variable.Items.Add(new ComboboxItem("order_no", "工单"));
            ckBox_variable.Items.Add(new ComboboxItem("version", "型号"));
            ckBox_variable.Items.Add(new ComboboxItem("product_code", "成品料号"));
            ckBox_variable.Items.Add(new ComboboxItem("input_time", "打印时间"));
        }
        public ModuleAdd(string mid)
        {
            this.moduleID = mid;
            this.Text = "修改模板";
            InitializeComponent();
            this.ktb_module.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            CheckList();
        }

        private void ModuleAdd_Load(object sender, EventArgs e)
        {
            this.Product_textBox.Text = NodeText;
            //BindCustomer();//绑定产品编码下拉选    
            //BindSpe();   //绑定特殊日期
            this.Text = "新增模板";
            if (this.moduleID != "")   //传入了式样编码，说明是修改模式
            {
                this.Text = "修改模板";
                string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplateByTemplate_id", moduleID);//获取模板
                List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
                BreakUpRule(proc[0].RuleStr);   //加载样式当前的规则
                BindModule(proc);   //绑定当前式样的其他属性
                this.tb_mid.Enabled = false;
                //数据库获取数据，循环获取数据，给checklistBox赋值
                string vartem = proc[0].variable.ToString();
                string[] vartemList = vartem.Split(',');
                for (int index = 0; index < vartemList.Count(); index++)
                {
                    for (int indextem = 0; indextem < ckBox_variable.Items.Count; indextem++)
                    {
                        if (((ComboboxItem)ckBox_variable.Items[indextem]).Value == vartemList[index].ToString())
                        {
                            ckBox_variable.SetItemChecked(indextem, true);
                        }
                    }
                }
            }
        }

        //把规则拆散写入flowPanel的方法
        private void BreakUpRule(string rulestr)
        {
            string start = "";
            for (int i = 0; i < rulestr.Length; i++)
            {
                if (rulestr[i] == '[' | rulestr[i] == '{' | rulestr[i] == '(')
                {
                    if (start != "")
                    {
                        KryptonTextBox ktb = GetRuleTextBox(start);
                        this.flowLayoutPanel1.Controls.Add(ktb);
                        start = "";
                        start += rulestr[i].ToString();
                    }
                    else
                    {
                        start += rulestr[i].ToString();
                    }
                }
                else if (rulestr[i] == ']' | rulestr[i] == '}' | rulestr[i] == ')')
                {
                    start += rulestr[i].ToString();
                    KryptonTextBox ktb = GetRuleTextBox(start);
                    this.flowLayoutPanel1.Controls.Add(ktb);
                    start = "";
                }
                else
                {
                    start += rulestr[i].ToString();
                    if (i == rulestr.Length - 1)
                    {
                        KryptonTextBox ktb = GetRuleTextBox(start);
                        this.flowLayoutPanel1.Controls.Add(ktb);
                    }
                }
            }
        }


        void BindModule(List<P_SSW_TemplateList> proc)
        {
            this.Product_textBox.Text = proc[0].product_code;
            this.tb_mid.Text = proc[0].Template_id;
            this.tb_mName.Text = proc[0].Template_name;
            this.tb_msn.Text = proc[0].maxSN.ToString();
            this.tb_currentSN.Text = proc[0].currentSN.ToString();
            this.cb_setZero.Text = proc[0].setZero;
            this.tb_filePath.Text = proc[0].TemplatePath;
            this.tb_Copies.Text = proc[0].printCopies.ToString();
            if (proc[0].faxType == 2)   //填充方式
                rb2.Checked = true;
            else
                rb1.Checked = true;

            if (proc[0].checkCode == 0) //是否有校验码
                kcb_checkCode.Checked = false;
            else
                kcb_checkCode.Checked = true;
        }
        /// <summary>
        /// 加入常量的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kbt_1_Click(object sender, EventArgs e)
        {
            if (this.tb_constant.Text.Trim() == "")
                return;
            KryptonTextBox ktb = GetRuleTextBox(this.tb_constant.Text.Trim());
            this.flowLayoutPanel1.Controls.Add(ktb);
            this.kbt_1.Enabled = false;
        }
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            if (this.tb_variable.Text.Trim() == "")
                return;
            KryptonTextBox ktb = GetRuleTextBox("(" + this.tb_variable.Text.Trim() + ")");
            this.flowLayoutPanel1.Controls.Add(ktb);
            kryptonButton1.Enabled = true;
        }
        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            if (this.tb_Customer.Text.Trim() == "")
                return;
            KryptonTextBox ktb = GetRuleTextBox(this.tb_Customer.Text.Trim());
            this.flowLayoutPanel1.Controls.Add(ktb);
            kryptonButton2.Enabled = false;
        }
        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            if (this.tb_identify.Text.Trim() == "")
                return;
            KryptonTextBox ktb = GetRuleTextBox(this.tb_identify.Text.Trim());
            this.flowLayoutPanel1.Controls.Add(ktb);
            kryptonButton3.Enabled = false;
        }
        /// <summary>
        /// 加入变量的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kbt_2_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "" | tb_maxsn.Text.Trim() == "")
            {
                this.ktg_message.Visible = true;
                this.ktb_message.Text = "!SN与最大值均不能为空";
                return;
            }
            int len = textBox2.Text.Length;
            int init = int.Parse(textBox2.Text);

            if (init > int.Parse(tb_maxsn.Text))
            {
                this.ktg_message.Visible = true;
                this.ktb_message.Text = "!最大值不能小于SN";
                return;
            }

            this.tb_currentSN.Text = int.Parse(textBox2.Text).ToString();
            this.tb_msn.Text = this.tb_maxsn.Text;

            string ruleS = "[SN:";
            ruleS += textBox2.Text.Trim();
            ruleS += "]";

            KryptonTextBox ktb = GetRuleTextBox(ruleS);
            this.flowLayoutPanel1.Controls.Add(ktb);
            this.kbt_2.Enabled = false;
        }

        private void kbt_3_Click(object sender, EventArgs e)
        {
            if (this.cb_y.SelectedIndex <= 0 & this.cb_spy.SelectedIndex <= 0)
            {
                this.ktg_message.Visible = true;
                this.ktb_message.Text = "没有选择任何年份规则";
                return;
            }

            string ruleS = "";
            if (this.cb_y.SelectedIndex > 0)  //正常规则
            {
                ruleS = "[Y:" + cb_y.Text + "]";
            }
            else  //特殊规则
            {
                ruleS = "{Y:" + cb_spy.Text + "}";
            }

            KryptonTextBox ktb = GetRuleTextBox(ruleS);
            this.flowLayoutPanel1.Controls.Add(ktb);
            this.kbt_3.Enabled = false;
        }

        private void kbt_4_Click(object sender, EventArgs e)
        {
            if (this.cb_m.SelectedIndex <= 0 & this.cb_spm.SelectedIndex <= 0)
            {
                this.ktg_message.Visible = true;
                this.ktb_message.Text = "没有选择任何月份规则";
                return;
            }

            string ruleS = "";
            if (this.cb_m.SelectedIndex > 0)  //正常规则
            {
                ruleS = "[M:" + cb_m.Text + "]";
            }
            else  //特殊规则
            {
                ruleS = "{M:" + cb_spm.Text + "}";
            }

            KryptonTextBox ktb = GetRuleTextBox(ruleS);
            this.flowLayoutPanel1.Controls.Add(ktb);
            this.kbt_4.Enabled = false;
        }


        private void kbt_5_Click(object sender, EventArgs e)
        {
            if (this.cb_d.SelectedIndex <= 0 & this.cb_spd.SelectedIndex <= 0)
            {
                this.ktg_message.Visible = true;
                this.ktb_message.Text = "没有选择任何月份规则";
                return;
            }

            string ruleS = "";
            if (this.cb_d.SelectedIndex > 0)  //正常规则
            {
                ruleS = "[D:" + cb_d.Text + "]";
            }
            else  //特殊规则
            {
                ruleS = "{D:" + cb_spd.Text + "}";
            }
            KryptonTextBox ktb = GetRuleTextBox(ruleS);
            this.flowLayoutPanel1.Controls.Add(ktb);
            this.kbt_5.Enabled = false;
        }
        /// <summary>
        /// 获取规则文本框的方法
        /// </summary>
        /// <returns></returns>
        private KryptonTextBox GetRuleTextBox(string text)
        {
            ButtonSpecAny bsa1 = new ButtonSpecAny();
            ButtonSpecAny bsa2 = new ButtonSpecAny();
            ButtonSpecAny bsa3 = new ButtonSpecAny();

            // 
            // buttonSpecAny1
            // 
            bsa1.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.Close;
            bsa1.UniqueName = "2600D6A1691343B72600D6A1691343B7";
            bsa1.Click += new System.EventHandler(this.buttonSpecAny1_Click_1);
            // 
            // buttonSpecAny8
            // 
            bsa2.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowLeft;
            bsa2.UniqueName = "81A6EAED0E4A49478AB7CCC0D19512A1";
            bsa2.Click += new System.EventHandler(this.buttonSpecAny1_Click_2);
            // 
            // buttonSpecAny9
            // 
            bsa3.Type = ComponentFactory.Krypton.Toolkit.PaletteButtonSpecStyle.ArrowRight;
            bsa3.UniqueName = "D44D64BE0EFF49C4C18C8DA6C886E6DD";
            bsa3.Click += new System.EventHandler(this.buttonSpecAny1_Click_3);

            KryptonTextBox ktb = new KryptonTextBox();
            ktb.AllowButtonSpecToolTips = true;
            ktb.ButtonSpecs.AddRange(new ComponentFactory.Krypton.Toolkit.ButtonSpecAny[] {
            bsa1,
            bsa2,
            bsa3});
            ktb.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            ktb.Location = new System.Drawing.Point(391, 96);
            ktb.Name = "ktb_module";
            ktb.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            ktb.ReadOnly = true;
            ktb.Size = new System.Drawing.Size(157, 23);
            ktb.TabIndex = 2;
            ktb.Visible = true;
            ktb.Text = text;

            return ktb;
        }

        private void buttonSpecAny1_Click_1(object sender, EventArgs e)
        {
            //MessageBox.Show(((KryptonTextBox)((ButtonSpecAny)sender).Owner).Text);
            if (((Control)((ButtonSpecAny)sender).Owner).Text.IndexOf("[SN:") > -1)
                this.kbt_2.Enabled = true;
            this.flowLayoutPanel1.Controls.Remove((Control)((ButtonSpecAny)sender).Owner);
            flowLayoutPanel1_ControlAdded(null, new ControlEventArgs(null));
        }
        private void buttonSpecAny1_Click_2(object sender, EventArgs e)
        {
            //MessageBox.Show(((KryptonTextBox)((ButtonSpecAny)sender).Owner).Text);
            this.flowLayoutPanel1.Controls.SetChildIndex(((KryptonTextBox)((ButtonSpecAny)sender).Owner), this.flowLayoutPanel1.Controls.GetChildIndex(((KryptonTextBox)((ButtonSpecAny)sender).Owner)) - 1);
            flowLayoutPanel1_ControlAdded(null, new ControlEventArgs(null));
        }
        private void buttonSpecAny1_Click_3(object sender, EventArgs e)
        {
            //MessageBox.Show(((KryptonTextBox)((ButtonSpecAny)sender).Owner).Text);
            this.flowLayoutPanel1.Controls.SetChildIndex(((KryptonTextBox)((ButtonSpecAny)sender).Owner), this.flowLayoutPanel1.Controls.GetChildIndex(((KryptonTextBox)((ButtonSpecAny)sender).Owner)) + 1);
            flowLayoutPanel1_ControlAdded(null, new ControlEventArgs(null));
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && (e.KeyChar != 8))
                e.Handled = true;
            base.OnKeyPress(e);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string sn = this.textBox2.Text;
            double n = Math.Pow(10, sn.Length) - 1;
            this.tb_maxsn.Text = n.ToString();
        }

        private void ktb_kown_Click(object sender, EventArgs e)
        {
            this.ktg_message.Visible = false;
        }

        private void flowLayoutPanel1_ControlAdded(object sender, ControlEventArgs e)
        {
            string ruletext = "";
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                ruletext += ctrl.Text.Trim();
            }
            this.ktb_ruleText.Text = ruletext;
            if (ruletext.IndexOf("[SN:") > -1)
            {
                this.kbt_2.Enabled = false;
            }
            else
            {
                this.kbt_2.Enabled = true;
                this.tb_msn.Text = "";
                this.tb_currentSN.Text = "";
            }
        }

        private void flowLayoutPanel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            string ruletext = "";
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                ruletext += ctrl.Text.Trim();
            }
            this.ktb_ruleText.Text = ruletext;
        }
        #region
        /// <summary>
        /// 绑定特殊日期的方法
        /// </summary>
        void BindSpe()
        {
            //DataTable dty = Objs.SpecialRule.Get_Y_SpecialName();
            //DataRow rowy = dty.NewRow();
            //rowy[0] = "";
            //dty.Rows.InsertAt(rowy, 0);

            //DataTable dtm = Objs.SpecialRule.Get_M_SpecialName();
            //DataRow rowm = dtm.NewRow();
            //rowm[0] = "";
            //dtm.Rows.InsertAt(rowm, 0);

            //DataTable dtd = Objs.SpecialRule.Get_D_SpecialName();
            //DataRow rowd = dtd.NewRow();
            //rowd[0] = "";
            //dtd.Rows.InsertAt(rowd, 0);

            //this.cb_spy.DisplayMember = "speName";
            //this.cb_spm.DisplayMember = "speName";
            //this.cb_spd.DisplayMember = "speName";

            //this.cb_spy.DataSource = dty;
            //this.cb_spm.DataSource = dtm;
            //this.cb_spd.DataSource = dtd;
        }
        #endregion
        #region  年月日下拉框的关联
        private void cb_spy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_spy.SelectedIndex > 0)
            //{
            //    this.cb_y.SelectedIndex = 0;
            //}
        }

        private void cb_spm_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_spm.SelectedIndex > 0)
            //{
            //    this.cb_m.SelectedIndex = 0;
            //}
        }

        private void cb_spd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_spd.SelectedIndex > 0)
            //{
            //    this.cb_d.SelectedIndex = 0;
            //}
        }

        private void cb_y_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_y.SelectedIndex > 0)
            //{
            //    this.cb_spy.SelectedIndex = 0;
            //}
        }

        private void cb_m_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cb_m.SelectedIndex > 0)
            //{
            //    this.cb_spm.SelectedIndex = 0;
            //}
        }

        private void cb_d_SelectedIndexChanged(object sender, EventArgs e)
        {
            //   if (cb_d.SelectedIndex > 0)
            //  {
            //     this.cb_d.SelectedIndex = 0;
            // }
        }
        #endregion
        //保存
        private void kryptonButton6_Click(object sender, EventArgs e)
        {
            if (this.tb_Copies.Text.Trim() == "")
            {
                MessageBox.Show("请输入打印份数");
                return;
            }
            if (this.tb_mid.Text.Trim() == "")
            {
                MessageBox.Show("请输入式样编号");
                return;
            }
            if (this.tb_mName.Text.Trim() == "")
            {
                MessageBox.Show("请输入式样名称");
                return;
            }
            if (this.tb_msn.Text.Trim() == "")
            {
                MessageBox.Show("请输入最大SN");
                return;
            }
            if (this.tb_filePath.Text.Trim() == "")
            {
                MessageBox.Show("请选择打印模板路径");
                return;
            }
            if (this.tb_filePath.Text.IndexOf(".Lab") < 0)
            {
                MessageBox.Show("请选择正确的模板保存");
                return;
            }
            if (this.cb_setZero.SelectedIndex < 0)
            {
                MessageBox.Show("请选择归零方式");
                return;
            }
            string var = string.Empty;
            for (int i = 0; i < ckBox_variable.CheckedItems.Count; i++)
            {
                if (i < ckBox_variable.CheckedItems.Count - 1)
                {
                    var += ((ComboboxItem)ckBox_variable.CheckedItems[i]).Value + ',';
                }
                else
                {
                    var += ((ComboboxItem)ckBox_variable.CheckedItems[i]).Value;
                }
            }

            P_SSW_TemplateList TemplateList = new P_SSW_TemplateList()
            {
                Template_id = this.tb_mid.Text.ToString(),
                Template_name = this.tb_mName.Text.ToString(),
                product_code = this.Product_textBox.Text.ToString(),
                RuleStr = this.ktb_ruleText.Text.ToString(),
                inputTime = DateTime.Now,
                currentSN = int.Parse(this.tb_currentSN.Text),
                //maxSN=int.Parse(this.tb_maxsn.Text),
                maxSN = moduleID != "" ? int.Parse(this.tb_msn.Text) : int.Parse(this.tb_maxsn.Text),
                setZero = this.cb_setZero.Text.ToString(),
                zeroDate = DateTime.Now,
                TemplatePath = this.tb_filePath.Text,
                faxType = rb2.Checked ? 2 : 1,
                checkCode = kcb_checkCode.Checked ? 1 : 0,
                variable = var,
                printCopies = Convert.ToInt32(tb_Copies.Text.ToString())
            };

            if (this.moduleID != "")   //修改
            {
                try
                {
                    string strJson = JsonToolsNet.ObjectToJson(TemplateList);
                    Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateTemplate", moduleID + "|" + strJson);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("修改成功！");
                    this.Hide();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("修改失败:" + exc.Message);
                    return;
                }
            }
            else                      //新增
            {
                try
                {
                    string strJson = JsonToolsNet.ObjectToJson(TemplateList);
                    Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertTemplate", strJson);
                    this.DialogResult = DialogResult.OK;
                    MessageBox.Show("新增成功！");
                    this.Hide();
                }
                catch (Exception exc)
                {
                    MessageBox.Show("新增失败:" + exc.Message);
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BindPath();
        }

        /// <summary>
        /// 装载指定的指图文件数据
        /// </summary>
        void BindPath()
        {
            OpenFileDialog openFiledialog1 = new OpenFileDialog();
            //openFiledialog1.Filter = "Excel Files|*.xls|Excel Files|*.xlsx";
            //this.dataGridView1.DataSource = DB.Database.getDataTable("select * from [WA50$]");
            if (openFiledialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFiledialog1.FileName;
                this.tb_filePath.Text = FileName;
            }
        }
    }
    //构造函数ComboboxItem
    public class ComboboxItem
    {
        private string value;
        private string text;
        public ComboboxItem(string _value, string _text)
        {
            value = _value;
            text = _text;
        }
        public string Text
        {
            get { return text; }
        }
        public string Value
        {
            get { return value; }
        }
        public override string ToString()
        {
            return text;
        }

    }
}
