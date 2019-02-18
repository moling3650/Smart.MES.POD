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


namespace DP_Quality
{
    public partial class QualityTest : Form
    {
        private string _product_code
        {
            get;
            set;
        }

        public List<ILE.Model.NGCode> ng_codes;

        public bool QltResult
        {
            get;
            set;
        }

        private List<dynamic> ngCodes;  //全部可选不良清单
        private List<dynamic> selectedCodes;  //全部可选不良清单

        public QualityTest(string product_code)
        {
            InitializeComponent();
            this._product_code = product_code;
            this.ng_codes = new List<ILE.Model.NGCode>();
            this.selectedCodes = new List<dynamic>();
        }

        private void QualityTest_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            ServiceReference.ServiceClient client = new ServiceReference.ServiceClient();
            string str = client.RunServerAPI("BLL.NGCode", "GetNGCode", _product_code);
            ngCodes = JsonConvert.DeserializeObject<List<dynamic>>(str);

            var types = ngCodes.Select(p => p.type_name).Distinct().ToList(); //对TYPE_NAME去重
            try
            {
                foreach (string va in types)
                {
                    this.lb_Type.Items.Add(va);
                }
            }
            catch (Exception exc)
            { }
            //this.lb_type.Items.Add(new ComboboxItem(
        }

        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        private void lb_code_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.ng_code = ((ComboboxItem)lb_code.SelectedItem).Value.ToString();
        }

        private void QualityTest_KeyUp(object sender, KeyEventArgs e)
        {
            int temp = e.KeyValue;
            if (temp == 39)
            {
                this.lb_code.Focus();
            }
            else if (temp == 9)
            {
                bool ck = this.checkButton1.Checked;
                this.checkButton1.Checked = !ck;
                this.checkButton2.Checked = ck;

            }
            else if (temp == 13)
            {
                //if()
            }
        }

        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButton1.Checked)
            {
                this.lb_Type.Enabled = false;
                this.lb_code.Enabled = false;
                this.lb_selected.Enabled = false;
            }
            else
            {
                this.lb_Type.Enabled = true;
                this.lb_code.Enabled = true;
                this.lb_selected.Enabled = true;
            }
        }

        private void lb_code_DoubleClick(object sender, EventArgs e)
        {
            string val = lb_code.SelectedItem.ToString();
            string code = val.Substring(val.IndexOf("-") + 1);

            var va = this.ngCodes.Find(p => p.ng_code == code);
            this.ngCodes.Remove(va);
            this.selectedCodes.Add(va);
            this.lb_code.Items.Remove(lb_code.SelectedItem);
            this.lb_selected.Items.Add(va.ng_name + "-" + va.ng_code);
        }

        private void lb_selected_DoubleClick(object sender, EventArgs e)
        {
            string val = lb_selected.SelectedItem.ToString();
            string code = val.Substring(val.IndexOf("-") + 1);

            var va = this.selectedCodes.Find(p => p.ng_code == code);
            this.selectedCodes.Remove(va);
            this.ngCodes.Add(va);
            this.lb_selected.Items.Remove(va.ng_name + "-" + va.ng_code);
            lb_Type_SelectedIndexChanged(lb_selected, new EventArgs());
        }


        private void checkButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void simpleButton_Click(object sender, EventArgs e)
        {
            if (checkButton1.Checked)
            {
                this.QltResult = true;
                this.ng_codes = null;
            }
            else
            {
                this.QltResult = false;
                foreach (var code in selectedCodes)
                {
                    ILE.Model.NGCode cd=new ILE.Model.NGCode();
                    cd.Code=code.ng_code;
                    cd.Name=code.ng_name;
                    cd.qty=1;
                    ng_codes.Add(cd);
                }
            }
            this.Close();
        }


        private void lb_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = lb_Type.SelectedItem.ToString();
            var codes = ngCodes.Where(p => p.type_name == name).OrderBy(p => p.idx).ToList();

            //item.Text = "aa";
            //item.Value = 1;
            this.lb_code.Items.Clear();
            foreach (var va in codes)
            {
                //ComboboxItem item = new ComboboxItem();
                //item.Text=va.ng_name;
                //item.Value=va.ng_code;
                string str = va.ng_name + "-" + va.ng_code;
                this.lb_code.Items.Add(str);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            string val = lb_code.SelectedItem.ToString();
            string code = val.Substring(val.IndexOf("-") + 1);

            var va = this.ngCodes.Find(p => p.ng_code == code);
            this.ngCodes.Remove(va);
            this.selectedCodes.Add(va);
            this.lb_code.Items.Remove(lb_code.SelectedItem);
            this.lb_selected.Items.Add(va.ng_name + "-" + va.ng_code);
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            string val = lb_selected.SelectedItem.ToString();
            string code = val.Substring(val.IndexOf("-") + 1);

            var va = this.selectedCodes.Find(p => p.ng_code == code);
            this.selectedCodes.Remove(va);
            this.ngCodes.Add(va);
            this.lb_selected.Items.Remove(va.ng_name + "-" + va.ng_code);
            lb_Type_SelectedIndexChanged(lb_selected, new EventArgs());
        }



    }
}
