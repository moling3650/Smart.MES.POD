using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace NVBarcode
{
    public class CodeRule:IRule
    {
        public string RuleStr { get; set; }

        /// <summary>
        /// 实例化规则对象
        /// </summary>
        /// <param name="rulestr"></param>
        public  CodeRule(string rulestr)
        {
            if (CheckRuleStr(rulestr))
            {
                this.RuleStr = rulestr;   //只有传入的规则串检查通过，才会被保存到对象
            }
        }

        //
        public static string GetCompleteSN(string num,int len)
        {
            string[] ss = { "", "0", "00", "000", "0000", "00000", "000000", "0000000", "00000000", "000000000", "0000000000", "00000000000", "000000000000" };
            int i = len - num.Length;
            return ss[i] + num;
        }

        public BarCode GetCodeByRule()
        {
            
            BarCode bc=new BarCode();

            int snIdx = this.RuleStr.IndexOf("[SN:");
            int pfx;        //前缀开始的下标
            int sfxIdx=-1;     //后缀开始的下标

            if(snIdx>-1)    //如果有流水号
            {
                //=================前缀=========================
                if (snIdx == 0) //如果SN的开头是第一个字符
                {
                    pfx = -1;   //没有前缀
                }
                else
                {
                    pfx = 0;    //有前缀
                }

                //===================后缀==========================
                for (int i = snIdx + 1; i < RuleStr.Length; i++)
                {
                    if (RuleStr[i] == ']')
                    {
                        if (i == RuleStr.Length - 1)  //如果SN的结尾是最后一个字符
                        {
                            sfxIdx = -1;   //没有后缀
                        }
                        else
                        {
                            sfxIdx = i + 1;
                        }
                        break;  //只要找到]就结束
                    }
                }
            }
            else          //如果没有流水号
            {
                //则只有前缀，没有后缀
                pfx=0;
                sfxIdx=-1;
            }
            
            if (pfx > -1)
            {
                int idx = snIdx;
                if (snIdx == -1)       //如果有前缀没SN，前缀就是这个字符串的全部内容
                {
                    idx = RuleStr.Length;
                }
                
                string buffs = "";  //常量的缓存
                string buffd = "";  //带规则的缓存
                for (int i = pfx; i < idx; i++)
                {
                    if (buffd != "")  //如果当前是在处理规则
                    {
                        if (RuleStr[i] == ']')
                        {
                            string s = GetStrByDateRule(buffd + "]");
                            buffs += s;
                            buffd = "";
                        }
                        else
                        {
                            buffd += RuleStr[i].ToString();
                        }
                    }
                    else
                    {
                        if (RuleStr[i] == '[')
                        {
                            buffd += "[";
                        }
                        else
                        {
                            buffs += RuleStr[i].ToString();
                        }
                    }
                }
                bc.prefix = buffs;   //得到条码的前缀;
                
            }

            if (snIdx != -1)
            { 
                int endidx=0;
                for(int j=snIdx+4;j<RuleStr.Length;j++)
                {
                    if(RuleStr[j]==']')
                    {
                        endidx=j;
                        break;
                    }
                }
                //[sn:01]
                bc.sn = RuleStr.Substring(snIdx + 4, endidx - snIdx - 4);

                foreach (char c in bc.sn)
                {
                    bc.snStr += "*";
                }
            }

            if (sfxIdx > -1)
            {
                string bfs = "";  //常量的缓存
                string bfd = "";  //带规则的缓存
                for (int i = sfxIdx; i < RuleStr.Length; i++)
                {
                    if (bfd != "")  //如果当前是在处理规则
                    {
                        if (RuleStr[i] == ']')
                        {
                            string s = GetStrByDateRule(bfd + "]");
                            bfs += s;
                            bfd = "";
                        }
                        else
                        {
                            bfd += RuleStr[i].ToString();
                        }
                    }
                    else
                    {
                        if (RuleStr[i] == '[')
                        {
                            bfd += "[";
                        }
                        else
                        {
                            bfs += RuleStr[i].ToString();
                        }
                    }
                }
                bc.suffix = bfs;   //得到条码的前缀;
            }

            return bc;
        }

        /// <summary>
        /// 获取指定规则的对应字符，只接受带[]的日期规则
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public string GetStrByDateRule(string rule)
        {
            DateTime tm=DateTime.Now;
            string format="";
            format = rule.Substring(3, rule.Length - 4);
            if (rule[1] == 'Y')         //此处分开方便日后升级
            { 
                //[Y:yyyy]
                return tm.ToString(format);
            }
            else if (rule[1] == 'M')
            {
                return tm.ToString(format);
            }
            else if (rule[1] == 'D')
            {
                return tm.ToString(format);
            }
            return "";
        }

        /// <summary>
        /// 判断RULE串是否成立的方法，只有此方法成立才能创建规则对象，一旦创建则表示规则创没有问题
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static bool CheckRuleStr(string str)
        {
            string buff="";
           
            //===========第一步，成对检查===============
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '[' | str[i] == ']' | str[i] == '{' | str[i] == '}')
                    buff += str[i].ToString();
            }

            for (int j = 0; j < buff.Length; j++)  
            {
                if (j % 2 == 1)  //检查双数位是否为闭合
                {
                    if (buff[j] == '[' | buff[j] == '{')
                    {
                        return false;
                    }
                    if (buff[j - 1] == ']' | buff[j - 1] == '}') //检查单数位是否为开口
                    {
                        return false;
                    }
                }
            }
           //=========================================
                return true;
        }
    }
}
