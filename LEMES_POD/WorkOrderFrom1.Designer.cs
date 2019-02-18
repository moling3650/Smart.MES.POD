namespace LEMES_POD
{
    partial class WorkerOrderFrom1
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
            this.components = new System.ComponentModel.Container();
            this.kryptonManager = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonGroupBox2 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.kryptonWrapLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.kryptonDataGridView1 = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.kryptonGroupBox1 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.kdt_plannedtime1 = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kdt_plannedtime = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.Order_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.父工单 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.主工单 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flow_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.计划日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.version = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flow_state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.process_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.indent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urgency = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.input_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emp_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).BeginInit();
            this.kryptonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).BeginInit();
            this.kryptonGroupBox1.Panel.SuspendLayout();
            this.kryptonGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonManager
            // 
            this.kryptonManager.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            // 
            // kryptonPanel
            // 
            this.kryptonPanel.Controls.Add(this.kryptonGroupBox2);
            this.kryptonPanel.Controls.Add(this.kryptonDataGridView1);
            this.kryptonPanel.Controls.Add(this.kryptonGroupBox1);
            this.kryptonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel.Name = "kryptonPanel";
            this.kryptonPanel.PanelBackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.ButtonCluster;
            this.kryptonPanel.Size = new System.Drawing.Size(1113, 459);
            this.kryptonPanel.TabIndex = 0;
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.Location = new System.Drawing.Point(13, 57);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.kryptonWrapLabel1);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(1093, 46);
            this.kryptonGroupBox2.TabIndex = 3;
            this.kryptonGroupBox2.Values.Heading = "";
            // 
            // kryptonWrapLabel1
            // 
            this.kryptonWrapLabel1.Font = new System.Drawing.Font("Segoe UI", 13.5F, System.Drawing.FontStyle.Bold);
            this.kryptonWrapLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(59)))), ((int)(((byte)(59)))));
            this.kryptonWrapLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.TitleControl;
            this.kryptonWrapLabel1.Location = new System.Drawing.Point(275, 8);
            this.kryptonWrapLabel1.Name = "kryptonWrapLabel1";
            this.kryptonWrapLabel1.Size = new System.Drawing.Size(175, 25);
            this.kryptonWrapLabel1.Text = "今日无生产指令......";
            // 
            // kryptonDataGridView1
            // 
            this.kryptonDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Order_no,
            this.父工单,
            this.主工单,
            this.产品编码,
            this.产品名称,
            this.flow_code,
            this.数量,
            this.计划日期,
            this.version,
            this.flow_state,
            this.process_code,
            this.id,
            this.indent,
            this.urgency,
            this.state,
            this.input_time,
            this.customer,
            this.property,
            this.emp_code});
            this.kryptonDataGridView1.Location = new System.Drawing.Point(12, 57);
            this.kryptonDataGridView1.Name = "kryptonDataGridView1";
            this.kryptonDataGridView1.RowTemplate.Height = 23;
            this.kryptonDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.kryptonDataGridView1.Size = new System.Drawing.Size(1094, 377);
            this.kryptonDataGridView1.TabIndex = 2;
            this.kryptonDataGridView1.Visible = false;
            this.kryptonDataGridView1.DoubleClick += new System.EventHandler(this.kryptonDataGridView1_DoubleClick);
            // 
            // kryptonGroupBox1
            // 
            this.kryptonGroupBox1.Location = new System.Drawing.Point(12, 12);
            this.kryptonGroupBox1.Name = "kryptonGroupBox1";
            // 
            // kryptonGroupBox1.Panel
            // 
            this.kryptonGroupBox1.Panel.Controls.Add(this.label1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kdt_plannedtime1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonButton2);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonButton1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kryptonLabel1);
            this.kryptonGroupBox1.Panel.Controls.Add(this.kdt_plannedtime);
            this.kryptonGroupBox1.Size = new System.Drawing.Size(1094, 39);
            this.kryptonGroupBox1.TabIndex = 1;
            this.kryptonGroupBox1.Values.Heading = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(177, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "--";
            // 
            // kdt_plannedtime1
            // 
            this.kdt_plannedtime1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.kdt_plannedtime1.Location = new System.Drawing.Point(195, 6);
            this.kdt_plannedtime1.Name = "kdt_plannedtime1";
            this.kdt_plannedtime1.Size = new System.Drawing.Size(120, 21);
            this.kdt_plannedtime1.TabIndex = 3;
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(402, 4);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(65, 25);
            this.kryptonButton2.TabIndex = 2;
            this.kryptonButton2.Values.Text = "确定";
            this.kryptonButton2.Click += new System.EventHandler(this.kryptonButton2_Click);
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Gallery;
            this.kryptonButton1.Location = new System.Drawing.Point(323, 4);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(66, 25);
            this.kryptonButton1.TabIndex = 1;
            this.kryptonButton1.Values.Text = "查询";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.LabelStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.BoldControl;
            this.kryptonLabel1.Location = new System.Drawing.Point(14, 7);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(39, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "日期:";
            // 
            // kdt_plannedtime
            // 
            this.kdt_plannedtime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.kdt_plannedtime.Location = new System.Drawing.Point(56, 6);
            this.kdt_plannedtime.Name = "kdt_plannedtime";
            this.kdt_plannedtime.Size = new System.Drawing.Size(120, 21);
            this.kdt_plannedtime.TabIndex = 0;
            // 
            // Order_no
            // 
            this.Order_no.DataPropertyName = "order_no";
            this.Order_no.HeaderText = "工单号";
            this.Order_no.Name = "Order_no";
            this.Order_no.ReadOnly = true;
            this.Order_no.Width = 200;
            // 
            // 父工单
            // 
            this.父工单.DataPropertyName = "parent_order";
            this.父工单.HeaderText = "父工单";
            this.父工单.Name = "父工单";
            this.父工单.ReadOnly = true;
            this.父工单.Visible = false;
            this.父工单.Width = 170;
            // 
            // 主工单
            // 
            this.主工单.DataPropertyName = "main_order";
            this.主工单.HeaderText = "主工单";
            this.主工单.Name = "主工单";
            this.主工单.ReadOnly = true;
            this.主工单.Width = 170;
            // 
            // 产品编码
            // 
            this.产品编码.DataPropertyName = "product_code";
            this.产品编码.HeaderText = "产品编码";
            this.产品编码.Name = "产品编码";
            this.产品编码.ReadOnly = true;
            this.产品编码.Width = 150;
            // 
            // 产品名称
            // 
            this.产品名称.DataPropertyName = "Product_name";
            this.产品名称.HeaderText = "产品名称";
            this.产品名称.Name = "产品名称";
            this.产品名称.Width = 150;
            // 
            // flow_code
            // 
            this.flow_code.DataPropertyName = "flow_code";
            this.flow_code.HeaderText = "工艺编码";
            this.flow_code.Name = "flow_code";
            this.flow_code.ReadOnly = true;
            this.flow_code.Width = 170;
            // 
            // 数量
            // 
            this.数量.DataPropertyName = "qty";
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.ReadOnly = true;
            // 
            // 计划日期
            // 
            this.计划日期.DataPropertyName = "planned_time";
            this.计划日期.HeaderText = "计划日期";
            this.计划日期.Name = "计划日期";
            this.计划日期.ReadOnly = true;
            this.计划日期.Width = 110;
            // 
            // version
            // 
            this.version.DataPropertyName = "version";
            this.version.HeaderText = "version";
            this.version.Name = "version";
            this.version.Visible = false;
            // 
            // flow_state
            // 
            this.flow_state.DataPropertyName = "flow_state";
            this.flow_state.HeaderText = "flow_state";
            this.flow_state.Name = "flow_state";
            this.flow_state.Visible = false;
            // 
            // process_code
            // 
            this.process_code.DataPropertyName = "process_code";
            this.process_code.HeaderText = "process_code";
            this.process_code.Name = "process_code";
            this.process_code.Visible = false;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // indent
            // 
            this.indent.DataPropertyName = "indent";
            this.indent.HeaderText = "indent";
            this.indent.Name = "indent";
            this.indent.ReadOnly = true;
            this.indent.Visible = false;
            // 
            // urgency
            // 
            this.urgency.DataPropertyName = "urgency";
            this.urgency.HeaderText = "urgency";
            this.urgency.Name = "urgency";
            this.urgency.ReadOnly = true;
            this.urgency.Visible = false;
            // 
            // state
            // 
            this.state.DataPropertyName = "state";
            this.state.HeaderText = "state";
            this.state.Name = "state";
            this.state.ReadOnly = true;
            this.state.Visible = false;
            // 
            // input_time
            // 
            this.input_time.DataPropertyName = "input_time";
            this.input_time.HeaderText = "input_time";
            this.input_time.Name = "input_time";
            this.input_time.ReadOnly = true;
            this.input_time.Visible = false;
            // 
            // customer
            // 
            this.customer.DataPropertyName = "customer";
            this.customer.HeaderText = "customer";
            this.customer.Name = "customer";
            this.customer.ReadOnly = true;
            this.customer.Visible = false;
            // 
            // property
            // 
            this.property.DataPropertyName = "property";
            this.property.HeaderText = "property";
            this.property.Name = "property";
            this.property.ReadOnly = true;
            this.property.Visible = false;
            // 
            // emp_code
            // 
            this.emp_code.DataPropertyName = "emp_code";
            this.emp_code.HeaderText = "emp_code";
            this.emp_code.Name = "emp_code";
            this.emp_code.ReadOnly = true;
            this.emp_code.Visible = false;
            // 
            // WorkerOrderFrom1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 459);
            this.Controls.Add(this.kryptonPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "WorkerOrderFrom1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工单";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel)).EndInit();
            this.kryptonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            this.kryptonGroupBox2.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1.Panel)).EndInit();
            this.kryptonGroupBox1.Panel.ResumeLayout(false);
            this.kryptonGroupBox1.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox1)).EndInit();
            this.kryptonGroupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker kdt_plannedtime;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryptonDataGridView1;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel kryptonWrapLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private System.Windows.Forms.Label label1;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker kdt_plannedtime1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Order_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn 父工单;
        private System.Windows.Forms.DataGridViewTextBoxColumn 主工单;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn flow_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 计划日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn version;
        private System.Windows.Forms.DataGridViewTextBoxColumn flow_state;
        private System.Windows.Forms.DataGridViewTextBoxColumn process_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn indent;
        private System.Windows.Forms.DataGridViewTextBoxColumn urgency;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.DataGridViewTextBoxColumn input_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn customer;
        private System.Windows.Forms.DataGridViewTextBoxColumn property;
        private System.Windows.Forms.DataGridViewTextBoxColumn emp_code;
    }
}

