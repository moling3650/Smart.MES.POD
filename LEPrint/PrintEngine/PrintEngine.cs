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
        /// 获取打印对象
        /// </summary>
        /// <param name="path">模板位置</param>
        /// <returns></returns>
        public static PrintEngine6 GetPrintEngine(string path)
        {
            if (bcEngine != null)    //bcEngine为空只有两种情况，1：第一次启动，2：被关闭
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
        /// 打印条码
        /// </summary>
        /// <param name="values">要打印的内容</param>
        public void printDocument(string values)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //清除前缀
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //清除后缀
                Idoc.Variables.Item(1).Value = values;
                //Idoc.Save();
                Idoc.PrintDocument(1);
            }
        }

        public void printDocument(string values, int num)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //清除前缀
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //清除后缀
                Idoc.Variables.Item(1).Value = values;
                //Idoc.Save();
                Idoc.PrintDocument(num);
            }
        }

        public void printDocument(int num)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //清除前缀
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //清除后缀
                Idoc.PrintDocument(num);
            }
        }

        public void printDocument(string values, int inc, int num)
        {
            if (Idoc != null)
            {
                Idoc.Variables.FormVariables.Item(1).Prefix = "";          //清除前缀
                Idoc.Variables.FormVariables.Item(1).Suffix = "";          //清除后缀
                Idoc.Variables.FormVariables.Item(1).Increment = inc;
                Idoc.Variables.Item(1).Value = values;
                //Idoc.Save();
                Idoc.PrintDocument(num);
            }
        }

        /// <summary>
        /// 打印多个参数的条码
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
                        Idoc.Variables.FormVariables.Item("bc").Increment = inc;        //增量
                        Idoc.Variables.FormVariables.Item("bc").Prefix = pref;          //前缀
                        Idoc.Variables.FormVariables.Item("bc").Suffix = suf;           //后缀
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.FormVariables.Item("bc").Value = values;         //流水号主体
                        //Idoc.Variables.FormVariables.Item(1).LabelCopies = labCopies;  //每张标签重复几次   //只在CS7之后被支持
                        for (int i = 0; i < V_variable.Count; i++)
                        {
                            Idoc.Variables.FormVariables.Item(V_variable[i].Text).Value = V_variable[i].value;
                        }
                    }
                    else
                    {
                        Idoc.Variables.Counters.Item("bc").Increment = inc;        //增量
                        Idoc.Variables.Counters.Item("bc").Prefix = pref;          //前缀
                        Idoc.Variables.Counters.Item("bc").Suffix = suf;           //后缀
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.Counters.Item("bc").Value = values;         //流水号主体
                        //Idoc.Variables.Counters.Item(1).LabelCopies = labCopies;  //每张标签重复几次   //只在CS7之后被支持
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
        /// 打印有一个填充器的标签
        /// </summary>
        /// <param name="values">流水号起始</param>
        /// <param name="inc">增量</param>
        /// <param name="pref">前缀</param>
        /// <param name="suf">后缀</param>
        /// <param name="num">打印数量，实际数量为num*labCopies</param>
        /// <param name="labCopies">每张标签重复几次，只在CS7之后被支持</param>
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
                        Idoc.Variables.FormVariables.Item("bc").Increment = inc;        //增量
                        Idoc.Variables.FormVariables.Item("bc").Prefix = pref;          //前缀
                        Idoc.Variables.FormVariables.Item("bc").Suffix = suf;           //后缀
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.FormVariables.Item("bc").Value = values;         //流水号主体
                        //Idoc.Variables.FormVariables.Item(1).LabelCopies = labCopies;  //每张标签重复几次   //只在CS7之后被支持

                    }
                    else
                    {
                        Idoc.Variables.Counters.Item("bc").Increment = inc;        //增量
                        Idoc.Variables.Counters.Item("bc").Prefix = pref;          //前缀
                        Idoc.Variables.Counters.Item("bc").Suffix = suf;           //后缀
                        //Idoc.Variables.FormVariables.Item(1)._Value = values;
                        Idoc.Variables.Counters.Item("bc").Value = values;         //流水号主体
                        //Idoc.Variables.Counters.Item(1).LabelCopies = labCopies;  //每张标签重复几次   //只在CS7之后被支持
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
        /// 打印有两个填充器的标签
        /// </summary>
        /// <param name="values">流水号起始</param>
        /// <param name="inc">增量</param>
        /// <param name="pref">前缀</param>
        /// <param name="suf">后缀</param>
        /// <param name="num">打印数量，实际数量为num*labCopies</param>
        /// <param name="labCopies">每张标签重复几次</param>
        /// <returns></returns>
        public string printDocument(string value1, string value2, int inc, string pref, string suf, int num, int labCopies)
        {
            if (Idoc != null)
            {
                try
                {
                    Idoc.Variables.FormVariables.Item(1).Value = value2;
                    Idoc.Variables.FormVariables.Item(2).Increment = inc;        //增量
                    Idoc.Variables.FormVariables.Item(2).Prefix = pref;          //前缀
                    Idoc.Variables.FormVariables.Item(2).Suffix = suf;           //后缀
                    //Idoc.Variables.FormVariables.Item(1)._Value = values;
                    Idoc.Variables.FormVariables.Item(2).Value = value1;         //流水号主体
                    //Idoc.Variables.FormVariables.Item(2).LabelCopies = labCopies;  //每张标签重复几次   //只在CS7之后被支持
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
        /// 打印有两个填充器的标签
        /// </summary>
        /// <param name="values">流水号起始</param>
        /// <param name="inc">增量</param>
        /// <param name="pref">前缀</param>
        /// <param name="suf">后缀</param>
        /// <param name="num">打印数量，实际数量为num*labCopies</param>
        /// <param name="labCopies">每张标签重复几次</param>
        /// <returns></returns>
        public string printDocument(string value, int inc, string pref, string suf, int num)
        {
            if (Idoc != null)
            {
                try
                {
                    Idoc.Variables.FormVariables.Item(1).Value = value;
                    Idoc.Variables.FormVariables.Item(2).Increment = inc;        //增量
                    Idoc.Variables.FormVariables.Item(2).Prefix = pref;          //前缀
                    Idoc.Variables.FormVariables.Item(2).Suffix = suf;           //后缀
                    //Idoc.Variables.FormVariables.Item(1)._Value = values;
                    //Idoc.Variables.FormVariables.Item(2).Value = value1;         //流水号主体
                    //Idoc.Variables.FormVariables.Item(2).LabelCopies = labCopies;  //每张标签重复几次   //只在CS7之后被支持
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
        /// 获得模板的参数数量
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
        /// 关闭打印对象的方法，因为是单件模式的对象，所以请最后再关闭
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
