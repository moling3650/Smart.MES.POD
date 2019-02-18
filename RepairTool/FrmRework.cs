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

namespace RepairTool
{
    public partial class FrmRework : Form
    {
        public FrmRework()
        {
            InitializeComponent();
        }
        public string g_strWorkShopCode = "";
        List<SFC_LVL_Data> g_lstSfclvlData = new List<SFC_LVL_Data>();
        private void FrmRework_Load(object sender, EventArgs e)
        {
            txtBox_registeredsfc.TabIndex = 0;
            txtBox_registeredsfc.Focus();

            panel1.Height = groupBox2.Height - 50;
            panel1.AutoScroll = true;
        }
        private void txtBox_registeredsfc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_getsfcinfo_Click(null,null);
            }
        }
        private void btn_getsfcinfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtBox_registeredsfc.Text.Trim()))
                {
                    //MessageBox.Show("批次号为空,请检查", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ((FrmRepairTool)this.Owner).SendMsgShow("批次号为空,请检查", 1);
                    return;
                }
                //添加根节点
                //TreeNode node = new TreeNode();
                //node.Text = txtBox_registeredsfc.Text.Trim();
                //node.Tag = 0;
                //treeView1.Nodes.Add(node);
                //txtBox_registeredsfc.Text = "";
                g_lstSfclvlData.Clear();
                treeView1.Nodes.Clear();
                treeView1.LabelEdit = true;//不可缺少

                ((FrmRepairTool)this.Owner).SendMsgShow("获取当前批次号下所有批次信息", 0);
                GetSFCLVLData(txtBox_registeredsfc.Text.Trim(), 0, "", "");

                ((FrmRepairTool)this.Owner).SendMsgShow("将批次信息展示为树结构", 0);
                setTreeView(treeView1, "", "");

                ((FrmRepairTool)this.Owner).SendMsgShow("获取批次信息完成", 0);

                //清楚panel中控件信息
                query.Clear();
                this.panel1.Controls.Clear();

            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        private void GetSFCLVLData(string strsfc,int lvl,string strparentsfc,string strparparentsfc)
        {
            try
            {
                SFC_LVL_Data sfclvldata = new SFC_LVL_Data();
                sfclvldata.SFC = strsfc.Trim();
                sfclvldata.LVL = lvl + 1;
                sfclvldata.strWorkOrder = "";
                sfclvldata.strMatCode = "";
                sfclvldata.dmaxqty = 0;
                sfclvldata.parentSFC = strparentsfc;
                sfclvldata.parparentSFC = strparparentsfc;
                g_lstSfclvlData.Add(sfclvldata);

                ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                string dt = client.RunServerAPI("LEDIS.BLL.SFC_ProcessData", "GetSFCInfoLVLBySFC", sfclvldata.SFC.Trim() + "," + g_strWorkShopCode.Trim());
                if (string.IsNullOrEmpty(dt)
                    || dt.Contains("GetSFCInfoLVLBySFC-Fail,"))
                {
                    ((FrmRepairTool)this.Owner).SendMsgShow(dt, 2);
                    return;
                }
                if (dt.Contains("GetSFCInfoLVLBySFC-OK,当前批次号没有下级批次"))
                {
                    //没有下级批次,获取当前sfc属性，工单，物料信息
                    string dtsfc = client.RunServerAPI("LEDIS.BLL.SFC_ProcessData", "GetOrderInfoBySFC", sfclvldata.SFC.Trim() + "," + g_strWorkShopCode.Trim());
                    if (string.IsNullOrEmpty(dtsfc)
                        || dtsfc.Contains("GetOrderInfoBySFC-Fail,"))
                    {
                        ((FrmRepairTool)this.Owner).SendMsgShow(dtsfc, 2);
                        return;
                    }
                    List<UserDefineData_SFCProcessData> lstsfcpsfcprocessdata = JsonConvert.DeserializeObject<List<UserDefineData_SFCProcessData>>(dtsfc);    //获取sfc属性
                    if (lstsfcpsfcprocessdata.Count < 1)
                    {
                        //没有下级批次
                        return;
                    }

                    sfclvldata.strWorkOrder = lstsfcpsfcprocessdata[0].order_no;
                    sfclvldata.strMatCode = lstsfcpsfcprocessdata[0].mat_code;
                    sfclvldata.dmaxqty = decimal.Parse(lstsfcpsfcprocessdata[0].step_code.Trim());
                    return;
                }
                List<UserDefineData_SFCProcessData> lstpsfcprocessdata = JsonConvert.DeserializeObject<List<UserDefineData_SFCProcessData>>(dt);    //P_BarCodeBing中获取sfc属性
                if (lstpsfcprocessdata.Count < 1)
                {
                    //没有下级批次
                    return;
                }
                sfclvldata.strWorkOrder = lstpsfcprocessdata[0].order_no;
                sfclvldata.strMatCode = lstpsfcprocessdata[0].mat_code;
                sfclvldata.dmaxqty = decimal.Parse(lstpsfcprocessdata[0].step_code.Trim());

                for (int i = 0; i < lstpsfcprocessdata.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(lstpsfcprocessdata[i].val))
                    {
                        GetSFCLVLData(lstpsfcprocessdata[i].val, lvl + 1, sfclvldata.SFC, sfclvldata.parentSFC);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        //调用的时候parentId以0值开始 setTreeView(treeView1, 0);
        private void setTreeView(TreeView tr1, string parentId, string parparentId)
        {
            try
            {
                List<SFC_LVL_Data> TmplstSfclvlData = new List<SFC_LVL_Data>();
                TmplstSfclvlData = g_lstSfclvlData.Where(x => x.parentSFC == parentId && x.parparentSFC == parparentId).ToList();
                if (TmplstSfclvlData.Count > 0)
                {
                    string pId = "";
                    foreach (SFC_LVL_Data row in TmplstSfclvlData)
                    {
                        TreeNode node = new TreeNode();
                        node.Text = row.SFC + " / " + row.strMatCode;
                        node.Tag = row.SFC;
                        pId = row.parentSFC;
                        if (pId == "")
                        {
                            //添加根节点
                            tr1.Nodes.Add(node);
                        }
                        else
                        {
                            //添加根节点之外的其他节点
                            RefreshChildNode(tr1, node, pId, row.parparentSFC);
                        }
                        //查找以node为父节点的子节点
                        setTreeView(tr1, node.Tag.ToString().Trim(), row.parentSFC);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        //处理根节点的子节点
        private void RefreshChildNode(TreeView tr1, TreeNode treeNode, string parentId,string parparentid)
        {
            try
            {
                foreach (TreeNode node in tr1.Nodes)
                {
                    if (node.Tag.ToString().Trim() == parentId)
                    {
                        node.Nodes.Add(treeNode);
                        return;
                    }
                    else if (node.Nodes.Count > 0)
                    {
                        FindChildNode(node, treeNode, parentId, parparentid);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        //处理根节点的子节点的子节点
        private void FindChildNode(TreeNode tNode, TreeNode treeNode, string parentId, string parparentid)
        {
            try
            {
                foreach (TreeNode node in tNode.Nodes)
                {
                    if (node.Tag.ToString().Trim() == parentId
                        && tNode.Tag.ToString().Trim() == parparentid)
                    {
                        try
                        {
                            node.Nodes.Add(treeNode);
                        }
                        catch (Exception exp)
                        {
                        }
                        return;
                    }
                    else if (node.Nodes.Count > 0)
                    {
                        FindChildNode(node, treeNode, parentId, parparentid);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        private void FindAllNode(TreeNode tNode, string strnodetext, int iop)
        {
            try
            {
                foreach (TreeNode node in tNode.Nodes)
                {
                    if (node.Text.ToString().Trim() == strnodetext)
                    {
                        try
                        {
                            if (iop == 0) //取消复选框
                            {
                                node.Checked = false;
                                node.BackColor = Color.White;
                            }
                        }
                        catch (Exception exp)
                        {
                        }
                        return;
                    }
                    else if (node.Nodes.Count > 0)
                    {
                        FindAllNode(node, strnodetext, iop);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        private void AddReTestSFC(string strsfcInfo, TreeNode treeNode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strsfcInfo))
                {
                    ((FrmRepairTool)this.Owner).SendMsgShow("批次信息为空", 1);
                    treeNode.Checked = false;
                    treeNode.BackColor = Color.White;
                    return;
                }
                string[] arrsfcitem = strsfcInfo.Split('/');
                if (arrsfcitem.Length < 2
                    || string.IsNullOrWhiteSpace(arrsfcitem[0])
                    || string.IsNullOrWhiteSpace(arrsfcitem[1]))
                {
                    ((FrmRepairTool)this.Owner).SendMsgShow("批次信息不全", 1);
                    treeNode.Checked = false;
                    treeNode.BackColor = Color.White;
                    return;
                }
                //根据批次对应的数量来判断是否可以重测
                List<SFC_LVL_Data> TmplstSfclvlData = new List<SFC_LVL_Data>();
                TmplstSfclvlData = g_lstSfclvlData.Where(x => x.SFC == arrsfcitem[0].Trim()).ToList();
                if (TmplstSfclvlData.Count > 0)
                {
                    decimal maxqty = TmplstSfclvlData[0].dmaxqty;
                    if (TmplstSfclvlData[0].dmaxqty > (decimal)1.0)
                    {
                        ((FrmRepairTool)this.Owner).SendMsgShow("该批次不可重测", 1);
                        treeNode.Checked = false;
                        treeNode.BackColor = Color.White;
                        return;
                    }
                }
                else
                {
                    ((FrmRepairTool)this.Owner).SendMsgShow("批次信息有误", 1);
                    treeNode.Checked = false;
                    treeNode.BackColor = Color.White;
                    return;
                }

                query.Clear();
                GetControls(panel1);
                if (query.Count < 1)
                {
                    ListBox lstbox = new ListBox();
                    //lstbox.Anchor = AnchorStyles.None;
                    lstbox.Name = "listboxs-"+arrsfcitem[1].Trim();
                    lstbox.Tag = "listboxs";
                    lstbox.Items.Add(arrsfcitem[0].Trim());
                    panel1.Controls.Add(lstbox);
                    lstbox.Location = new Point(5, 15);
                    lstbox.Size = new Size(300, 150);
                    Label labeladd = new Label();
                    //labeladd.Anchor = AnchorStyles.None;
                    labeladd.Name = "Labels-"+arrsfcitem[1].Trim();
                    labeladd.Tag = "Labels";
                    labeladd.Text = arrsfcitem[1].Trim();
                    labeladd.Location = new Point(320, 15);
                    labeladd.Size = new Size(230, 30);
                    panel1.Controls.Add(labeladd);
                    ComboBox cmbx = new ComboBox();
                    //cmbx.Anchor = AnchorStyles.None;
                    cmbx.Name = "ComboBoxs-"+arrsfcitem[1].Trim();
                    cmbx.Tag = "ComboBoxs";
                    cmbx.Location = new Point(320, 50);
                    cmbx.Size = new Size(230, 30);
                    panel1.Controls.Add(cmbx);

                    ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                    string dt = client.RunServerAPI("BLL.Process", "GetProcessFlowDetail", TmplstSfclvlData[0].strWorkOrder.Trim());
                    if (string.IsNullOrEmpty(dt))
                    {
                        ((FrmRepairTool)this.Owner).SendMsgShow("获取工艺流失败", 2);
                        return;
                    }
                    List<B_ProcessList> lstprocess = new List<B_ProcessList>();
                    lstprocess = JsonConvert.DeserializeObject<List<B_ProcessList>>(dt);
                    for (int i = 0; i < lstprocess.Count; i++)
                    {
                        ComboBoxItem comboxitem = new ComboBoxItem(lstprocess[i].process_code + ";" + lstprocess[i].route_type, lstprocess[i].process_name);
                        cmbx.Items.Add(comboxitem);   //工序列表
                    }
                }
                else
                {
                    int ifindcon = 0;
                    //for (int i = 0; i < query.Count; i++)
                    //{
                    //}
                    foreach (Control c1 in query)
                    {
                        if (c1.Name == "listboxs-"+arrsfcitem[1].Trim())
                        {
                            ifindcon = 1;
                            int index = ((ListBox)c1).FindStringExact(arrsfcitem[0].Trim());
                            if (index < 0)
                            {
                                ((ListBox)c1).Items.Add(arrsfcitem[0].Trim());
                            }
                        }
                    }
                    if (ifindcon == 0)
                    {
                        ListBox lstbox = new ListBox();
                        //lstbox.Anchor = AnchorStyles.None;
                        lstbox.Name = "listboxs-"+arrsfcitem[1].Trim();
                        lstbox.Tag = "listboxs";
                        lstbox.Items.Add(arrsfcitem[0].Trim());
                        panel1.Controls.Add(lstbox);
                        lstbox.Location = new Point(5, 15 + 150 * query.Count);
                        lstbox.Size = new Size(300, 150);
                        Label labeladd = new Label();
                        //labeladd.Anchor = AnchorStyles.None;
                        labeladd.Name = "Labels-"+arrsfcitem[1].Trim();
                        labeladd.Tag = "Labels";
                        labeladd.Text = arrsfcitem[1].Trim();
                        labeladd.Location = new Point(320, 15 + 150 * query.Count);
                        labeladd.Size = new Size(230, 30);
                        panel1.Controls.Add(labeladd);
                        ComboBox cmbx = new ComboBox();
                        //cmbx.Anchor = AnchorStyles.None;
                        cmbx.Name = "ComboBoxs-" + arrsfcitem[1].Trim();
                        cmbx.Tag = "ComboBoxs";
                        cmbx.Location = new Point(320, 50 + 150 * query.Count);
                        cmbx.Size = new Size(230, 30);
                        panel1.Controls.Add(cmbx);

                        ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                        string dt = client.RunServerAPI("BLL.Process", "GetProcessFlowDetail", TmplstSfclvlData[0].strWorkOrder.Trim());
                        if (string.IsNullOrEmpty(dt))
                        {
                            ((FrmRepairTool)this.Owner).SendMsgShow("获取工艺流失败", 2);
                            return;
                        }
                        List<B_ProcessList> lstprocess = new List<B_ProcessList>();
                        lstprocess = JsonConvert.DeserializeObject<List<B_ProcessList>>(dt);
                        for (int i = 0; i < lstprocess.Count; i++)
                        {
                            ComboBoxItem comboxitem = new ComboBoxItem(lstprocess[i].process_code + ";" + lstprocess[i].route_type, lstprocess[i].process_name);
                            cmbx.Items.Add(comboxitem);   //工序列表
                        }

                        panel1.Height = groupBox2.Height - 50;
                        panel1.AutoScroll = true;
                    }
                }
                ((FrmRepairTool)this.Owner).SendMsgShow("增加[" + arrsfcitem[0].Trim() + "]重测成功", 0);
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        private void DelReTestSFC(string strsfcInfo, TreeNode treeNode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(strsfcInfo))
                {
                    ((FrmRepairTool)this.Owner).SendMsgShow("批次信息为空", 1);
                    return;
                }
                string[] arrsfcitem = strsfcInfo.Split('/');
                if (arrsfcitem.Length < 2
                    || string.IsNullOrWhiteSpace(arrsfcitem[0])
                    || string.IsNullOrWhiteSpace(arrsfcitem[1]))
                {
                    ((FrmRepairTool)this.Owner).SendMsgShow("批次信息不全", 1);
                    return;
                }
                //GetControls(panel1);
                foreach (Control c1 in query)
                {
                    if (c1.Name == "listboxs-"+arrsfcitem[1].Trim())
                    {
                        int index = ((ListBox)c1).FindStringExact(arrsfcitem[0].Trim());
                        if (index >= 0)
                        {
                            ((ListBox)c1).Items.RemoveAt(index);
                        }
                    }
                }
                ((FrmRepairTool)this.Owner).SendMsgShow("取消[" + arrsfcitem[0].Trim() + "]重测成功", 0);
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        Queue<Control> query = new Queue<Control>();
        //临时存储获取到的control控件(ListBox)
        /// <summary>
        /// 递归获取panel1上的所有控件，并临时存储到一个队列中
        /// </summary>
        /// <param name="item"></param>
        private void GetControls(Control item)
        {
            try
            {
                for (int i = 0; i < item.Controls.Count; i++)
                {
                    if (item.Controls[i].Tag.ToString().Trim() == "listboxs")
                    {
                        query.Enqueue(item.Controls[i]);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                //已经执行后，在响应该事件
                if (e.Node.Checked == true)
                {
                    //当前为选中状态
                    e.Node.BackColor = Color.Yellow;
                    AddReTestSFC(e.Node.Text.ToString().Trim(), e.Node);

                }
                else
                {
                    if (e.Action == TreeViewAction.ByMouse
                        || e.Action == TreeViewAction.ByKeyboard)
                    {
                        e.Node.BackColor = Color.White;
                        DelReTestSFC(e.Node.Text.ToString().Trim(), e.Node);
                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            } 
        }
        private void btn_commit_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < panel1.Controls.Count; i++)
                {
                    if (panel1.Controls[i].Tag.ToString().Trim() == "listboxs")
                    {
                        //处理存在需要重测批次
                        if (((ListBox)panel1.Controls[i]).Items.Count > 0)
                        {
                            string strlstboxname = panel1.Controls[i].Name.ToString().Trim();
                            string []arrtmpitem = strlstboxname.Split('-');
                            string strComboxname = strlstboxname.Replace("listboxs-", "ComboBoxs-");
                            ComboBox cmbx = (ComboBox)(panel1.Controls.Find(strComboxname, true)[0]);
                            if (cmbx != null)
                            {
                                ComboBoxItem comboxitem = (ComboBoxItem)cmbx.SelectedItem;
                                if (comboxitem != null)
                                {
                                    //修改表数据
                                    //--del P_Material_WIP --删记录
                                    //--mod P_SFC_State   --- state
                                    //--mod P_SFC_Process_IOLog  --首工序不用处理，非首工序，需要把对应工序增加一条记录  ; 暂时不需要修改
                                    string strsfcs = ""; //批次号集合
                                    for (int j1 = 0; j1 < ((ListBox)panel1.Controls[i]).Items.Count; j1++)
                                    {
                                        if (string.IsNullOrWhiteSpace(strsfcs))
                                        {
                                            strsfcs = ((ListBox)panel1.Controls[i]).Items[j1].ToString().Trim();
                                        }
                                        else
                                        {
                                            strsfcs = strsfcs + "," + ((ListBox)panel1.Controls[i]).Items[j1].ToString().Trim();
                                        }
                                    }
                                    //解析物料代码
                                    string []strTmpmatcode = arrtmpitem[1].ToString().Trim().Split('_');
                                    if (strTmpmatcode.Length <= 0)
                                    {
                                        ((FrmRepairTool)this.Owner).SendMsgShow("解析物料代码失败", 2);
                                        return;
                                    }
                                    string strmatcode = strTmpmatcode[0];

                                    //获取工单
                                    List<SFC_LVL_Data> TmplstSfclvlData = new List<SFC_LVL_Data>();
                                    TmplstSfclvlData = g_lstSfclvlData.Where(x => x.SFC == ((ListBox)panel1.Controls[i]).Items[0].ToString().Trim()).ToList();
                                    if (TmplstSfclvlData.Count <= 0
                                        || string.IsNullOrWhiteSpace(TmplstSfclvlData[0].strWorkOrder))
                                    {
                                        ((FrmRepairTool)this.Owner).SendMsgShow("解析工单失败", 2);
                                        return;
                                    }

                                    ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
                                    //参数解析 批次号集合多个已","隔开;重测工序代码;工序类别（首工序，中间工序...）;物料代码;工单;车间代码
                                    string dt = client.RunServerAPI("BLL.JobSubmit", "CommitReTest", strsfcs.Trim() + ";" 
                                                                                                    +comboxitem.Value.ToString().Trim()+";"
                                                                                                    +strmatcode + ";"
                                                                                                    + TmplstSfclvlData[0].strWorkOrder + ";"
                                                                                                    + g_strWorkShopCode.Trim());
                                    if (string.IsNullOrEmpty(dt)
                                        || dt.Contains("CommitReTest-Fail;"))
                                    {
                                        ((FrmRepairTool)this.Owner).SendMsgShow("提交重测数据失败:"+dt, 2);
                                        return;
                                    }

                                    //清理树上的复选框
                                    int ireworksfccount = ((ListBox)panel1.Controls[i]).Items.Count;
                                    for (int j = 0; j < ireworksfccount; j++)
                                    {
                                        string strtmpnodetext =((ListBox)panel1.Controls[i]).Items[j].ToString().Trim()+ " / "+arrtmpitem[1];
                                        foreach (TreeNode node in treeView1.Nodes)
                                        {
                                            if (node.Text.ToString().Trim() == strtmpnodetext)
                                            {
                                                //先检查根节点
                                                node.Checked = false;
                                                node.BackColor = Color.White;
                                            }
                                            else
                                            {
                                                //再检查根节点下的子节点
                                                FindAllNode(node, strtmpnodetext, 0);
                                            }
                                        }
                                    }
                                    //删除已提交批次
                                    ((ListBox)panel1.Controls[i]).Items.Clear();
                                    //cmbx.Items.Clear(); //不能清空，因为只有增加listbox时才加载新的数据
                                    cmbx.SelectedIndex = -1;
                                    ((FrmRepairTool)this.Owner).SendMsgShow("[" + arrtmpitem[1].ToString().Trim() + "]中批次已经提交重测", 0);
                                }
                                else
                                {
                                    ((FrmRepairTool)this.Owner).SendMsgShow("请选择[" + arrtmpitem[1].ToString().Trim() + "]需要重测工序", 1);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception exp)
            {
                ((FrmRepairTool)this.Owner).SendMsgShow(exp.Message, 2);
            }
        }
    }
    public class SFC_LVL_Data
    {
        //LVL
        public int LVL { get; set; }
        //批次号
        public string SFC { get; set; }
        //父SFC
        public string parentSFC { get; set; }
        //两级父SFC
        public string parparentSFC { get; set; }
        //工单
        public string strWorkOrder { get; set; }
        //物料编号
        public string strMatCode { get; set; }
        //物料数量
        public decimal dmaxqty { get; set; }
        ////父id
        //public int parentLVL { get; set; }
    }
    internal class ComboBoxItem : Object
    {
        private string value; //工序code+工序类型，首工序，中间工序...;eg: "pk005;中间站"
        private string text;
        public ComboBoxItem(string _value, string _text)
        {
            value = _value;
            text = _text;
        }
        public string Text
        {
            get { return text; }
        }
        public string Value
        {
            get { return value; }
        }
        public override string ToString()
        {
            return text;
        }
    }
}
