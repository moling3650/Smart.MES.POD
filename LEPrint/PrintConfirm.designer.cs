namespace NV_SNP
{
    partial class PrintConfirm
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
            this.ktb_ent = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.ktb_cansel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.ktb_num = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktb_planNum = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.aa = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ktb_planSN = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.SuspendLayout();
            // 
            // ktb_ent
            // 
            this.ktb_ent.Location = new System.Drawing.Point(367, 171);
            this.ktb_ent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ktb_ent.Name = "ktb_ent";
            this.ktb_ent.Size = new System.Drawing.Size(151, 50);
            this.ktb_ent.TabIndex = 0;
            this.ktb_ent.Values.Image = global::NV_SNP.Properties.Resources.save_32;
            this.ktb_ent.Values.Text = "确认数量";
            this.ktb_ent.Click += new System.EventHandler(this.ktb_ent_Click);
            // 
            // ktb_cansel
            // 
            this.ktb_cansel.Location = new System.Drawing.Point(201, 171);
            this.ktb_cansel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ktb_cansel.Name = "ktb_cansel";
            this.ktb_cansel.Size = new System.Drawing.Size(151, 50);
            this.ktb_cansel.TabIndex = 1;
            this.ktb_cansel.Values.Image = global::NV_SNP.Properties.Resources.delete_32;
            this.ktb_cansel.Values.Text = "打印失败";
            this.ktb_cansel.Click += new System.EventHandler(this.ktb_cansel_Click);
            // 
            // ktb_num
            // 
            this.ktb_num.Location = new System.Drawing.Point(201, 121);
            this.ktb_num.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ktb_num.Name = "ktb_num";
            this.ktb_num.Size = new System.Drawing.Size(316, 24);
            this.ktb_num.TabIndex = 2;
            this.ktb_num.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ktb_num_KeyPress);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(37, 122);
            this.kryptonLabel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(144, 24);
            this.kryptonLabel1.TabIndex = 3;
            this.kryptonLabel1.Values.Text = "实际打印到流水号:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(89, 38);
            this.kryptonLabel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(96, 24);
            this.kryptonLabel2.TabIndex = 5;
            this.kryptonLabel2.Values.Text = "计划打印数:";
            // 
            // ktb_planNum
            // 
            this.ktb_planNum.Location = new System.Drawing.Point(201, 38);
            this.ktb_planNum.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ktb_planNum.Name = "ktb_planNum";
            this.ktb_planNum.ReadOnly = true;
            this.ktb_planNum.Size = new System.Drawing.Size(117, 24);
            this.ktb_planNum.TabIndex = 4;
            // 
            // aa
            // 
            this.aa.Location = new System.Drawing.Point(37, 80);
            this.aa.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.aa.Name = "aa";
            this.aa.Size = new System.Drawing.Size(144, 24);
            this.aa.TabIndex = 7;
            this.aa.Values.Text = "计划打印到流水号:";
            // 
            // ktb_planSN
            // 
            this.ktb_planSN.Location = new System.Drawing.Point(201, 79);
            this.ktb_planSN.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ktb_planSN.Name = "ktb_planSN";
            this.ktb_planSN.ReadOnly = true;
            this.ktb_planSN.Size = new System.Drawing.Size(316, 24);
            this.ktb_planSN.TabIndex = 6;
            // 
            // PrintConfirm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(601, 292);
            this.Controls.Add(this.aa);
            this.Controls.Add(this.ktb_planSN);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.ktb_planNum);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.ktb_num);
            this.Controls.Add(this.ktb_cansel);
            this.Controls.Add(this.ktb_ent);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PrintConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印确认";
            this.Load += new System.EventHandler(this.PrintConfirm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton ktb_ent;
        private ComponentFactory.Krypton.Toolkit.KryptonButton ktb_cansel;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox ktb_num;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox ktb_planNum;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel aa;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox ktb_planSN;
    }
}