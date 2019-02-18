using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ILE;
using LEMES_POD.UserForm;
using Newtonsoft.Json;
using LEDAO;

namespace LEMES_POD.CustomControl
{
    public partial class StepPanel : UserControl
    {
        private Main _main;
        public StepPanel(ILE.IStep Step, Main Main)
        {
            InitializeComponent();
            _main = Main;
            lbStep.Text = Step.StepName;
            lbStyle.Text = Step.StepType;
            if (!string.IsNullOrEmpty(Step.Matcode))
            {
                label5.Visible = true;
                lbDrive.Visible = true;
                lbDrive.Text = Step.Matcode + "/" + Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Step", "GetMatCodebyName", Step.Matcode);

            }

            lbAuto.Text = Step.AutoRun == 1 ? "自动" : "手动";
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="val"></param>
        public void FunInit()
        {
            ptbImg.Image = Properties.Resources.control_pause_32;
            txtValue.Text = "";
        }
        /// <summary>
        /// 单纯的加载完成标志
        /// </summary>
        public void FunCompleted()
        {
            ptbImg.Image = Properties.Resources.yes;
        }
        /// <summary>
        /// 完成标志
        /// </summary>
        /// <param name="val"></param>
        public void FunCompleted(string val, string step_code, IJob job, int j, Main _main,int nowidx)
        {
            //LoadJude(job, j, _main);
            ptbImg.Image = Properties.Resources.yes;
            txtValue.Text = val;
            if (job.StepIdx > nowidx+1)
            {
                if (job.StepList[j].AllowReuse == 0)
                    LoadSonStep(step_code, job, j, _main);
            }
            else
            {
                LoadSonStep(step_code, job, j, _main);
            }
        }

        /// <summary>
        /// 完成标志
        /// </summary>
        /// <param name="val"></param>
        public void FunCompletedAll(string val, string step_code, IJob job, int j, Main _main)
        {
            //LoadJude(job, j, _main);
            ptbImg.Image = Properties.Resources.yes;
            txtValue.Text = val;
            if (job.StepIdx < job.StepList.Count - 1)
            {
                if (job.StepList[j].AllowReuse == 0)
                    LoadSonStep(step_code, job, j, _main);
            }
            else  //如果当前工步时最后一个工步，说明最后一个工步不可能是重用工步
            {
                LoadSonStep(step_code, job, j, _main);
            }
        }
        /// <summary>
        /// 弹出待判
        /// </summary>
        /// <param name="job"></param>
        //public void LoadJude(IJob job, int j, Main main)
        //{
        //    if (job.StepList[j].AutoRun != 1)
        //    {
        //        string JudeJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SFC", "GetJudeBySfc", job.SFC);
        //        List<P_SFC_Jude> listJude = JsonConvert.DeserializeObject<List<P_SFC_Jude>>(JudeJson);
        //        if (listJude != null)
        //        {
        //            UserForm.JudePrompt Jude = new UserForm.JudePrompt(job.SFC, listJude);
        //            Jude.ShowDialog();
        //            if (Jude.State == 0)
        //            {
        //                return;
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 弹出子工步
        /// </summary>
        /// <param name="step_code"></param>
        /// <param name="job"></param>
        /// <param name="j"></param>
        /// <param name="_main"></param>
        public void LoadSonStep(string step_code, IJob job, int j, Main _main)
        {
            bool isfedbatch = false;
            string sfc = "";
            ILE.IResult res = BindSonStep(step_code, job.FlowCode);
            if (res.obj != null)
            {
                List<B_ProcessSonStep> SonStepList = JsonConvert.DeserializeObject<List<B_ProcessSonStep>>(res.obj.ToString());
                if (res.Result)
                {
                    ServiceReference.ServiceClient clien = new ServiceReference.ServiceClient();
                    string step_name = clien.RunServerAPI("BLL.Step", "GetStepNmae", step_code + "," + job.Product);
                    ProcessSonStepForm gridform = new ProcessSonStepForm(SonStepList, job, isfedbatch, sfc, step_name, j, _main);
                    gridform.ShowDialog();
                }

            }
        }

        public static IResult BindSonStep(string step_code, string flow_code)
        {
            return BLL.ServiceReference.DISResult("BLL.Step", "GetSonStepAll", step_code + "," + flow_code);
        }
        /// <summary>
        /// 失败
        /// </summary>
        public void FunError()
        {
            ptbImg.Image = Properties.Resources.no;
        }

        /// <summary>
        /// 等待
        /// </summary>
        public void FunWait()
        {
            ptbImg.Image = Properties.Resources.loading2;
            txtValue.Text = "";
        }

        public void FunStop()
        {
            ptbImg.Image = Properties.Resources.control_pause_32;
        }

        private void lbAuto_Click(object sender, EventArgs e)
        {

        }

        private void ptbImg_Click(object sender, EventArgs e)
        {
            if (ptbImg.Image == Properties.Resources.control_play_32)
            {
                _main.StartJob();
            }
        }
    }
}
