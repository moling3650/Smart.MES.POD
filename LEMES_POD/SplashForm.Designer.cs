namespace LEMES_POD
{
    partial class SplashForm
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
            this.progressPanelSample = new DevExpress.XtraWaitForm.ProgressPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // progressPanelSample
            // 
            this.progressPanelSample.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanelSample.Appearance.Font = new System.Drawing.Font("Tahoma", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressPanelSample.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.progressPanelSample.Appearance.Options.UseBackColor = true;
            this.progressPanelSample.Appearance.Options.UseFont = true;
            this.progressPanelSample.Appearance.Options.UseForeColor = true;
            this.progressPanelSample.AppearanceCaption.Font = new System.Drawing.Font("幼圆", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressPanelSample.AppearanceCaption.ForeColor = System.Drawing.Color.Lavender;
            this.progressPanelSample.AppearanceCaption.Options.UseFont = true;
            this.progressPanelSample.AppearanceCaption.Options.UseForeColor = true;
            this.progressPanelSample.AppearanceDescription.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressPanelSample.AppearanceDescription.ForeColor = System.Drawing.Color.White;
            this.progressPanelSample.AppearanceDescription.Options.UseFont = true;
            this.progressPanelSample.AppearanceDescription.Options.UseForeColor = true;
            this.progressPanelSample.BarAnimationElementThickness = 2;
            this.progressPanelSample.Caption = " 加载数据中!";
            this.progressPanelSample.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.progressPanelSample.Description = "     Loading ...";
            this.progressPanelSample.Location = new System.Drawing.Point(91, 110);
            this.progressPanelSample.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.progressPanelSample.Name = "progressPanelSample";
            this.progressPanelSample.Size = new System.Drawing.Size(218, 87);
            this.progressPanelSample.TabIndex = 2;
            this.progressPanelSample.Text = "progressPanel1";
            this.progressPanelSample.WaitAnimationType = DevExpress.Utils.Animation.WaitingAnimatorType.Ring;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(12, 240);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(203, 12);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status: Connecting to database...";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LEMES_POD.Properties.Resources.smart_mes;
            this.pictureBox1.Location = new System.Drawing.Point(183, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(84, 84);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(81)))), ((int)(((byte)(165)))));
            this.ClientSize = new System.Drawing.Size(463, 260);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressPanelSample);
            this.ForeColor = System.Drawing.Color.Red;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "欢迎使用Smart.MES系统";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraWaitForm.ProgressPanel progressPanelSample;
        public System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}