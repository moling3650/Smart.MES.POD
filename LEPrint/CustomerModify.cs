using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Data.OleDb;
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
    public partial class CustomerModify : KryptonForm
    {
        DataTable dt;
        OleDbDataAdapter oda;

        public CustomerModify()
        {
            InitializeComponent();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.oda.Update(dt);
                dt.AcceptChanges();
                MessageBox.Show("保存成功");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        void BindCustomer()
        {
            string dt = Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProductInfo", "00000");
            List<B_Product> proc = JsonConvert.DeserializeObject<List<B_Product>>(dt);
            this.kryptonDataGridView1.DataSource = proc;
        }

        private void CustomerModify_Load(object sender, EventArgs e)
        {
            BindCustomer();
        }

        private void kryptonButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void kryptonButton3_Click(object sender, EventArgs e)
        {
            if (kryptonDataGridView1.SelectedRows[0].Index != null)
            {
                string P_code = this.kryptonDataGridView1.SelectedRows[0].Cells["Column1"].Value.ToString();
                Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "DeleteProduct", P_code);
                BindCustomer();
                MessageBox.Show("删除成功");
            }
        }
    }
}
