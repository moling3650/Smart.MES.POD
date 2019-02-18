using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using LEDAO;
using NVBarcode.Serial;
using System.Xml;
using Newtonsoft.Json;
using JsonTools;
namespace NV_SNP
{
    public partial class PrintVariable : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        string var = string.Empty;
        string Main_Od = string.Empty;
        Form form;
        List<P_SSW_TemplateList> proc1;
        public PrintVariable(string variable,string Main_order,Form fm,List<P_SSW_TemplateList> list)
        {
            proc1 = list;
            form = fm;
            var = variable;
            Main_Od = Main_order;
            InitializeComponent();
        }

        private void PrintVariable_Load(object sender, EventArgs e)
        {
            kryptonLabel1.Text = var+":";
        }
  
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            string classes = this.kryptonTextBox1.Text.ToString().Trim();
            if (form.Name == "PrintAll")
            {
                (new PrintAll(Main_Od, classes, form)).ShowDialog();
            }
            if (form.Name == "RePrint")
            {
                this.Close();
                PrintEngine.Reprint.Print(var, "", form, proc1, classes);
            }
            this.Close();
        }
    }
}