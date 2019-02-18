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

namespace LEMES_POD.UserForm
{
    public partial class MachineReportFault : Form
    {
        Dictionary<String, String> SaveFaultreason = new Dictionary<string, string>(); //保存工位
        string model = string.Empty;
        string machine_code = string.Empty;
        int machine_tate = 0;
        Main main;
        string Ecode = string.Empty;
        public MachineReportFault(string _model, string _machine_code, int _machine_state, Main _main, string _Ecode)
        {
            InitializeComponent();
            model = _model;
            machine_code = _machine_code;
            machine_tate = _machine_state;
            main = _main;
            Ecode = _Ecode;
            initFaultreason();
        }
        //加载故障原因
        public void initFaultreason()
        {
            string MachineJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachine", machine_code);
            List<V_Station_Machine> MachineList = JsonConvert.DeserializeObject<List<V_Station_Machine>>(MachineJson);
            string type_id=string.Empty;
            if (MachineList != null)
            {
                type_id =MachineList[0].type_id.ToString();
            }
            string Json = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineFaultphenomenon", type_id);
            List<B_Machine_Faultphenomenon> ListFaultreason = JsonConvert.DeserializeObject<List<B_Machine_Faultphenomenon>>(Json);
            if (ListFaultreason != null)
            {
                for (int i = 0; i < ListFaultreason.Count(); i++)
                {
                    Tools.ComboboxItem item = new Tools.ComboboxItem();
                    item.Value = ListFaultreason[i].faultphenomenon_code;
                    item.Text = ListFaultreason[i].faultphenomenon_name;
                    this.comboBox1.Items.Add(item);
                    SaveFaultreason.Add(ListFaultreason[i].faultphenomenon_name, ListFaultreason[i].faultphenomenon_code);//保存键值
                }
                this.comboBox1.SelectedIndex = this.comboBox1.Items.Count > 0 ? 0 : -1;
            }
            this.comboBox2.SelectedIndex = this.comboBox2.Items.Count > 0 ? 0 : -1;
        }
        //确定
        private void button1_Click(object sender, EventArgs e)
        {
            string Faultreason_code = ((Tools.ComboboxItem)this.comboBox1.SelectedItem).Value.ToString();// 选中故障原因
            string Grade = this.comboBox2.Text.ToString();
            string Remarks = this.textBox2.Text.ToString();
            string ExceptionJson = string.Empty;
            try
            {
                P_Machine_Exception Exception = new P_Machine_Exception()
                {
                    exception_code = Guid.NewGuid(),
                    faultphenomenon_code = Faultreason_code,
                    machine_code = machine_code,
                    state = 1,
                    grade = Grade,
                    submit_time = DateTime.Now,
                    submit_person = Ecode,
                    remarks = Remarks,
                };
                string strJson = JsonToolsNet.ObjectToJson(Exception);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineException", strJson);
                MessageBox.Show("上报成功", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("上报失败", "提示");
                return;
            }
            this.Close();

        }
    }
}
