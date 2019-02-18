using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LEDAO;
using Newtonsoft.Json;
using JsonTools;

namespace RepairTool
{
    public partial class FrmRepairProcess : Form
    {
        public string strsfc = "";
        public string strqty = "";
        public string strorderno = "";
        public string strfromprocess_code = "";
        public string strfromprocess_name = "";
        public string strfid = "";
        public string strfguid = "";
        Dictionary<string, string> diczrprocess = new Dictionary<string, string>();
        List<B_ProcessList> lstprocess = new List<B_ProcessList>();
        BindingList<NGCode_Reason_Data> blstNGCodeReasonData = new BindingList<NGCode_Reason_Data>();
        Dictionary<string, string> dicReason = new Dictionary<string, string>();    //不良原因字典
        Dictionary<string, string> dicTypeReason = new Dictionary<string, string>();  //不良原因类型字典
        Dictionary<string, B_Product_Grade> dicProductGrade = new Dictionary<string, B_Product_Grade>();//产品级别字典
        int iRepairProcc = 0;   //维修操作 0-不做操作，1-降级，2-待判，3-报废
        public FrmRepairProcess()
        {
            InitializeComponent();
        }

        private void FrmRepairProcess_Load(object sender, EventArgs e)
        {
            //参数检查
            if (string.IsNullOrWhiteSpace(strorderno))
            {
                MessageBox.Show("工单信息有误,请检查", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(strsfc))
            {
                MessageBox.Show("批次号信息有误,请检查", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(strqty))
            {
                MessageBox.Show("不良数量有误,请检查", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //批次降级后"待判"，"报废"不能使用，所以加载页面是重置一下
            btn_confirmed.Enabled = true;
            btn_discarde.Enabled = true;
            iRepairProcc = 0;

            lstprocess.Clear();
            textBox_sfc.Text = strsfc;
            textBox_sfcqty.Text = strqty;
            textBox_fromprocess.Text = strfromprocess_name;

            //GetProcessFlowDetail(strorderno);
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string dt = client.RunServerAPI("BLL.Process", "GetProcessFlowDetail", strorderno);//根据工单，获取工艺流，根据工艺流得到工序
            if (string.IsNullOrWhiteSpace(dt))
            {
                //没有数据
                MessageBox.Show("未获取到责任工序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lstprocess = JsonConvert.DeserializeObject<List<B_ProcessList>>(dt);
            //lstprocess.Add();
            for (int i = 0; i < lstprocess.Count; i++)
            {
                diczrprocess.Add(lstprocess[i].process_name, lstprocess[i].process_code);
                kComboBox_Process.Items.Add(lstprocess[i].process_name);   //责任工序列表
                kComboBox_rebackprocess.Items.Add(lstprocess[i].process_name);   //待返回工序列表
            }
            diczrprocess.Add("END", "END");
            kComboBox_rebackprocess.Items.Add("END");   //待返回工序列表
            kComboBox_Process.Text = strfromprocess_name;
            kComboBox_Process.SelectedValue = strfromprocess_code;
            string strProductTypeCode = "";
            string strProductTypeName = "";
            //根据工单获取产品类型
            dt = "";
            dt = client.RunServerAPI("BLL.Product", "GetProductInfoByOrder", strorderno);
            if (string.IsNullOrWhiteSpace(dt))
            {
                MessageBox.Show("根据工单[" + strorderno + "]获取产品类型失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<V_Order_Produc_Type_Name> lstvorderProductInfo = JsonConvert.DeserializeObject<List<V_Order_Produc_Type_Name>>(dt);
            if (lstvorderProductInfo.Count > 0)
            {
                strProductTypeCode = lstvorderProductInfo[0].typecode;
                strProductTypeName = lstvorderProductInfo[0].type_name;
            }
            if (string.IsNullOrWhiteSpace(strProductTypeCode))
            {
                MessageBox.Show("根据工单[" + strorderno + "]获取产品类型失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            kTextBox_ProductType.Text = strProductTypeName;

            dt = "";
            dt = client.RunServerAPI("BLL.Fail_Detail", "GetNgCode", strfid + "," + strProductTypeCode);//根据不良id，在Fail_Detail中获取不良现象
            if (string.IsNullOrWhiteSpace(dt))
            {
                //没有数据
                MessageBox.Show("获取不良现象失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<V_Fail_Detail_NGName> lstvfaildetailngname = new List<V_Fail_Detail_NGName>();
            lstvfaildetailngname = JsonConvert.DeserializeObject<List<V_Fail_Detail_NGName>>(dt);
            for (int i = 0; i < lstvfaildetailngname.Count; i++)
            {
                NGCode_Reason_Data ngcodereasondata = new NGCode_Reason_Data();
                ngcodereasondata.detail_id = lstvfaildetailngname[i].id;
                ngcodereasondata.failPhenomenoncode = lstvfaildetailngname[i].ng_code;
                ngcodereasondata.failPhenomenondesc = lstvfaildetailngname[i].ng_name;
                ngcodereasondata.failNum = (double)lstvfaildetailngname[i].qty;
                if (string.IsNullOrWhiteSpace(strfguid))
                {
                    strfguid = lstvfaildetailngname[i].fguid;
                }
                blstNGCodeReasonData.Add(ngcodereasondata);
            }
            //获取原因代码
            dt = "";
            dt = client.RunServerAPI("BLL.NGDetect", "GetNGReason", strProductTypeCode);//获取不良原因
            if (string.IsNullOrWhiteSpace(dt))
            {
                //没有数据
                MessageBox.Show("无不良原因记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                List<B_NG_Reason> lstbngReason = new List<B_NG_Reason>();
                lstbngReason = JsonConvert.DeserializeObject<List<B_NG_Reason>>(dt);
                for (int i = 0; i < lstbngReason.Count; i++)
                {
                    try
                    //for (int j = 0; j < blstNGCodeReasonData.Count; j++)
                    {
                        dicReason.Add(lstbngReason[i].reason_name, lstbngReason[i].reason_code);
                        ((DataGridViewComboBoxColumn)this.dataGridView_faildetail.Columns["failReasoncode"]).Items.Add(lstbngReason[i].reason_name);
                    }
                    catch (Exception exp)
                    {
                    }
                }
            }
            //获取原因类型代码
            dt = "";
            dt = client.RunServerAPI("BLL.NGDetect", "GetBNGReasonType", "");//获取不良原因类型
            if (string.IsNullOrWhiteSpace(dt))
            {
                //没有数据
                MessageBox.Show("无不良原因记录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                List<B_NG_ReasonType> lstbngReasonType = new List<B_NG_ReasonType>();
                lstbngReasonType = JsonConvert.DeserializeObject<List<B_NG_ReasonType>>(dt);
                for (int i = 0; i < lstbngReasonType.Count; i++)
                {
                    //for (int j = 0; j < blstNGCodeReasonData.Count; j++)
                    {
                        dicTypeReason.Add(lstbngReasonType[i].reasontype_name, lstbngReasonType[i].reasontype_code);
                        ((DataGridViewComboBoxColumn)this.dataGridView_faildetail.Columns["failtype"]).Items.Add(lstbngReasonType[i].reasontype_name);
                    }
                }
            }
            dataGridView_faildetail.DataSource = blstNGCodeReasonData;
            GetProductGrade(strProductTypeCode);
            //GetPendingJudgment(strProductTypeCode);
        }
        //获取产品待判编码
        //private void GetPendingJudgment(string strProductTypeCode)
        //{
        //    try
        //    {
        //        ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
        //        string dt = client.RunServerAPI("BLL.Product", "GetPendingJudgmentByType", strProductTypeCode);//根据产品类别代码获取待判编码
        //        if (string.IsNullOrWhiteSpace(dt))
        //        {
        //            //没有数据
        //            MessageBox.Show("获取产品待判编码失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }
        //        List<B_Judge_code> listJudecode = JsonConvert.DeserializeObject<List<B_Judge_code>>(dt);
        //        for (int i = 0; i < listJudecode.Count; i++)
        //        {
        //            string judecode = listJudecode[i].judgecode;
        //            string judename = listJudecode[i].judgename;
        //            //CheckBox com = new CheckBox();
        //            //checkedListBox1.Controls.Add(com);
        //            checkedListBox1.Items.Add(new ComboboxItem(judecode, judename));
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        MessageBox.Show("获取待判编码失败:" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }

        //}
        //获取产品级别
        private void GetProductGrade(string parstrProductTypeCode)
        {
            try
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string dt = client.RunServerAPI("BLL.Product", "GetProductGradeByType", parstrProductTypeCode);//根据产品类别代码获取产品级别
                if (string.IsNullOrWhiteSpace(dt))
                {
                    //没有数据
                    MessageBox.Show("获取产品级别失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                List<B_Product_Grade> lstProductGrade = new List<B_Product_Grade>();
                lstProductGrade = JsonConvert.DeserializeObject<List<B_Product_Grade>>(dt);
                for (int i = 0; i < lstProductGrade.Count; i++)
                {
                    dicProductGrade.Add(lstProductGrade[i].grade_name, lstProductGrade[i]);
                    kComboBox_ProductGrade.Items.Add(lstProductGrade[i].grade_name);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("获取产品级别失败:" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //降级事件
        private void btn_demoted_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(kComboBox_ProductGrade.Text.Trim()))
            {
                MessageBox.Show("请先设置级别", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //先在P_SFC_State表中查找对应批次记录，如果不存在，说明该批次在首工序就进入维修，不存在记录，此时提示不能降级
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string str = client.RunServerAPI("BLL.Pack", "GetSfcState", textBox_sfc.Text);
            if (string.IsNullOrWhiteSpace(str))
            {
                //不存在记录
                MessageBox.Show("此批次不能降级", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);  //  如何提示不能降级原因???
                kComboBox_ProductGrade.Text = "";
                return;
            }
            List<P_SFC_State> dt = JsonConvert.DeserializeObject<List<P_SFC_State>>(str);
            if (dt.Count <= 0)
            {
                //不存在记录
                MessageBox.Show("此批次不能降级", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);  //  如何提示不能降级原因???
                kComboBox_ProductGrade.Text = "";
                return;
            }
            //如果批次被降级，该批次就不能设置为"待判"，"报废"
            btn_confirmed.Enabled = false;
            btn_discarde.Enabled = false;
            iRepairProcc = 1; //降级操作
        }
        //报废
        private void btn_discarde_Click(object sender, EventArgs e)
        {
            try
            {
                btn_confirmed.Enabled = false;
                kComboBox_ProductGrade.Text = "";
                btn_demoted.Enabled = false;
                iRepairProcc = 3; //报废
            }
            catch (Exception exp)
            {
                MessageBox.Show("报废提交失败:" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        //取消操作
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            //如果批次取消降级，恢复设置为"待判"，"报废"和降级标志
            kComboBox_ProductGrade.Text = "";
            btn_confirmed.Enabled = true;
            btn_discarde.Enabled = true;
            btn_demoted.Enabled = true;
            iRepairProcc = 0;  //取消操作
        }
        //提交操作
        private void btn_reback_Click(object sender, EventArgs e)
        {
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            try
            {
                if (string.IsNullOrWhiteSpace(richTextBox_faildesc.Text.Trim()))
                {
                    MessageBox.Show("请给出维修说明，在记录中填写", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (string.IsNullOrWhiteSpace(kComboBox_Process.Text.Trim()))
                {
                    MessageBox.Show("请选择责任工序", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //修改数据库信息，1.P_SFC_State(如果P_SFC_State表中存在记录才修改)，
                //更新P_SFC_State 当前工序和过站时间 +P_SFC_Process_IOLog 当前工序
                int ishebeifail = 0;
                string str = "";
                List<P_Fail_Detail> lstpFailDetail = new List<P_Fail_Detail>();
                for (int i = 0; i < dataGridView_faildetail.Rows.Count; i++)
                {
                    P_Fail_Detail onepfaildetail = new P_Fail_Detail();
                    onepfaildetail.id = int.Parse(dataGridView_faildetail.Rows[i].Cells["detail_id"].Value.ToString());
                    if ((this.dataGridView_faildetail.Rows[i].Cells["failReasoncode"] as DataGridViewComboBoxCell).Value == null)
                    {
                        MessageBox.Show("原因代码为空，请选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    onepfaildetail.reason_code = dicReason[(string)(this.dataGridView_faildetail.Rows[i].Cells["failReasoncode"] as DataGridViewComboBoxCell).Value];
                    if (onepfaildetail.reason_code.Contains("EEE"))  //判断是否为设备不良原因代码
                    {
                        ishebeifail = 1;
                    }
                    if ((this.dataGridView_faildetail.Rows[i].Cells["failtype"] as DataGridViewComboBoxCell).Value == null)
                    {
                        MessageBox.Show("原因类型代码为空，请选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    onepfaildetail.reasontype_code = dicTypeReason[(string)(this.dataGridView_faildetail.Rows[i].Cells["failtype"] as DataGridViewComboBoxCell).Value];
                    lstpFailDetail.Add(onepfaildetail);
                }
                //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
                if (checkBox1.Checked == true)
                {
                    if (checkedListBox1.CheckedItems.Count > 0)
                    {
                        for (int items = 0; items < checkedListBox1.CheckedItems.Count; items++)
                        {
                            string judecode = ((ComboboxItem)checkedListBox1.CheckedItems[items]).Value;
                            string judename = ((ComboboxItem)checkedListBox1.CheckedItems[items]).Text;
                            string SFC = textBox_sfc.Text.ToString();
                            string JudeCodeJson = client.RunServerAPI("BLL.SFC", "GetJudeSFC", judecode + "," + SFC);
                            if (string.IsNullOrEmpty(JudeCodeJson))
                            {
                                P_SFC_Jude sfc_jude = new P_SFC_Jude()
                                {
                                    jude_code = judecode,
                                    jude_name = judename,
                                    sfc = SFC,
                                    state = 0
                                };
                                string judejson = JsonToolsNet.ObjectToJson(sfc_jude);
                                client.RunServerAPI("BLL.SFC", "InsertSFCJude", judejson);
                            }
                            else
                            {
                                MessageBox.Show("该批次已存在[ " + judename + " ]的待判，不可重复", "提示");
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("请勾选待判名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                }
                string strJson = "";
                P_SFC_State clpsfcstateTmp = new P_SFC_State();
                clpsfcstateTmp.order_no = strorderno;
                clpsfcstateTmp.SFC = strsfc;
                clpsfcstateTmp.fail_times = 0;
                if (iRepairProcc == 1)  //降级
                {
                    if (string.IsNullOrWhiteSpace(kComboBox_ProductGrade.Text.Trim()))
                    {
                        MessageBox.Show("请先设置级别", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(kComboBox_rebackprocess.Text.Trim()))
                    {
                        MessageBox.Show("请先设置返回工序", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    clpsfcstateTmp.from_process = strfromprocess_code;
                    clpsfcstateTmp.now_process = diczrprocess[this.kComboBox_rebackprocess.Text.ToString().Trim()];
                    if (clpsfcstateTmp.now_process == "END")
                    {
                        clpsfcstateTmp.state = 2;//该批次已完成，可以流向下一工序
                    }
                    else
                    {
                        clpsfcstateTmp.state = 1;  //设置状态为 1 ，未完成，,正常加工
                    }

                    B_Product_Grade clBProductGrade = new B_Product_Grade();
                    clBProductGrade = dicProductGrade[kComboBox_ProductGrade.Text.Trim()];
                    clpsfcstateTmp.grade_id = clBProductGrade.grade_id.ToString();
                    clpsfcstateTmp.grade_type = clBProductGrade.grade_type;
                    //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
                    strJson = strorderno + "," + strsfc + "," + diczrprocess[this.kComboBox_rebackprocess.Text.ToString().Trim()]
                        + "," + "1" + "," + "0" + "," + "0" + "," + clBProductGrade.grade_id + "," + clBProductGrade.grade_type + "," + "0" + "," + strfromprocess_code;
                }
                else if (iRepairProcc == 2) //待判
                {
                    //待完善？？？
                }
                else if (iRepairProcc == 3) //报废
                {

                    clpsfcstateTmp.state = -1;  //设置状态为 -1 ，已报废

                    clpsfcstateTmp.now_process = "END";
                    //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
                    strJson = strorderno + "," + strsfc + "," + "END" + "," + "-1" + "," + "0" + "," + "0" + "," + "grade_id" + "," + "grade_type" + "," + "0" + "," + strfromprocess_code;
                }
                else  //不做操作，直接返回指定工序
                {
                    if (string.IsNullOrWhiteSpace(kComboBox_rebackprocess.Text.Trim()))
                    {
                        MessageBox.Show("请先设置返回工序", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    clpsfcstateTmp.state = 1;  //设置状态为 1 ，未完成，,正常加工
                    clpsfcstateTmp.now_process = diczrprocess[this.kComboBox_rebackprocess.Text.ToString().Trim()];
                    //order,sfc,nowprocess,state,failtimes,pass,grade_id,grade_type,iofailtimes
                    strJson = strorderno + "," + strsfc + "," + diczrprocess[this.kComboBox_rebackprocess.Text.ToString().Trim()]
                        + "," + "1" + "," + "0" + "," + "0" + "," + "grade_id" + "," + "grade_type" + "," + "0" + "," + strfromprocess_code;
                }

                str = client.RunServerAPI("BLL.SFC", "UpDataSFCInfoAndSFCIOLogData", strJson);
                if (!str.Contains("OK"))
                {
                    MessageBox.Show("更新P_SFC_State 失败 Or P_SFC_Process_IOLog 失败，" + str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //更新维修结果  P_FailLog  P_Fail_Detail
                P_FailLog clpfaillog = new P_FailLog();
                clpfaillog.fid = int.Parse(strfid);
                if (!string.IsNullOrWhiteSpace(kComboBox_Process.Text.Trim()))
                {
                    clpfaillog.process_code = strfromprocess_code;
                    clpfaillog.Disposal_Process = strfromprocess_code;
                    #region
                    //if (diczrprocess.Keys.Contains(kComboBox_Process.Text.Trim()))
                    //{
                    //    clpfaillog.process_code = diczrprocess[kComboBox_Process.Text.Trim()];
                    //}
                    //else
                    //{
                    //    MessageBox.Show("请选择责任工序！");
                    //    return;
                    //}
                    //clpfaillog.Disposal_Process = diczrprocess[kComboBox_Process.Text.Trim()];
                    #endregion
                    //根据责任工序，找到责任人和工位
                    strJson = "";
                    strJson = strsfc + "," + clpfaillog.process_code;
                    str = client.RunServerAPI("BLL.SFC", "GetSFCIOLogInfo", strJson);
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        //
                    }
                    else
                    {
                        List<P_SFC_Process_IOLog> lstsfciolog = JsonConvert.DeserializeObject<List<P_SFC_Process_IOLog>>(str);
                        if (lstsfciolog.Count > 0)
                        {
                            clpfaillog.emp_code = lstsfciolog[0].emp_code;
                        }
                        if (ishebeifail == 1) //存在设备不良，需要根据工位代码找到设备代码
                        {
                            //根据工位，找到设备
                            str = client.RunServerAPI("BLL.Station", "GetStationEquipmentByStation", lstsfciolog[0].station_code);
                            if (string.IsNullOrWhiteSpace(str))
                            {
                                //
                            }
                            else
                            {
                                List<B_StationMachine> lststationequipment = JsonConvert.DeserializeObject<List<B_StationMachine>>(str);
                                if (lstsfciolog.Count > 0)
                                {
                                    clpfaillog.equipment_code = lststationequipment[0].machine_code;
                                }
                            }
                        }
                    }
                }
                if (iRepairProcc == 1) //降级
                {
                    clpfaillog.state = 1;
                }
                else if (iRepairProcc == 2) //待判
                {
                    //待完善？？？
                }
                else if (iRepairProcc == 3) //报废
                {
                    clpfaillog.state = 9;  //报废状态
                }
                else
                {
                    clpfaillog.state = 1;
                }
                if (!string.IsNullOrWhiteSpace(richTextBox_faildesc.Text.Trim()))
                {
                    clpfaillog.repair_remark = richTextBox_faildesc.Text.Trim();
                }
                str = "";
                str = client.RunServerAPI("BLL.Faillog", "UpdateFailLog", JsonConvert.SerializeObject(clpfaillog));
                if (!str.Contains("1"))
                {
                    MessageBox.Show("更新P_FailLog 失败" + str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                str = "";
                str = client.RunServerAPI("BLL.Fail_Detail", "UpdateFail_Detail", JsonConvert.SerializeObject(lstpFailDetail));
                if (!str.Contains("1"))
                {
                    MessageBox.Show("更新P_Fail_Detail 失败" + str, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("提交失败:" + exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //提交完成
            MessageBox.Show("维修完成", "OK", MessageBoxButtons.OK, MessageBoxIcon.None);
            this.Close();
        }

        private void btn_materialchange_Click(object sender, EventArgs e)
        {
            FrmMaterialreplacement frmMatReplacement = new FrmMaterialreplacement();
            frmMatReplacement.strMainSfc = textBox_sfc.Text;
            frmMatReplacement.strorderon = strorderno;
            frmMatReplacement.strfguid = strfguid;
            frmMatReplacement.ShowDialog();
        }
        //是否待判
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == false)
            {
                checkedListBox1.Enabled = false;
            }
            else
            {
                checkedListBox1.Enabled = true;
            }
        }

        private void btn_confirmed_Click(object sender, EventArgs e)
        {

        }
    }
    public class NGCode_Reason_Data
    {
        //不良现象id
        public int detail_id { get; set; }
        //不良代码
        public string failPhenomenoncode { get; set; }
        //不良描述
        public string failPhenomenondesc { get; set; }
        //不良数量
        public double failNum { get; set; }
        ////不良原因
        //public List<string> lstfailreasoncode { get; set; }
        ////不良原因类型
        //public List<string> lstfailreasonTypecode { get; set; }
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
