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
using LEMES_POD.Tools;
using ComponentFactory.Krypton.Toolkit;

namespace LEMES_POD.UserForm
{
    public partial class Accessories : Form
    {
        string model = string.Empty;
        string machine_code = string.Empty;
        int machine_tate = 0;
        Main main;
        string Ecode = string.Empty;
        string machine_name = string.Empty;
        public Accessories(string _model, string _machine_code, int _machine_state, Main _main, string _Ecode, string _machine_name)
        {
            InitializeComponent();
            model = _model;
            machine_code = _machine_code;
            machine_tate = _machine_state;
            main = _main;
            Ecode = _Ecode;
            machine_name = _machine_name;
            this.label2.Text = "[" + _machine_name + "] 配件管理";
            treeView1.ImageList = imageList1;
            initAccessoryRecords(2, "");
            GetAccessoryType();
        }

        /// <summary>
        ///  卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {

            string Accessory_code = this.textBox1.Text.ToString().Trim().ToUpper();
            //判断已装载的配件是否卸载，如果已卸载，不可重复卸载
            #region
            //string RecordsJson1 = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessory", Accessory_code + "," + machine_code);
            //List<B_Machine_Accessories> ListAccessory = JsonConvert.DeserializeObject<List<B_Machine_Accessories>>(RecordsJson1);
            //if (ListAccessory != null)
            //{
            //    foreach (var item in ListAccessory)
            //    {
            //        if (item.accessory_isload == 0)
            //        {
            //            MessageBox.Show("编码为[" + Accessory_code + "]的配件已卸载！", "提示");
            //            return;
            //        }
            //    }
            //}
            #endregion
            string RecordsJson1 = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_1", machine_code + "," + Accessory_code);
            List<P_Accessoris_Load> ListAccessory = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(RecordsJson1);
            if (ListAccessory == null)
            {
                MessageBox.Show("编码为[" + Accessory_code + "]的配件已卸载！", "提示");
                return;
            }
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
            initAccessoryRecords(2, "");
            GetAccessoryType();
            this.textBox1.Text = "";
        }
        /// <summary>
        ///  装载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            string Accessory_code = this.textBox1.Text.Trim().ToString().ToUpper();
            if (Accessory_code == "")
            {
                MessageBox.Show("配件编码不可为空", "提示");
                return;
            }
            //判断设备是否保养，是否可用
            string AccessoryJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetAccessory", Accessory_code);
            List<B_Accessories> listAccessory = JsonConvert.DeserializeObject<List<B_Accessories>>(AccessoryJson);
            int type_id = 0;
            if (listAccessory != null)
            {
                foreach (var item in listAccessory)
                {
                    if (item.state == 1)
                    {
                        MessageBox.Show("配件 [" + item.accessory_name + " ]已使用", "提示");
                        return;
                    }
                    if (item.state == 2)
                    {
                        MessageBox.Show("配件 [" + item.accessory_name + " ]保养中", "提示");
                        return;
                    }
                    if (item.state == 3)
                    {
                        MessageBox.Show("配件 [" + item.accessory_name + " ]维修中", "提示");
                        return;
                    }
                    if (item.state == 4)
                    {
                        MessageBox.Show("配件 [" + item.accessory_name + " ]已报废", "提示");
                        return;
                    }
                    if (item.maintian_type == 0)
                    {
                        if (((DateTime)(item.maintain_nexttime)).AddMinutes(-5) <= DateTime.Now && DateTime.Now <= ((DateTime)(item.maintain_nexttime)).AddMinutes(5))
                        {
                            MessageBox.Show("配件[" + item.accessory_name + "]已到保养时间，请保养！", "提示");
                            return;
                        }
                    }
                    if (item.maintian_type == 1)
                    {
                        if (item.Userqty >= item.maintain_quality)
                        {
                            MessageBox.Show("配件[" + item.accessory_name + "]已达到保养件数，请保养！", "提示");
                            return;
                        }
                    }
                    type_id = (int)item.type_id;
                }
            }
            else
            {
                MessageBox.Show("配件[" + Accessory_code + "]不存在", "提示");
                return;
            }
            //判断扫描设备是否属于绑定的设备型号
            string AccessorytypeAllJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryAllType", machine_code);
            List<V_Machnie_Accessory_type> ListAccessorytypeAll = JsonConvert.DeserializeObject<List<V_Machnie_Accessory_type>>(AccessorytypeAllJson);
            if (ListAccessorytypeAll != null)
            {
                List<string> list = new List<string>();
                foreach (var item in ListAccessorytypeAll)
                {
                    list.Add(item.type_id.ToString());
                }
                if (!list.Contains(type_id.ToString()))
                {
                    MessageBox.Show("该配件不属于该设备,请核对", "提示");
                    return;
                }
            }
            else
            {
                MessageBox.Show("该设备没有绑定配件，不可装载", "提示");
                return;
            }

            //判断已装载的配件是否卸载，如果未卸载，不可重复安装
            string RecordsJson1 = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_1", machine_code + "," + Accessory_code);
            List<P_Accessoris_Load> ListAccessory = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(RecordsJson1);
            if (ListAccessory != null)
            {
                MessageBox.Show("编码为[" + Accessory_code + "]的配件已装载！", "提示");
                return;
            }
            //判断是否转载完成，完成则不可超出装载
            string MachineRecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessory", type_id.ToString() + "," + machine_code);
            List<B_Machine_Accessories> ListMachineAccessory = JsonConvert.DeserializeObject<List<B_Machine_Accessories>>(MachineRecordsJson);
            int accessory_count = 0;
            if (ListMachineAccessory != null)
            {
                foreach (var item in ListMachineAccessory)
                {
                    accessory_count = (int)item.accessory_count;
                }
            }
            string StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_0", machine_code + "," + type_id.ToString());
            List<P_Accessoris_Load> ListAccessoryLoad = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(StrJson);
            if (ListAccessoryLoad != null)
            {
                if (ListAccessoryLoad.Count >= accessory_count)
                {
                    MessageBox.Show("同类型号的配件已装载完成", "提示");
                    return;
                }
            }
            string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryRecords_1", Accessory_code + "," + machine_code);
            List<P_Accessory_Records> ListAccessoryRecords = JsonConvert.DeserializeObject<List<P_Accessory_Records>>(RecordsJson);
            if (ListAccessoryRecords == null)
            {
                //说明第一次装载配件，啥也不干
            }
            else
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineAccessoryRecord_0", Accessory_code + "," + machine_code);
            }
            try
            {
                //配件使用记录
                P_Accessory_Records Accessory_Records = new P_Accessory_Records()
                {
                    Accessory_code = Accessory_code,
                    machine_code = machine_code,
                    type = 0,
                    state = 1,
                    loadtime = DateTime.Now,
                    input_time = DateTime.Now,
                    Accessory_preson = Ecode,
                };
                string strJson = JsonToolsNet.ObjectToJson(Accessory_Records);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineAccessoryRecord_0", strJson);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineAccessoryIsLoad", Accessory_code);
                //装载过后配件绑定设备
                P_Accessoris_Load Accessoris_Load = new P_Accessoris_Load()
                {
                    accessory_code = Accessory_code,
                    accessory_type = listAccessory[0].type_id,
                    machine_code = machine_code,
                };
                string Accessoris_LoadJson = JsonToolsNet.ObjectToJson(Accessoris_Load);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineAccessoryLoad", Accessoris_LoadJson);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateAccessorystate_1", Accessory_code);
                //MessageBox.Show("装载成功", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("装载失败", "提示");
                return;
            }
            initAccessoryRecords(2, "");
            this.textBox1.Text = "";
            GetAccessoryType();
            //装载配件打开定时器
            //main.Timer();
        }
        /// <summary>
        /// 加载正在运行的配件件信息
        /// </summary>
        public void initAccessoryRecords(int isN, string nodetag)
        {
            this.flowLayoutPanel1.Controls.Clear();
            string StrJson = string.Empty;
            List<P_Accessoris_Load> ListAccessoryLoad = null;
            if (isN == 2)
            {
                StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_2", machine_code);
                ListAccessoryLoad = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(StrJson);
            }
            if (isN == 0)
            {
                StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_0", machine_code + "," + nodetag);
                ListAccessoryLoad = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(StrJson);
            }
            if (isN == 1)
            {
                StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_1", machine_code + "," + nodetag);
                ListAccessoryLoad = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(StrJson);
            }
            if (ListAccessoryLoad != null)
            {
                foreach (var item in ListAccessoryLoad)
                {
                    List<AccessoryList> accessoryList = new List<AccessoryList>();
                    accessoryList.Clear();
                    AccessoryList accessory = new AccessoryList();
                    accessory.accessory_code = item.accessory_code.ToString();

                    string AccessoryJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetAccessoryInfo", item.accessory_code);
                    List<V_Accesssory_type> ListAccessory = JsonConvert.DeserializeObject<List<V_Accesssory_type>>(AccessoryJson);
                    string accesssory_name = string.Empty;
                    string type_name = string.Empty;
                    if (ListAccessory != null)
                    {
                        foreach (var item1 in ListAccessory)
                        {
                            accesssory_name = item1.accessory_name;
                            type_name = item1.type_name;
                        }
                    }
                    accessory.accessory_name = accesssory_name.ToString();
                    //accessory.accessory_isload = (int)item.accessory_isload;
                    accessory.accessory_type = type_name.ToString();
                    accessory.machine_code = item.machine_code;
                    accessoryList.Add(accessory);
                    CustomControl.AccessoryPanel control = new CustomControl.AccessoryPanel(accessoryList, Ecode, this);
                    this.flowLayoutPanel1.Controls.Add(control);
                }
            }
        }
        /// <summary>
        /// 获取设备下配件型号，加载树
        /// </summary>
        public void GetAccessoryType()
        {
            treeView1.Nodes.Clear();
            string StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryAllType", machine_code);
            List<V_Machnie_Accessory_type> ListAccessoryAll = JsonConvert.DeserializeObject<List<V_Machnie_Accessory_type>>(StrJson);
            if (ListAccessoryAll != null)
            {
                foreach (var item in ListAccessoryAll)
                {
                    string Json = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_0", item.machine_code + "," + item.accessory_type.ToString());
                    List<P_Accessoris_Load> ListAccessoryLoad = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(Json);
                    int count = ListAccessoryLoad != null ? ListAccessoryLoad.Count : 0;
                    TreeNode node = new TreeNode(item.accessory_type.ToString());
                    node.Tag = item.accessory_type;
                    node.Text = item.type_name + "(" + count + "/" + item.accessory_count + ")";
                    //node.ImageIndex = node.SelectedImageIndex;

                    if (count < item.accessory_count)
                    {
                        pictureBox1.Image = Properties.Resources.attention;
                        node.ImageIndex = 0;
                        node.SelectedImageIndex = 0;
                    }
                    else
                    {
                        pictureBox1.Image = Properties.Resources.correct;
                        node.ImageIndex = 1;
                        node.SelectedImageIndex = 1;
                    }
                    treeView1.Nodes.Add(node);
                    GetAccessoryCode(item.machine_code.ToString(), item.accessory_type.ToString(), item.accessory_count.ToString(), node);
                }
            }
        }
        /// <summary>
        /// 给树赋值
        /// </summary>
        /// <param name="accessory_type"></param>
        /// <param name="accesssory_count"></param>
        /// <param name="node"></param>
        public void GetAccessoryCode(string machine_code, string accessory_type, string accesssory_count, TreeNode node)
        {
            string StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineAccessoryLoad_0", machine_code + "," + accessory_type);
            List<P_Accessoris_Load> ListAccessoryLoad = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(StrJson);
            if (ListAccessoryLoad == null)
            {
                for (int i = 0; i < int.Parse(accesssory_count); i++)
                {
                    TreeNode treenode = new TreeNode(accessory_type);
                    treenode.Tag = "";
                    treenode.Text = "无";
                    treenode.ImageIndex = 0;
                    treenode.SelectedImageIndex = 0;
                    node.Nodes.Add(treenode);
                }
            }
            else
            {
                foreach (var item in ListAccessoryLoad)
                {
                    TreeNode treenode = new TreeNode(accessory_type);
                    treenode.Tag = item.accessory_code;
                    string AccessoryJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetAccessory", item.accessory_code);
                    List<P_Accessoris_Load> ListAccessory = JsonConvert.DeserializeObject<List<P_Accessoris_Load>>(AccessoryJson);
                    string accesssory_name = string.Empty;
                    if (ListAccessory != null)
                    {
                        foreach (var item1 in ListAccessory)
                        {
                            accesssory_name = item1.accessory_name;
                        }
                    }
                    treenode.ImageIndex = 1;
                    treenode.SelectedImageIndex = 1;
                    treenode.Text = accesssory_name;
                    node.Nodes.Add(treenode);
                }
            }
        }
        /// <summary>
        /// 扫描配件，进行装卸载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            if (radioButton1.Checked == true)
            {
                button1_Click(sender, e);
            }
            if (radioButton2.Checked == true)
            {
                button2_Click(sender, e);
            }
        }
        /// <summary>
        /// 操作树
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        string nodetag = string.Empty;
        string nodetext = string.Empty;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            nodetag = e.Node.Tag.ToString();
            nodetext = e.Node.Text.ToString();
            if (treeView1.SelectedNode.Level == 0)
            {
                int isN = 0;
                initAccessoryRecords(isN, nodetag);
            }
            if (treeView1.SelectedNode.Level == 1)
            {
                int isN = 1;
                initAccessoryRecords(isN, nodetag);
            }
        }
    }
    /// <summary>
    /// 构造对象，传参用
    /// </summary>
    public class AccessoryList
    {
        public string accessory_code { get; set; }
        public string accessory_name { get; set; }
        public int accessory_isload { get; set; }
        public string machine_code { get; set; }
        public string accessory_type { get; set; }
    }
}
