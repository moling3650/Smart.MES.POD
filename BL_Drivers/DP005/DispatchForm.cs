using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ILE;
using Newtonsoft.Json;
using LEDAO;
using ComponentFactory.Krypton.Toolkit;

namespace DP005
{
    public partial class DispatchForm :KryptonForm
    {
        ServiceReference.ServiceClient client;
        private string _station { get; set; }
        public string Dispetching { get; set; }
        public string OrderNO { get; set; }
        public string Product { get; set; }

        public DispatchForm(string station)
        {
            InitializeComponent();
            this.client= new ServiceReference.ServiceClient();
            this._station = station;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void DispatchForm_Load(object sender, EventArgs e)
        {
            BindAllDispatching();
        }

        void BindAllDispatching()
        {
            ILE.IResult res;
            string ResDispatching = client.RunServerAPI("BLL.WorkDispatching", "GetOrderByTime", _station);

            List<V_WorkDespatching> produc = JsonConvert.DeserializeObject<List<V_WorkDespatching>>(ResDispatching);

            this.dataGridView1.DataSource = produc;

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.Dispetching = this.dataGridView1.SelectedRows[0].Cells["dispatching_no"].Value.ToString();
            this.OrderNO = this.dataGridView1.SelectedRows[0].Cells["order_no"].Value.ToString();
            this.Product = this.dataGridView1.SelectedRows[0].Cells["product_code"].Value.ToString();
            this.Close();
        }

        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            this.Dispetching = this.dataGridView1.SelectedRows[0].Cells["dispatching_no"].Value.ToString();
            this.OrderNO = this.dataGridView1.SelectedRows[0].Cells["order_no"].Value.ToString();
            this.Product = this.dataGridView1.SelectedRows[0].Cells["product_code"].Value.ToString();
            this.Close();
        }

        private void button3System_Click(object sender, EventArgs e)
        {

        }
    }
}
