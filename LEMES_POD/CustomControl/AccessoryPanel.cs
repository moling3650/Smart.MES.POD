using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using LEDAO;
using LEMES_POD.Tools;
using ComponentFactory.Krypton.Toolkit;

namespace LEMES_POD.CustomControl
{
    public partial class AccessoryPanel : UserControl
    {
        List<LEMES_POD.UserForm.AccessoryList> accessoryList = new List<UserForm.AccessoryList>();
        string Ecode = string.Empty;
        LEMES_POD.UserForm.Accessories Acc;
        public AccessoryPanel(List<LEMES_POD.UserForm.AccessoryList> _accessoryList, string _Ecode, LEMES_POD.UserForm.Accessories _Acc)
        {
            InitializeComponent();
            accessoryList = _accessoryList;
            Ecode = _Ecode;
            Acc = _Acc;
            InitPanel();
        }
        /// <summary>
        /// 加载配件信息
        /// </summary>
        public void InitPanel()
        {
            label2.Text = accessoryList[0].accessory_code;
            //label3.Text = accessoryList[0].accessory_name;
            label5.Text = accessoryList[0].accessory_type;
            //if (accessoryList[0].accessory_isload == 0)
            //{
            //    pictureBox1.Image = Properties.Resources.cancel_32;
            //}
            //if (accessoryList[0].accessory_isload == 1)
            //{
            //    pictureBox1.Image = Properties.Resources.confirm;
            //}
        }
        /// <summary>
        /// 卸载配件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox2_Click(object sender, EventArgs e)
        {

            string Accessory_code = accessoryList[0].accessory_code.ToString().Trim().ToUpper();
            string machine_code = accessoryList[0].machine_code.ToString().Trim().ToUpper();
            DialogResult result = KryptonMessageBox.Show("确定卸载吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryRecords_0", Accessory_code + "," + machine_code);
            List<P_Accessory_Records> ListAccessoryRecords = JsonConvert.DeserializeObject<List<P_Accessory_Records>>(RecordsJson);
            if (ListAccessoryRecords == null)
            {
                //说明第一次装载备件，啥也不干
            }
            else
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineAccessoryRecord_1", Accessory_code + "," + machine_code);
            }
            try
            {
                P_Accessory_Records Sparepart = new P_Accessory_Records()
                {
                    Accessory_code = Accessory_code,
                    machine_code = machine_code,
                    type = 1,
                    state = 0,
                    loadtime = DateTime.Now,
                    input_time = DateTime.Now,
                    Accessory_preson = Ecode,
                };
                string strJson = JsonToolsNet.ObjectToJson(Sparepart);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineAccessoryRecord_0", strJson);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "DeleteAccessoryLoad", Accessory_code);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateAccessorystate_0", Accessory_code);
                //Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineAccessoryIsLoad_1", Accessory_code);
                //MessageBox.Show("卸载成功", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("卸载失败", "提示");
                return;
            }
            Acc.initAccessoryRecords(2, "");
            Acc.GetAccessoryType();
        }
        /// <summary>
        /// 报障
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            string Accessory_code = accessoryList[0].accessory_code.ToString().Trim().ToUpper();
            string machine_code=accessoryList[0].machine_code.ToString().Trim().ToUpper();


            string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryRecords_0", Accessory_code + "," + machine_code);
            List<P_Accessory_Records> ListAccessoryRecords = JsonConvert.DeserializeObject<List<P_Accessory_Records>>(RecordsJson);
            if (ListAccessoryRecords == null)
            {
                //说明第一次装载备件，啥也不干
            }
            else
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineAccessoryRecord_1", Accessory_code + "," + machine_code);
            }
            try
            {
                P_Accessory_Records Sparepart = new P_Accessory_Records()
                {
                    Accessory_code = Accessory_code,
                    machine_code = machine_code,
                    type = 1,
                    state = 0,
                    loadtime = DateTime.Now,
                    input_time = DateTime.Now,
                    Accessory_preson = Ecode,
                };
                string strJson = JsonToolsNet.ObjectToJson(Sparepart);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineAccessoryRecord_0", strJson);
            }
            catch { }

            Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "DeleteAccessoryLoad", Accessory_code);
            Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateAccessorystate", Accessory_code);
            MessageBox.Show("报障成功","提示");
            Acc.initAccessoryRecords(2, "");
            Acc.GetAccessoryType();
        }
    }
}
