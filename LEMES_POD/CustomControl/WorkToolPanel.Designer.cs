namespace LEMES_POD.CustomControl
{
    partial class WorkToolPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.flp_workTool = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.flp_workTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.SuspendLayout();
            // 
            // flp_workTool
            // 
            this.flp_workTool.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flp_workTool.Controls.Add(this.pictureBox4);
            this.flp_workTool.Controls.Add(this.label6);
            this.flp_workTool.Location = new System.Drawing.Point(0, 0);
            this.flp_workTool.Margin = new System.Windows.Forms.Padding(16, 6, 3, 3);
            this.flp_workTool.Name = "flp_workTool";
            this.flp_workTool.Size = new System.Drawing.Size(230, 30);
            this.flp_workTool.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 8);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 8, 3, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 3;
            this.label6.Text = "注塑磨具003";
            // 
            // pictureBox4
            // 
            this.pictureBox4.Image = global::LEMES_POD.Properties.Resources.bullet_wrench;
            this.pictureBox4.Location = new System.Drawing.Point(6, 6);
            this.pictureBox4.Margin = new System.Windows.Forms.Padding(6, 6, 3, 3);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(16, 16);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox4.TabIndex = 0;
            this.pictureBox4.TabStop = false;
            // 
            // WorkToolPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flp_workTool);
            this.Name = "WorkToolPanel";
            this.Size = new System.Drawing.Size(231, 31);
            this.flp_workTool.ResumeLayout(false);
            this.flp_workTool.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flp_workTool;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label label6;

    }
}
