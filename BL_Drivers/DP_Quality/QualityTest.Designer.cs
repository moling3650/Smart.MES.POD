namespace DP_Quality
{
    partial class QualityTest
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
            this.lb_selected = new DevExpress.XtraEditors.ListBoxControl();
            this.lb_code = new DevExpress.XtraEditors.ListBoxControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.lb_Type = new DevExpress.XtraEditors.ListBoxControl();
            this.checkButton2 = new DevExpress.XtraEditors.CheckButton();
            this.checkButton1 = new DevExpress.XtraEditors.CheckButton();
            ((System.ComponentModel.ISupportInitialize)(this.lb_selected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lb_code)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lb_Type)).BeginInit();
            this.SuspendLayout();
            // 
            // lb_selected
            // 
            this.lb_selected.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lb_selected.Appearance.Options.UseFont = true;
            this.lb_selected.Cursor = System.Windows.Forms.Cursors.Default;
            this.lb_selected.Enabled = false;
            this.lb_selected.Location = new System.Drawing.Point(452, 70);
            this.lb_selected.Name = "lb_selected";
            this.lb_selected.Size = new System.Drawing.Size(185, 181);
            this.lb_selected.TabIndex = 17;
            this.lb_selected.DoubleClick += new System.EventHandler(this.lb_selected_DoubleClick);
            // 
            // lb_code
            // 
            this.lb_code.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lb_code.Appearance.Options.UseFont = true;
            this.lb_code.Cursor = System.Windows.Forms.Cursors.Default;
            this.lb_code.Enabled = false;
            this.lb_code.Location = new System.Drawing.Point(220, 70);
            this.lb_code.Name = "lb_code";
            this.lb_code.Size = new System.Drawing.Size(185, 181);
            this.lb_code.TabIndex = 16;
            this.lb_code.DoubleClick += new System.EventHandler(this.lb_code_DoubleClick);
            // 
            // simpleButton3
            // 
            this.simpleButton3.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButton3.Location = new System.Drawing.Point(419, 160);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(21, 40);
            this.simpleButton3.TabIndex = 15;
            this.simpleButton3.Text = "<";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton2
            // 
            this.simpleButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.simpleButton2.Location = new System.Drawing.Point(419, 109);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(21, 40);
            this.simpleButton2.TabIndex = 14;
            this.simpleButton2.Text = ">";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // simpleButton
            // 
            this.simpleButton.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.simpleButton.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.simpleButton.Location = new System.Drawing.Point(452, 260);
            this.simpleButton.Name = "simpleButton";
            this.simpleButton.Size = new System.Drawing.Size(82, 33);
            this.simpleButton.TabIndex = 13;
            this.simpleButton.Text = "保存";
            this.simpleButton.Click += new System.EventHandler(this.simpleButton_Click);
            // 
            // lb_Type
            // 
            this.lb_Type.Appearance.Font = new System.Drawing.Font("微软雅黑", 10F);
            this.lb_Type.Appearance.Options.UseFont = true;
            this.lb_Type.Cursor = System.Windows.Forms.Cursors.Default;
            this.lb_Type.Enabled = false;
            this.lb_Type.Location = new System.Drawing.Point(26, 70);
            this.lb_Type.Name = "lb_Type";
            this.lb_Type.Size = new System.Drawing.Size(185, 181);
            this.lb_Type.TabIndex = 12;
            this.lb_Type.SelectedIndexChanged += new System.EventHandler(this.lb_Type_SelectedIndexChanged);
            // 
            // checkButton2
            // 
            this.checkButton2.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.checkButton2.GroupIndex = 1;
            this.checkButton2.Location = new System.Drawing.Point(132, 16);
            this.checkButton2.Name = "checkButton2";
            this.checkButton2.Size = new System.Drawing.Size(79, 33);
            this.checkButton2.TabIndex = 11;
            this.checkButton2.TabStop = false;
            this.checkButton2.Text = "FAIL";
            this.checkButton2.CheckedChanged += new System.EventHandler(this.checkButton2_CheckedChanged);
            // 
            // checkButton1
            // 
            this.checkButton1.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.HotFlat;
            this.checkButton1.Checked = true;
            this.checkButton1.GroupIndex = 1;
            this.checkButton1.Location = new System.Drawing.Point(26, 16);
            this.checkButton1.Name = "checkButton1";
            this.checkButton1.Size = new System.Drawing.Size(80, 33);
            this.checkButton1.TabIndex = 10;
            this.checkButton1.Text = "PASS";
            this.checkButton1.CheckedChanged += new System.EventHandler(this.checkButton1_CheckedChanged);
            // 
            // QualityTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 308);
            this.Controls.Add(this.lb_selected);
            this.Controls.Add(this.lb_code);
            this.Controls.Add(this.simpleButton3);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton);
            this.Controls.Add(this.lb_Type);
            this.Controls.Add(this.checkButton2);
            this.Controls.Add(this.checkButton1);
            this.Name = "QualityTest";
            this.Text = "QualityTest";
            this.Load += new System.EventHandler(this.QualityTest_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.QualityTest_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.lb_selected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lb_code)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lb_Type)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.ListBoxControl lb_selected;
        private DevExpress.XtraEditors.ListBoxControl lb_code;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton;
        private DevExpress.XtraEditors.ListBoxControl lb_Type;
        private DevExpress.XtraEditors.CheckButton checkButton2;
        private DevExpress.XtraEditors.CheckButton checkButton1;

    }
}