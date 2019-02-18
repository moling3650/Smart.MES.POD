using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LEMES_POD.CustomControl
{
    public partial class SelectUser : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        private ControlType selCont;
        private List<string> selList;
        public string  Value { get; set; }
        public SelectUser(ControlType control, List<string> list)
        {
            Value = "";
            selCont = control;
            selList = list;
            InitializeComponent();
            DataBind();
        }

        void DataBind()
        {
            Control[] controls;
            switch (selCont)
            {
                case ControlType.Check:
                    controls = CheckBind();
                    break;
                case ControlType.Radio:
                    controls = RadioBind();
                    break;
                default:
                    controls = null;
                    break;
            }
            Flp_CheckButton.Controls.AddRange(controls);
           
        }
        
        CheckBox[] CheckBind()
        {
            CheckBox[] check=new CheckBox[selList.Count];
            for (int i = 0; i < selList.Count; i++)
			{
			  CheckBox box = new CheckBox();
              box.AutoSize = true;
              box.Text=selList[i];
              check[i] = box; 
			}
            return check;
        }

        RadioButton[] RadioBind()
        {
            RadioButton[] radio = new RadioButton[selList.Count];
            for (int i = 0; i < selList.Count; i++)
            {
                RadioButton box = new RadioButton();
                box.AutoSize = true;
                box.Text = selList[i];
                radio[i] = box;
            }
            return radio;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (Control item in Flp_CheckButton.Controls)
            {
                if (item is CheckBox)
                {
                    CheckBox box = (CheckBox)item;
                    if (box.Checked)
                    {
                        Value += box.Text+",";
                    }
                }
                else if (item is RadioButton)
                {
                    RadioButton box = (RadioButton)item;
                    if (box.Checked)
                    {
                        Value = box.Text;
                        this.Close();
                        return;
                    }
                }
            }
            if (Value.Length>0)
            {
                Value = Value.Substring(0, Value.Length - 1);
            }
    
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Value = "";
            this.Close();
        }

        
    }
    public enum ControlType
    { 
         Check=1,
         Radio=0
         
    }
}
