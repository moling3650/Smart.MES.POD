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
using ComponentFactory.Krypton.Toolkit;
using LEMES_POD.Tools;

namespace LEMES_POD.UserForm
{
    public partial class MachineOFF : Form
    {
        public string F_code { get; set; }
        Dictionary<String, String> SaveFaultreason = new Dictionary<string, string>(); //保存工位
        string model = string.Empty;
        string machine_code = string.Empty;
        int machine_tate = 0;
        Main main;
        string Ecode = string.Empty;
        List<V_Station_Shop> List;
        bool isAll;
        public MachineOFF( string _machine_code, int _machine_state, Main _main, string _Ecode, bool _isAll)
        {
            InitializeComponent();
            //model = _model;
            machine_code = _machine_code;
            machine_tate = _machine_state;
            main = _main;
            //Ecode = _Ecode;
            //List = _List;
            isAll = _isAll;
            initOffFaultreason();
        }

        //加载关机原因
        public void initOffFaultreason()
        {
            string Json = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineOFFFaultreason", "");
            List<B_Machine_Stopreason> ListFaultreason = JsonConvert.DeserializeObject<List<B_Machine_Stopreason>>(Json);
            for (int i = 0; i < ListFaultreason.Count(); i++)
            {
                Tools.ComboboxItem item = new Tools.ComboboxItem();
                item.Value = ListFaultreason[i].stopreason_code;
                item.Text = ListFaultreason[i].stopreason_name;
                this.comboBox1.Items.Add(item);
                SaveFaultreason.Add(ListFaultreason[i].stopreason_name, ListFaultreason[i].stopreason_code);//保存键值
                this.comboBox1.SelectedIndex = this.comboBox1.Items.Count > 0 ? 0 : -1;
            }

        }
       
        //关闭窗体
        bool isCloseClient = true;
        private void MachineOFF_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCloseClient == true)
            {
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void MachineOFF_Load(object sender, EventArgs e)
        {

        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string Faultreason_code = ((Tools.ComboboxItem)this.comboBox1.SelectedItem).Value.ToString();// 选中关机原因
          
            if (!isAll)
            {
                DialogResult result = KryptonMessageBox.Show("确定登记此原因吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                else
                {
                    P_Machine_State_Record record = new P_Machine_State_Record();
                    record.machine_code = this.machine_code;
                    record.be_current=1;
                    record.state = this.machine_tate;
                    record.start_time = DateTime.Now;
                    record.stop_reason = ((Tools.ComboboxItem)this.comboBox1.SelectedItem).Value.ToString();
                    //AddMachineStateRecord
                    string json = JsonConvert.SerializeObject(record);
                    string StopreasonJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineStateRecord", json);
                }
                //Component.Machine.AddMachine_StopRecordsClose(machine_code, List, Faultreason_code, Ecode);
                //KryptonComboBox com = new KryptonComboBox();
                //com = (KryptonComboBox)main.Controls.Find("kcb_station", true)[0];
                //string Station_code = ((Tools.ComboboxItem)com.SelectedItem).Value.ToString();// 选中的工序名称
                //string ip = "";
                //main.GetMachineInfo(ip, Station_code);
            }
            else
            {
                DialogResult result = KryptonMessageBox.Show("确定设备全部关机吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                F_code = Faultreason_code;
            }
            isCloseClient = false;
            this.Close();
        }

    }
}
