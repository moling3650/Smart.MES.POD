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
using ComponentFactory.Krypton.Toolkit;
using LEMES_POD.Tools;

namespace LEMES_POD.CustomControl
{
    public partial class MachinePanel : UserControl
    {
        string machine_code = string.Empty;
        int machine_state = 0;
        string model = string.Empty;
        Main main;
        string _machine_name = string.Empty;
        ILE.IJob job;
        List<V_Station_Shop> List;
        public MachinePanel(string machine_name, int _state, string _machine_code, string _model, ILE.IJob _job, Main _main, List<V_Station_Shop> list)
        {
            InitializeComponent();
            this.label1.Text = machine_name;
            machine_code = _machine_code;
            machine_state = _state;
            model = _model;
            _machine_name = machine_name;
            main = _main;
            job = _job;
            List = list;
            initImage(machine_state);
        }
        //加载图片 
        public void initImage(int machine_state)
        {
            if (machine_state == 0)
            {
                this.pictureBox1.Image = Properties.Resources.ball_y;
            }
            if (machine_state == 1)
            {
                this.pictureBox1.Image = Properties.Resources.ball_green;
            }
            if (machine_state == 2 || machine_state == 3)
            {
                this.pictureBox1.Image = Properties.Resources.ball_red;
            }

        }
        //双击panle事件 
        private void MachinePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (job.EmpCode != null)
            {
                (new UserForm.Accessories(model, machine_code, machine_state, main, job.EmpCode, _machine_name)).ShowDialog();
            }
            else
            {
                MessageBox.Show("请输入员工号", "提示");
            }

        }
        //开机
        public void pictureBox2_Click(object sender, EventArgs e)
        {
            if (job.EmpCode != null)
            {
                if (machine_state == 1)
                {
                    MessageBox.Show("该设备正在运行中", "提示");
                    return;
                }
                if (machine_state == 2)
                {
                    MessageBox.Show("该设备维修中", "提示");
                    return;
                }
                if (machine_state == 3)
                {
                    MessageBox.Show("该设备保养中", "提示");
                    return;
                }
                else
                {
                    DialogResult result = KryptonMessageBox.Show("确定开机吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    #region
                    //string Json = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineStopRecords", machine_code);
                    //List<B_Machine_StopRecords> ListMachineStopRecords = JsonConvert.DeserializeObject<List<B_Machine_StopRecords>>(Json);
                    //if (ListMachineStopRecords == null)
                    //{
                    //    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineStopRecordsClose", machine_code + "," + "" + "," + machine_state.ToString());
                    //}
                    //else
                    //{
                    //    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineStopRecordsClose", machine_code + "," + "" + "," + machine_state.ToString());
                    //}
                    //Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineStateOpen", machine_code);
                    #endregion
                    Component.Machine.AddMachine_StopRecordsOpen(machine_code, List, job.EmpCode);
                    //开机开启计时器
                    //main.Timer();
                }

                KryptonComboBox com = new KryptonComboBox();
                com = (KryptonComboBox)main.Controls.Find("kcb_station", true)[0];
                string Station_code = ((Tools.ComboboxItem)com.SelectedItem).Value.ToString();// 选中的工序名称
                string ip = "";
                //main.GetMachineInfo(ip, Station_code);
            }
            else
            {
                MessageBox.Show("请输入员工号", "提示");
            }
        }
        //关机
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            bool isAll = false;
            if (job.EmpCode != null)
            {
                if (machine_state == 0 || machine_state == 2 || machine_state == 3)
                {
                    MessageBox.Show("该设备已关机", "提示");
                    return;
                }
                else
                {
                    (new UserForm.MachineOFF(machine_code, machine_state, main, job.EmpCode, isAll)).ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("请输入员工号", "提示");
            }
        }
        //报障
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            string ExceptionJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineExceptionData", machine_code);
            List<P_Machine_Exception> ListException = JsonConvert.DeserializeObject<List<P_Machine_Exception>>(ExceptionJson);

            if (job.EmpCode != null)
            {
                if (machine_state == 1)
                {
                    MessageBox.Show("该设备正在运行中，请关机", "提示");
                    return;
                }
                if (machine_state == 0)
                {
                    MessageBox.Show("该设备待机中,无需报障", "提示");
                    return;
                }
                if (machine_state == 2 && ListException != null)
                {
                    MessageBox.Show("该设备出现故障，维修中", "提示");
                    return;
                }
                if (machine_state == 3)
                {
                    MessageBox.Show("该设备保养中", "提示");
                    return;
                }
                else
                {
                    (new UserForm.MachineReportFault(model, machine_code, machine_state, main, job.EmpCode)).ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("请输入员工号", "提示");
            }
        }
        //单击
        private void label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (job.EmpCode != null)
            {
                (new UserForm.Accessories(model, machine_code, machine_state, main, job.EmpCode, _machine_name)).ShowDialog();
            }
            else
            {
                MessageBox.Show("请输入员工号", "提示");
            }
        }

        //private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        //{
        //    toolTip1.SetToolTip(this.pictureBox1, "设备状态");

        //}
    }
}
