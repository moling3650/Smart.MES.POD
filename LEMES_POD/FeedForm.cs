using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ILE;
using LEDAO;
using Newtonsoft.Json;
using ComponentFactory.Krypton.Toolkit;
using System.Net;

namespace LEMES_POD
{
    public partial class FeedForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        //安装点
        private List<string> pointList;
        //自动投料
        private List<LEDAO.V_Material_WIP> lotlist;
        //获取wip信息
        private LEDAO.V_Material_WIP lotWIP;

        //复工
        private void deWork()
        {
            ktxt_lot.Text = "";
            ktxt_piont.Text = "";
            //txt_respiont.Visible = false;
            ktxt_piont.Visible = false;
            kryptonLabel2.Visible = false;
            ktxt_lot.Focus();
            Component.Tool.DisplayResult(txt_resgx, true, "");
            Component.Tool.DisplayResult(txt_resgx, true, "");
            Component.Tool.DisplayResult(txt_resgx, true, "");
            Component.Tool.DisplayResult(txt_resgx, true, "");


        }

        //工序信息  
        public FeedForm()
        {
            FeedInitialize();
            SetIPADD();
            Component.Tool.DisplayResult(txt_resgx, true, "");
        }

        //获取IP
        void SetIPADD()
        {
            IPAddress[] ipAdds = Dns.Resolve(Dns.GetHostName()).AddressList;//获得当前IP地址
            this.kcb_ipadd.Items.Clear();
            foreach (IPAddress ip in ipAdds)
            {
                this.kcb_ipadd.Items.Add(ip.ToString());
            }
            if (kcb_ipadd.Items.Count > 0)
            {
                kcb_ipadd.SelectedIndex = 0;
            }
        }

        //获取工序
        void SetProcess()
        {
            string ip = kcb_ipadd.SelectedItem.ToString();

            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Process", "GetProcessList", ip);

            if (dt == "")
            {
                Component.Tool.DisplayResult(txt_resgx, false, "未获取到当前工序");
                ktxt_emp.Enabled = false;
                return;
            }
            else
            {
                Component.Tool.DisplayResult(txt_resgx, true, "");
            }
            List<B_ProcessList> proc;
            proc = JsonConvert.DeserializeObject<List<B_ProcessList>>(dt);

            this.kcb_process.DataSource = proc;
            kcb_process.ValueMember = "process_code";
            kcb_process.DisplayMember = "process_name";

        }

        //获取工位
        void SetStation()
        {
            string ip = kcb_ipadd.Text;

            string processCode = this.kcb_process.SelectedValue.ToString();

            string cds = ip + "," + processCode;

            string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Station", "GetStationList", cds);

            if (dt == "")
            {
                //当前IP找不到任何工位
                Component.Tool.DisplayResult(txt_resgx, false, "未获取到当前工位");
                ktxt_emp.Enabled = false;
                return;
            }
            else
            {
                Component.Tool.DisplayResult(txt_resgx, true, "");
            }

            //获取站点
            ktxt_emp.Enabled = true;
            List<B_StationList> stationList = JsonConvert.DeserializeObject<List<B_StationList>>(dt);
            kcb_Station.DataSource = stationList;
            kcb_Station.ValueMember = "station_code";
            kcb_Station.DisplayMember = "station_name";
            ktxt_emp.TabIndex = 0;
        }

        //自动投料
        void AutoMat()
        {
            ILE.IResult res = BLL.WIP.Get_WIP_AutoSend(ktxtOrder.Text, kcb_process.SelectedValue.ToString());
            if (res.Result)
            {
                lotlist = JsonConvert.DeserializeObject<List<LEDAO.V_Material_WIP>>(res.obj.ToString());
                kryptonButton1.Visible = true;
            }
            else
            {
                kryptonButton1.Visible = false;
            }


        }
        //弹窗
        public FeedForm(string _ip, string _process, string _station, string _empCode, string _orderno)
        {
            FeedInitialize();
            SetIPADD();
            kcb_ipadd.SelectedItem = _ip;
            SetProcess();
            kcb_process.SelectedValue = _process;
            SetStation();
            kcb_Station.SelectedValue = _station;
            ktxt_emp.Text = _empCode;
            if (!string.IsNullOrWhiteSpace(_orderno))
            {
                buttonSpecAny2.Visible = false;
                ktxtOrder.ReadOnly = true;
                ktxtOrder.Text = _orderno;
                AutoMat();
                ktxt_lot.TabIndex = 0;

            }
            else
            {
                ktxtOrder.Focus();
            }
            DataBind();
            Groplock(false);

        }
        //检查工序是否有不关注的物料，如果有，自动把这个批次的物料平均投到各工位仓,如果该物料在线边仓有多个批次，取数量最多的批次投到工位仓
        private void AutoFeedToStation()
        {
            string process_code = kcb_process.SelectedValue.ToString();
        }
        //初始化
        private void FeedInitialize()
        {
            InitializeComponent();
            Component.Tool.DisplayResult(txt_resgx, true, "");
            deWork();
            Groplock(true);
        }

        //锁区
        void Groplock(bool processLock)
        {

            kcb_ipadd.Enabled = processLock;
            kcb_process.Enabled = processLock;
            kcb_Station.Enabled = processLock;
            ktxt_emp.Enabled = processLock;
            ktxt_lot.ReadOnly = processLock;
            ktxt_piont.ReadOnly = processLock;

        }




        private void FeedForm_Load(object sender, EventArgs e)
        {
        }

        //WIP刷新
        private void DataBind()
        {
            if (!string.IsNullOrWhiteSpace(ktxtOrder.Text)) //输入工单
            {

                AutoMat();
            }
            string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetWIPState", kcb_Station.SelectedValue.ToString() + "," + kcb_process.SelectedValue.ToString());
            kryptonDataGridView1.DataSource = JsonConvert.DeserializeObject<List<LEDAO.V_WIP_Seed>>(str);
            initCheckBox();
        }

        //右键删除
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView1.SelectedRows.Count > 0)
            {

                DialogResult result = KryptonMessageBox.Show("确定删除当前选择行装载物料吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                foreach (DataGridViewRow item in kryptonDataGridView1.SelectedRows)
                {
                    LEDAO.V_WIP_Seed wip = (LEDAO.V_WIP_Seed)item.DataBoundItem;
                    string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "DeleteWIPSeed", wip.id.ToString());
                }
                DataBind();


            }
        }

        //ip
        private void kcb_ipadd_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetProcess();
        }

        //工序
        private void kcb_process_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetStation();
        }

        //员工
        private void ktxt_emp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string jsdata = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Employee", "GetStaff", ktxt_emp.Text);
                if (jsdata == "")
                {
                    //当前找不到员工号
                    Component.Tool.DisplayResult(txt_resgx, false, "未获取到当前工序");
                    ktxt_emp.Text = string.Empty;
                    return;
                }
                DataBind();

                Groplock(false);
                ktxtOrder.Focus();
            }
        }
        
        //工单
        private void ktxtOrder_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && !ktxtOrder.ReadOnly)
            {
                ILE.IResult res = BLL.WIP.Check_WIP_Work(kcb_process.SelectedValue.ToString(), ktxtOrder.Text);
                //当前IP找不到任何工位
                Component.Tool.DisplayResult(txt_resgx, res.Result, res.ExtMessage);
                AutoMat();
                ktxt_lot.Focus();
            }

        }
        //工单重录
        private void buttonSpecAny2_Click(object sender, EventArgs e)
        {
            ktxtOrder.ReadOnly = false;
            deWork();
            ktxtOrder.Focus();

        }
        //批次
        private void ktxt_lot_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == 13 && !ktxt_lot.ReadOnly)
            {
                if (string.IsNullOrWhiteSpace(ktxt_lot.Text))
                {
                    Component.Tool.DisplayResult(txt_resgx, false, "装料批次不能为空");
                    return;
                }
                string lot = ktxt_lot.Text.Trim().Replace("#", "");
                try
                {
                    string mat_code = string.Empty;
                    if (lotWIP == null)
                    {
                        string order_no = ktxtOrder.Text;
                        if (order_no == "")
                        {
                            MessageBox.Show("工单号不可为空，请选择工单", "提示");
                            return;
                        }
                        //根据批次号及工单获取物料编号
                        mat_code = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetMat_codeBylot", lot + "," + order_no);
                        if (mat_code == "")
                        {
                            MessageBox.Show("该工单和批次未能获取物料编号,请确认工单号及批次号", "提示");
                            return;
                        }
                    }
                    else
                    {
                        mat_code = lotWIP.mat_code;
                    }
                    DateTime MaterialInputTime = Convert.ToDateTime(Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetWipInputTimeByLot", lot + "," + mat_code));
                    string strwip_valid = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetWip_validByProductCode", mat_code);
                    if (strwip_valid != "")
                    {
                        decimal wip_valid = Convert.ToDecimal(strwip_valid);
                        TimeSpan ts = DateTime.Now - MaterialInputTime;
                        decimal h = ts.Hours;
                        if (h > wip_valid)
                        {
                            DialogResult result = KryptonMessageBox.Show("该物料已超出时效时间,确定继续使用吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (result == System.Windows.Forms.DialogResult.No)
                            {
                                return;
                            }
                        }
                    }

                    ILE.IResult res = BLL.WIP.Check_WIP_Lot(ktxtOrder.Text, lot);
                    if (res.Result)
                    {
                        res = BLL.WIP.Get_WIP_LotInfo(ktxtOrder.Text, lot);
                        if (!res.Result)
                        {
                            Component.Tool.DisplayResult(txt_resgx, res.Result, res.ExtMessage);
                            return;
                        }
                        List<LEDAO.V_Material_WIP> wip = JsonConvert.DeserializeObject<List<LEDAO.V_Material_WIP>>(res.obj.ToString());
                        if (lotWIP == null)
                        {
                            if (wip.Count == 1)
                            {
                                lotWIP = wip[0];
                            }

                            //多个wip_id，弹窗选择
                            if (wip.Count > 1)
                            {
                                SelGridForm gridform = new SelGridForm(wip);
                                gridform.ShowDialog();
                                if (gridform.WIP != null)
                                {
                                    lotWIP = gridform.WIP;
                                    ktxt_lot.Text = lotWIP.lot_no;
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                        //判断是否是最小批次
                        if (!BLL.WIP.Check_Mbm(ktxtOrder.Text))
                        {
                            //是否可分批投入
                            if (BLL.WIP.Check_Mat_Split(Convert.ToInt32(lotWIP.id), ktxtOrder.Text))
                            {
                                UserForm.NumForm num = new UserForm.NumForm();
                                num.ShowDialog();
                                switch (num.State)
                                {
                                    case 1:
                                        if (lotWIP.lot_qty < num.Num)
                                        {
                                            Component.Tool.DisplayResult(txt_resgx, false, "当前批次记录不能大于" + lotWIP.lot_qty);
                                            return;
                                        }
                                        lotWIP.lot_qty = num.Num;
                                        break;
                                    case 2:
                                        break;
                                    default:
                                        return;
                                }
                            }

                        }

                        int id = Convert.ToInt32(lotWIP.id);
                        res = BLL.WIP.Get_WIP_Point(kcb_Station.SelectedValue.ToString(), ktxtOrder.Text, id);


                        //判断是否有安装点
                        if (res.Result)
                        {

                            pointList = JsonConvert.DeserializeObject<List<string>>(res.obj.ToString());
                            if (pointList.Count > 0)
                            {
                                ktxt_piont.Visible = true;
                                kryptonLabel2.Visible = true;
                                ktxt_piont.Focus();
                            }
                        }
                        else
                        {
                            res = BLL.WIP.Sumit_FeedMatToStation(id, null, kcb_Station.SelectedValue.ToString(), ktxtOrder.Text, ktxt_emp.Text, lotWIP.lot_qty);
                            deWork();
                            DataBind();
                        }
                    }
                    else
                    {
                        ktxt_lot.Text = "";
                    }

                    Component.Tool.DisplayResult(txt_resgx, res.Result, res.ExtMessage);

                }
                catch (Exception exc)
                {
                    KryptonMessageBox.Show(exc.Message);
                }
                finally
                {
                    lotWIP = null;
                }
            }
        }
        //卸料
        private void kbtnclose_Click(object sender, EventArgs e)
        {
            if (this.kryptonDataGridView1.Rows.Count > 0)
            {
                string Num = this.kryptonDataGridView1.SelectedRows[0].Cells["Column5"].Value.ToString();
                if (Convert.ToDecimal(Num) > 0)
                {
                    UserForm.ReturnMateralNumForm Numform = new UserForm.ReturnMateralNumForm(Num);
                    Numform.ShowDialog();
                    switch (Numform.State)
                    {
                        case 1:
                            decimal num = Numform.Num;
                            UpdateWipSeed(Num, num);
                            break;
                        case 2:
                            decimal num1 = Convert.ToDecimal(Num);
                            UpdateWipSeed(Num, num1);
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    foreach (DataGridViewRow item in kryptonDataGridView1.SelectedRows)
                    {
                        LEDAO.V_WIP_Seed wip = (LEDAO.V_WIP_Seed)item.DataBoundItem;
                        string str1 = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "UpdateWIPSeed", wip.id.ToString() + "," + "0");
                    }
                    DialogResult result = KryptonMessageBox.Show("该物料消耗为负,是否卸料？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    DataBind();
                }
            }
            else
            {
                MessageBox.Show("暂无投料，不可卸料", "提示");
                return;
            }
            //this.Close();
        }
        /// <summary>
        /// 卸料是更改线边仓及工位仓
        /// </summary>
        /// <param name="Num"></param>
        /// <param name="num"></param>
        public void UpdateWipSeed(string Num, decimal num)
        {
            string SFC = this.kryptonDataGridView1.SelectedRows[0].Cells["Column4"].Value.ToString();
            string mat_code = this.kryptonDataGridView1.SelectedRows[0].Cells["Column3"].Value.ToString();
            string order_no = this.kryptonDataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
            //工单料卸料
            string qty = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetWipQtyByLot", SFC + "," + mat_code + "," + order_no);
            if (!string.IsNullOrWhiteSpace(qty))
            {
                string lot_qty = (num + Convert.ToDecimal(qty)).ToString();
                string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "UpdateWIPMaterial", SFC + "," + lot_qty + "," + mat_code + "," + order_no);
            }
            //公用料卸料
            string commonqty = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetWipComQtyByLot", SFC + "," + mat_code);
            if (!string.IsNullOrWhiteSpace(commonqty))
            {
                string lot_qty = (num + Convert.ToDecimal(commonqty)).ToString();
                string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "UpdateWIPMaterialCom", SFC + "," + lot_qty + "," + mat_code);
            }
            foreach (DataGridViewRow item in kryptonDataGridView1.SelectedRows)
            {
                LEDAO.V_WIP_Seed wip = (LEDAO.V_WIP_Seed)item.DataBoundItem;
                string str1 = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "UpdateWIPSeed", wip.id.ToString() + "," + (decimal.Parse(Num) - num).ToString());
            }
            DataBind();
        }
        //清料
        private void kbtnxl_Click(object sender, EventArgs e)
        {
            int m = 0;
            for (int i = 0; i < this.kryptonDataGridView1.Rows.Count; i++)
            {
                if ((bool)this.kryptonDataGridView1.Rows[i].Cells[0].EditedFormattedValue)
                {
                    m++;
                }
            }
            if (m > 0)
            {
                DialogResult result = KryptonMessageBox.Show("确定清除当前选择的物料吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                for (int i = 0; i < this.kryptonDataGridView1.Rows.Count; i++)
                {
                    if ((bool)this.kryptonDataGridView1.Rows[i].Cells[0].EditedFormattedValue)
                    {
                        LEDAO.V_WIP_Seed wip = (LEDAO.V_WIP_Seed)(this.kryptonDataGridView1.Rows[i]).DataBoundItem;
                        string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "DeleteWIPSeed", wip.id.ToString());
                    }
                }
                DataBind();
            }
        }
        //全部卸料
        private void kbtnallxl_Click(object sender, EventArgs e)
        {
            UserForm.CheckPassWord password = new UserForm.CheckPassWord();
            password.ShowDialog();
            DialogResult result = KryptonMessageBox.Show("确定把全部物料清除吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            foreach (DataGridViewRow item in kryptonDataGridView1.Rows)
            {
                LEDAO.V_WIP_Seed wip = (LEDAO.V_WIP_Seed)item.DataBoundItem;
                string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "DeleteWIPSeed", wip.id.ToString());
            }
            DataBind();
        }
        //右键删除
        //private void kryptonDataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Left)
        //    {
        //        if (e.RowIndex >= 0)
        //        {
        //            //若行已是选中状态就不再进行设置  
        //            if (kryptonDataGridView1.Rows[e.RowIndex].Selected == false)
        //            {
        //                kryptonDataGridView1.ClearSelection();
        //                kryptonDataGridView1.Rows[e.RowIndex].Selected = true;
        //            }
        //            //只选中一行时设置活动单元格  
        //            if (kryptonDataGridView1.SelectedRows.Count == 1)
        //            {
        //                kryptonDataGridView1.CurrentCell = kryptonDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
        //            }
        //            //弹出操作菜单  
        //            contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
        //        }
        //    }
        //}


        //安装点
        private void ktxt_piont_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)13)
            {
                string lot = ktxt_lot.Text.Trim();
                string point = ktxt_piont.Text.Trim();
                ILE.IResult res = new ILE.LEResult();
                try
                {

                    int id = Convert.ToInt32(lotWIP.id);
                    foreach (string item in pointList)
                    {
                        if (point == item)
                        {
                            res = BLL.WIP.Sumit_FeedMatToStation(id, point, kcb_Station.SelectedValue.ToString(), ktxtOrder.Text, ktxt_emp.Text, lotWIP.lot_qty);
                            Component.Tool.DisplayResult(txt_resgx, res.Result, res.ExtMessage);
                            deWork();
                            DataBind();
                            return;
                        }
                    }
                    ktxt_piont.Text = "";
                    Component.Tool.DisplayResult(txt_resgx, false, "没有安装点[" + point + "]");
                }
                catch (Exception exc)
                {
                    KryptonMessageBox.Show(exc.Message);
                }
            }
        }
        //自动投料事件
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            SelGridForm gridform = new SelGridForm(lotlist);
            gridform.ShowDialog();
            if (gridform.WIP != null)
            {
                lotWIP = gridform.WIP;
                ktxt_lot.Text = lotWIP.lot_no;
                ktxt_lot_KeyPress(sender, new KeyPressEventArgs((char)13));
            }
        }
        //生产指令
        private void buttonSpecHeaderGroup2_Click(object sender, EventArgs e)
        {

            (new WorkerOrderFrom1(this, kcb_process.SelectedValue.ToString(), "")).ShowDialog(this);
        }

        //为gridView加载复选框
        private void initCheckBox()
        {
            for (int i = 0; i < this.kryptonDataGridView1.Rows.Count; i++)
            {
                //为datagridviewcheckbox列赋值
                this.kryptonDataGridView1.Rows[i].Cells[0].Value = false;
            }
        }


        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex != -1)
            {
                kryptonDataGridView1.Rows[e.RowIndex].Cells[0].Value = (bool)kryptonDataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue;
            }
        }


    }

}

