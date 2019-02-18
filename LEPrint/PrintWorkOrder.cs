using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using LEDAO;
using Newtonsoft.Json;

namespace NV_SNP
{
    public partial class PrintWorkOrder : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        public PrintWorkOrder()
        {
            InitializeComponent();
        }

        private void PrintWorkOrder_Load(object sender, EventArgs e)
        {
            Load_Data();
        }
        //加载数据
        private void Load_Data()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetMainOrder", "");
            List<P_MainWorkOrder> proc = JsonConvert.DeserializeObject<List<P_MainWorkOrder>>(dt);
            kryptonDataGridView1.DataSource = proc;
        }
        //打印
        private void ktb_print_Click(object sender, EventArgs e)
        {
            string Main_order = this.kryptonDataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
            (new PrintAll(Main_order, "", this)).ShowDialog();
        }
        //删除，即更改打印状态
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            DialogResult result = KryptonMessageBox.Show("确定清除该工单吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            string Main_order = this.kryptonDataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
            Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "DeleteNoPrintOrder", Main_order);
            Load_Data();
        }
    }
}