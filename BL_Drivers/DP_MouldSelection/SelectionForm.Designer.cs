namespace DP_MouldSelection
{
    partial class SelectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lvBind = new System.Windows.Forms.ListView();
            this.ItemName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemQuality = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ItemVerdict = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lb_orderModel = new System.Windows.Forms.Label();
            this.kryptonBorderEdge4 = new ComponentFactory.Krypton.Toolkit.KryptonBorderEdge();
            this.kryptonLabel9 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.llb_machine = new System.Windows.Forms.LinkLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.ktb_shutMould = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(465, 343);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonButton1.Size = new System.Drawing.Size(92, 38);
            this.kryptonButton1.TabIndex = 32;
            this.kryptonButton1.Values.Image = global::DP_MouldSelection.Properties.Resources.confirm;
            this.kryptonButton1.Values.Text = "确定";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(531, 30);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(29, 12);
            this.linkLabel1.TabIndex = 39;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "更换";
            // 
            // lvBind
            // 
            this.lvBind.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ItemName,
            this.ItemValue,
            this.ItemQuality,
            this.ItemVerdict});
            this.lvBind.FullRowSelect = true;
            this.lvBind.Location = new System.Drawing.Point(29, 241);
            this.lvBind.Name = "lvBind";
            this.lvBind.Size = new System.Drawing.Size(528, 90);
            this.lvBind.TabIndex = 38;
            this.lvBind.UseCompatibleStateImageBehavior = false;
            this.lvBind.View = System.Windows.Forms.View.Details;
            // 
            // ItemName
            // 
            this.ItemName.Text = "匹配属性";
            this.ItemName.Width = 150;
            // 
            // ItemValue
            // 
            this.ItemValue.Text = "匹配条件";
            this.ItemValue.Width = 150;
            // 
            // ItemQuality
            // 
            this.ItemQuality.Text = "模具属性";
            this.ItemQuality.Width = 150;
            // 
            // ItemVerdict
            // 
            this.ItemVerdict.Text = "结论";
            this.ItemVerdict.Width = 50;
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(151, 22);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(178, 23);
            this.textBox1.TabIndex = 37;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(30, 21);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(115, 26);
            this.kryptonLabel1.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateDisabled.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel1.StateDisabled.LongText.Color2 = System.Drawing.Color.White;
            this.kryptonLabel1.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateDisabled.ShortText.Color1 = System.Drawing.Color.Blue;
            this.kryptonLabel1.StateDisabled.ShortText.Color2 = System.Drawing.Color.Blue;
            this.kryptonLabel1.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateNormal.LongText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.TabIndex = 36;
            this.kryptonLabel1.Values.ImageTransparentColor = System.Drawing.Color.Blue;
            this.kryptonLabel1.Values.Text = "录入模具编号:";
            // 
            // lb_orderModel
            // 
            this.lb_orderModel.AutoSize = true;
            this.lb_orderModel.BackColor = System.Drawing.Color.Transparent;
            this.lb_orderModel.ForeColor = System.Drawing.Color.Navy;
            this.lb_orderModel.Location = new System.Drawing.Point(430, 25);
            this.lb_orderModel.Name = "lb_orderModel";
            this.lb_orderModel.Size = new System.Drawing.Size(11, 12);
            this.lb_orderModel.TabIndex = 35;
            this.lb_orderModel.Text = "*";
            // 
            // kryptonBorderEdge4
            // 
            this.kryptonBorderEdge4.Location = new System.Drawing.Point(425, 41);
            this.kryptonBorderEdge4.Name = "kryptonBorderEdge4";
            this.kryptonBorderEdge4.Size = new System.Drawing.Size(100, 1);
            this.kryptonBorderEdge4.Text = "kryptonBorderEdge4";
            // 
            // kryptonLabel9
            // 
            this.kryptonLabel9.Location = new System.Drawing.Point(340, 21);
            this.kryptonLabel9.Name = "kryptonLabel9";
            this.kryptonLabel9.Size = new System.Drawing.Size(82, 26);
            this.kryptonLabel9.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel9.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel9.StateDisabled.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel9.StateDisabled.LongText.Color2 = System.Drawing.Color.White;
            this.kryptonLabel9.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel9.StateDisabled.ShortText.Color1 = System.Drawing.Color.Blue;
            this.kryptonLabel9.StateDisabled.ShortText.Color2 = System.Drawing.Color.Blue;
            this.kryptonLabel9.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel9.StateNormal.LongText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel9.TabIndex = 34;
            this.kryptonLabel9.Values.ImageTransparentColor = System.Drawing.Color.Blue;
            this.kryptonLabel9.Values.Text = "派工模具:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(2, 0);
            this.kryptonLabel2.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(69, 23);
            this.kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateDisabled.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel2.StateDisabled.LongText.Color2 = System.Drawing.Color.White;
            this.kryptonLabel2.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateDisabled.ShortText.Color1 = System.Drawing.Color.Blue;
            this.kryptonLabel2.StateDisabled.ShortText.Color2 = System.Drawing.Color.Blue;
            this.kryptonLabel2.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateNormal.LongText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.TabIndex = 41;
            this.kryptonLabel2.Values.ImageTransparentColor = System.Drawing.Color.Blue;
            this.kryptonLabel2.Values.Text = "当前设备";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(160, 0);
            this.kryptonLabel3.Margin = new System.Windows.Forms.Padding(0);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(112, 23);
            this.kryptonLabel3.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel3.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateDisabled.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel3.StateDisabled.LongText.Color2 = System.Drawing.Color.White;
            this.kryptonLabel3.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateDisabled.ShortText.Color1 = System.Drawing.Color.Blue;
            this.kryptonLabel3.StateDisabled.ShortText.Color2 = System.Drawing.Color.Blue;
            this.kryptonLabel3.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateNormal.LongText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.TabIndex = 42;
            this.kryptonLabel3.Values.ImageTransparentColor = System.Drawing.Color.Blue;
            this.kryptonLabel3.Values.Text = "匹配要求清单：";
            // 
            // llb_machine
            // 
            this.llb_machine.AutoSize = true;
            this.llb_machine.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.llb_machine.Location = new System.Drawing.Point(71, 6);
            this.llb_machine.Margin = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.llb_machine.Name = "llb_machine";
            this.llb_machine.Size = new System.Drawing.Size(89, 12);
            this.llb_machine.TabIndex = 43;
            this.llb_machine.TabStop = true;
            this.llb_machine.Text = "【鸿运搅拌机】";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.kryptonLabel2);
            this.flowLayoutPanel1.Controls.Add(this.llb_machine);
            this.flowLayoutPanel1.Controls.Add(this.kryptonLabel3);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(23, 211);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(570, 32);
            this.flowLayoutPanel1.TabIndex = 44;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(35, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 12);
            this.label1.TabIndex = 49;
            // 
            // ktb_shutMould
            // 
            this.ktb_shutMould.Location = new System.Drawing.Point(163, 3);
            this.ktb_shutMould.Name = "ktb_shutMould";
            this.ktb_shutMould.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.ktb_shutMould.Size = new System.Drawing.Size(80, 25);
            this.ktb_shutMould.TabIndex = 53;
            this.ktb_shutMould.Values.Image = global::DP_MouldSelection.Properties.Resources.brick_go;
            this.ktb_shutMould.Values.Text = "卸模";
            this.ktb_shutMould.Click += new System.EventHandler(this.ktb_shutMould_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.kryptonLabel5);
            this.flowLayoutPanel2.Controls.Add(this.linkLabel2);
            this.flowLayoutPanel2.Controls.Add(this.ktb_shutMould);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(23, 73);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(570, 32);
            this.flowLayoutPanel2.TabIndex = 55;
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(2, 4);
            this.kryptonLabel5.Margin = new System.Windows.Forms.Padding(2, 4, 0, 0);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(69, 23);
            this.kryptonLabel5.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel5.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.StateDisabled.LongText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel5.StateDisabled.LongText.Color2 = System.Drawing.Color.White;
            this.kryptonLabel5.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.StateDisabled.ShortText.Color1 = System.Drawing.Color.Blue;
            this.kryptonLabel5.StateDisabled.ShortText.Color2 = System.Drawing.Color.Blue;
            this.kryptonLabel5.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.StateNormal.LongText.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.TabIndex = 41;
            this.kryptonLabel5.Values.ImageTransparentColor = System.Drawing.Color.Blue;
            this.kryptonLabel5.Values.Text = "当前设备";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.linkLabel2.Location = new System.Drawing.Point(71, 10);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(89, 12);
            this.linkLabel2.TabIndex = 43;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "【鸿运搅拌机】";
            // 
            // treeView1
            // 
            this.treeView1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(29, 107);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(528, 97);
            this.treeView1.TabIndex = 57;
            // 
            // SelectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(594, 392);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.lvBind);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.lb_orderModel);
            this.Controls.Add(this.kryptonBorderEdge4);
            this.Controls.Add(this.kryptonLabel9);
            this.Controls.Add(this.kryptonButton1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "磨具选择器";
            this.Load += new System.EventHandler(this.SelectionForm_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ListView lvBind;
        private System.Windows.Forms.ColumnHeader ItemName;
        private System.Windows.Forms.ColumnHeader ItemValue;
        private System.Windows.Forms.ColumnHeader ItemQuality;
        private System.Windows.Forms.ColumnHeader ItemVerdict;
        private System.Windows.Forms.TextBox textBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.Label lb_orderModel;
        private ComponentFactory.Krypton.Toolkit.KryptonBorderEdge kryptonBorderEdge4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel9;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private System.Windows.Forms.LinkLabel llb_machine;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton ktb_shutMould;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.TreeView treeView1;

    }
}