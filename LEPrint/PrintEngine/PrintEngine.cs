using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using LabelManager2;

namespace NVBarcode
{
    public class PrintEngine6
    {
        private static PrintEngine6 bcEngine;
        private static LabelManager2.Application app;
        private LabelManager2.Document Idoc;
        private PrintEngine6()
        { ; }

        /// <summary>
        /// ��ȡ��ӡ����
        /// </summary>
        /// <param name="path">ģ��λ��</param>
        /// <returns></returns>
        public static PrintEngine6 GetPrintEngine(string path)
        {
            if (bcEngine != null)    //bcEngineΪ��ֻ�����������1����һ��������2�����ر�
            {
                bcEngine.Idoc = app.Documents.Open(@path, false);
                return bcEngine;
            }
            app = new Application();
            bcEngine = new PrintEngine6();
            bcEngine.Idoc = app.Documents.Open(@path, false);
            return bcEngine;
        }

        /// <summary>
        /// ��ӡ����
        /// </summary>
        /// <param name="values">Ҫ��ӡ������</param>
        public void printDocument(string values)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //���ǰ׺
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //�����׺
                Idoc.Variables.Item(1).Value = values;
                //Idoc.Save();
                Idoc.PrintDocument(1);
            }
        }

        public void printDocument(string values, int num)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //���ǰ׺
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //�����׺
                Idoc.Variables.Item(1).Value = values;
                //Idoc.Save();
                Idoc.PrintDocument(num);
            }
        }

        public void printDocument(int num)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //���ǰ׺
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //�����׺
                Idoc.PrintDocument(num);
            }
        }

        public void printDocument(string values, int inc, int num)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //���ǰ׺
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //�����׺
                Idoc.Variables.FormVariables.Item(1).Increment = inc;
                Idoc.Variables.Item(1).Value = values;
                //Idoc.Save();
                Idoc.PrintDocument(num);
            }
        }

        /// <summary>
        /// ��ӡ�������������
        /// </summary>
        /// <param name="values"></param>
        /// <param name="inc"></param>
        /// <param name="pref"></param>
        /// <param name="suf"></param>
        /// <param name="num"></param>
        /// <param name="labCopies"></param>
        /// <param name="faxType"></param>
        /// <param name="V_variable"></param>
        /// <returns></returns>
        public string printDocument(string values, int inc, string pref, string suf, int num, int labCopies, int faxType, List<NV_SNP.V_variable> V_variable)
        {
            if (Idoc != null)
            {
                try
                {
                    //Idoc.Variables.Counters.Item(1)
                    if (faxType == 1)
                    {
                        Idoc.Variables.FormVariables.Item("bc").Increment = inc;        //����
                        Idoc.Variables.FormVariables.Item("bc").Prefix = pref;          //ǰ׺
                        Idoc.Variables.FormVariables.Item("bc").Suffix = suf;           //��׺
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.FormVariables.Item("bc").Value = values;         //��ˮ������
                        //Idoc.Variables.FormVariables.Item(1).LabelCopies = labCopies;  //ÿ�ű�ǩ�ظ�����   //ֻ��CS7֮��֧��
                        for (int i = 0; i < V_variable.Count; i++)
                        {
                            Idoc.Variables.FormVariables.Item(V_variable[i].Text).Value = V_variable[i].value;
                        }
                    }
                    else
                    {
                        Idoc.Variables.Counters.Item("bc").Increment = inc;        //����
                        Idoc.Variables.Counters.Item("bc").Prefix = pref;          //ǰ׺
                        Idoc.Variables.Counters.Item("bc").Suffix = suf;           //��׺
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.Counters.Item("bc").Value = values;         //��ˮ������
                        //Idoc.Variables.Counters.Item(1).LabelCopies = labCopies;  //ÿ�ű�ǩ�ظ�����   //ֻ��CS7֮��֧��
                        for (int i = 0; i < V_variable.Count; i++)
                        {
                            Idoc.Variables.FormVariables.Item(V_variable[i].Text).Value = V_variable[i].value;
                        }
                    }
                    
                    Idoc.PrintDocument(num * labCopies);
                    return null;
                }
                catch (Exception ext)
                {
                    return ext.ToString();
                }
            }
            return null;
        }


        /// <summary>
        /// ��ӡ��һ��������ı�ǩ
        /// </summary>
        /// <param name="values">��ˮ����ʼ</param>
        /// <param name="inc">����</param>
        /// <param name="pref">ǰ׺</param>
        /// <param name="suf">��׺</param>
        /// <param name="num">��ӡ������ʵ������Ϊnum*labCopies</param>
        /// <param name="labCopies">ÿ�ű�ǩ�ظ����Σ�ֻ��CS7֮��֧��</param>
        /// <returns></returns>
        public string printDocument(string values, int inc, string pref, string suf, int num, int labCopies, int faxType)
        {
            if (Idoc != null)
            {
                try
                {
                    //Idoc.Variables.Counters.Item(1)
                    if (faxType == 1)
                    {
                        Idoc.Variables.FormVariables.Item("bc").Increment = inc;        //����
                        Idoc.Variables.FormVariables.Item("bc").Prefix = pref;          //ǰ׺
                        Idoc.Variables.FormVariables.Item("bc").Suffix = suf;           //��׺
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.FormVariables.Item("bc").Value = values;         //��ˮ������
                        //Idoc.Variables.FormVariables.Item(1).LabelCopies = labCopies;  //ÿ�ű�ǩ�ظ�����   //ֻ��CS7֮��֧��

                    }
                    else
                    {
                        Idoc.Variables.Counters.Item("bc").Increment = inc;        //����
                        Idoc.Variables.Counters.Item("bc").Prefix = pref;          //ǰ׺
                        Idoc.Variables.Counters.Item("bc").Suffix = suf;           //��׺
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.Counters.Item("bc").Value = values;         //��ˮ������
                        //Idoc.Variables.Counters.Item(1).LabelCopies = labCopies;  //ÿ�ű�ǩ�ظ�����   //ֻ��CS7֮��֧��
                    }
                    //Idoc.Save();
                    Idoc.PrintDocument(num * labCopies);
                    return null;
                }
                catch (Exception ext)
                {
                    return ext.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// ��ӡ������������ı�ǩ
        /// </summary>
        /// <param name="values">��ˮ����ʼ</param>
        /// <param name="inc">����</param>
        /// <param name="pref">ǰ׺</param>
        /// <param name="suf">��׺</param>
        /// <param name="num">��ӡ������ʵ������Ϊnum*labCopies</param>
        /// <param name="labCopies">ÿ�ű�ǩ�ظ�����</param>
        /// <returns></returns>
        public string printDocument(string value1, string value2, int inc, string pref, string suf, int num, int labCopies)
        {
            if (Idoc != null)
            {
                try
                {
                    Idoc.Variables.FormVariables.Item(1).Value = value2;
                    Idoc.Variables.FormVariables.Item(2).Increment = inc;        //����
                    Idoc.Variables.FormVariables.Item(2).Prefix = pref;          //ǰ׺
                    Idoc.Variables.FormVariables.Item(2).Suffix = suf;           //��׺
                    //Idoc.Variables.FormVariables.Item(1)._Value = values;
                    Idoc.Variables.FormVariables.Item(2).Value = value1;         //��ˮ������
                    //Idoc.Variables.FormVariables.Item(2).LabelCopies = labCopies;  //ÿ�ű�ǩ�ظ�����   //ֻ��CS7֮��֧��
                    //Idoc.Save();
                    Idoc.PrintDocument(num * labCopies);
                    return null;
                }
                catch (Exception ext)
                {
                    return ext.ToString();
                }
            }
            return null;
        }

        /// <summary>
        /// ��ӡ������������ı�ǩ
        /// </summary>
        /// <param name="values">��ˮ����ʼ</param>
        /// <param name="inc">����</param>
        /// <param name="pref">ǰ׺</param>
        /// <param name="suf">��׺</param>
        /// <param name="num">��ӡ������ʵ������Ϊnum*labCopies</param>
        /// <param name="labCopies">ÿ�ű�ǩ�ظ�����</param>
        /// <returns></returns>
        public string printDocument(string value, int inc, string pref, string suf, int num)
        {
            if (Idoc != null)
            {
                try
                {
                    Idoc.Variables.FormVariables.Item(1).Value = value;
                    Idoc.Variables.FormVariables.Item(2).Increment = inc;        //����
                    Idoc.Variables.FormVariables.Item(2).Prefix = pref;          //ǰ׺
                    Idoc.Variables.FormVariables.Item(2).Suffix = suf;           //��׺
                    //Idoc.Variables.FormVariables.Item(1)._Value = values;
                    //Idoc.Variables.FormVariables.Item(2).Value = value1;         //��ˮ������
                    //Idoc.Variables.FormVariables.Item(2).LabelCopies = labCopies;  //ÿ�ű�ǩ�ظ�����   //ֻ��CS7֮��֧��
                    //Idoc.Save();
                    Idoc.PrintDocument(num);
                    return null;
                }
                catch (Exception ext)
                {
                    return ext.ToString();
                }
            }
            return null;
        }


        /// <summary>
        /// ���ģ��Ĳ�������
        /// </summary>
        /// <returns>int</returns>
        public int getVariablesNum()
        {
            int i = 0;
            while (true)
            {
                if (Idoc.Variables.Item(i + 1).Name != "")
                {
                    i++;
                }
                else
                {
                    break;
                }
            }
            return i;
        }

        /// <summary>
        /// �رմ�ӡ����ķ�������Ϊ�ǵ���ģʽ�Ķ�������������ٹر�
        /// </summary>
        public static void closeApp()
        {
            try
            {
                if (app != null)
                {
                    bcEngine = null;
                    app.Quit();
                }
            }
            catch { }
        }
    }
}
