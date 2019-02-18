using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using ComponentFactory.Krypton.Toolkit;
using LEDAO;
using JsonTools;

namespace LEMES_POD
{
    public partial class LotPacker : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
      
        public LotPacker()
        {
            InitializeComponent();
           
        }

        private void LotPacker_Load(object sender, EventArgs e)
        {
            //Component.Tool.DisplayResult(textBox4, true, "");
            //Component.Tool.DisplayResult(textBox3, true, "请输入托盘号");
            Component.Tool.DisplayResult(textBox1, true, "");
            ktb_input1.Text = "";
            ktb_input2.Text = "";
            ktb_input1.Focus();
            ktb_input2.Enabled = false;
            
        }
        string order_no;
        int state;
        string now_process;
        private void ktb_input1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {

                Component.Tool.DisplayResult(textBox1, true, "");
                string SFC = ktb_input1.Text.Trim();
                try
                {
                    string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetSfcState", SFC);
                    if (str.Length == 0)
                    {

                        //Component.Tool.DisplayResult(textBox3, true, "托盘号不存在，是否创建托盘？");
                        DialogResult result = KryptonMessageBox.Show("确定创建此托盘号吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == System.Windows.Forms.DialogResult.No)
                        {
                            ktb_input1.Text = "";
                            ktb_input1.Focus();
                            //Component.Tool.DisplayResult(textBox3, true, "请输入托盘号");
                            List<P_SFC_State> dp = new List<P_SFC_State>();
                            kryptonDataGridView1.DataSource = dp;
                            return;
                        
                        }
                      
                        P_SFC_State sfcList = new P_SFC_State()
                        {
                             is_tray=1,
                             SFC = ktb_input1.Text,
                        };
                        string strJson = JsonToolsNet.ObjectToJson(sfcList);
                        string opq = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "AddSFCState", strJson);
                        ktb_input1.Text = "";
                        ktb_input2.Focus();
                        ktb_input2.Enabled = true;
                        //Component.Tool.DisplayResult(textBox4, true, "请输入SFC");
                        //Component.Tool.DisplayResult(textBox3, true, "");
                        List<P_SFC_State> kn = new List<P_SFC_State>();
                        kryptonDataGridView1.DataSource = kn;
                        return;

                    }
                    else
                    {
                        List<P_SFC_State> dt = JsonConvert.DeserializeObject<List<P_SFC_State>>(str);

                        if (dt[0].is_tray == 0)
                        {

                            //Component.Tool.DisplayResult(textBox3, false, "不是托盘号，请重新输入！");
                            ktb_input1.Text = "";
                            ktb_input1.Focus();
                            List<P_SFC_State> kn = new List<P_SFC_State>();
                            kryptonDataGridView1.DataSource = kn;
                            return;
                        }
                        else
                        {
                            string tray_sfc = dt[0].SFC;
                            string msn = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetTrayDetail", tray_sfc);
                            if (msn.Length == 0)
                            {
                                ktb_input2.Enabled = true;
                                //Component.Tool.DisplayResult(textBox3, true, "托盘目前为空");
                                List<P_SFC_State> kn = new List<P_SFC_State>();
                                kryptonDataGridView1.DataSource = kn;
                                ktb_input2.Focus();
                                return;

                            }
                            else
                            {
                                ktb_input2.Enabled = true;
                                //Component.Tool.DisplayResult(textBox3, true, "加载成功");
                                LoadGridView();
                                ktb_input2.Focus();
                            }
                        }
                    }


                }
                catch (Exception exc)
                {
                    KryptonMessageBox.Show(exc.Message);
                }
            }
        }
        private void ktb_input2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Component.Tool.DisplayResult(textBox1, true, "");
                //Component.Tool.DisplayResult(textBox4, true, "");
                string SFC = ktb_input2.Text.Trim();

                try
                {

                    string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetSfcState", SFC);
                    if (str.Length == 0)
                    {

                        //Component.Tool.DisplayResult(textBox4, false, "SFC不存在，请重新输入");
                        ktb_input2.Text = "";
                        ktb_input2.Focus();
                        return;

                    }
                    else
                    {
                        List<LEDAO.P_SFC_State> dt = JsonConvert.DeserializeObject<List<LEDAO.P_SFC_State>>(str);
                        if (kryptonDataGridView1.RowCount > 0)
                        {
                            if (dt[0].is_tray == 0 && dt[0].order_no == order_no && dt[0].state == state&&dt[0].now_process==now_process)
                            {

                                P_Tray_Detail sfcList = new P_Tray_Detail()
                                {
                                    tray_sfc = ktb_input1.Text,
                                    sfc = dt[0].SFC,
                                    input_time = DateTime.Now,
                                };
                                string strJson = JsonToolsNet.ObjectToJson(sfcList);
                                string  opq=Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "AddSFC", strJson);
                                if (opq.Length > 0)
                                {
                                   
                                    //Component.Tool.DisplayResult(textBox4, false, "此SFC已在托盘中！");
                                }
                                else 
                                {
                                    //Component.Tool.DisplayResult(textBox4, true, "添加成功");
                                }
                                //Component.Tool.DisplayResult(textBox3, true, "");
                                LoadGridView();
                            }
                            else if (dt[0].is_tray == 1 && dt[0].order_no == order_no && dt[0].state == state)
                            {
                                string strn = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetTrayDetail", SFC);
                                if (strn.Length == 0)
                                {

                                    //Component.Tool.DisplayResult(textBox4, false, "空托盘，请重新输入");
                                    ktb_input2.Text = "";
                                    ktb_input2.Focus();
                                    return;

                                }
                                else
                                {
                                    List<LEDAO.P_Tray_Detail> kp = JsonConvert.DeserializeObject<List<LEDAO.P_Tray_Detail>>(strn);
                                    foreach (var item in kp)
                                    {
                                        P_Tray_Detail sfcList = new P_Tray_Detail()
                                        {
                                            tray_sfc = ktb_input1.Text,
                                            sfc = item.sfc,
                                            input_time = DateTime.Now,
                                        };
                                        string strJson = JsonToolsNet.ObjectToJson(sfcList);
                                        string opq = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "AddSFC", strJson);
                                        if (opq.Length > 0)
                                        {

                                            //Component.Tool.DisplayResult(textBox4, false, "此SFC已在托盘中！");
                                        }
                                        else
                                        {
                                            //Component.Tool.DisplayResult(textBox4, true, "添加成功");
                                        }
                                    }
                                  
                                   //Component.Tool.DisplayResult(textBox3, true, "");
                                    LoadGridView();

                                }
                            }

                            else { 
                            //Component.Tool.DisplayResult(textBox4, false, "工单或状态不匹配，请重新输入");
                            ktb_input2.Text = "";
                            ktb_input2.Focus();
                            }

                        }
                        else
                        {
                            if (dt[0].is_tray == 0)
                            {
                                P_Tray_Detail sfcList = new P_Tray_Detail()
                                {
                                    tray_sfc = ktb_input1.Text,
                                    sfc = dt[0].SFC,
                                    input_time = DateTime.Now,
                                };
                                string strJson = JsonToolsNet.ObjectToJson(sfcList);
                                string opq = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "AddSFC", strJson);
                                if (opq.Length > 0)
                                {

                                    //Component.Tool.DisplayResult(textBox4, false, "此SFC已在托盘中！");
                                }
                                else
                                {
                                    //Component.Tool.DisplayResult(textBox4, true, "添加成功");
                                }
                                //Component.Tool.DisplayResult(textBox3, true, "");
                                LoadGridView();

                            }
                            else
                            {
                                string stro = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetTrayDetail", SFC);
                                if (stro.Length == 0)
                                {

                                    //Component.Tool.DisplayResult(textBox4, false, "空托盘");
                                    ktb_input2.Text = "";
                                    ktb_input2.Focus();
                                    return;

                                }
                                else
                                {
                                    string strn = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetTrayDetail", SFC);

                                    List<LEDAO.P_Tray_Detail> kp = JsonConvert.DeserializeObject<List<LEDAO.P_Tray_Detail>>(strn);
                                    foreach (var item in kp)
                                    {
                                        P_Tray_Detail sfcList = new P_Tray_Detail()
                                        {
                                            tray_sfc = ktb_input1.Text,
                                            sfc = item.sfc,
                                            input_time = DateTime.Now,
                                        };
                                        string strJson = JsonToolsNet.ObjectToJson(sfcList);
                                        string opq = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "AddSFC", strJson);
                                        if (opq.Length > 0)
                                        {

                                            //Component.Tool.DisplayResult(textBox4, false, "此SFC已在托盘中！");
                                        }
                                        else
                                        {
                                            //Component.Tool.DisplayResult(textBox4, true, "添加成功");
                                        }
                                    }
                                   
                                    //Component.Tool.DisplayResult(textBox3, true, "");
                                    LoadGridView();
                                }

                            }
                        }

                        ktb_input2.Text = "";
                    }
                }
                catch (Exception exc)
                {
                    KryptonMessageBox.Show(exc.Message);
                }
               
            }
        }
        private void LoadGridView()
        {
            string SFC = ktb_input1.Text.Trim();
            string tra = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetTrayDetail", SFC);
            List<P_SFC_State> mn = JsonConvert.DeserializeObject<List<P_SFC_State>>(tra);
            if (mn == null)
            {
                List<P_SFC_State> dt = new List<P_SFC_State>();
                kryptonDataGridView1.DataSource = dt; 
                return;
            }
            else
            {
                List<P_SFC_State> res = new List<P_SFC_State>();
                foreach (var item in mn)
                {
                    string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetSfcState", item.SFC);
                    List<P_SFC_State> dt = JsonConvert.DeserializeObject<List<P_SFC_State>>(str);
                    order_no = dt[0].order_no;
                    state = (int)dt[0].state;
                    now_process = dt[0].now_process;
                    res.AddRange(dt);
                }
                kryptonDataGridView1.DataSource = res;
            }
        }

        private void kryptonLabel1_Paint(object sender, PaintEventArgs e){ }
        private void kryptonTextBox1_TextChanged(object sender, EventArgs e){ }
        private void kryptonLabel2_Paint(object sender, PaintEventArgs e) {       }
        private void textBox4_TextChanged(object sender, EventArgs e){ }
        private void kryptonDataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e){ }
        private void textBox2_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void kryptonDataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e) { }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            //Component.Tool.DisplayResult(textBox4, true, "");
            //Component.Tool.DisplayResult(textBox3, true, "");
            ktb_input1.Text = "";
            ktb_input2.Text = "";
            ktb_input1.Focus();
            ktb_input2.Enabled = false;
            List<P_SFC_State> dt=new List<P_SFC_State>();
            kryptonDataGridView1.DataSource = dt;
            Component.Tool.DisplayResult(textBox1, true, "打包完成，输入下一个托盘号");
          
        }

        private void kryptonButton4_Click(object sender, EventArgs e)
        {
            //Component.Tool.DisplayResult(textBox4, true, "");
            //Component.Tool.DisplayResult(textBox3, true, "");
            if (kryptonDataGridView1.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请先选中需要删除的列", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult result = KryptonMessageBox.Show("确定删除吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            try
            {
                string ID = kryptonDataGridView1.SelectedRows[0].Cells["id"].Value.ToString();
                string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetSfcID", ID );  
                List<P_SFC_State> dt= JsonConvert.DeserializeObject<List<P_SFC_State>>(str);
                string tray_sfc = ktb_input1.Text;
                string  sfc= dt[0].SFC;
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "DelSFC", sfc + "," + tray_sfc);
                Component.Tool.DisplayResult(textBox1, true, "移除成功");
            }
            catch (Exception)
            {
                MessageBox.Show("删除失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoadGridView();
        }

        private void kryptonButton5_Click(object sender, EventArgs e)
        {
            //Component.Tool.DisplayResult(textBox4, true, "");
            //Component.Tool.DisplayResult(textBox3, true, "");
            string SFC = ktb_input1.Text.Trim();
            DialogResult result = KryptonMessageBox.Show("确定拆包吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
               try
            {
            string tra = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "GetTrayDetail", SFC);
            List<P_SFC_State> mn = JsonConvert.DeserializeObject<List<P_SFC_State>>(tra);
            
            foreach (var item in mn)
            {
                string sfc =item.SFC;
                string tray_sfc = SFC;
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Pack", "DelSFC", sfc + "," + tray_sfc);
            }
            List<P_SFC_State> kn = new List<P_SFC_State>();
            Component.Tool.DisplayResult(textBox1, true, "拆包成功，输入下一个托盘号");
            kryptonDataGridView1.DataSource = kn;
            ktb_input1.Text = "";
            ktb_input2.Text = "";
            ktb_input1.Focus();
            ktb_input2.Enabled = false;
         
            }
               catch (Exception)
               {
                   MessageBox.Show("拆包失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                   throw;
               }
        }

        private void kryptonLabel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
