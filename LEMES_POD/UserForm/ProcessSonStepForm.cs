using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LEDAO;
using ILE;
using System.Timers;
using System.Threading;

namespace LEMES_POD.UserForm
{
    public partial class ProcessSonStepForm : Form
    {
        List<B_ProcessSonStep> _SonStepList;
        IJob job;
        bool isfedbatch;
        string sfc = string.Empty;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        int j = 0;
        Main main;
        public ProcessSonStepForm(List<B_ProcessSonStep> SonStepList, IJob _job, bool _isfedbatch, string _sfc, string _step_name, int _j, Main _main)
        {

            InitializeComponent();
            _SonStepList = SonStepList;
            job = _job;
            isfedbatch = _isfedbatch;
            sfc = _sfc;
            j = _j;
            main = _main;
            this.label1.Text = _step_name + "数据采集";
            aTimer.Elapsed += new ElapsedEventHandler(button2_Click);
            aTimer.Start();
            aTimer.Interval = 500;
            aTimer.Enabled = true;
        }
        #region 定时器
        public void Wait(int ms)
        {
            DateTime t = DateTime.Now;
            for (int i = 0; i < ms; i++)
            {
                TimeSpan ts = DateTime.Now - t;
                if (ts.Seconds >= ms)
                {
                    return;
                }
            }
            return;
        }
        #endregion
        TextBox[] textBox1;
        ComboBox[] comboBox1;
        DateTimePicker[] datetimepicker;
        /// <summary>
        /// //加载页面
        /// </summary>
        public void loadFrom1()
        {
            DataTable dt = new DataTable();
            //dt.Columns.Add("编号", typeof(Int32));
            dt.Columns.Add("项目编码", typeof(string));
            dt.Columns.Add("项目名称", typeof(string));
            dt.Columns.Add("工艺标准", typeof(string));
            dt.Columns.Add("误差", typeof(string));
            dt.Columns.Add("实际值", typeof(string));
            dt.Columns.Add("结论", typeof(string));
            DataRow dr;//行
            for (int i = 0; i < _SonStepList.Count(); i++)
            {
                dr = dt.NewRow();
                dr["项目编码"] = _SonStepList[i].stepid;
                dr["项目名称"] = _SonStepList[i].stepname;
                dr["工艺标准"] = _SonStepList[i].steppre;
                dr["误差"] = _SonStepList[i].steperror;
                dr["实际值"] = "";
                dr["结论"] = "";
                dt.Rows.Add(dr);//在表的对象的行里添加此行
            }
            BeginInvoke(new Action(() =>
                           {
                               dataGridView1.DataSource = dt;
                               dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                           }));

            //TextBox[]
            textBox1 = new TextBox[30];
            //ComboBox[] 
            comboBox1 = new ComboBox[30];
            //DateTimePicker[]
            datetimepicker = new DateTimePicker[30];
            BeginInvoke(new Action(() =>
                          {
                              DataGridViewCell dgvc = this.dataGridView1[4, 0];
                              //RnDevObject obj = devTreeView2._SelectedDevObject;
                              for (int i = 0; i < dataGridView1.Rows.Count; i++)
                              {
                                  textBox1[i] = new TextBox();
                                  comboBox1[i] = new ComboBox();
                                  datetimepicker[i] = new DateTimePicker();
                                  if (dgvc != null && dgvc.OwningColumn.Name == "实际值")
                                  {
                                      if (_SonStepList[i].steptype == 5)
                                      {
                                          dataGridView1.Controls.Add(datetimepicker[i]);
                                          Rectangle R = dataGridView1.GetCellDisplayRectangle(4, i, true);
                                          datetimepicker[i].CustomFormat = "yyyy-MM-dd HH:mm:ss";
                                          datetimepicker[i].Format = DateTimePickerFormat.Custom;
                                          //datetimepicker[i].ShowUpDown =true;
                                          datetimepicker[i].Size = R.Size;
                                          datetimepicker[i].Left = R.Left;
                                          datetimepicker[i].Top = R.Top;
                                          datetimepicker[i].Width = 240;
                                          datetimepicker[i].Visible = true;

                                          Point oPoint = new Point();
                                          oPoint.X = R.Location.X + (R.Width - R.Width) / 2 + 1;
                                          oPoint.Y = R.Location.Y + (R.Height - R.Height) / 2 + 1;
                                          datetimepicker[i].Location = oPoint;

                                      }
                                      if (_SonStepList[i].steptype == 4 && job != null && !isfedbatch)
                                      {
                                          decimal Lot_Qty = job.QTY;
                                          //耗料比例
                                          decimal consume_percent = Convert.ToDecimal(job.StepList[job.StepIdx - 1].consume_percent.ToString());
                                          string Mat_code = job.StepList[job.StepIdx - 1].Matcode.ToString();
                                          string Product_code = job.Product.ToString();
                                          decimal qty = 0;
                                          int baseqty = 0;
                                          if (Mat_code != "")
                                          {
                                              qty = decimal.Parse(Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetMaterialQty", Mat_code + "," + Product_code));
                                              baseqty = int.Parse(Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Product", "GetMaterialbaseQty", Mat_code + "," + Product_code));
                                          }
                                          else
                                          {
                                              MessageBox.Show("子工步类型与实际操作不符,请检测该子工步类型", "提示");
                                              return;
                                          }
                                          decimal MulQty = Lot_Qty * (qty / baseqty) * consume_percent;
                                          this.dataGridView1.Rows[i].Cells[4].Value = MulQty;
                                      }
                                      else
                                      {
                                          this.dataGridView1.Rows[i].Cells[4].Value = 0;
                                      }

                                      if (_SonStepList[i].steptype == 3 || _SonStepList[i].steptype == 1)
                                      {
                                          dataGridView1.Controls.Add(textBox1[i]);
                                          Rectangle R = dataGridView1.GetCellDisplayRectangle(4, i, true);
                                          textBox1[i].Name = "TextBox1_" + i.ToString() + "," + _SonStepList[i].steptype + "," + _SonStepList[i].steppre + "," + _SonStepList[i].steperror;
                                          textBox1[i].Size = R.Size;
                                          textBox1[i].Left = R.Left;
                                          textBox1[i].Top = R.Top;
                                          textBox1[i].Width = 240;
                                          textBox1[i].Visible = true;
                                          comboBox1[i].Visible = false;
                                          textBox1[i].TextChanged += new EventHandler(textBox1_TextChanged);
                                      }
                                      else if (_SonStepList[i].steptype == 2)
                                      {
                                          dataGridView1.Controls.Add(comboBox1[i]);
                                          //绑定combobox;
                                          comboBox1[i].Name = "comboBox1_" + i.ToString();
                                          comboBox1[i].Items.Add("OK");
                                          comboBox1[i].Items.Add("NG");
                                          Rectangle R = dataGridView1.GetCellDisplayRectangle(4, i, true);
                                          comboBox1[i].Size = R.Size;
                                          comboBox1[i].Left = R.Left;
                                          comboBox1[i].Top = R.Top;
                                          comboBox1[i].Width = 240;
                                          comboBox1[i].DropDownStyle = ComboBoxStyle.DropDownList;
                                          comboBox1[i].Visible = true;
                                          textBox1[i].Visible = false;

                                          comboBox1[i].SelectedIndexChanged += new EventHandler(comboBox1_SelectedIndexChanged);
                                      }

                                  }
                                  else
                                  {
                                      textBox1[i].Visible = false;
                                      comboBox1[i].Visible = false;
                                  }
                              }
                          }));
            #region
            //DataTable dt = new DataTable();//建立个数据表
            //dt.Columns.Add(new DataColumn("项目编码", typeof(string)));
            //dt.Columns.Add(new DataColumn("项目名称", typeof(string)));//在表中添加string类型的列
            //dt.Columns.Add(new DataColumn("工艺要求", typeof(string)));//在表中添加string类型的Name列

            //DataRow dr;//行
            //for (int i = 0; i < _SonStepList.Count(); i++)
            //{
            //    dr = dt.NewRow();
            //    dr["项目编码"] = _SonStepList[i].stepid;
            //    dr["项目名称"] = _SonStepList[i].stepname;
            //    dr["工艺要求"] = _SonStepList[i].steppre;
            //    dt.Rows.Add(dr);//在表的对象的行里添加此行

            //}
            //dataGridView1.DataSource = dt;
            //#region
            ////DataGridViewColumn dc = new DataGridViewColumn();
            ////dc.Name = "val";
            ////dc.DataPropertyName = "val"; 
            ////dc.HeaderText = "实际值";
            ////dc.CellType = ;
            ////dataGridView1.Columns.Add(dc);

            ////DataGridViewRow dgvr = new DataGridViewRow();
            ////dgvr.CreateCells(dataGridView1);

            ////DataGridViewComboBoxCell d = new DataGridViewComboBoxCell();//新建一个列，列的类型是DataGridViewComboBoxColumn
            ////d.ReadOnly = false;
            ////d.DropDownWidth = 500;
            //////d = "实际值";
            ////d.Items.Add("OK");//下拉列表选项
            ////d.Items.Add("NG");
            ////dgvr.Cells.Add(d);
            ////dataGridView1.Rows.Insert(0, dgvr);//添加的行作为第一行 
            //////dgvVouchers.Rows.Add(dr_new); //添加的行作为最后一行 
            //#endregion
            //DataGridViewTextBoxColumn dtb = new DataGridViewTextBoxColumn();
            //dtb.HeaderText = "实际值";
            //dtb.Width = 400;
            //dtb.Name = "实际值";
            ////DataGridViewComboBoxColumn d = new DataGridViewComboBoxColumn();//新建一个列，列的类型是DataGridViewComboBoxColumn
            ////d.ReadOnly = false;
            ////d.Width = 350;
            ////d.HeaderText = "实际值";
            ////d.Items.Add("OK");//下拉列表选项
            ////d.Items.Add("NG");

            ////for (int j = 0; j < _SonStepList.Count(); j++)
            ////{
            ////    if (_SonStepList[j].steptype == 3)
            ////    {
            ////        //(DataGridViewTextBoxColumn)this.dataGridView1.Rows[j].Cells[2] = dtb;
            ////        d =
            ////    }
            ////}

            //dataGridView1.Columns.Add(dtb);//把d这个列添加到datagridview里
            #endregion
        }

        /// <summary>
        ///  //触发下拉框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cob = (ComboBox)sender;
            string sname = cob.Name;
            string sitem = cob.SelectedItem.ToString();
            int row = int.Parse(sname.Replace("comboBox1_", ""));
            this.dataGridView1.Rows[row].Cells[5].Value = sitem;
        }

        /// <summary>
        /// 触发文本框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox teb = (TextBox)sender;
            string sname = teb.Name;
            string text = teb.Text;
            string[] name = sname.Split(',');
            string name1 = name[0].ToString();
            string name2 = name[1].ToString();
            string steppre = name[2].ToString();
            string steperror = name[3].ToString();
            int row = int.Parse(name1.Replace("TextBox1_", ""));
            int steptype = int.Parse(name2);
            decimal m = 0;
            if (steptype == 1 && steperror != "" && steppre != "" && text != "" && decimal.TryParse(text.ToString(), out m))
            {
                if (steperror.Trim() == "-0")
                {
                    if (decimal.Parse(text) < decimal.Parse(steppre))
                    {
                        this.dataGridView1.Rows[row].Cells[5].Value = "OK";
                        this.dataGridView1.Rows[row].Cells[5].Style.ForeColor = System.Drawing.Color.Green;

                    }
                    else
                    {
                        this.dataGridView1.Rows[row].Cells[5].Value = "NG";
                        this.dataGridView1.Rows[row].Cells[5].Style.ForeColor = System.Drawing.Color.Red;
                    }
                }
                if (steppre.Trim() == "+0")
                {
                    if (decimal.Parse(text) < decimal.Parse(steppre))
                    {
                        this.dataGridView1.Rows[row].Cells[5].Value = "NG";
                        this.dataGridView1.Rows[row].Cells[5].Style.ForeColor = System.Drawing.Color.Red;
                    }
                    else
                    {
                        this.dataGridView1.Rows[row].Cells[5].Value = "OK";
                        this.dataGridView1.Rows[row].Cells[5].Style.ForeColor = System.Drawing.Color.Green;
                    }
                }
                else
                {
                    if ((decimal.Parse(steppre) - decimal.Parse(steperror)) < decimal.Parse(text) && decimal.Parse(text) < (decimal.Parse(steppre) + decimal.Parse(steperror)))
                    {
                        this.dataGridView1.Rows[row].Cells[5].Value = "OK";
                        this.dataGridView1.Rows[row].Cells[5].Style.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        this.dataGridView1.Rows[row].Cells[5].Value = "NG";
                        this.dataGridView1.Rows[row].Cells[5].Style.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isfedbatch && job != null)
            {

                job.StepList[j].StepSonDetail = new List<ILE.StepData>();

                List<ILE.StepData> stepDetail = new List<ILE.StepData>();
                for (int m = 0; m < this.dataGridView1.Rows.Count; m++)
                {
                    string StepName = this.dataGridView1.Rows[m].Cells["项目名称"].Value.ToString();
                    string stepVal = string.Empty;
                    if (_SonStepList[m].steptype == 5)
                    {
                        stepVal = datetimepicker[m].Text.ToString();
                    }
                    if (_SonStepList[m].steptype == 1 || _SonStepList[m].steptype == 3)
                    {
                        //stepVal = this.dataGridView1.Rows[m].Cells["实际值"].Value.ToString();
                        //string str = ((TextBox)(this.dataGridView1.Rows[m].Cells["实际值"].).Text.Trim();
                        stepVal = textBox1[m].Text;
                    }
                    if (_SonStepList[m].steptype == 2)
                    {
                        if (comboBox1[m].SelectedItem != null)
                        {
                            stepVal = comboBox1[m].SelectedItem.ToString();
                        }
                    }
                    if (_SonStepList[m].steptype == 4)
                    {
                        stepVal = this.dataGridView1.Rows[m].Cells["实际值"].Value.ToString();
                    }
                    if (stepVal == "")
                    {
                        MessageBox.Show("[" + StepName + "]的实际值不能为空", "提示");
                        return;
                    }
                    string stepPre = this.dataGridView1.Rows[m].Cells["工艺标准"].Value.ToString();
                    string stepSonCode = this.dataGridView1.Rows[m].Cells["项目编码"].Value.ToString();
                    string stepconclude = this.dataGridView1.Rows[m].Cells["结论"].Value.ToString();
                    ILE.StepData stepdata = new StepData();
                    stepdata.stepname = StepName;

                    stepdata.StepVal = stepVal;
                    stepdata.stepSonPre = stepPre;
                    stepdata.stepsoncode = stepSonCode;
                    stepdata.stepconclude = stepconclude;
                    stepDetail.Add(stepdata);
                }

                job.StepList[j].StepSonDetail.AddRange(stepDetail);

            }
            else
            {
                string orderno = job.OrderNO;
                string SFC = sfc;
                for (int index = 0; index < _SonStepList.Count; index++)
                {
                    string P_code = _SonStepList[index].parentstepid.ToString();
                    string S_code = _SonStepList[index].stepid.ToString();
                    string StepName = this.dataGridView1.Rows[index].Cells["项目名称"].Value.ToString();
                    string stepVal = string.Empty;
                    if (_SonStepList[index].steptype == 5)
                    {
                        stepVal = datetimepicker[index].Text.ToString();
                    }
                    if (_SonStepList[index].steptype == 1 || _SonStepList[index].steptype == 3)
                    {
                        stepVal = textBox1[index].Text;
                    }
                    if (_SonStepList[index].steptype == 2)
                    {
                        if (comboBox1[index].SelectedItem != null)
                        {
                            stepVal = comboBox1[index].SelectedItem.ToString();
                        }
                    }
                    if (_SonStepList[index].steptype == 4)
                    {
                        stepVal = this.dataGridView1.Rows[index].Cells["实际值"].Value.ToString();
                    }
                    if (stepVal == "")
                    {
                        MessageBox.Show("[" + StepName + "]的实际值不能为空", "提示");
                        return;
                    }
                    string stepconclude = this.dataGridView1.Rows[index].Cells["结论"].Value.ToString();
                    string json = orderno + "," + SFC + "," + P_code + "," + S_code + "," + stepVal + "," + stepconclude;
                    Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Step", "SubSonStepData", json);
                }
                MessageBox.Show("提交成功", "提示");
            }
            isCloseClilt = true;
            this.Close();
            isCloseClilt = false;
        }
        public static bool isCloseClilt = false;
        /// <summary>
        /// //加载项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            loadFrom1();
            aTimer.Stop();
        }

        /// <summary>
        ///  //滚动条事件，gridview中的控件随滚动条滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            textBox1 = new TextBox[30];
            comboBox1 = new ComboBox[30];
            datetimepicker = new DateTimePicker[30];
            for (int m = 0; m < _SonStepList.Count; m++)
            {
                textBox1[m] = new TextBox();
                comboBox1[m] = new ComboBox();
                datetimepicker[m] = new DateTimePicker();
                #region  横向滚动条
                //if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                //{
                //    int i = e.NewValue;
                //    int j = e.OldValue;
                //    if (_SonStepList[m].steptype == 3 || _SonStepList[m].steptype == 1)
                //    {
                //        textBox1[m].Left = textBox1[m].Left - i + j;
                //    }
                //}
                #endregion
                #region 纵向滚动条
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    int i = e.NewValue;
                    int j = e.OldValue;
                    if (_SonStepList[m].steptype == 3 || _SonStepList[m].steptype == 1)
                    {
                        textBox1[m].Top = textBox1[m].Top - i + j;
                    }
                    if (_SonStepList[m].steptype == 5)
                    {
                        datetimepicker[m].Top = datetimepicker[m].Top - i + j;
                    }
                    else if (_SonStepList[i].steptype == 2)
                    {
                        comboBox1[m].Top = comboBox1[m].Top - i + j;
                    }
                }
                #endregion
            }
        }
        /// <summary>
        /// GridView的回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
        /// <summary>
        /// 关闭窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessSonStepForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isCloseClilt)
            {
                //main.CloseSonStep();
                main.buttonSpecHeaderGroup6_Click_1(sender, e);
            }
        }

        /// <summary>
        /// 给窗体回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessSonStepForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
