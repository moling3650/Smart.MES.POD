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
using ILE;

namespace LEMES_POD.UserForm
{
    public partial class FedBatchManage : Form
    {
        IJob job;
        Main main;
        public FedBatchManage(IJob _job, Main _main)
        {
            InitializeComponent();
            job = _job;
            main = _main;
            this.textOrderNo.Text = job.OrderNO;
        }
        //查询
        private void button1_Click(object sender, EventArgs e)
        {
            BingData();
        }
        public void BingData()
        {
            string sfc = this.textSFC.Text.ToString().Trim();
            if (sfc == "")
            {
                MessageBox.Show("批次号不能为空", "提示");
                return;
            }
            string FedBatchList = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetFedBatchList", sfc);
            List<V_FedBatch> JsonFedBatch = JsonConvert.DeserializeObject<List<V_FedBatch>>(FedBatchList);
            this.dataGridView1.DataSource = JsonFedBatch;
        }
        //双击行事件
        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            bool isFedBatch = true;
            string sfc = this.textSFC.Text.ToString().Trim().ToUpper();
            string step_code = this.dataGridView1.SelectedRows[0].Cells["step_code"].Value.ToString();
            string flow_code = this.dataGridView1.SelectedRows[0].Cells["flow_code"].Value.ToString();
            ILE.IResult res = BindSonStep(step_code, flow_code);
            if (res.obj != null)
            {
                List<B_ProcessSonStep> SonStepList = JsonConvert.DeserializeObject<List<B_ProcessSonStep>>(res.obj.ToString());
                if (res.Result)
                {
                    ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                    string step_name = clien.RunServerAPI("BLL_Step", "GetStepNmae", step_code + "," + job.Product);
                    ProcessSonStepForm gridform = new ProcessSonStepForm(SonStepList, job, isFedBatch, sfc, step_name, 0, main);
                    gridform.ShowDialog();
                }
            }
        }
        public static IResult BindSonStep(string step_code, string flow_code)
        {
            return BLL.ServiceReference.DISResult("BLL.Step", "GetSonStepAll", step_code + "," + flow_code);
        }
    }
}
