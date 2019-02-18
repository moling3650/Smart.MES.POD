namespace LEMES_POD.UserForm
{
    partial class UnpackPackForm
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
            this.txtSelPC = new System.Windows.Forms.TextBox();
            this.txtCBSL = new System.Windows.Forms.TextBox();
            this.btnCF = new System.Windows.Forms.Button();
            this.txtCBPC = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPC = new System.Windows.Forms.TextBox();
            this.txtGD = new System.Windows.Forms.TextBox();
            this.txtSL = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSelPC
            // 
            this.txtSelPC.Location = new System.Drawing.Point(64, 32);
            this.txtSelPC.Name = "txtSelPC";
            this.txtSelPC.Size = new System.Drawing.Size(230, 25);
            this.txtSelPC.TabIndex = 4;
            this.txtSelPC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSelPC_KeyPress);
            // 
            // txtCBSL
            // 
            this.txtCBSL.Enabled = false;
            this.txtCBSL.Location = new System.Drawing.Point(130, 385);
            this.txtCBSL.Name = "txtCBSL";
            this.txtCBSL.Size = new System.Drawing.Size(230, 25);
            this.txtCBSL.TabIndex = 6;
            // 
            // btnCF
            // 
            this.btnCF.Location = new System.Drawing.Point(402, 185);
            this.btnCF.Name = "btnCF";
            this.btnCF.Size = new System.Drawing.Size(75, 55);
            this.btnCF.TabIndex = 7;
            this.btnCF.Text = "拆分";
            this.btnCF.UseVisualStyleBackColor = true;
            this.btnCF.Click += new System.EventHandler(this.btnCF_Click);
            // 
            // txtCBPC
            // 
            this.txtCBPC.Enabled = false;
            this.txtCBPC.Location = new System.Drawing.Point(130, 349);
            this.txtCBPC.Name = "txtCBPC";
            this.txtCBPC.Size = new System.Drawing.Size(230, 25);
            this.txtCBPC.TabIndex = 8;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(383, 345);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 65);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "投料";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 15);
            this.label1.TabIndex = 10;
            this.label1.Text = "批次:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 388);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "输入拆包数量：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 353);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 15);
            this.label3.TabIndex = 12;
            this.label3.Text = "拆包批次：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "批次：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "工单：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 15);
            this.label6.TabIndex = 2;
            this.label6.Text = "总数量：";
            // 
            // txtPC
            // 
            this.txtPC.Enabled = false;
            this.txtPC.Location = new System.Drawing.Point(84, 39);
            this.txtPC.Name = "txtPC";
            this.txtPC.Size = new System.Drawing.Size(159, 25);
            this.txtPC.TabIndex = 13;
            // 
            // txtGD
            // 
            this.txtGD.Enabled = false;
            this.txtGD.Location = new System.Drawing.Point(84, 91);
            this.txtGD.Name = "txtGD";
            this.txtGD.Size = new System.Drawing.Size(159, 25);
            this.txtGD.TabIndex = 14;
            // 
            // txtSL
            // 
            this.txtSL.Enabled = false;
            this.txtSL.Location = new System.Drawing.Point(84, 149);
            this.txtSL.Name = "txtSL";
            this.txtSL.Size = new System.Drawing.Size(159, 25);
            this.txtSL.TabIndex = 15;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtSL);
            this.groupBox1.Controls.Add(this.txtPC);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtGD);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Enabled = false;
            this.groupBox1.Location = new System.Drawing.Point(50, 109);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(333, 201);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.Yellow;
            this.textBox3.ForeColor = System.Drawing.Color.White;
            this.textBox3.Location = new System.Drawing.Point(64, 58);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(230, 25);
            this.textBox3.TabIndex = 17;
            // 
            // UnpackPackForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 466);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtCBPC);
            this.Controls.Add(this.btnCF);
            this.Controls.Add(this.txtCBSL);
            this.Controls.Add(this.txtSelPC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "UnpackPackForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "拆包";
            this.Load += new System.EventHandler(this.UnpackPackForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelPC;
        private System.Windows.Forms.TextBox txtCBSL;
        private System.Windows.Forms.Button btnCF;
        private System.Windows.Forms.TextBox txtCBPC;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSL;
        private System.Windows.Forms.TextBox txtGD;
        private System.Windows.Forms.TextBox txtPC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox3;
    }
}