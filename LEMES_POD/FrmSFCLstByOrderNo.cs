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

namespace LEMES_POD
{
    public partial class FrmSFCLstByOrderNo : Form
    {
        public FrmSFCLstByOrderNo()
        {
            InitializeComponent();
        }
        public string OrderNO = "";
        public string ProcessCode = "";
        public string StationCode = "";
        public List<P_BarCodeBing> lstSFCData = new List<P_BarCodeBing>();
        private void FrmSFCLstByOrderNo_Load(object sender, EventArgs e)
        {
            try
            {
                List<Show_SFCData> lstshowsfcdata = new List<Show_SFCData>();
                foreach (P_BarCodeBing var in lstSFCData)
                {
                    Show_SFCData sfcdata = new Show_SFCData();
                    sfcdata.SFC = var.barcode.ToString();
                    sfcdata.OrderNo1 = var.order.ToString();
                    sfcdata.state = "未完成";
                    lstshowsfcdata.Add(sfcdata);
                }
                
                if (lstshowsfcdata.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(OrderNO)
                        && !string.IsNullOrWhiteSpace(ProcessCode)
                        && !string.IsNullOrWhiteSpace(StationCode))
                    {
                        //DCKJC04
                        string strpar = OrderNO + "," + ProcessCode + ",00";
                        string dt = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WorkOrder", "GetSFCCountByOrder", strpar); //通过工单，工序，工位获取sfc过站记录
                        if (string.IsNullOrWhiteSpace(dt))
                        {
                            // 未查到对应工单下已完成记录
                        }
                        else
                        {
                            List<P_SFC_Process_IOLog> proc = JsonConvert.DeserializeObject<List<P_SFC_Process_IOLog>>(dt);    //sfc过站记录
                            if (proc == null)
                            {
                                // 未查到对应工单下已完成记录
                            }
                            else
                            {
                                foreach (P_SFC_Process_IOLog Iolog in proc)
                                {
                                    if (!string.IsNullOrWhiteSpace(Iolog.SFC))
                                    {
                                        Show_SFCData c = lstshowsfcdata.First(x => x.SFC == Iolog.SFC);
                                        c.state = "已完成";
                                    }
                                }
                            }
                        }
                    }
                }
                dataGridView1.DataSource = lstshowsfcdata;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string strstate = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
                    //if (i%2==0)
                    if (strstate.Contains("未完成"))
                    {
                        this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                        this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                        this.dataGridView1.BackgroundColor = Color.White;
                        //对cell 属性进行设置
                        //this.kryptonDataGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                        //this.kryptonDataGridView1.Rows[i].Cells[1].Style.SelectionForeColor = Color.Red;
                        //this.kryptonDataGridView1.Rows[i].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                    }
                    else
                    {
                        this.dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                        this.dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                        this.dataGridView1.BackgroundColor = Color.White;
                    }
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message, "错误");
                return;
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X,
                                                            e.RowBounds.Location.Y,
                                                            dataGridView1.RowHeadersWidth - 4,
                                                            e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                                        dataGridView1.RowHeadersDefaultCellStyle.Font,
                                        rectangle,
                                        dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                                        TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
    public class Show_SFCData
    {
        //sfc
        public string SFC { get; set; }
        //工单
        public string OrderNo1 { get; set; }
        //状态
        public string state { get; set; }
    }
}
