namespace LEMES_LEOQC
{
    partial class Main
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kckl_lot = new ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox();
            this.kcb_Level = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonComboBox1 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonComboBox2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.ktl_total = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel7 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kbtn_del = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel8 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kcb_order = new System.Windows.Forms.TextBox();
            this.ktb_lotNo = new System.Windows.Forms.TextBox();
            this.gb_box = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.kcb_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).BeginInit();
            this.gb_box.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Cluster;
            this.kryptonButton1.Location = new System.Drawing.Point(218, 322);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(108, 46);
            this.kryptonButton1.TabIndex = 6;
            this.kryptonButton1.Values.Image = global::LEMES_LEOQC.Properties.Resources.save_32;
            this.kryptonButton1.Values.Text = "生成";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // kckl_lot
            // 
            this.kckl_lot.CheckOnClick = true;
            this.kckl_lot.Location = new System.Drawing.Point(160, 20);
            this.kckl_lot.Name = "kckl_lot";
            this.kckl_lot.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Blue;
            this.kckl_lot.Size = new System.Drawing.Size(241, 106);
            this.kckl_lot.TabIndex = 2;
            this.kckl_lot.Leave += new System.EventHandler(this.kckl_lot_Leave);
            // 
            // kcb_Level
            // 
            this.kcb_Level.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kcb_Level.DropDownWidth = 121;
            this.kcb_Level.Location = new System.Drawing.Point(160, 269);
            this.kcb_Level.Name = "kcb_Level";
            this.kcb_Level.Size = new System.Drawing.Size(241, 21);
            this.kcb_Level.TabIndex = 5;
            // 
            // kryptonComboBox1
            // 
            this.kryptonComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBox1.DropDownWidth = 121;
            this.kryptonComboBox1.Location = new System.Drawing.Point(160, 221);
            this.kryptonComboBox1.Name = "kryptonComboBox1";
            this.kryptonComboBox1.Size = new System.Drawing.Size(241, 21);
            this.kryptonComboBox1.TabIndex = 4;
            // 
            // kryptonComboBox2
            // 
            this.kryptonComboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.kryptonComboBox2.DropDownWidth = 121;
            this.kryptonComboBox2.Location = new System.Drawing.Point(160, 175);
            this.kryptonComboBox2.Name = "kryptonComboBox2";
            this.kryptonComboBox2.Size = new System.Drawing.Size(241, 21);
            this.kryptonComboBox2.TabIndex = 3;
            this.kryptonComboBox2.SelectedIndexChanged += new System.EventHandler(this.kryptonComboBox2_SelectedIndexChanged);
            // 
            // ktl_total
            // 
            this.ktl_total.Location = new System.Drawing.Point(181, 134);
            this.ktl_total.Name = "ktl_total";
            this.ktl_total.Size = new System.Drawing.Size(25, 30);
            this.ktl_total.StateCommon.ShortText.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktl_total.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktl_total.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktl_total.TabIndex = 30;
            this.ktl_total.Values.Text = "0";
            this.ktl_total.TextChanged += new System.EventHandler(this.ktl_total_TextChanged);
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(36, 48);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(101, 27);
            this.kryptonLabel7.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel7.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel7.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel7.TabIndex = 32;
            this.kryptonLabel7.Values.Text = "送检批次:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(56, 136);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(81, 27);
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.TabIndex = 33;
            this.kryptonLabel1.Values.Text = "总数量:";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(34, 175);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(101, 27);
            this.kryptonLabel5.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel5.TabIndex = 34;
            this.kryptonLabel5.Values.Text = "检验水准:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(7, 221);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(140, 27);
            this.kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel2.TabIndex = 35;
            this.kryptonLabel2.Values.Text = "水准下属等级:";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(34, 269);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(101, 27);
            this.kryptonLabel3.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel3.TabIndex = 36;
            this.kryptonLabel3.Values.Text = "抽样等级:";
            // 
            // kbtn_del
            // 
            this.kbtn_del.Location = new System.Drawing.Point(427, 49);
            this.kbtn_del.Name = "kbtn_del";
            this.kbtn_del.Size = new System.Drawing.Size(103, 48);
            this.kbtn_del.TabIndex = 7;
            this.kbtn_del.Values.Image = global::LEMES_LEOQC.Properties.Resources.delete_32;
            this.kbtn_del.Values.Text = "删除批次";
            this.kbtn_del.Click += new System.EventHandler(this.kbtn_del_Click);
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(63, 59);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(140, 27);
            this.kryptonLabel8.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel8.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel8.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel8.TabIndex = 39;
            this.kryptonLabel8.Values.Text = "请输入批次号:";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(64, 12);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(140, 27);
            this.kryptonLabel6.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel6.StateDisabled.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel6.StateDisabled.ShortText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel6.TabIndex = 31;
            this.kryptonLabel6.Values.Text = "请输入工单号:";
            // 
            // kcb_order
            // 
            this.kcb_order.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kcb_order.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kcb_order.Location = new System.Drawing.Point(203, 9);
            this.kcb_order.Name = "kcb_order";
            this.kcb_order.Size = new System.Drawing.Size(251, 30);
            this.kcb_order.TabIndex = 40;
            this.kcb_order.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.kcb_order_KeyPress);
            // 
            // ktb_lotNo
            // 
            this.ktb_lotNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ktb_lotNo.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ktb_lotNo.Location = new System.Drawing.Point(203, 56);
            this.ktb_lotNo.Name = "ktb_lotNo";
            this.ktb_lotNo.Size = new System.Drawing.Size(250, 30);
            this.ktb_lotNo.TabIndex = 41;
            this.ktb_lotNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ktb_lotNo_KeyPress);
            // 
            // gb_box
            // 
            this.gb_box.Controls.Add(this.kckl_lot);
            this.gb_box.Controls.Add(this.kryptonButton1);
            this.gb_box.Controls.Add(this.kcb_Level);
            this.gb_box.Controls.Add(this.kryptonComboBox1);
            this.gb_box.Controls.Add(this.kbtn_del);
            this.gb_box.Controls.Add(this.kryptonComboBox2);
            this.gb_box.Controls.Add(this.ktl_total);
            this.gb_box.Controls.Add(this.kryptonLabel3);
            this.gb_box.Controls.Add(this.kryptonLabel7);
            this.gb_box.Controls.Add(this.kryptonLabel1);
            this.gb_box.Controls.Add(this.kryptonLabel2);
            this.gb_box.Controls.Add(this.kryptonLabel5);
            this.gb_box.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gb_box.Location = new System.Drawing.Point(44, 112);
            this.gb_box.Name = "gb_box";
            this.gb_box.Size = new System.Drawing.Size(581, 396);
            this.gb_box.TabIndex = 42;
            this.gb_box.TabStop = false;
            this.gb_box.Text = "检验标准";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(704, 564);
            this.Controls.Add(this.kryptonLabel6);
            this.Controls.Add(this.ktb_lotNo);
            this.Controls.Add(this.gb_box);
            this.Controls.Add(this.kryptonLabel8);
            this.Controls.Add(this.kcb_order);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Main";
            this.Text = "设置抽检规则";
            ((System.ComponentModel.ISupportInitialize)(this.kcb_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).EndInit();
            this.gb_box.ResumeLayout(false);
            this.gb_box.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckedListBox kckl_lot;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kcb_Level;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel ktl_total;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel7;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kbtn_del;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private System.Windows.Forms.TextBox kcb_order;
        private System.Windows.Forms.TextBox ktb_lotNo;
        private System.Windows.Forms.GroupBox gb_box;


    }
}

