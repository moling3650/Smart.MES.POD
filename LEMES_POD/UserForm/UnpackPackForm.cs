using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace LEMES_POD.UserForm
{
    public partial class UnpackPackForm : KryptonForm
    {
        private List<LEDAO.P_Material_WIP> _wip;
        private List<LEDAO.V_WIPRank> _wipRank;
        public string lot_no;
        public UnpackPackForm(List<LEDAO.P_Material_WIP> wip)
        {

            _wip = wip;
            InitializeComponent();
        }

        private void UnpackPackForm_Load(object sender, EventArgs e)
        {

        }
        private LEDAO.P_Material_WIP wip;
        private void txtSelPC_KeyPress(object sender, KeyPressEventArgs e)
        {
            string lotno = txtSelPC.Text;
            if (e.KeyChar==(char)13)
            {
               var var_wip= _wip.Where(c => c.lot_no == lotno).ToList();
               if (var_wip.Count>0)
               {
                   wip = var_wip[0];
                   txtPC.Text = var_wip[0].lot_no;
                   txtGD.Text = var_wip[0].order_no;
                   txtSL.Text = var_wip[0].lot_qty.Value.ToString("f2");
               }
               else
               {
                   Component.Tool.DisplayResult(textBox3, false, "没有该批次");
               }

            }
        }

        private int i=1;
        private void btnCF_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtPC.Text))
            {

                _wipRank = JsonConvert.DeserializeObject<List<LEDAO.V_WIPRank>>(BLL.ServiceReference.DISObject("BLL.WIP", "GetWIPDisminPack", txtPC.Text));
               {
                   if (_wipRank.Count > 0)
                   {
                       i = _wipRank.Max(c => c.number).Value;
                       i++;
                   }
               }


               txtCBPC.Text = txtPC.Text + "_"+i.ToString();
               txtCBSL.Enabled = true;
               txtCBSL.Focus();

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            decimal a=0.00M;
            if (decimal.TryParse(txtCBSL.Text,out a))
            {
                if (a>0)
                {
                    if (a>Convert.ToDecimal(txtSL.Text))
                    {
                        MessageBox.Show("拆分数量大于总数量");
                        return;
                    }
                    LEDAO.V_WIPRank rank = new LEDAO.V_WIPRank();
                    rank.child_qty = Convert.ToDecimal(txtCBSL.Text);
                    rank.child_sfc = txtCBPC.Text;
                    if (_wipRank != null)
                    {
                        if (_wipRank.Count > 0)
                        {
                            rank.main_sfc = _wipRank[0].main_sfc;
                        }
                        else
                        {
                            rank.main_sfc = wip.lot_no;
                        }
                    }
                    else
                    {
                        rank.main_sfc = wip.lot_no;
                    }
                    rank.mat_code = wip.mat_code;
                    rank.number = i;
                    rank.order_no = wip.order_no;
                    rank.parent_sfc = wip.lot_no;
                    rank.parent_station = wip.parent_station;
                    rank.parent_qty = wip.lot_qty;
                    rank.Parent_order = wip.Parent_order;
                    if (string.IsNullOrWhiteSpace(wip.Parent_order))
                    {
                        rank.lot_type = 0;
                    }
                    else
                    {
                        rank.lot_type = 1;
                    }
                    string json = JsonConvert.SerializeObject(rank);
                    ILE.IResult  result= BLL.ServiceReference.DISResult("BLL.WIP", "WIPDisminSubmit", json);
                    if (result.Result)
                    {
                        this.lot_no = txtCBPC.Text;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show(result.ExtMessage);
                    }
                   
                     
                }
                else
                {
                    MessageBox.Show("拆包数量不可能小于0");
                }
            }
            else
            {
                MessageBox.Show("拆包数量不是有效数字");
            }
        }
    }
}
