using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.Reflection;

namespace LEMES_POD
{
    public partial class FrmRepairTool : Form
    {

        Dictionary<String, String> dicTestProcessInfo = new Dictionary<string, string>(); //保存测试工位
        Dictionary<String, String> dicTestStationInfo = new Dictionary<string, string>(); //保存测试工位
        BindingList<NGCode_Data> lstngcode = new BindingList<NGCode_Data>();       //不良现象记录表
        Dictionary<string, string> dicProductTypeInfo = new Dictionary<string, string>(); //保存产品类型

        public string g_strWorkShopCode = "001";  //车间代码
        public string g_stremp = "10000";//操作员工编号
        private string strGroupCode = "";         //产品所在工序组

        int iRepairProcc = 0; //等级模式，0-正常等级，3-报废

        public FrmRepairTool()
        {
            InitializeComponent();
        }
        private void FrmRepairTool_Load(object sender, EventArgs e)
        {
            txtBox_registeredsfc.Focus();
            this.WindowState = FormWindowState.Maximized; //最大化窗口
            //是否需要拆分批次
            txtBox_newregisteredsfc.Enabled = false;
            KtxtBox_newregisteredsfcNum.Enabled = false;
            label1.Visible = false;
            txtBox_newregisteredsfc.Visible = false;
            label2.Visible = false;
            KtxtBox_newregisteredsfcNum.Visible = false;
            tabControl1.SelectedIndex = 0;                      //指定当前Tab页面

            //tabPage_Rework.Parent = null;                     //隐藏Tab选项卡

        }
        //strMsg 提示信息，
        //iErrorLvl 0-正常，1-提示，2-错误
        public void SendMsgShow(string strMsg, int iErrorLvl)
        {
            try
            {
                textBox_msg.Text = strMsg;
                if (iErrorLvl == 0)
                {
                    textBox_msg.ForeColor = Color.Black;
                    textBox_msg.BackColor = Color.GreenYellow;
                    //textBox_msg.Font.
                }
                else if (iErrorLvl == 1)
                {
                    textBox_msg.ForeColor = Color.Black;
                    textBox_msg.BackColor = Color.Yellow;
                }
                else
                {
                    textBox_msg.ForeColor = Color.Yellow;
                    textBox_msg.BackColor = Color.Red;
                }
                LogClass.WriteLogFile(strMsg);
                Application.DoEvents();   //处理所有的当前在消息队列中的Windows消息
            }
            catch (Exception exp)
            {
                MessageBox.Show("显示提示信息失败:" + exp.Message, "错误");
                LogClass.WriteLogFile("显示提示信息失败:" + exp.Message);
            }
        }
        private void ClearFormPar()
        {
            try
            {
                ktxtBox_orderno.Text = "";
                kComboBox_Process1.Text = "";
                kComboBox_Process1.Items.Clear();
                dicTestProcessInfo.Clear();
                kTextBox_failtimes.Text = "";
                kComboBox_TestStation.Text = "";
                kTextBox_ProductType.Text = "";
                kTxtBox_SFCQty.Text = "";
                kComboBox_TestStation.Items.Clear();
                dicTestStationInfo.Clear();
                txtBox_empname.Text = "";
                lstngcode.Clear();
                dataGridView_faildesc.DataSource = lstngcode;
                richTextBox_ng_remark.Text = "说明:";
                kryptonCheckBox1.Checked = false;
                txtBox_newregisteredsfc.Text = "";
                KtxtBox_newregisteredsfcNum.Text = "";
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "提示");
            }

        }
        //显示行号
        private void dataGridView_faildesc_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                                                e.RowBounds.Location.Y,
                                                dataGridView_faildesc.RowHeadersWidth - 4,
                                                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                        dataGridView_faildesc.RowHeadersDefaultCellStyle.Font,
                                        rectangle,
                                        dataGridView_faildesc.RowHeadersDefaultCellStyle.ForeColor,
                                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
        private void txtBox_registeredsfc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_getsfcinfo_Click(null, null);
            }
        }
        //根据SFC查询sfc信息，以及不良现象信息
        private void btn_getsfcinfo_Click(object sender, EventArgs e)
        {
            //frmPBar.Close();
            iRepairProcc = 0;
            ClearFormPar();
            kTxtBox_SFCQty.Enabled = false;
            string strProductTypeCode = "";
            string strProductTypeName = "";
            if (string.IsNullOrWhiteSpace(txtBox_registeredsfc.Text))
            {
                SendMsgShow("SFC 信息为空，请检查", 2);
                return;
            }
            List<B_ProcessList> lstprocess = new List<B_ProcessList>();
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string str = client.RunServerAPI("BLL.Pack", "GetSfcState", txtBox_registeredsfc.Text);
            if (string.IsNullOrWhiteSpace(str))
            {
                //如果首工序失败，P_SFC_State 表中不会有记录，此时需要登记失败，就取该工单的工艺流程的首工序作为当前批次所在工序；
                string strorderno = "";
                client = new ServiceReference.ServiceClient();
                string dt = client.RunServerAPI("BLL.SSW", "GetWorkerOrderbyBar", txtBox_registeredsfc.Text);//根据条码，获取工单
                if (string.IsNullOrWhiteSpace(dt))
                {
                    //没有数据
                    SendMsgShow("根据批次[" + txtBox_registeredsfc.Text + "]获取工单失败", 2);
                    return;
                }
                List<P_BarCodeBing> lstpbarcodebing = new List<P_BarCodeBing>();
                lstpbarcodebing = JsonConvert.DeserializeObject<List<P_BarCodeBing>>(dt);
                if (lstpbarcodebing.Count <= 0)
                {
                    //没有数据
                    SendMsgShow("根据批次[" + txtBox_registeredsfc.Text + "]获取工单失败", 2);
                    return;
                }
                strorderno = lstpbarcodebing[0].order;
                lstpbarcodebing.Clear();

                dt = "";
                client = new ServiceReference.ServiceClient();
                //string StrOrderInfo = client.RunServerAPI("BLL.SSW", "GetMainOrderByOrderNo", strorderno);
                //List<P_WorkOrder> ListWork = JsonConvert.DeserializeObject<List<P_WorkOrder>>(StrOrderInfo);
                //string mainorder = string.Empty;
                //if (ListWork != null)
                //{
                //    mainorder = ListWork[0].main_order.ToString();
                //}

                dt = client.RunServerAPI("BLL.Process", "GetProcessFlowDetail", strorderno);//根据工单，获取工艺流，根据工艺流得到工序
                if (string.IsNullOrWhiteSpace(dt))
                {
                    //没有数据
                    SendMsgShow("获取到当前工序集合失败", 2);
                    return;
                }

                lstprocess = JsonConvert.DeserializeObject<List<B_ProcessList>>(dt);
                if (lstprocess.Count <= 0)
                {
                    //没有数据
                    SendMsgShow("获取到当前工序集合失败", 2);
                    return;
                }
                for (int i = 0; i < lstprocess.Count; i++)
                {
                    dicTestProcessInfo.Add(lstprocess[i].process_name, lstprocess[i].process_code);
                    kComboBox_Process1.Items.Add(lstprocess[i].process_name);   //责任工序列表
                }
                kComboBox_Process1.SelectedItem = -1;
                ktxtBox_orderno.Text = strorderno;
                strGroupCode = lstprocess[0].group_code;
            }
            else
            {
                List<P_SFC_State> dt = JsonConvert.DeserializeObject<List<P_SFC_State>>(str);
                if (dt.Count > 0)
                {
                    ktxtBox_orderno.Text = dt[0].order_no;
                    kTxtBox_SFCQty.Text = dt[0].qty.ToString();
                    //???
                }
                else
                {
                    SendMsgShow("获取SFC[" + txtBox_registeredsfc.Text + "]信息失败", 2);
                    return;
                }
                List<B_ProcessList> lstTmpprocess = new List<B_ProcessList>();
                string strlstProcess = "";
                client = new ServiceReference.ServiceClient();
                //根据子工单找到主工单
                //string StrOrderInfo = client.RunServerAPI("BLL.SSW", "GetMainOrderByOrderNo", ktxtBox_orderno.Text);
                //List<P_WorkOrder> ListWork = JsonConvert.DeserializeObject<List<P_WorkOrder>>(StrOrderInfo);
                //string mainorder = string.Empty;
                //if (ListWork != null)
                //{
                //    mainorder = ListWork[0].main_order.ToString();
                //}
                //strlstProcess = client.RunServerAPI("BLL.Process", "GetProcessFlowDetail", mainorder);//根据工单，获取工艺流，根据工艺流得到工序
                strlstProcess = client.RunServerAPI("BLL.Process", "GetProcessFlowDetail_1", ktxtBox_orderno.Text + "," + txtBox_registeredsfc.Text.ToString().Trim());
                if (string.IsNullOrWhiteSpace(strlstProcess))
                {
                    //没有数据
                    SendMsgShow("获取到当前工序组失败", 2);
                    return;
                }
                lstTmpprocess = JsonConvert.DeserializeObject<List<B_ProcessList>>(strlstProcess);
                if (lstTmpprocess.Count <= 0)
                {
                    //没有数据
                    SendMsgShow("获取到当前工序组失败", 2);
                    return;
                }
                for (int i = 0; i < lstTmpprocess.Count; i++)
                {
                    dicTestProcessInfo.Add(lstTmpprocess[i].process_name, lstTmpprocess[i].process_code);
                    kComboBox_Process1.Items.Add(lstTmpprocess[i].process_name);   //责任工序列表
                }
                kComboBox_Process1.SelectedItem = -1;
                strGroupCode = lstTmpprocess[0].group_code;
            }

            SendMsgShow("获取批次[" + txtBox_registeredsfc.Text + "]相关信息完成", 0);

            if (string.IsNullOrWhiteSpace(kTxtBox_SFCQty.Text))
            {
                kTxtBox_SFCQty.Enabled = true;
                kTxtBox_SFCQty.Text = "";
            }
            //根据工单获取产品类型
            str = "";
            str = client.RunServerAPI("BLL.Product", "GetProductInfoByOrder", ktxtBox_orderno.Text);
            if (string.IsNullOrWhiteSpace(str))
            {
                SendMsgShow("根据工单[" + ktxtBox_orderno.Text + "]获取产品类型失败", 2);
                return;
            }
            dicProductTypeInfo.Clear();
            List<V_Order_Produc_Type_Name> lstvorderProductInfo = JsonConvert.DeserializeObject<List<V_Order_Produc_Type_Name>>(str);
            if (lstvorderProductInfo.Count > 0)
            {
                strProductTypeCode = lstvorderProductInfo[0].typecode;
                strProductTypeName = lstvorderProductInfo[0].type_name;
                dicProductTypeInfo.Add(strProductTypeName, strProductTypeCode);
            }
            if (string.IsNullOrWhiteSpace(strProductTypeCode))
            {
                SendMsgShow("根据工单[" + ktxtBox_orderno.Text + "]获取产品类型失败", 2);
                return;
            }
            kTextBox_ProductType.Text = strProductTypeName;

            //根据SFC在P_Faillog表中查询失败次数
            str = "";
            str = client.RunServerAPI("BLL.Faillog", "GetFailTimesBySFC", txtBox_registeredsfc.Text);
            if (string.IsNullOrWhiteSpace(str))
            {
                SendMsgShow("SFC[" + txtBox_registeredsfc.Text + "]维修次数为 0", 0);
                kTextBox_failtimes.Text = "0";
            }
            else
            {
                List<P_FailLog> dt = JsonConvert.DeserializeObject<List<P_FailLog>>(str);
                kTextBox_failtimes.Text = dt.Count.ToString();
                if (dt.Count > 0)
                {
                    for (int i = 0; i < dt.Count; i++)
                    {
                        if (dt[i].state == 0)
                        {
                            SendMsgShow("SFC[" + txtBox_registeredsfc.Text + "]存在未维修记录", 2);
                            return;
                        }
                    }
                }
            }

            //获取不良现象  B_NG_Code表
            str = "";
            str = client.RunServerAPI("BLL.NGCode", "GetNGCodeByType", dicProductTypeInfo[kTextBox_ProductType.Text]);
            if (str.Length == 0)
            {
                SendMsgShow("获取不良现象信息失败", 2);
                return;
            }
            else
            {
                lstngcode.Clear();
                List<B_NG_Code> dtngcode = JsonConvert.DeserializeObject<List<B_NG_Code>>(str);
                if (dtngcode.Count > 0)
                {
                    foreach (var par in dtngcode)
                    {
                        if (string.IsNullOrWhiteSpace(par.ng_code)
                            || string.IsNullOrWhiteSpace(par.ng_name)
                            || string.IsNullOrWhiteSpace(par.type_code))
                        {
                            SendMsgShow("不良现象信息不全，请检查", 2);
                            return;
                        }
                        NGCode_Data clngcode = new NGCode_Data();
                        clngcode.failPhenomenoncode = par.ng_code;
                        clngcode.failPhenomenondesc = par.ng_name;
                        clngcode.failtypecode = par.type_code;
                        clngcode.strfailNum = "0";
                        clngcode.isProcess = par.exec_proc == 0 ? "否" : "是";
                        clngcode.bselect = false;
                        lstngcode.Add(clngcode);
                    }
                    dataGridView_faildesc.DataSource = lstngcode;
                }
                else
                {
                    SendMsgShow("获取不良现象信息失败", 2);
                    return;
                }
            }
        }
        private void kComboBox_Process1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            try
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                //根据工序获取工位信息
                if (kComboBox_Process1.Text != "结束")
                {
                    str = "";
                    str = client.RunServerAPI("BLL.Station", "GetStationListByProcess", dicTestProcessInfo[kComboBox_Process1.Text]);
                    if (str.Length == 0)
                    {
                        SendMsgShow("获取测试工位信息失败", 2);
                        return;
                    }
                    else
                    {
                        List<B_StationList> dtteststation = JsonConvert.DeserializeObject<List<B_StationList>>(str);
                        if (dtteststation.Count > 0)
                        {
                            foreach (var par in dtteststation)
                            {
                                string strstationcode = par.station_code;
                                string strstationname = par.station_name;
                                if (!string.IsNullOrWhiteSpace(strstationcode)
                                   && !string.IsNullOrWhiteSpace(strstationname))
                                {
                                    //dicTestStationInfo.Add(strstationname, strstationcode);
                                    dicTestStationInfo[strstationname] = strstationcode;
                                    this.kComboBox_TestStation.Items.Add(strstationname);
                                }
                            }

                        }
                        else
                        {
                            SendMsgShow("获取测试工位信息失败", 2);
                            return;
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                SendMsgShow(exp.Message, 2);
            }
        }

        #region
        //提交fail 登记
        private void btn_addfaillog_Click(object sender, EventArgs e)
        {
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            try
            {
                string strfailsfc = txtBox_registeredsfc.Text;
                if (string.IsNullOrWhiteSpace(txtBox_registeredsfc.Text))
                {
                    SendMsgShow("SFC 信息为空，请检查", 2);
                    return;
                }
                if (string.IsNullOrWhiteSpace(ktxtBox_orderno.Text))
                {
                    SendMsgShow("工单 信息为空，请检查", 2);
                    return;
                }
                if (string.IsNullOrWhiteSpace(kComboBox_Process1.Text))
                {
                    SendMsgShow("测试工序 信息为空，请检查", 2);
                    return;
                }
                //if (string.IsNullOrWhiteSpace(kComboBox_TestStation.Text))
                //{
                //    SendMsgShow("测试工位 信息为空，请检查", 2);
                //    return;
                //}
                //if (string.IsNullOrWhiteSpace(txtBox_empname.Text))
                //{
                //    SendMsgShow("测试人 信息为空，请检查", 2);
                //    return;
                //}
                //检查员工号是否正常？？？？

                double dfailqty = 0;
                //是否需要拆分批次
                if (kryptonCheckBox1.Checked == true)
                {
                    if (string.IsNullOrWhiteSpace(txtBox_newregisteredsfc.Text))
                    {
                        SendMsgShow("新的批次号为空，请检查", 2);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(KtxtBox_newregisteredsfcNum.Text))
                    {
                        SendMsgShow("新的批次号的数量为空，请检查", 2);
                        return;
                    }
                    if (double.Parse(KtxtBox_newregisteredsfcNum.Text.Trim()) - double.Parse(kTxtBox_SFCQty.Text.Trim()) >= 0.000001)
                    {
                        dfailqty = 0;
                        MessageBox.Show("拆分批次数量大于或等于总数量 ", "错误");
                        return;
                    }
                    strfailsfc = txtBox_newregisteredsfc.Text;
                }
                //////////////////////////////////////////////////////
                string wipdata = client.RunServerAPI("BLL.WIP", "GetWipQty1ByLot", txtBox_registeredsfc.Text);
                if (!string.IsNullOrEmpty(wipdata))
                {
                    decimal SumNum = 0;
                    for (int i = 0; i < dataGridView_faildesc.RowCount; i++)
                    {
                        decimal num = Convert.ToDecimal(this.dataGridView_faildesc.Rows[i].Cells["strfailNum"].Value.ToString());
                        if (num != 0)
                        {
                            SumNum += num;
                        }
                    }
                    decimal wipqty = Convert.ToDecimal(wipdata);
                    if (SumNum > wipqty)
                    {
                        MessageBox.Show("该批次已使用，线边仓剩余数量小于登记不良数量 ", "提示");
                        return;
                    }
                }
                /////////////////////////////////////////////////////
                try
                {
                    if (KtxtBox_newregisteredsfcNum.Enabled == false)
                    {
                        dfailqty = double.Parse(kTxtBox_SFCQty.Text.Trim());
                    }
                    else
                    {
                        dfailqty = double.Parse(KtxtBox_newregisteredsfcNum.Text.Trim());
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("请输入正确数量 " + exp.Message, "提示");
                    return;
                }
                int iisProcess = 0;   // 提交维修登记时，如果现象中都是不需要处理的，则维修状态为-1=不需要处理，否则维修状态为0=需要处理；
                List<NGCode_Data> lstcurrngcode = new List<NGCode_Data>();
                for (int i = 0; i < dataGridView_faildesc.Rows.Count; i++)
                {
                    //如果DataGridView是可编辑的，将数据提交，否则处于编辑状态的行无法取到
                    dataGridView_faildesc.EndEdit();
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView_faildesc.Rows[i].Cells["bselect"];
                    Boolean flag = Convert.ToBoolean(checkCell.Value);
                    if (flag == true)     //查找被选择的数据行
                    {
                        //从 DATAGRIDVIEW 中获取数据项
                        NGCode_Data clngcode = new NGCode_Data();
                        clngcode.failPhenomenoncode = dataGridView_faildesc.Rows[i].Cells[0].Value.ToString().Trim();
                        clngcode.failPhenomenondesc = dataGridView_faildesc.Rows[i].Cells[1].Value.ToString().Trim();
                        clngcode.failtypecode = dataGridView_faildesc.Rows[i].Cells[2].Value.ToString().Trim();
                        clngcode.strfailNum = dataGridView_faildesc.Rows[i].Cells[3].Value.ToString().Trim();
                        clngcode.isProcess = dataGridView_faildesc.Rows[i].Cells[4].Value.ToString().Trim();
                        if (clngcode.isProcess == "是")
                        {
                            iisProcess = 1;
                        }
                        double dfailNum = double.Parse(clngcode.strfailNum);
                        if (dfailNum <= 0.0000001)
                        {
                            SendMsgShow("不良现象[" + clngcode.failPhenomenoncode + "]对应数量为0 请检查", 2);
                            lstcurrngcode.Clear();
                            return;
                        }
                        if ((dfailNum - dfailqty) > 0.0000001)
                        {
                            SendMsgShow("不良现象[" + clngcode.failPhenomenoncode + "]对应数量大于总数量, 请检查", 2);
                            lstcurrngcode.Clear();
                            return;
                        }
                        lstcurrngcode.Add(clngcode);
                    }
                }
                if (lstcurrngcode.Count <= 0)
                {
                    SendMsgShow("未选择不良现象, 请检查", 2);
                    return;
                }

                if (kryptonCheckBox1.Checked)
                {
                    if (txtBox_newregisteredsfc.Text == txtBox_registeredsfc.Text)
                    {
                        SendMsgShow("拆分新批次不能与批次号相同", 2);
                        return;
                    }
                    string jsdata = client.RunServerAPI("BLL.SFC", "GetSfcState", txtBox_newregisteredsfc.Text);
                    if (!string.IsNullOrEmpty(jsdata))
                    {
                        SendMsgShow("拆分批次号已存在，请更改", 2);
                        return;
                    }
                    //如果有新的批次号，需要将主批次号的过站记录复制到新的批次号中
                    int iRet = CreateNewSFCInfo(txtBox_registeredsfc.Text, txtBox_newregisteredsfc.Text, double.Parse(KtxtBox_newregisteredsfcNum.Text.Trim()));
                    if (iRet == 0)
                    {
                        //strfailsfc = txtBox_newregisteredsfc.Text;
                        //kTxtBox_SFCQty.Text = KtxtBox_newregisteredsfcNum.Text;
                    }
                    else
                    {
                        MessageBox.Show("拆分批次失败 ", "错误");
                        return;
                    }
                }
                //在P_FailLog表中增加不良记录
                //获取P_Date
                string str = "";
                str = client.RunServerAPI("BLL.Faillog", "GetPDate", strGroupCode);
                DateTime dPdate = new DateTime();
                dPdate = DateTime.Parse(str);

                P_FailLog dtpfaillog = new P_FailLog();
                dtpfaillog.fguid = Guid.NewGuid().ToString();
                dtpfaillog.order_no = ktxtBox_orderno.Text;
                dtpfaillog.sfc = strfailsfc;
                //dtpfaillog.from_process=

                dtpfaillog.from_process = dicTestProcessInfo[kComboBox_Process1.Text];     //来源工序id
                //dtpfaillog.from_station = dicTestStationInfo[kComboBox_TestStation.Text];   //来源工站id
                dtpfaillog.process_code = dicTestProcessInfo[kComboBox_Process1.Text];   //当前工序id，责任工序，先预设维修工序id，再在维修中来修改
                dtpfaillog.from_emp = g_stremp;
                dtpfaillog.fail_times = 1;
                dtpfaillog.p_date = dPdate;
                dtpfaillog.class_code = client.RunServerAPI("BLL.Faillog", "GetClassCode", strGroupCode);
                dtpfaillog.Disposal_Process = g_stremp;
                dtpfaillog.ws_code = g_strWorkShopCode;

                if (iRepairProcc == 3)
                {
                    dtpfaillog.state = 9;  //报废
                    iisProcess = 1;
                }
                else
                {
                    if (iisProcess == 1)
                    {
                        dtpfaillog.state = 0;   //后续会根据不良现象来判断是否需要给“-1”，
                    }
                    else
                    {
                        dtpfaillog.state = -1;
                    }
                }
                dtpfaillog.qty = (decimal)dfailqty;
                dtpfaillog.ng_remark = richTextBox_ng_remark.Text.Trim();

                string strJson = JsonConvert.SerializeObject(dtpfaillog);
                str = "";
                str = client.RunServerAPI("BLL.Faillog", "AddFailLog", strJson);
                if (string.IsNullOrWhiteSpace(str)
                        || str == "0")
                {
                    SendMsgShow("增加不良记录失败", 2);
                    return;
                }
                //在P_Fail_Detail表中增加不良记录
                for (int i = 0; i < lstcurrngcode.Count; i++)
                {
                    P_Fail_Detail clpfiledetail = new P_Fail_Detail();
                    clpfiledetail.fguid = dtpfaillog.fguid;
                    clpfiledetail.order_no = ktxtBox_orderno.Text;
                    clpfiledetail.sfc = strfailsfc;
                    clpfiledetail.ng_code = lstcurrngcode[i].failPhenomenoncode;
                    clpfiledetail.qty = decimal.Parse(lstcurrngcode[i].strfailNum);
                    clpfiledetail.ws_code = g_strWorkShopCode;
                    strJson = JsonConvert.SerializeObject(clpfiledetail);
                    str = "";
                    str = client.RunServerAPI("BLL.Fail_Detail", "AddFail_Detail", strJson);
                    if (string.IsNullOrWhiteSpace(str)
                        || str == "0")
                    {
                        SendMsgShow("增加不良明细失败", 2);
                        return;
                    }
                }
                //更新P_SFC_State 当前工序和过站时间 +P_SFC_Process_IOLog 当前工序和工站
                if (iisProcess == 1)   //如果需要处理，才更新state表信息？？？
                {
                    str = "";
                    //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
                    strJson = ktxtBox_orderno.Text + "," + strfailsfc + "," + dicTestProcessInfo[kComboBox_Process1.Text]
                        + "," + "0" + "," + "1" + "," + "0" + "," + "grade_id" + "," + "grade_type" + "," + "1" + "," + dicTestProcessInfo[kComboBox_Process1.Text];
                    str = client.RunServerAPI("BLL.SFC", "UpDataSFCInfoAndSFCIOLogData", strJson);
                    if (!str.Contains("OK"))
                    {
                        SendMsgShow("更新P_SFC_State 失败 Or P_SFC_Process_IOLog 失败，" + str, 2);
                        return;
                    }
                }
                SendMsgShow("提交不良信息已完成", 0);
                //判断是否需要快速报废
                if (iRepairProcc == 3)
                {
                    P_SFC_State clpsfcstateTmp = new P_SFC_State();
                    clpsfcstateTmp.order_no = ktxtBox_orderno.Text;
                    clpsfcstateTmp.SFC = strfailsfc;
                    clpsfcstateTmp.fail_times = 0;

                    clpsfcstateTmp.state = -1;  //设置状态为 -1 ，已报废
                    clpsfcstateTmp.now_process = "END";
                    //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
                    strJson = clpsfcstateTmp.order_no + "," + strfailsfc + "," + "END" + "," + "-1" + "," + "0" + "," + "0" + "," + "grade_id" + "," + "grade_type" + "," + "0" + "," + dicTestProcessInfo[kComboBox_Process1.Text];

                    str = client.RunServerAPI("BLL.SFC", "UpDataSFCInfoAndSFCIOLogData", strJson);
                    if (!str.Contains("OK"))
                    {
                        MessageBox.Show("更新P_SFC_State 失败 Or P_SFC_Process_IOLog 失败，" + str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                ClearFormPar();
                txtBox_registeredsfc.Text = "";
            }
            catch (Exception exp)
            {
                SendMsgShow(exp.Message, 2);
            }
            return;
        }
        #endregion
        //是否需要拆分批次
        private void kryptonCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            //是否需要拆分批次
            if (kryptonCheckBox1.Checked == true)
            {
                txtBox_newregisteredsfc.Enabled = true;
                KtxtBox_newregisteredsfcNum.Enabled = true;
                label1.Visible = true;
                txtBox_newregisteredsfc.Visible = true;
                label2.Visible = true;
                KtxtBox_newregisteredsfcNum.Visible = true;
            }
            else
            {
                txtBox_newregisteredsfc.Text = "";
                txtBox_newregisteredsfc.Enabled = false;
                KtxtBox_newregisteredsfcNum.Text = "";
                KtxtBox_newregisteredsfcNum.Enabled = false;
                label1.Visible = false;
                txtBox_newregisteredsfc.Visible = false;
                label2.Visible = false;
                KtxtBox_newregisteredsfcNum.Visible = false;
            }
        }
        private void dataGridView_faildesc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                if (Convert.ToString(dataGridView_faildesc.Rows[e.RowIndex].Cells["bselect"].Value).ToUpper() == "FALSE")
                {
                    dataGridView_faildesc.Rows[e.RowIndex].Cells["strfailNum"].Value = "0";
                    this.dataGridView_faildesc.CurrentCell = dataGridView_faildesc.Rows[e.RowIndex].Cells["strfailNum"];  //设置datagridview 单元格获得焦点且处于编辑状态
                    this.dataGridView_faildesc.CurrentCell.Style.BackColor = Color.White;
                }
                else
                {
                    this.dataGridView_faildesc.CurrentCell = dataGridView_faildesc.Rows[e.RowIndex].Cells["strfailNum"];  //设置datagridview 单元格获得焦点且处于编辑状态
                    this.dataGridView_faildesc.CurrentCell.Style.BackColor = Color.Yellow;
                    this.dataGridView_faildesc.BeginEdit(true);

                }
            }
        }
        private void dataGridView_faildesc_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                grid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void dataGridView_faildesc_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int x = dataGridView_faildesc.CurrentCellAddress.X;
            int y = dataGridView_faildesc.CurrentCellAddress.Y;
            if (dataGridView_faildesc[x, y].FormattedValue.ToString() == "") return;
        }
        //拆分批次，产生新的批次信息，P_SFC_State，P_SFC_Process_IOLog，P_SFC_ProcessData,P_Material_WIP
        private int CreateNewSFCInfo(string stroldsfc, string strnewsfc, double dfailqty)
        {
            int iRet = 0; //0-成功，非0-失败
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string str = "";
            str = client.RunServerAPI("BLL.JobSubmit", "CommitFailNewSFCInfo", stroldsfc + ";" + strnewsfc + ";" + dfailqty.ToString());
            if (str != "0")
            {
                iRet = 1;
                SendMsgShow("拆分批次 失败，" + str, 2);
                return iRet;
            }
            return iRet;
        }
        private void btn_discarde_Click(object sender, EventArgs e)
        {
            iRepairProcc = 3;//报废

            btn_addfaillog_Click(sender, e);
        }

        private void kryptonTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                dataGridView_faildesc.ClearSelection();
                string Phenomenoncode = this.kryptonTextBox1.Text.ToString().Trim().ToUpper();
                for (int i = 0; i < lstngcode.Count; i++)
                {
                    if (lstngcode[i].failPhenomenoncode.Contains(Phenomenoncode))
                    {
                        dataGridView_faildesc.Rows[i].Selected = true;
                    }
                }
            }
        }
    }
    public class NGCode_Data
    {
        //不良代码
        public string failPhenomenoncode { get; set; }
        //不良描述
        public string failPhenomenondesc { get; set; }
        //不良类型代码
        public string failtypecode { get; set; }
        //不良数量
        public string strfailNum { get; set; }
        //是否处理0-否，1-是
        public string isProcess { get; set; }
        //是否选择
        public bool bselect { get; set; }
    }
    public class FailLog_Data
    {
        //不良记录id
        public string fid { get; set; }
        //不良记录sfc
        public string sfc { get; set; }
        ////数量
        //public string qty { get; set; }
        //不良记录工单
        public string order_no { get; set; }
        //不良记录来源工序
        public string from_process_code { get; set; }
        //不良记录来源工序
        public string from_process_name { get; set; }
        ////不良记录来源工位
        //public string from_station_code { get; set; }
        ////不良记录来源工位
        //public string from_station_name { get; set; }
        //数量
        public double qty { get; set; }
    }
}
