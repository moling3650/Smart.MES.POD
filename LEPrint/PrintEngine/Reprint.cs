using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LEDAO;
using NVBarcode.Serial;
using NVBarcode;
using Newtonsoft.Json;
using JsonTools;
using System.Xml;
using ComponentFactory.Krypton.Toolkit;
using NV_SNP;


namespace PrintEngine
{
    class Reprint
    {
        static NVBarcode.PrintEngine6 pn;
        static NVBarcode.BarCode barcode;
        static SerialManager sm = null;
        public static void Print(string variable, string Main_order, Form fm, List<P_SSW_TemplateList> list, string clas)
        {
            string var = variable;
            string Main_Od = Main_order;
            Form form = fm;
            List<P_SSW_TemplateList> proc1 = list;

            KryptonComboBox com = new KryptonComboBox();
            com = (KryptonComboBox)form.Controls.Find("kryptonComboBox1", true)[0];
            KryptonTextBox tb = new KryptonTextBox();
            tb = (KryptonTextBox)form.Controls.Find("ktb_orderNO", true)[0];
            TextBox tb_1 = new TextBox();
            tb_1 = (TextBox)form.Controls.Find("tb_1", true)[0];
            TextBox tb_2 = new TextBox();
            tb_2 = (TextBox)form.Controls.Find("tb_2", true)[0];
            KryptonLabel lb = new KryptonLabel();
            lb = (KryptonLabel)form.Controls.Find("klb_barcode", true)[0];

            pn = NVBarcode.PrintEngine6.GetPrintEngine(proc1[0].TemplatePath);
            string Mid = proc1[0].RuleStr.ToString();
            if (Mid.IndexOf("(") > -1)
            {
                PrintVari pv = new PrintVari(Mid, "", "");
                pv.ShowDialog();
                Mid = pv.result;
            }
            NVBarcode.CodeRule coderule = NV_SNP.Objs.SpecialRule.TransformRuleStr(Mid);   //将特殊规则转成一般规则
            barcode = coderule.GetCodeByRule();
            lb.Text = barcode.prefix + barcode.snStr + barcode.suffix;
            if (com.SelectedIndex > 0)   //如果打印进制不是10进制
            {
                string entName = com.SelectedValue.ToString();
                sm = GetSM(entName);   //获取一个序列号管理对象
            }
            for (int it = int.Parse(tb_1.Text); it <= int.Parse(tb_2.Text); it++)
            {
                string sn = NVBarcode.CodeRule.GetCompleteSN(GetSNByEnter(sm, it), barcode.snStr.Length);  //获取完整的SN
                string suffix = "";
                if (proc1[0].checkCode == 1)
                {
                    barcode.sn = GetSNByEnter(sm, it);
                    suffix = barcode.suffix + NVBarcode.CheckCode.ModCheckCode(barcode);
                }
                else
                    suffix = barcode.suffix;
                pn.printDocument(sn, 1, barcode.prefix, suffix, 1, 1, int.Parse(proc1[0].faxType.ToString()));
                string order_No = tb.Text.ToString();
                string workinfo = NV_SNP.Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetWorder", order_No);
                List<P_WorkOrder> proc2 = JsonConvert.DeserializeObject<List<P_WorkOrder>>(workinfo);
                //补打是重复的条码不重复插入清单
                string product_code = proc2[0].product_code;
                string ProductCode = NV_SNP.Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetProduct", product_code);
                List<B_Product> ProductCodeList = JsonConvert.DeserializeObject<List<B_Product>>(ProductCode);
                if (ProductCodeList[0].print_bind == 1)
                {
                    string Bar = barcode.prefix + sn + suffix;
                    string BarCode = NV_SNP.Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "GetBarCode", Bar);
                    List<P_WorkOrder> BarCodeList = JsonConvert.DeserializeObject<List<P_WorkOrder>>(BarCode);
                    if (BarCodeList == null)
                    {
                        //打印一个条码，在条码绑定表插入一条数据
                        P_BarCodeBing P_barcode = new P_BarCodeBing()
                        {
                            order = order_No,
                            parent_order = proc2[0].parent_order,
                            main_order = proc2[0].main_order,
                            state = 0,
                            barcode = Bar,
                            product_code = proc2[0].product_code,
                            InputTime = DateTime.Now
                        };
                        string strJson = JsonToolsNet.ObjectToJson(P_barcode);
                        NV_SNP.Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "InsertBarCode", strJson);
                    }
                }
                //string MID=proc1[0].Template_id;
                //if (Convert.ToInt32(tb_2.Text) > proc1[0].currentSN)
                //{
                //    int CSN = Convert.ToInt32(tb_2.Text);
                //    NV_SNP.Toos.ServiceReferenceManager.GetClient().RunServerAPI("BLL.SSW", "UpdateTemplateByMid", MID + "|" + CSN);
                //}
            }
        }
        private static SerialManager GetSM(string EntName)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("EntersConfig.xml");
            XmlNode node = xd.SelectSingleNode("Enters");
            if (node.ChildNodes.Count == 0)  //如果没有找到下属节点，说明配置错误
                return null;
            XmlNodeList nl = node.ChildNodes;
            SerialManager sm = null;
            foreach (XmlNode nod in nl)
            {
                if (nod.Name == EntName)
                {
                    SerialManager.Enters ent = (SerialManager.Enters)System.Enum.Parse(typeof(SerialManager.Enters), EntName);
                    string val = ((XmlElement)nod).GetAttribute("value");
                    sm = new SerialManager(ent, val);
                    return sm;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取对应进制下的序列号
        /// </summary>
        /// <returns></returns>
        private static string GetSNByEnter(SerialManager sm, int sn)
        {
            if (sm == null)
                return sn.ToString();
            return sm.GetEnterString(sn);

        }
    }
}
