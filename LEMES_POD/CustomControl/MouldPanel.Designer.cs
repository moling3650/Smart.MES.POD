namespace LEMES_POD.CustomControl
{
    partial class MouldPanel
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
            this.flp_moulds = new System.Windows.Forms.FlowLayoutPanel();
            this.label4 = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.flp_moulds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            this.SuspendLayout();
            // 
            // flp_moulds
            // 
            this.flp_moulds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flp_moulds.Controls.Add(this.pictureBox8);
            this.flp_moulds.Controls.Add(this.label4);
            this.flp_moulds.Location = new System.Drawing.Point(0, 0);
            this.flp_moulds.Margin = new System.Windows.Forms.Padding(18, 6, 3, 3);
            this.flp_moulds.Name = "flp_moulds";
            this.flp_moulds.Size = new System.Drawing.Size(230, 30);
            this.flp_moulds.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 9);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 9, 3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "注塑磨具003";
            // 
            // pictureBox8
            // 
            this.pictureBox8.Image = global::LEMES_POD.Properties.Resources.brick;
            this.pictureBox8.Location = new System.Drawing.Point(6, 7);
            this.pictureBox8.Margin = new System.Windows.Forms.Padding(6, 7, 3, 3);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(16, 16);
            this.pictureBox8.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox8.TabIndex = 0;
            this.pictureBox8.TabStop = false;
            // 
            // MouldPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.flp_moulds);
            this.Name = "MouldPanel";
            this.Size = new System.Drawing.Size(231, 31);
            this.flp_moulds.ResumeLayout(false);
            this.flp_moulds.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flp_moulds;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.Label label4;
    }
}
