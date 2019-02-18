namespace DP005
{
    partial class DispatchForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.button3System = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.order_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dispatching_no = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.product_code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cplt_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planned_start_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planned_start_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planned_cplt_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.order_no,
            this.dispatching_no,
            this.product_name,
            this.product_code,
            this.qty,
            this.cplt_qty,
            this.state,
            this.planned_start_date,
            this.planned_start_time,
            this.planned_cplt_time,
            this.description});
            this.dataGridView1.Location = new System.Drawing.Point(12, 69);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(933, 306);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CalendarFont = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTimePicker1.Location = new System.Drawing.Point(12, 22);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 26);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonButton1.Location = new System.Drawing.Point(847, 386);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            this.kryptonButton1.Size = new System.Drawing.Size(98, 32);
            this.kryptonButton1.TabIndex = 6;
            this.kryptonButton1.Values.Image = global::DP005.Properties.Resources.application_start;
            this.kryptonButton1.Values.Text = "  选择";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // button3System
            // 
            this.button3System.AutoSize = true;
            this.button3System.Location = new System.Drawing.Point(431, 23);
            this.button3System.Name = "button3System";
            this.button3System.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.button3System.Size = new System.Drawing.Size(79, 24);
            this.button3System.TabIndex = 8;
            this.button3System.Values.Image = global::DP005.Properties.Resources.application_view_columns;
            this.button3System.Values.Text = "全部任务";
            this.button3System.Click += new System.EventHandler(this.button3System_Click);
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.AutoSize = true;
            this.kryptonButton2.Location = new System.Drawing.Point(236, 23);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonButton2.Size = new System.Drawing.Size(79, 24);
            this.kryptonButton2.TabIndex = 9;
            this.kryptonButton2.Values.Image = global::DP005.Properties.Resources.arrow_left;
            this.kryptonButton2.Values.Text = "前一天";
            // 
            // kryptonButton3
            // 
            this.kryptonButton3.AutoSize = true;
            this.kryptonButton3.Location = new System.Drawing.Point(323, 23);
            this.kryptonButton3.Name = "kryptonButton3";
            this.kryptonButton3.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.kryptonButton3.Size = new System.Drawing.Size(79, 24);
            this.kryptonButton3.TabIndex = 10;
            this.kryptonButton3.Values.Image = global::DP005.Properties.Resources.arrow_right;
            this.kryptonButton3.Values.Text = "后一天";
            // 
            // order_no
            // 
            this.order_no.DataPropertyName = "order_no";
            this.order_no.HeaderText = "工单";
            this.order_no.Name = "order_no";
            this.order_no.ReadOnly = true;
            // 
            // dispatching_no
            // 
            this.dispatching_no.DataPropertyName = "dispatching_no";
            this.dispatching_no.HeaderText = "派工单";
            this.dispatching_no.Name = "dispatching_no";
            // 
            // product_name
            // 
            this.product_name.DataPropertyName = "product_name";
            this.product_name.HeaderText = "成品名";
            this.product_name.Name = "product_name";
            // 
            // product_code
            // 
            this.product_code.DataPropertyName = "product_code";
            this.product_code.HeaderText = "成品编号";
            this.product_code.Name = "product_code";
            this.product_code.Visible = false;
            // 
            // qty
            // 
            this.qty.DataPropertyName = "qty";
            this.qty.HeaderText = "计划数";
            this.qty.Name = "qty";
            // 
            // cplt_qty
            // 
            this.cplt_qty.DataPropertyName = "cplt_qty";
            this.cplt_qty.HeaderText = "完成数";
            this.cplt_qty.Name = "cplt_qty";
            // 
            // state
            // 
            this.state.DataPropertyName = "state";
            this.state.HeaderText = "状态";
            this.state.Name = "state";
            // 
            // planned_start_date
            // 
            this.planned_start_date.DataPropertyName = "planned_start_date";
            this.planned_start_date.HeaderText = "计划日";
            this.planned_start_date.Name = "planned_start_date";
            // 
            // planned_start_time
            // 
            this.planned_start_time.DataPropertyName = "planned_start_time";
            this.planned_start_time.HeaderText = "计划开工";
            this.planned_start_time.Name = "planned_start_time";
            // 
            // planned_cplt_time
            // 
            this.planned_cplt_time.DataPropertyName = "planned_cplt_time";
            this.planned_cplt_time.HeaderText = "计划完工";
            this.planned_cplt_time.Name = "planned_cplt_time";
            // 
            // description
            // 
            this.description.DataPropertyName = "description";
            this.description.HeaderText = "备注";
            this.description.Name = "description";
            // 
            // DispatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 430);
            this.Controls.Add(this.kryptonButton3);
            this.Controls.Add(this.kryptonButton2);
            this.Controls.Add(this.button3System);
            this.Controls.Add(this.kryptonButton1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "DispatchForm";
            this.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "派工单选择";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.DispatchForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton button3System;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton3;
        private System.Windows.Forms.DataGridViewTextBoxColumn order_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn dispatching_no;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn product_code;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn cplt_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
        private System.Windows.Forms.DataGridViewTextBoxColumn planned_start_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn planned_start_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn planned_cplt_time;
        private System.Windows.Forms.DataGridViewTextBoxColumn description;
    }
}