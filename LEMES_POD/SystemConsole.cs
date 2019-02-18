using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Reflection;
using ILE;
using System.Net;
using Newtonsoft.Json;

namespace LEMES_POD
{
    public partial class SystemConsole : KryptonForm
    {
        public SystemConsole()
        {
            InitializeComponent();
            Inital();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.KeyPreview = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            //keycode_label.Text = "KeyCode:"+e.KeyCode;
            //keydata_label.Text ="KeyData:"+e.KeyData;
            //keyvalue_label.Text = "KeyValue:"+e.KeyValue;
        }

        

        void Inital()
        {
            
        }

        /// <summary>
        /// 确定IP地址后绑定对应工艺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kcb_ipadd_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 确定工艺后绑定对应工站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void kcb_process_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string ip = kcb_ipadd.SelectedItem.ToString();
            //List<B_StationList> stationList = BLL.Process.GetStationList(ip);

            //string processID = ((Tools.ComboboxItem)kcb_process.SelectedItem).Value.ToString();

            //var stations = stationList.Where(x => x.process_code == processID);
            //List<B_StationList> stas = stations.ToList<B_StationList>();
            //if (stas.Count > 0)
            //{
            //    this.ktb_station.Text = stas[0].station_name.ToString();
            //}
        }

       

        private void SystemConsole_KeyUp(object sender, KeyEventArgs e)
        {
            int temp = e.KeyValue;
            if (temp == 9)
            {
                bool ck = this.checkButton1.Checked;
                this.checkButton1.Checked = !ck;
                this.checkButton2.Checked = ck; ;
            }
        }


        List<ILE.PointVal> lst = new List<PointVal>(){
                new PointVal{ValTime=DateTime.Now,Val="aa"},
                new PointVal{ValTime=DateTime.Now,Val="bb"},
                new PointVal{ValTime=DateTime.Now,Val="cc"},
                new PointVal{ValTime=DateTime.Now,Val="cc"},
                new PointVal{ValTime=DateTime.Now,Val="dd"}
            };
        private void button1_Click(object sender, EventArgs e)
        {

            //Tools.ComboboxItem item = new Tools.ComboboxItem();
            //item.Text = "aa";
            //item.Value = 1;
            //this.lb_ngType.Items.Add(item);
            //var vals = lst.Select(p => p.Val).ToList();
            var vals = (from u in lst select u.Val).FirstOrDefault();

            var types=lst.Select(p =>p.Val).Distinct().ToList();
            foreach (var va in types)
            {
                this.lb_ngType.Items.Add(va);
            }
            string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.NGCode", "GetNGCode", "INR26650-50A-01-JX");
            List<dynamic> list= JsonConvert.DeserializeObject<List<dynamic>>(str);


        }

        private void lb_ngType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(((Tools.ComboboxItem)lb_ngType.SelectedItem).Value.ToString());
            string name = lb_ngType.SelectedItem.ToString();
            var codes = lst.Where(p => p.Val == name).ToList();
        }

    }
}
