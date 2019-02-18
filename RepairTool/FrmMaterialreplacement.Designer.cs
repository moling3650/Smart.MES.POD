namespace RepairTool
{
    partial class FrmMaterialreplacement
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
            this.txtbox_sfc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.kryptonDataGridView_sublstsfc = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            this.subsfc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subsfcorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subsfcmatcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subchangesfc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_commit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtbox_shoporder = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView_sublstsfc)).BeginInit();
            this.SuspendLayout();
            // 
            // txtbox_sfc
            // 
            this.txtbox_sfc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtbox_sfc.Location = new System.Drawing.Point(122, 15);
            this.txtbox_sfc.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtbox_sfc.Name = "txtbox_sfc";
            this.txtbox_sfc.ReadOnly = true;
            this.txtbox_sfc.Size = new System.Drawing.Size(335, 36);
            this.txtbox_sfc.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "主批次号:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.kryptonDataGridView_sublstsfc);
            this.groupBox1.Location = new System.Drawing.Point(18, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1206, 537);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "子批次号列表";
            // 
            // kryptonDataGridView_sublstsfc
            // 
            this.kryptonDataGridView_sublstsfc.AllowUserToAddRows = false;
            this.kryptonDataGridView_sublstsfc.AllowUserToDeleteRows = false;
            this.kryptonDataGridView_sublstsfc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.subsfc,
            this.subsfcorder,
            this.subsfcmatcode,
            this.subchangesfc});
            this.kryptonDataGridView_sublstsfc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonDataGridView_sublstsfc.Location = new System.Drawing.Point(3, 32);
            this.kryptonDataGridView_sublstsfc.Name = "kryptonDataGridView_sublstsfc";
            this.kryptonDataGridView_sublstsfc.RowTemplate.Height = 27;
            this.kryptonDataGridView_sublstsfc.Size = new System.Drawing.Size(1200, 502);
            this.kryptonDataGridView_sublstsfc.StateCommon.BackStyle = ComponentFactory.Krypton.Toolkit.PaletteBackStyle.GridBackgroundList;
            this.kryptonDataGridView_sublstsfc.StateCommon.DataCell.Content.Color1 = System.Drawing.Color.Black;
            this.kryptonDataGridView_sublstsfc.StateCommon.DataCell.Content.Color2 = System.Drawing.Color.Black;
            this.kryptonDataGridView_sublstsfc.StateCommon.DataCell.Content.Font = new System.Drawing.Font("宋体", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonDataGridView_sublstsfc.TabIndex = 0;
            this.kryptonDataGridView_sublstsfc.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.kryptonDataGridView_sublstsfc_CellEndEdit);
            this.kryptonDataGridView_sublstsfc.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.kryptonDataGridView_sublstsfc_RowPostPaint);
            // 
            // subsfc
            // 
            this.subsfc.DataPropertyName = "subsfc";
            this.subsfc.HeaderText = "子批次号";
            this.subsfc.Name = "subsfc";
            this.subsfc.ReadOnly = true;
            this.subsfc.Width = 300;
            // 
            // subsfcorder
            // 
            this.subsfcorder.DataPropertyName = "subsfcorder";
            this.subsfcorder.HeaderText = "工单";
            this.subsfcorder.Name = "subsfcorder";
            this.subsfcorder.ReadOnly = true;
            this.subsfcorder.Width = 300;
            // 
            // subsfcmatcode
            // 
            this.subsfcmatcode.DataPropertyName = "subsfcmatcode";
            this.subsfcmatcode.HeaderText = "物料号";
            this.subsfcmatcode.Name = "subsfcmatcode";
            this.subsfcmatcode.ReadOnly = true;
            this.subsfcmatcode.Width = 200;
            // 
            // subchangesfc
            // 
            this.subchangesfc.DataPropertyName = "subchangesfc";
            this.subchangesfc.HeaderText = "替换批次号";
            this.subchangesfc.Name = "subchangesfc";
            this.subchangesfc.Width = 300;
            // 
            // btn_commit
            // 
            this.btn_commit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_commit.Location = new System.Drawing.Point(1111, 603);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(113, 41);
            this.btn_commit.TabIndex = 1;
            this.btn_commit.Text = "提交";
            this.btn_commit.UseVisualStyleBackColor = true;
            this.btn_commit.Click += new System.EventHandler(this.btn_commit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(491, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "工单:";
            // 
            // txtbox_shoporder
            // 
            this.txtbox_shoporder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.txtbox_shoporder.Location = new System.Drawing.Point(556, 15);
            this.txtbox_shoporder.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.txtbox_shoporder.Name = "txtbox_shoporder";
            this.txtbox_shoporder.ReadOnly = true;
            this.txtbox_shoporder.Size = new System.Drawing.Size(335, 36);
            this.txtbox_shoporder.TabIndex = 4;
            // 
            // FrmMaterialreplacement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 656);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbox_shoporder);
            this.Controls.Add(this.btn_commit);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtbox_sfc);
            this.Font = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.Name = "FrmMaterialreplacement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "物料替换";
            this.Load += new System.EventHandler(this.FrmMaterialreplacement_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonDataGridView_sublstsfc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbox_sfc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView kryptonDataGridView_sublstsfc;
        private System.Windows.Forms.Button btn_commit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtbox_shoporder;
        private System.Windows.Forms.DataGridViewTextBoxColumn subsfc;
        private System.Windows.Forms.DataGridViewTextBoxColumn subsfcorder;
        private System.Windows.Forms.DataGridViewTextBoxColumn subsfcmatcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn subchangesfc;
    }
}