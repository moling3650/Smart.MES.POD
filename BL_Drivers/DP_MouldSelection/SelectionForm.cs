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

namespace DP_MouldSelection
{
    public partial class SelectionForm : Form
    {
        public ILE.IResult res
        {
            get;
            private set;
        }
        ServiceReference.ServiceClient client;
        public string Mould_Code
        {
            get;
            set;
        }

        public B_Machine _machine
        {
             get;
             set;
        }
        private string _station
        {
            get;
            set;
        }
        private string _emp
        {
            get;
            set;
        }

        public SelectionForm(B_Machine machine,string station,string empCode)
        {
            InitializeComponent();
            this.client = new ServiceReference.ServiceClient();
            this._machine = machine;
            this._station = station;
            this._emp = empCode;
            this.res = new ILE.LEResult();
        }

        

        private void SelectionForm_Load(object sender, EventArgs e)
        {

            ///检查设备是否需要模具
            this.treeView1.ExpandAll();
            string mouldkinds = client.RunServerAPI("BLL.Machine", "GetMachineMouldKinds", this._machine.machine_code);
            ////////

            if (mouldkinds == "")
            {
                this.label1.Text = "当前设备不需要模具";
                this.res.Result = true;
                this.res.ExtMessage = "当前设备不需要模具";
                return;
            }

            List<dynamic> kinds = JsonConvert.DeserializeObject<List<dynamic>>(mouldkinds);

            //获取设备下已安装的模具
            string ists = client.RunServerAPI("BLL.Machine", "GetMachineInstallMoulds", this._machine.machine_code);
            List<dynamic> istMoulds = JsonConvert.DeserializeObject<List<dynamic>>(ists);

            foreach (dynamic kind in kinds)
            {
                TreeNode nod = new TreeNode();
                nod.Name = kind.mould_kind_id.ToString();
                nod.Tag = kind.qty;
                nod.Text =kind.manufacturer+"-"+kind.model_code;
                
                //var res = context.B_Process_Flow_Detail.Where(x => x.pid == intpid).Select(c => c.strict).ToList();
                if (istMoulds != null)
                {
                    var res = istMoulds.Where(x => x.kind_id == kind.mould_kind_id);

                    foreach (dynamic a in res)
                    {
                        TreeNode snode = new TreeNode();
                        snode.Name = a.mould_code;
                        snode.Text = a.mould_code + "[" + a.mould_name + "]";
                        
                        nod.Nodes.Add(snode);
                    }
                    nod.Text = nod.Text + "(" + res.Count().ToString() + "/" + kind.qty + ")";
                }
                else
                {
                    nod.Text = nod.Text + "(0/"+kind.qty+")";
                }

                this.treeView1.Nodes.Add(nod);
            }

            this.llb_machine.Text = "【" + this._machine.machine_name + "】";
            string ResDispatching = client.RunServerAPI("BLL.Machine", "GetMachinePPT_Detail", this._machine.machine_code);
            List<V_Machine_PPT_Detail> produc = JsonConvert.DeserializeObject<List<V_Machine_PPT_Detail>>(ResDispatching);
            foreach (V_Machine_PPT_Detail bmp in produc)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text=bmp.ppt_name;

                string cdt=null;
                if(bmp.ppt_condition=="between")
                {
                    cdt=bmp.ppt_min.ToString()+"<="+bmp.ppt_name+"<="+bmp.ppt_max.ToString();
                }
                else
                {
                    cdt=bmp.ppt_condition+bmp.ppt_val;
                }
                lvi.SubItems.Add(cdt);
                
                lvBind.Items.Add(lvi);
            }
            //if (produc.Count < 1)
            //{
            //    res.Result = false;
            //    res.ExtMessage = "该工位未绑定设备，无法使用模具";
            //    return res;
            //}
        }
    


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13) return;
            //ktb_input.Enabled = false;
            textBox1.Text = textBox1.Text.Replace("#", "");
            textBox1.Text = textBox1.Text.ToString().Trim(',');
            textBox1.Text = textBox1.Text.ToString().Trim('，');
            textBox1.Text = textBox1.Text.ToString().Trim('.');
            textBox1.Text = textBox1.Text.ToString().Trim('。');
            textBox1.Text = textBox1.Text.ToString().Trim();

            string md = this.textBox1.Text;
            this.textBox1.Text = "";

            try
            {
                string json = client.RunServerAPI("BLL.Moulds", "GetMouldByCode", md);

                if (this.treeView1.Nodes.Count == 0)
                {
                    this.label1.Text = "当前设备不需要模具";
                    return;
                }

                if (json.IndexOf("EXCEPTION") > -1)
                {
                    this.label1.Text = json;
                    return;
                }

                if (json == "")
                {
                    this.label1.Text = "模具不存在";
                    return;
                }

                dynamic mds = JsonConvert.DeserializeObject<dynamic>(json);

                if (mds.state == 0)
                {
                    this.label1.Text = "模具未启用!";
                    return;
                }

                string json1 = client.RunServerAPI("BLL.Moulds", "GetMouldInstall", md);
                if (json1!="")
                {
                    this.label1.Text = "模具已被使用!";
                    return;
                }

                foreach (TreeNode ctrl in treeView1.Nodes)
                {
                    if (ctrl.Name == mds.mould_kind_id.ToString()) //
                    {
                        if (ctrl.Nodes.Count.ToString() == ctrl.Tag.ToString()) //TAG中存的是这个类型的需求量
                        {
                            this.label1.Text = "此类型模具已安装完毕，无法继续安装";
                            return;
                        }
                        else //安装模具
                        {
                            TreeNode snode = new TreeNode();
                            snode.Name = mds.mould_code;
                            snode.Text = mds.mould_code + "[" + mds.mould_name + "]";
                            snode.ForeColor = Color.Blue;
                            ctrl.Nodes.Add(snode);

                            string name=ctrl.Text.Substring(0,ctrl.Text.IndexOf("("));
                            name = name + "(" + ctrl.Nodes.Count.ToString() + "/" + ctrl.Tag.ToString() + ")";
                            ctrl.Text = name;

                            string param = this._machine.machine_code + "," + this._station + "," + mds.mould_code + "," + DateTime.Now.ToString() + "," + this._emp;
                            client.RunServerAPI("BLL.Moulds", "MouldInstall", param);

                            return;
                        }
                    }
                }
                this.label1.Text = "模具不是可被当前设备使用的类型";
                return;
            }
            catch(Exception exc)
            { 
                this.label1.Text=exc.Message;
            }
            finally
            {
                this.textBox1.Text = "";
                this.treeView1.ExpandAll();
            }
        }

        private void ktb_shutMould_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Tag == null)
            {
                string mouldCode = treeView1.SelectedNode.Name;
                TreeNode parent = treeView1.SelectedNode.Parent;
                treeView1.SelectedNode.Remove();

                client.RunServerAPI("BLL.Moulds", "MouldUninstall", mouldCode);
                string name = parent.Text.Substring(0, parent.Text.IndexOf("("));
                name = name + "(" + parent.Nodes.Count.ToString() + "/" + parent.Tag.ToString() + ")";
                parent.Text = name;
                
            }

            //把模具与设备绑定表数据删掉【P_Machine_Mould_Install】
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            foreach (TreeNode ctrl in treeView1.Nodes)
            {
                if (ctrl.Nodes.Count.ToString() != ctrl.Tag.ToString()) //TAG中存的是这个类型的需求量
                {
                    this.res.Result = false;
                    this.res.ExtMessage = "模具安装不全";
                    this.Close();
                    return;
                }
            }
            this.res.Result = true;
            this.res.ExtMessage = "";
            this.Close();
            //return true;
        }

    }
}
