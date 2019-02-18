using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using NV_SNP.Toos;
using System.Linq;
using ILE;
using System.Net;
using LEDAO;
using Newtonsoft.Json;
using System.ServiceModel;
using System.IO;

namespace NV_SNP
{
    public partial class ModuleModify : KryptonForm
    {
        public ModuleModify()
        {
            InitializeComponent();
            this.kryptonDataGridView1.AutoGenerateColumns = false;
        }

        private void ModuleModify_Load(object sender, EventArgs e)
        {
            //BindCustomer();
            //方案一
            //BingProduct();
            //方案二
            BingProductType();

        }
        void BindCustomer()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductCode", "cid");
            List<B_Product> proc = JsonConvert.DeserializeObject<List<B_Product>>(dt);
            this.cb_custom.ValueMember = "product_code";
            this.cb_custom.DisplayMember = "Product_Name";
            this.cb_custom.DataSource = proc;
        }

        //方案二，获取全部产品类型，然后根据产品类型生成树状结构，只有两级
        void BingProductType()
        {
            treeView1.Nodes.Clear();
            string ProductTypeData = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductType", "");
            List<B_Product_Type> productList = JsonConvert.DeserializeObject<List<B_Product_Type>>(ProductTypeData);
            if (productList != null)
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    TreeNode node = new TreeNode(productList[i].type_code.ToString());
                    node.Tag = productList[i].type_name;
                    node.Text = productList[i].type_name;
                    treeView1.Nodes.Add(node);
                    FillNode(productList[i].type_code.ToString(), node);
                }
            }
        }
        public void FillNode(string typecode, TreeNode node)
        {
            string productData = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductByType", typecode);
            List<B_Product> productList = JsonConvert.DeserializeObject<List<B_Product>>(productData);
            if (productList != null)
            {
                for (int j = 0; j < productList.Count; j++)
                {
                    TreeNode Treenode = new TreeNode(productList[j].product_code.ToString());
                    Treenode.Tag = productList[j].product_code;
                    Treenode.Text = productList[j].product_name;
                    node.Nodes.Add(Treenode);
                }
            }
        }
        //查询详细产品
        public void GetNode()
        {
            treeView1.Nodes.Clear();
            string dt1 = string.Empty;
            if (kryptonRadioButton1.Checked == true)
            {
                string Product_code = this.kryptonTextBox1.Text.ToString();
                dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductByCode", Product_code);
            }
            if (kryptonRadioButton2.Checked == true)
            {
                string Product_name = this.kryptonTextBox1.Text.ToString();
                dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductByName1", Product_name);
            }
            List<B_Product> productList = JsonConvert.DeserializeObject<List<B_Product>>(dt1);
            if (productList != null)
            {
                for (int i = 0; i < productList.Count; i++)
                {
                    TreeNode node = new TreeNode(productList[i].product_code.ToString());
                    node.Tag = productList[i].product_code;
                    node.Text = productList[i].product_name;
                    treeView1.Nodes.Add(node);
                }
            }
        }

        //方案一，获取全部产品，然后根据bom生成树状结构
        //树栏目获取全部
        void BingProduct()
        {
            string ProductData = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductAll", "CP01");
            List<B_Product> ProductDataList = JsonConvert.DeserializeObject<List<B_Product>>(ProductData);
            for (int i = ProductDataList.Count - 1; i >= 0; i--)
            {
                string product_code = ProductDataList[i].product_code;
                string dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetBomByCode", product_code);
                string dt2 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProduct", product_code);
                GetRootN(dt1, dt2);
            }
        }

        //获取父工单
        private void GetRoot()
        {
            treeView1.Nodes.Clear();
            string dt1 = string.Empty;
            string dt2 = string.Empty;
            if (kryptonRadioButton1.Checked == true)
            {
                string Product_code = this.kryptonTextBox1.Text.ToString();
                dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetBomByCode", Product_code);
                dt2 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProduct", Product_code);
            }
            if (kryptonRadioButton2.Checked == true)
            {
                string Product_name = this.kryptonTextBox1.Text.ToString();
                dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetBomByNmae", Product_name);
                dt2 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductByName", Product_name);
            }
            GetRootN(dt1, dt2);
        }
        //获取父工单的子节点
        private void GetRootN(string dt1, string dt2)
        {
            List<B_Bom> proc1 = JsonConvert.DeserializeObject<List<B_Bom>>(dt1);
            List<B_Product> proc2 = JsonConvert.DeserializeObject<List<B_Product>>(dt2);
            if (proc1 == null)
                return;
            if (proc2 == null)
                return;
            for (int i = 0; i < proc1.Count; i++)
            {
                TreeNode node = new TreeNode(proc2[0].product_code.ToString());
                string bom_code = proc1[i].bom_code;
                node.Tag = proc2[0].product_code;
                node.Text = proc2[0].product_name;
                treeView1.Nodes.Add(node);
                FillNodes(node, bom_code);
            }
            return;
        }
        //子节点
        private void FillNodes(TreeNode node, string bom_code)
        {
            try
            {
                string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetBom_Detail", bom_code);
                List<B_Bom_Detail> proc = JsonConvert.DeserializeObject<List<B_Bom_Detail>>(dt);
                if (proc == null)
                    return;
                for (int j = 0; j < proc.Count; j++)
                {
                    if (bom_code == proc[j].bom_code)
                    {
                        string p_code = proc[j].mat_code;
                        string dt1 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProduct", p_code);
                        List<B_Product> proc1 = JsonConvert.DeserializeObject<List<B_Product>>(dt1);
                        TreeNode Treenode = new TreeNode(proc[j].mat_code.ToString());
                        if (proc[j].mat_type == 0 && proc1 != null)
                        {
                            Treenode.Tag = proc1[0].product_code;
                            Treenode.Text = proc1[0].product_name;
                            node.Nodes.Add(Treenode);
                        }
                        string dt2 = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetBom", p_code);
                        List<B_Bom> proc2 = JsonConvert.DeserializeObject<List<B_Bom>>(dt2);
                        if (proc2 != null)
                        {
                            string bom_code1 = proc2[0].bom_code;
                            FillNodes(Treenode, bom_code1);
                        }
                    }
                }
            }
            catch
            {

            }
        }
        private void cb_custom_SelectedIndexChanged(object sender, EventArgs e)
        {
            //GetRoot();
            //this.treeView1.ExpandAll();
        }

        void BindModule()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplate", nodeText);
            List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
        }
        //判断该产品是否有模板
        private bool IsTemplate()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplate", nodeText);
            List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
            if (proc == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (IsTemplate())
            {
                ModuleAdd md = new ModuleAdd(nodeText, node);
                md.ShowDialog();
                if (md.DialogResult == DialogResult.OK)
                {
                    BindModule();
                }
            }
            else
            {
                MessageBox.Show("该产品已有模板，不可重复添加！");
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetTemplate", nodeText);
            List<P_SSW_TemplateList> proc = JsonConvert.DeserializeObject<List<P_SSW_TemplateList>>(dt);
            if (IsTemplate())
            {
                MessageBox.Show("暂无模板，不可修改，请添加模板！");
            }
            else
            {
                string mid = proc[0].Template_id.ToString();
                ModuleAdd md = new ModuleAdd(mid);
                md.ShowDialog();
                if (md.DialogResult == DialogResult.OK)
                {
                    BindModule();
                }
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsTemplate())
                {
                    MessageBox.Show("暂无模板，无法删除，请添加模板！");
                }
                else
                {
                    Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "DeleteTemplate", nodeText);
                    MessageBox.Show("删除成功");
                    BindModule();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("删除失败:" + exc.Message);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        #region
        private void kryptonDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            //if (kryptonDataGridView1.SelectedRows.Count < 1)
            //    return;
            //string mid=kryptonDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //this.kbt_CanBeZero.Enabled = Objs.Module.CanBeSetZero(mid); //判断此规则是否可以归零
            //this.klb1.Visible = kbt_CanBeZero.Enabled;
        }

        private void kbt_CanBeZero_Click(object sender, EventArgs e)
        {
            //PassWord pw = new PassWord();
            //pw.ShowDialog();
            //if (pw.DialogResult != DialogResult.OK)
            //{
            //    MessageBox.Show("没有通过权限验证");
            //    return;
            //}

            //string mid = kryptonDataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            //Objs.Module.SetModuleZero(mid);
            //string cid = this.cb_custom.SelectedValue.ToString();
            ////MessageBox.Show(mid);
            //BindModule(cid); 
        }
        #endregion

        string node = null;
        string nodeText = null;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            nodeText = e.Node.Tag.ToString();
            node = e.Node.Text.ToString();
            if (IsTemplate())
            {
                BindModule();
                MessageBox.Show("暂无模板，请添加...");
            }
            else
            {
                BindModule();
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
        //根据textBox值的变化获取左侧树
        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (kryptonTextBox1.Text == "")
            {
                //方案一
                //BingProduct();
                //方案二
                BingProductType();
            }
            else
            {
                //方案一
                //GetRoot();
                //方案二
                GetNode();
                this.treeView1.ExpandAll();
            }
        }

    }
}
