using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using LEDAO;

namespace RepairTool
{
    public partial class FrmMaterialreplacement : Form
    {
        public string strMainSfc = "";
        public string strorderon = "";
        public string strStation = "";
        public string strfguid = "";

        private List<ChangeSubSFC_Data> lstchangesubsfc = new List<ChangeSubSFC_Data>();
        public FrmMaterialreplacement()
        {
            InitializeComponent();
        }
        //显示datagridview 行号
        private void kryptonDataGridView_sublstsfc_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                                                e.RowBounds.Location.Y,
                                                kryptonDataGridView_sublstsfc.RowHeadersWidth - 4,
                                                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                        kryptonDataGridView_sublstsfc.RowHeadersDefaultCellStyle.Font,
                                        rectangle,
                                        kryptonDataGridView_sublstsfc.RowHeadersDefaultCellStyle.ForeColor,
                                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void FrmMaterialreplacement_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strMainSfc))
                {
                    MessageBox.Show("批次号为空，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
                if (string.IsNullOrWhiteSpace(strorderon))
                {
                    MessageBox.Show("工单为空，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                }
                txtbox_sfc.Text = strMainSfc;
                txtbox_shoporder.Text = strorderon;

                lstchangesubsfc.Clear();
                ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                string json = strorderon;
                string ResultJson = clien.RunServerAPI("BLL.SFC", "GetLstSFCByParentorder", json);  //根据父工单获取子工单中批次列表
                if (string.IsNullOrWhiteSpace(ResultJson))
                {
                    MessageBox.Show("当前主批次号下没有子物料，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }
                List<P_BarCodeBing> lstpbarcodebing = new List<P_BarCodeBing>();
                lstpbarcodebing = JsonConvert.DeserializeObject<List<P_BarCodeBing>>(ResultJson);
                if (lstpbarcodebing.Count <= 0)
                {
                    MessageBox.Show("当前主批次号下没有子物料，请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }
                for (int i = 0; i < lstpbarcodebing.Count; i++)
                {
                    ChangeSubSFC_Data changesubsfcdata = new ChangeSubSFC_Data();
                    changesubsfcdata.subsfc = lstpbarcodebing[i].barcode;
                    changesubsfcdata.subsfcorder = lstpbarcodebing[i].order;
                    changesubsfcdata.subsfcmatCode = lstpbarcodebing[i].product_code;
                    changesubsfcdata.subchangesfc = "";
                    lstchangesubsfc.Add(changesubsfcdata);
                }

                kryptonDataGridView_sublstsfc.DataSource = lstchangesubsfc;

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool CheckSFCRule(string parstrorder,string parstrmatcode,string parstrstation, string parstrChangesfc)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(parstrChangesfc))
                {
                    //MessageBox.Show("替换物料代码为空，请检查","提示");
                    return false;
                }

                ILE.LEResult result = new ILE.LEResult();
                ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                string json = parstrorder + "," + parstrmatcode + "," + parstrstation + "," + parstrChangesfc;
                string ResultJson = clien.RunServerAPI("BLL.SFCConsume", "GetMaterialConsumptionManual", json);
                result = JsonConvert.DeserializeObject<ILE.LEResult>(ResultJson);
                if (result.Result == false)
                {
                    MessageBox.Show(result.ExtMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return result.Result;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }
        private void kryptonDataGridView_sublstsfc_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex < 0
                    || e.RowIndex < 0)
                {
                    return;
                }
                if (string.IsNullOrWhiteSpace(kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subchangesfc"].Value.ToString()))
                {
                    return;
                }
                bool bchecksfc = CheckSFCRule(kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subsfcorder"].Value.ToString(),
                    kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subsfcmatcode"].Value.ToString(), "DEFAULT",
                    kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subchangesfc"].Value.ToString());
                if (bchecksfc == false)
                {
                    kryptonDataGridView_sublstsfc.ClearSelection();
                    kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subchangesfc"].Selected = true;
                    kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subchangesfc"].Value = "";
                    this.kryptonDataGridView_sublstsfc.CurrentCell = kryptonDataGridView_sublstsfc.Rows[e.RowIndex].Cells["subchangesfc"];
                    this.kryptonDataGridView_sublstsfc.BeginEdit(true);
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                
                for (int i = 0; i < kryptonDataGridView_sublstsfc.Rows.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(kryptonDataGridView_sublstsfc.Rows[i].Cells["subchangesfc"].Value.ToString().Trim()))    //检查是否存在替换批次，如果存在则修改原批次信息
                    {

                        //1.修改P_SFC_State表中原sfc的状态为 -1；
                        //2.修改P_SFC_ProcessData表中的绑定关系
                        //3.增加P_SFC_ProcessData_Back表（新增加表 P_FailLog.fguid, P_SFC_ProcessData.order_no,P_SFC_ProcessData.mat_code,P_SFC_ProcessData.sfc,P_SFC_ProcessData.val as oldsubsfc）数据
                        P_SFC_ProcessData_Back psfcprocessdataback = new P_SFC_ProcessData_Back();
                        psfcprocessdataback.fguid = strfguid;
                        psfcprocessdataback.order_no = kryptonDataGridView_sublstsfc.Rows[i].Cells["subsfcorder"].Value.ToString().Trim();
                        psfcprocessdataback.SFC = txtbox_sfc.Text.ToString().Trim();
                        psfcprocessdataback.mat_code = kryptonDataGridView_sublstsfc.Rows[i].Cells["subsfcmatCode"].Value.ToString().Trim();
                        psfcprocessdataback.val = kryptonDataGridView_sublstsfc.Rows[i].Cells["subsfc"].Value.ToString().Trim();
                        psfcprocessdataback.New_val = kryptonDataGridView_sublstsfc.Rows[i].Cells["subchangesfc"].Value.ToString().Trim();
                        string ResultJson = clien.RunServerAPI("BLL.SFC_ProcessData", "AddSFCprocessDataBack", JsonConvert.SerializeObject(psfcprocessdataback));

                        if (ResultJson != "OK")
                        {
                            string strMsgTmp = "替换批次[" + psfcprocessdataback.SFC + "]失败";
                            MessageBox.Show(strMsgTmp, "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
    public class ChangeSubSFC_Data
    {
        //子批次条码
        public string subsfc { get; set; }
        //子批次条码所属工单
        public string subsfcorder { get; set; }
        //子批次条码物料号
        public string subsfcmatCode { get; set; }
        //子批次替换条码
        public string subchangesfc { get; set; }
    }
}
