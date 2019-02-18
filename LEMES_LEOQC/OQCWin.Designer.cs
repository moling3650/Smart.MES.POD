namespace LEMES_LEOQC
{
    partial class OQCWin
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
            this.kryptonPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.ktbn_end = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.text_empcode = new System.Windows.Forms.TextBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktb_orderNo = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktb_NGnum = new System.Windows.Forms.TextBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktb_NGcoude = new System.Windows.Forms.TextBox();
            this.ktb_oqcoreder = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.gb_box = new System.Windows.Forms.GroupBox();
            this.kryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ktbn_del = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).BeginInit();
            this.kryptonPanel.SuspendLayout();
            this.gb_box.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // kryptonPanel
            // 
            this.kryptonPanel.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel.Controls.Add(this.kryptonButton2);
            this.kryptonPanel.Controls.Add(this.ktbn_end);
            this.kryptonPanel.Controls.Add(this.text_empcode);
            this.kryptonPanel.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel.Controls.Add(this.ktb_orderNo);
            this.kryptonPanel.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel.Controls.Add(this.ktb_NGnum);
            this.kryptonPanel.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel.Controls.Add(this.ktb_NGcoude);
            this.kryptonPanel.Controls.Add(this.ktb_oqcoreder);
            this.kryptonPanel.Controls.Add(this.kryptonLabel6);
            this.kryptonPanel.Controls.Add(this.gb_box);
            this.kryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel.Name = "kryptonPanel";
            this.kryptonPanel.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ContextMenuHeading;
            this.kryptonPanel.Size = new System.Drawing.Size(906, 579);
            this.kryptonPanel.TabIndex = 0;
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(37, 149);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(101, 27);
            this.kryptonLabel4.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel4.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel4.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel4.TabIndex = 75;
            this.kryptonLabel4.Values.Text = "不良代码:";
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(551, 101);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(61, 31);
            this.kryptonButton2.TabIndex = 74;
            this.kryptonButton2.Values.Image = global::LEMES_LEOQC.Properties.Resources.door_out;
            this.kryptonButton2.Values.Text = "退出";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // ktbn_end
            // 
            this.ktbn_end.Location = new System.Drawing.Point(662, 146);
            this.ktbn_end.Name = "ktbn_end";
            this.ktbn_end.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            this.ktbn_end.Size = new System.Drawing.Size(116, 73);
            this.ktbn_end.TabIndex = 73;
            this.ktbn_end.Values.Image = global::LEMES_LEOQC.Properties.Resources.save_32;
            this.ktbn_end.Values.Text = "完成检验";
            this.ktbn_end.Click += new System.EventHandler(this.ktbn_end_Click);
            // 
            // text_empcode
            // 
            this.text_empcode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.text_empcode.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.text_empcode.Location = new System.Drawing.Point(144, 101);
            this.text_empcode.Name = "text_empcode";
            this.text_empcode.Size = new System.Drawing.Size(407, 30);
            this.text_empcode.TabIndex = 0;
            this.text_empcode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.text_empcode_KeyPress);
            this.text_empcode.Leave += new System.EventHandler(this.text_empcode_Leave);
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(37, 104);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(101, 27);
            this.kryptonLabel3.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.TabIndex = 72;
            this.kryptonLabel3.Values.Text = "员工工号:";
            // 
            // ktb_orderNo
            // 
            this.ktb_orderNo.Location = new System.Drawing.Point(325, 55);
            this.ktb_orderNo.Name = "ktb_orderNo";
            this.ktb_orderNo.Size = new System.Drawing.Size(6, 2);
            this.ktb_orderNo.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_orderNo.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_orderNo.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_orderNo.TabIndex = 71;
            this.ktb_orderNo.Values.Text = "";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(252, 55);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(81, 27);
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.TabIndex = 70;
            this.kryptonLabel1.Values.Text = "工单号:";
            // 
            // ktb_NGnum
            // 
            this.ktb_NGnum.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ktb_NGnum.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_NGnum.Location = new System.Drawing.Point(144, 189);
            this.ktb_NGnum.Name = "ktb_NGnum";
            this.ktb_NGnum.Size = new System.Drawing.Size(474, 30);
            this.ktb_NGnum.TabIndex = 66;
            this.ktb_NGnum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ktb_NGnum_KeyPress);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(37, 192);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(101, 27);
            this.kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.TabIndex = 69;
            this.kryptonLabel2.Values.Text = "不良数量:";
            // 
            // ktb_NGcoude
            // 
            this.ktb_NGcoude.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ktb_NGcoude.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_NGcoude.Location = new System.Drawing.Point(144, 146);
            this.ktb_NGcoude.Name = "ktb_NGcoude";
            this.ktb_NGcoude.Size = new System.Drawing.Size(474, 30);
            this.ktb_NGcoude.TabIndex = 65;
            this.ktb_NGcoude.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ktb_NGcoude_KeyPress);
            // 
            // ktb_oqcoreder
            // 
            this.ktb_oqcoreder.Location = new System.Drawing.Point(325, 12);
            this.ktb_oqcoreder.Name = "ktb_oqcoreder";
            this.ktb_oqcoreder.Size = new System.Drawing.Size(6, 2);
            this.ktb_oqcoreder.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_oqcoreder.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_oqcoreder.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_oqcoreder.TabIndex = 68;
            this.ktb_oqcoreder.Values.Text = "";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(213, 12);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(120, 27);
            this.kryptonLabel6.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel6.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel6.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel6.TabIndex = 67;
            this.kryptonLabel6.Values.Text = "检验单据号:";
            // 
            // gb_box
            // 
            this.gb_box.BackColor = System.Drawing.SystemColors.Menu;
            this.gb_box.Controls.Add(this.kryptonDataGridView1);
            this.gb_box.Controls.Add(this.ktbn_del);
            this.gb_box.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gb_box.Location = new System.Drawing.Point(21, 225);
            this.gb_box.Name = "gb_box";
            this.gb_box.Size = new System.Drawing.Size(873, 342);
            this.gb_box.TabIndex = 50;
            this.gb_box.TabStop = false;
            this.gb_box.Text = "不良记录";
            // 
            // kryptonDataGridView1
            // 
            this.kryptonDataGridView1.AllowUserToAddRows = false;
            this.kryptonDataGridView1.AllowUserToDeleteRows = false;
            this.kryptonDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.kryptonDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.kryptonDataGridView1.Location = new System.Drawing.Point(6, 25);
            this.kryptonDataGridView1.Name = "kryptonDataGridView1";
            this.kryptonDataGridView1.ReadOnly = true;
            this.kryptonDataGridView1.RowTemplate.Height = 23;
            this.kryptonDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView1.Size = new System.Drawing.Size(772, 316);
            this.kryptonDataGridView1.TabIndex = 42;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "编号";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "oqc_order_no";
            this.Column2.HeaderText = "检验单号";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "order_no";
            this.Column3.HeaderText = "工单号";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "ng_code";
            this.Column4.HeaderText = "不良代码";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "ng_qty";
            this.Column5.HeaderText = "不良数量";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "input_time";
            this.Column6.HeaderText = "记录时间";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "emp_code";
            this.Column7.HeaderText = "员工号";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            // 
            // ktbn_del
            // 
            this.ktbn_del.Location = new System.Drawing.Point(784, 149);
            this.ktbn_del.Name = "ktbn_del";
            this.ktbn_del.Size = new System.Drawing.Size(76, 48);
            this.ktbn_del.TabIndex = 4;
            this.ktbn_del.Values.Image = global::LEMES_LEOQC.Properties.Resources.delete_32;
            this.ktbn_del.Values.Text = "删除";
            this.ktbn_del.Click += new System.EventHandler(this.ktbn_del_Click);
            // 
            // OQCWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(906, 579);
            this.Controls.Add(this.kryptonPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OQCWin";
            this.Text = "成品检验";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OQCWin_FormClosing);
            this.Load += new System.EventHandler(this.OQCWin_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).EndInit();
            this.kryptonPanel.ResumeLayout(false);
            this.kryptonPanel.PerformLayout();
            this.gb_box.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryptonDataGridView1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton ktbn_del;
        private System.Windows.Forms.GroupBox gb_box;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton ktbn_end;
        private System.Windows.Forms.TextBox text_empcode;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel ktb_orderNo;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.TextBox ktb_NGnum;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.TextBox ktb_NGcoude;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel ktb_oqcoreder;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
    }
}

