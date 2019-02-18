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
    public partial class Sparepart : Form
    {
        string model = string.Empty;
        string machine_code = string.Empty;
        int machine_tate = 0;
        Main main;
        string Ecode = string.Empty;
        string machine_name = string.Empty;
        public Sparepart(string _model, string _machine_code, int _machine_state, Main _main, string _Ecode, string _machine_name)
        {
            InitializeComponent();
            model = _model;
            machine_code = _machine_code;
            machine_tate = _machine_state;
            main = _main;
            Ecode = _Ecode;
            machine_name = _machine_name;
            this.label2.Text = _machine_name + "备件管理";
            initSparepartRecords();
        }
        //卸载
        private void button2_Click(object sender, EventArgs e)
        {
            string Sparepart_code = this.dataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
            string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineSparepartRecords_0", Sparepart_code + "," + machine_code);
            List<P_Sparepart_Records> ListSparepartRecords = JsonConvert.DeserializeObject<List<P_Sparepart_Records>>(RecordsJson);
            if (ListSparepartRecords == null)
            {
                //说明第一次装载备件，啥也不干
            }
            else
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineSparepartRecord_1", Sparepart_code + "," + machine_code);
            }
            try
            {
                P_Sparepart_Records Sparepart = new P_Sparepart_Records()
                {
                    sparepart_code = Sparepart_code,
                    machine_code = machine_code,
                    type = 1,
                    state=0,
                    loadtime = DateTime.Now,
                    operate_person = Ecode,
                };
                string strJson = JsonToolsNet.ObjectToJson(Sparepart);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineSparepartRecord_0", strJson);
                MessageBox.Show("卸载成功", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("卸载失败", "提示");
                return;
            }
            initSparepartRecords();
            this.textBox1.Text = "";
        }
        //装载
        private void button1_Click(object sender, EventArgs e)
        {
            string Sparepart_code = this.textBox1.Text.Trim().ToString().ToUpper();
            if (Sparepart_code == "")
            {
                MessageBox.Show("备件编码不可为空", "提示");
                return;
            }
            string Json = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineSparepart", Sparepart_code + "," + machine_code);
            List<B_Sparepart> ListSparepart = JsonConvert.DeserializeObject<List<B_Sparepart>>(Json);
            if (ListSparepart != null)
            {
                string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineSparepartRecords_1", Sparepart_code + "," + machine_code);
                List<P_Sparepart_Records> ListSparepartRecords = JsonConvert.DeserializeObject<List<P_Sparepart_Records>>(RecordsJson);
                if (ListSparepartRecords == null)
                {
                    //说明第一次装载备件，啥也不干
                }
                else
                {
                    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineSparepartRecord_0", Sparepart_code + "," + machine_code);
                }
                try
                {
                    P_Sparepart_Records Sparepart = new P_Sparepart_Records()
                    {
                        sparepart_code = Sparepart_code,
                        machine_code = machine_code,
                        type = 0,
                        state = 1,
                        loadtime = DateTime.Now,
                        operate_person = Ecode,
                    };
                    string strJson = JsonToolsNet.ObjectToJson(Sparepart);
                    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineSparepartRecord_0", strJson);
                    MessageBox.Show("装载成功", "提示");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("装载失败", "提示");
                    return;
                }
            }
            else
            {
                MessageBox.Show("该备件不属于设备型号：[" + machine_name + "]", "提示");
                return;
            }
            initSparepartRecords();
        }
        //加载正在运行的备件信息
        public void initSparepartRecords()
        {
            string StrJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineSparepartRecord", machine_code);
            List<P_GetSparepartRecords_Result> proc = JsonConvert.DeserializeObject<List<P_GetSparepartRecords_Result>>(StrJson);
            this.dataGridView1.DataSource = proc;
        }
    }
}
