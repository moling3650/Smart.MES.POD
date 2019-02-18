using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace LEMES_POD.Component
{
    class Tool
    {
        public static void DisplayResult(TextBox tb, bool res, string msg)
        {
            if (res)
            {
                tb.BackColor = ColorTranslator.FromHtml("#FFFFCC");
                tb.ForeColor = Color.Black;
                tb.Text = msg;
            }
            else
            {
                tb.BackColor = ColorTranslator.FromHtml("#FF9999"); 
                tb.ForeColor = Color.Black;
                tb.Text = msg;
            }
        }

        public static void CtrlMsgPanel(Control panel, int state, string msg)
        {
            panel.Text = msg;
            LEDAO.LogClass.WriteLogFile(msg);
            if (state == 1)
            {
                panel.BackColor = ColorTranslator.FromHtml("#FFFFCC");
                panel.ForeColor = Color.Black;
            }
            else if (state == 2)
            {
                panel.BackColor = Color.Blue;
            }
            else
            {
                panel.BackColor = ColorTranslator.FromHtml("#FF9999"); 
            }
        }

        //创建数据模拟量监测图表
        public static Control GetChart(string name, float ConstantU, float ConstantL)
        {
            //DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.ConstantLine constantLine1 = new DevExpress.XtraCharts.ConstantLine();
            DevExpress.XtraCharts.ConstantLine constantLine2 = new DevExpress.XtraCharts.ConstantLine();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartControl chartControl1 = new DevExpress.XtraCharts.ChartControl();
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = (DevExpress.XtraCharts.XYDiagram)chartControl1.Diagram;
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            DevExpress.XtraCharts.SplineSeriesView splineSeriesView1 = new DevExpress.XtraCharts.SplineSeriesView();

            chartControl1.BorderOptions.Visibility = DevExpress.Utils.DefaultBoolean.True;
            chartControl1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            xyDiagram1.AxisX.DateTimeScaleOptions.ScaleMode = DevExpress.XtraCharts.ScaleMode.Continuous;
            xyDiagram1.AxisX.GridLines.Visible = true;
            //xyDiagram1.AxisX.Title.Text = "Time of Day";
            xyDiagram1.AxisY.GridLines.MinorVisible = true;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            constantLine1.AxisValueSerializable = ConstantU.ToString();
            constantLine1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            constantLine1.LegendText = "UCL";
            constantLine1.Name = "U";
            constantLine2.AxisValueSerializable = ConstantL.ToString();
            constantLine2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            constantLine2.LegendText = "LCL";
            constantLine2.Name = "L";
            xyDiagram1.AxisY.ConstantLines.AddRange(new DevExpress.XtraCharts.ConstantLine[] {
                constantLine1,
                constantLine2});
            xyDiagram1.AxisY.Interlaced = true;
            xyDiagram1.AxisY.Label.TextPattern = "{V:G}";
            //xyDiagram1.AxisY.Title.Text = "Power, kW";
            xyDiagram1.AxisY.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.AlwaysShowZeroLevel = false;
            
            chartControl1.Diagram = xyDiagram1;
            
            chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            chartControl1.Legend.Name = "Default Legend";
            chartControl1.Location = new System.Drawing.Point(0, 0);
            chartControl1.Name = "chartControl1";
            chartControl1.PaletteName = "Equity";
            chartControl1.SeriesDataMember = "Branch";
            series1.LabelsVisibility = DevExpress.Utils.DefaultBoolean.False;
            series1.Name = name;
            lineSeriesView1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(73)))), ((int)(((byte)(125)))));
            lineSeriesView1.LineMarkerOptions.Size = 5;
            lineSeriesView1.MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            series1.View = lineSeriesView1;
            chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
            series1};
            chartControl1.SeriesTemplate.ArgumentDataMember = "Time";
            chartControl1.SeriesTemplate.ArgumentScaleType = DevExpress.XtraCharts.ScaleType.DateTime;
            chartControl1.SeriesTemplate.CrosshairLabelPattern = "{S}: {V}kWt";
            chartControl1.SeriesTemplate.ShowInLegend = false;
            chartControl1.SeriesTemplate.ValueDataMembersSerializable = "Power";
            chartControl1.SeriesTemplate.View = splineSeriesView1;
            chartControl1.SeriesTemplate.Visible = false;
            chartControl1.Size = new System.Drawing.Size(735, 174);
            chartControl1.TabIndex = 1;
            chartControl1.TabStop = false;

            return chartControl1;

        }

        public static Control GetWorkToolPanel(string toolName,string toolCode)
        {
            // pictureBox4
            // 
            PictureBox pb = new PictureBox();
            pb.Image = global::LEMES_POD.Properties.Resources.bullet_wrench;
            pb.Location = new System.Drawing.Point(6, 6);
            pb.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
            pb.Name = "pb_" + toolCode;
            pb.Size = new System.Drawing.Size(16, 16);
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pb.TabIndex = 0;
            pb.TabStop = false;

            Label lb = new Label();
            lb.AutoSize = true;
            lb.Location = new System.Drawing.Point(31, 8);
            lb.Margin = new System.Windows.Forms.Padding(6, 8, 3, 0);
            lb.Name = "lb_" + toolCode;
            lb.Size = new System.Drawing.Size(71, 12);
            lb.TabIndex = 3;
            lb.Text =toolName;

            FlowLayoutPanel flp_workTool = new FlowLayoutPanel();
            flp_workTool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            flp_workTool.Controls.Add(pb);
            flp_workTool.Controls.Add(lb);
            flp_workTool.Location = new System.Drawing.Point(847, 323);
            flp_workTool.Margin = new System.Windows.Forms.Padding(16, 6, 3, 3);
            flp_workTool.Name = "flp_" + toolCode;
            flp_workTool.Size = new System.Drawing.Size(230, 30);
            flp_workTool.TabIndex = 15;

            return flp_workTool;
        }

        public static Control GetMouldPanel(string mouldName,string mouldCode)
        {
            // pictureBox4
            // 
            PictureBox pb = new PictureBox();
            pb.Image = global::LEMES_POD.Properties.Resources.brick;
            pb.Location = new System.Drawing.Point(6, 6);
            pb.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
            pb.Name = "pb_" + mouldCode;
            pb.Size = new System.Drawing.Size(16, 16);
            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pb.TabIndex = 0;
            pb.TabStop = false;

            Label lb = new Label();
            lb.AutoSize = true;
            lb.Location = new System.Drawing.Point(31, 8);
            lb.Margin = new System.Windows.Forms.Padding(6, 8, 3, 0);
            lb.Name = "lb_" + mouldCode;
            lb.Size = new System.Drawing.Size(71, 12);
            lb.TabIndex = 3;
            lb.Text = mouldName;

            FlowLayoutPanel flp_workTool = new FlowLayoutPanel();
            flp_workTool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            flp_workTool.Controls.Add(pb);
            flp_workTool.Controls.Add(lb);
            flp_workTool.Location = new System.Drawing.Point(840, 323);
            flp_workTool.Margin = new System.Windows.Forms.Padding(16, 6, 3, 3);
            flp_workTool.Name = "flp_" + mouldCode;
            flp_workTool.Size = new System.Drawing.Size(230, 30);
            flp_workTool.TabIndex = 15;

            return flp_workTool;
        }
    }
}
