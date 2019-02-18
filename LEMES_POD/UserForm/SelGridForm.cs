using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using LEDAO;
using Newtonsoft.Json;

namespace LEMES_POD
{
    public partial class SelGridForm : KryptonForm
    {
        private List<LEDAO.V_Material_WIP> _wipList;
        public LEDAO.V_Material_WIP WIP { get; set; }

        public SelGridForm(List<LEDAO.V_Material_WIP> wipList)
        {
            InitializeComponent();
            _wipList = wipList;
            WIP = null;
            if (wipList != null)
            {
                dataGridView1.DataSource = _wipList.Select(c => new { c.id, c.lot_no, c.lot_qty, c.mat_code, c.p_name, c.parent_station }).ToList();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in dataGridView1.SelectedRows)
            {
                //string mat_code = item.Cells["Column3"].Value.ToString();
                //string sfc = item.Cells["Column1"].Value.ToString();
                //DateTime MaterialInputTime = Convert.ToDateTime(Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "GetWipInputTimeByLot", sfc + "," + mat_code));
                //string strwip_valid = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetWip_validByProductCode", mat_code);
                //if (strwip_valid != "")
                //{
                //    decimal wip_valid = Convert.ToDecimal(strwip_valid);
                //    TimeSpan ts = DateTime.Now - MaterialInputTime;
                //    decimal h = ts.Hours;
                //    if (h > wip_valid)
                //    {
                //        DialogResult result = KryptonMessageBox.Show("该物料已超出时效时间,确定继续使用吗？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //        if (result == System.Windows.Forms.DialogResult.No)
                //        {
                //            return;
                //        }
                //    }
                //}
                //string ProductJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetproductInfoByCode", mat_code);
                //if (!string.IsNullOrEmpty(ProductJson))
                //{
                //    string JudeJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SFC", "GetJudeBySfc", sfc);
                //    List<P_SFC_Jude> listJude = JsonConvert.DeserializeObject<List<P_SFC_Jude>>(JudeJson);
                //    if (listJude != null)
                //    {
                //        UserForm.JudePrompt Jude = new UserForm.JudePrompt(sfc, listJude);
                //        Jude.ShowDialog();
                //        if (Jude.State == 0)
                //        {
                //            return;
                //        }
                //    }
                //}
                int id = int.Parse(item.Cells["Column5"].Value.ToString());
                WIP = _wipList.Where(c => c.id == id).First();
                break;
            }
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
