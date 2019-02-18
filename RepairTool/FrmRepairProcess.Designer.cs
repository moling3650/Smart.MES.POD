namespace RepairTool
{
    partial class FrmRepairProcess
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.textBox_sfc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_sfcqty = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_fromprocess = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.kComboBox_Process = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.dataGridView_faildetail = new System.Windows.Forms.DataGridView();
            this.detail_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failPhenomenoncode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failPhenomenondesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.failReasoncode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.failtype = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.richTextBox_faildesc = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_demoted = new System.Windows.Forms.Button();
            this.btn_confirmed = new System.Windows.Forms.Button();
            this.btn_reback = new System.Windows.Forms.Button();
            this.kComboBox_rebackprocess = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_discarde = new System.Windows.Forms.Button();
            this.kTextBox_ProductType = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.kComboBox_ProductGrade = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.btn_materialchange = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.kComboBox_Process)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_faildetail)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kComboBox_rebackprocess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kComboBox_ProductGrade)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox_sfc
            // 
            this.textBox_sfc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_sfc.Location = new System.Drawing.Point(81, 7);
            this.textBox_sfc.Name = "textBox_sfc";
            this.textBox_sfc.ReadOnly = true;
            this.textBox_sfc.Size = new System.Drawing.Size(264, 30);
            this.textBox_sfc.TabIndex = 102;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 24);
            this.label2.TabIndex = 103;
            this.label2.Text = "批次号：";
            // 
            // textBox_sfcqty
            // 
            this.textBox_sfcqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_sfcqty.Location = new System.Drawing.Point(413, 7);
            this.textBox_sfcqty.Name = "textBox_sfcqty";
            this.textBox_sfcqty.ReadOnly = true;
            this.textBox_sfcqty.Size = new System.Drawing.Size(116, 30);
            this.textBox_sfcqty.TabIndex = 104;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(354, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 105;
            this.label1.Text = "数量：";
            // 
            // textBox_fromprocess
            // 
            this.textBox_fromprocess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_fromprocess.Location = new System.Drawing.Point(595, 7);
            this.textBox_fromprocess.Name = "textBox_fromprocess";
            this.textBox_fromprocess.ReadOnly = true;
            this.textBox_fromprocess.Size = new System.Drawing.Size(182, 30);
            this.textBox_fromprocess.TabIndex = 106;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(536, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 24);
            this.label3.TabIndex = 107;
            this.label3.Text = "来源：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(797, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 24);
            this.label4.TabIndex = 108;
            this.label4.Text = "责任工序：";
            // 
            // kComboBox_Process
            // 
            this.kComboBox_Process.DropDownWidth = 256;
            this.kComboBox_Process.Enabled = false;
            this.kComboBox_Process.Location = new System.Drawing.Point(902, 8);
            this.kComboBox_Process.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.kComboBox_Process.Name = "kComboBox_Process";
            this.kComboBox_Process.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kComboBox_Process.Size = new System.Drawing.Size(223, 29);
            this.kComboBox_Process.StateActive.ComboBox.Content.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateCommon.ComboBox.Content.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateCommon.Item.Content.LongText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateCommon.Item.Content.ShortText.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateDisabled.ComboBox.Content.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateDisabled.Item.Content.ShortText.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateNormal.ComboBox.Content.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateNormal.Item.Content.ShortText.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.StateTracking.Item.Content.ShortText.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_Process.TabIndex = 109;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonPanel1.Controls.Add(this.checkBox1);
            this.kryptonPanel1.Controls.Add(this.checkedListBox1);
            this.kryptonPanel1.Controls.Add(this.dataGridView_faildetail);
            this.kryptonPanel1.Location = new System.Drawing.Point(18, 89);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonPanel1.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.HeaderForm;
            this.kryptonPanel1.Size = new System.Drawing.Size(1315, 369);
            this.kryptonPanel1.TabIndex = 110;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Location = new System.Drawing.Point(1141, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(101, 28);
            this.checkBox1.TabIndex = 134;
            this.checkBox1.Text = "是否待判";
            this.checkBox1.UseVisualStyleBackColor = false;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.SystemColors.Control;
            this.checkedListBox1.Enabled = false;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(1138, 52);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(174, 304);
            this.checkedListBox1.TabIndex = 133;
            // 
            // dataGridView_faildetail
            // 
            this.dataGridView_faildetail.AllowUserToAddRows = false;
            this.dataGridView_faildetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView_faildetail.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_faildetail.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_faildetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_faildetail.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.detail_id,
            this.failPhenomenoncode,
            this.failPhenomenondesc,
            this.failNum,
            this.failReasoncode,
            this.failtype});
            this.dataGridView_faildetail.Location = new System.Drawing.Point(3, 3);
            this.dataGridView_faildetail.Name = "dataGridView_faildetail";
            this.dataGridView_faildetail.RowTemplate.Height = 27;
            this.dataGridView_faildetail.Size = new System.Drawing.Size(1130, 355);
            this.dataGridView_faildetail.TabIndex = 115;
            // 
            // detail_id
            // 
            this.detail_id.DataPropertyName = "detail_id";
            this.detail_id.HeaderText = "ID";
            this.detail_id.Name = "detail_id";
            this.detail_id.Visible = false;
            // 
            // failPhenomenoncode
            // 
            this.failPhenomenoncode.DataPropertyName = "failPhenomenoncode";
            this.failPhenomenoncode.HeaderText = "现象代码";
            this.failPhenomenoncode.Name = "failPhenomenoncode";
            this.failPhenomenoncode.ReadOnly = true;
            this.failPhenomenoncode.Width = 200;
            // 
            // failPhenomenondesc
            // 
            this.failPhenomenondesc.DataPropertyName = "failPhenomenondesc";
            this.failPhenomenondesc.HeaderText = "现象描述";
            this.failPhenomenondesc.Name = "failPhenomenondesc";
            this.failPhenomenondesc.ReadOnly = true;
            this.failPhenomenondesc.Width = 295;
            // 
            // failNum
            // 
            this.failNum.DataPropertyName = "failNum";
            this.failNum.HeaderText = "数量";
            this.failNum.Name = "failNum";
            this.failNum.ReadOnly = true;
            // 
            // failReasoncode
            // 
            this.failReasoncode.DataPropertyName = "failReasoncode";
            this.failReasoncode.HeaderText = "原因";
            this.failReasoncode.Name = "failReasoncode";
            this.failReasoncode.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.failReasoncode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.failReasoncode.Width = 245;
            // 
            // failtype
            // 
            this.failtype.DataPropertyName = "failtype";
            this.failtype.HeaderText = "原因类型";
            this.failtype.Name = "failtype";
            this.failtype.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.failtype.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.failtype.Width = 245;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.richTextBox_faildesc);
            this.groupBox1.Location = new System.Drawing.Point(18, 507);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1321, 110);
            this.groupBox1.TabIndex = 111;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "记录";
            // 
            // richTextBox_faildesc
            // 
            this.richTextBox_faildesc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBox_faildesc.Location = new System.Drawing.Point(9, 32);
            this.richTextBox_faildesc.Name = "richTextBox_faildesc";
            this.richTextBox_faildesc.Size = new System.Drawing.Size(1306, 69);
            this.richTextBox_faildesc.TabIndex = 0;
            this.richTextBox_faildesc.Text = "";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(728, 475);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 24);
            this.label8.TabIndex = 118;
            this.label8.Text = "级别：";
            // 
            // btn_demoted
            // 
            this.btn_demoted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_demoted.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btn_demoted.Font = new System.Drawing.Font("微软雅黑", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_demoted.Location = new System.Drawing.Point(921, 469);
            this.btn_demoted.Name = "btn_demoted";
            this.btn_demoted.Size = new System.Drawing.Size(75, 36);
            this.btn_demoted.TabIndex = 117;
            this.btn_demoted.Text = "降级";
            this.btn_demoted.UseVisualStyleBackColor = false;
            this.btn_demoted.Click += new System.EventHandler(this.btn_demoted_Click);
            // 
            // btn_confirmed
            // 
            this.btn_confirmed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_confirmed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btn_confirmed.Font = new System.Drawing.Font("微软雅黑", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_confirmed.Location = new System.Drawing.Point(1010, 469);
            this.btn_confirmed.Name = "btn_confirmed";
            this.btn_confirmed.Size = new System.Drawing.Size(75, 36);
            this.btn_confirmed.TabIndex = 119;
            this.btn_confirmed.Text = "待判";
            this.btn_confirmed.UseVisualStyleBackColor = false;
            this.btn_confirmed.Visible = false;
            this.btn_confirmed.Click += new System.EventHandler(this.btn_confirmed_Click);
            // 
            // btn_reback
            // 
            this.btn_reback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_reback.Location = new System.Drawing.Point(1264, 617);
            this.btn_reback.Name = "btn_reback";
            this.btn_reback.Size = new System.Drawing.Size(69, 36);
            this.btn_reback.TabIndex = 120;
            this.btn_reback.Text = "提交";
            this.btn_reback.UseVisualStyleBackColor = true;
            this.btn_reback.Click += new System.EventHandler(this.btn_reback_Click);
            // 
            // kComboBox_rebackprocess
            // 
            this.kComboBox_rebackprocess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kComboBox_rebackprocess.DropDownWidth = 256;
            this.kComboBox_rebackprocess.Location = new System.Drawing.Point(1016, 621);
            this.kComboBox_rebackprocess.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.kComboBox_rebackprocess.Name = "kComboBox_rebackprocess";
            this.kComboBox_rebackprocess.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kComboBox_rebackprocess.Size = new System.Drawing.Size(207, 28);
            this.kComboBox_rebackprocess.StateCommon.ComboBox.Content.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_rebackprocess.StateCommon.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_rebackprocess.StateDisabled.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_rebackprocess.StateNormal.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_rebackprocess.StateTracking.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_rebackprocess.TabIndex = 121;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(911, 623);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(100, 24);
            this.label9.TabIndex = 122;
            this.label9.Text = "返回工序：";
            // 
            // btn_discarde
            // 
            this.btn_discarde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_discarde.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.btn_discarde.Font = new System.Drawing.Font("微软雅黑", 13.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_discarde.Location = new System.Drawing.Point(1101, 469);
            this.btn_discarde.Name = "btn_discarde";
            this.btn_discarde.Size = new System.Drawing.Size(75, 36);
            this.btn_discarde.TabIndex = 123;
            this.btn_discarde.Text = "报废";
            this.btn_discarde.UseVisualStyleBackColor = false;
            this.btn_discarde.Click += new System.EventHandler(this.btn_discarde_Click);
            // 
            // kTextBox_ProductType
            // 
            this.kTextBox_ProductType.Enabled = false;
            this.kTextBox_ProductType.Location = new System.Drawing.Point(1235, 8);
            this.kTextBox_ProductType.Name = "kTextBox_ProductType";
            this.kTextBox_ProductType.Size = new System.Drawing.Size(95, 29);
            this.kTextBox_ProductType.StateActive.Content.Color1 = System.Drawing.Color.Purple;
            this.kTextBox_ProductType.StateActive.Content.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kTextBox_ProductType.StateCommon.Content.Color1 = System.Drawing.Color.Purple;
            this.kTextBox_ProductType.StateCommon.Content.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kTextBox_ProductType.StateDisabled.Content.Color1 = System.Drawing.Color.Purple;
            this.kTextBox_ProductType.StateDisabled.Content.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kTextBox_ProductType.StateNormal.Content.Color1 = System.Drawing.Color.Purple;
            this.kTextBox_ProductType.StateNormal.Content.Font = new System.Drawing.Font("宋体", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kTextBox_ProductType.TabIndex = 130;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(1136, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(86, 24);
            this.label10.TabIndex = 129;
            this.label10.Text = "产品类型:";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cancel.BackColor = System.Drawing.SystemColors.Control;
            this.btn_cancel.Location = new System.Drawing.Point(1264, 469);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(69, 36);
            this.btn_cancel.TabIndex = 123;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.UseVisualStyleBackColor = false;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // kComboBox_ProductGrade
            // 
            this.kComboBox_ProductGrade.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kComboBox_ProductGrade.DropDownWidth = 256;
            this.kComboBox_ProductGrade.Location = new System.Drawing.Point(792, 473);
            this.kComboBox_ProductGrade.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.kComboBox_ProductGrade.Name = "kComboBox_ProductGrade";
            this.kComboBox_ProductGrade.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kComboBox_ProductGrade.Size = new System.Drawing.Size(114, 28);
            this.kComboBox_ProductGrade.StateActive.ComboBox.Content.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_ProductGrade.StateCommon.ComboBox.Content.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_ProductGrade.StateCommon.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_ProductGrade.StateDisabled.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_ProductGrade.StateNormal.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_ProductGrade.StateTracking.Item.Content.ShortText.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kComboBox_ProductGrade.TabIndex = 117;
            // 
            // btn_materialchange
            // 
            this.btn_materialchange.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_materialchange.BackColor = System.Drawing.Color.Transparent;
            this.btn_materialchange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btn_materialchange.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_materialchange.Image = global::RepairTool.Properties.Resources.brick_go;
            this.btn_materialchange.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btn_materialchange.Location = new System.Drawing.Point(1235, 43);
            this.btn_materialchange.Name = "btn_materialchange";
            this.btn_materialchange.Size = new System.Drawing.Size(98, 36);
            this.btn_materialchange.TabIndex = 117;
            this.btn_materialchange.Text = "物料替换";
            this.btn_materialchange.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btn_materialchange.UseVisualStyleBackColor = false;
            this.btn_materialchange.Click += new System.EventHandler(this.btn_materialchange_Click);
            // 
            // FrmRepairProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1345, 671);
            this.Controls.Add(this.btn_discarde);
            this.Controls.Add(this.btn_confirmed);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.kComboBox_ProductGrade);
            this.Controls.Add(this.btn_demoted);
            this.Controls.Add(this.kTextBox_ProductType);
            this.Controls.Add(this.btn_reback);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.kComboBox_rebackprocess);
            this.Controls.Add(this.btn_materialchange);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.kryptonPanel1);
            this.Controls.Add(this.kComboBox_Process);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox_fromprocess);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_sfcqty);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_sfc);
            this.Controls.Add(this.label2);
            this.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "FrmRepairProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "维修";
            this.Load += new System.EventHandler(this.FrmRepairProcess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.kComboBox_Process)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_faildetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kComboBox_rebackprocess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kComboBox_ProductGrade)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_sfc;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_sfcqty;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_fromprocess;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kComboBox_Process;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private System.Windows.Forms.DataGridView dataGridView_faildetail;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox richTextBox_faildesc;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_demoted;
        private System.Windows.Forms.Button btn_confirmed;
        private System.Windows.Forms.Button btn_reback;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kComboBox_rebackprocess;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_materialchange;
        private System.Windows.Forms.Button btn_discarde;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox kTextBox_ProductType;
        private System.Windows.Forms.Label label10;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kComboBox_ProductGrade;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn detail_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn failPhenomenoncode;
        private System.Windows.Forms.DataGridViewTextBoxColumn failPhenomenondesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn failNum;
        private System.Windows.Forms.DataGridViewComboBoxColumn failReasoncode;
        private System.Windows.Forms.DataGridViewComboBoxColumn failtype;
    }
}