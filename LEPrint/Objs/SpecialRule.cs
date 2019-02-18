using System;
using System.Collections.Generic;

using System.Text;
using System.Data;

namespace NV_SNP.Objs
{
    class SpecialRule
    {
        public string speName;
        public string type;
        public string originalStr;
        public string newStr;

        //public static DataTable Get_Y_SpecialRule(string ruleName)
        //{
        //    string sql = "select * from SpecialList where type='年' and speName='"+ruleName+"'";
        //    return DB.Database.getDataTable(sql);
        //}


        /// <summary>
        /// 获取所有特殊年
        /// </summary>
        /// <returns></returns>
        //public static DataTable Get_Y_SpecialName()
        //{
        //    string sql = "select speName from SpecialList where type='年' group by speName";
        //    return DB.Database.getDataTable(sql);
        //}

        //========
        //public static DataTable Get_M_SpecialRule(string ruleName)
        //{
        //    string sql = "select * from SpecialList where type='月' and speName='"+ruleName+"'";
        //    return DB.Database.getDataTable(sql);
        //}

        /// <summary>
        /// 获取所有特殊月
        /// </summary>
        /// <returns></returns>
        //public static DataTable Get_M_SpecialName()
        //{
        //    string sql = "select speName from SpecialList where type='月' group by speName";
        //    return DB.Database.getDataTable(sql);
        //}

        //=============
        //public static DataTable Get_D_SpecialRule(string ruleName)
        //{
        //    string sql = "select * from SpecialList where type='日' and speName='"+ruleName+"'";
        //    return DB.Database.getDataTable(sql);
        //}


        /// <summary>
        /// 获取所有特殊日
        /// </summary>
        /// <returns></returns>
        //public static DataTable Get_D_SpecialName()
        //{
        //    string sql = "select speName from SpecialList where type='日' group by speName";
        //    return DB.Database.getDataTable(sql);
        //}

        //public static int AddSpecia(SpecialRule speRule)
        //{
        //    string sql = "insert into specialList values('" + speRule.speName + "','" + speRule.type + "','" + speRule.originalStr + "','" + speRule.newStr + "')";
        //    return DB.Database.RunNoneQuery(sql);
        //}

        //public static int DeleteSpe(int id)
        //{
        //    string sql = "delete  specialList where id=" + id + "";
        //    return DB.Database.RunNoneQuery(sql);
        //}

        /// <summary>
        /// 将带有特殊规则的规则串转换为一般的规则串
        /// </summary>
        /// <returns></returns>
        public static NVBarcode.CodeRule TransformRuleStr(string rule)
        {
            if (rule.IndexOf("{") < 0)
            {
                return new NVBarcode.CodeRule(rule);
            }


            string bfs = "";  //常量的缓存
            string bfd = "";  //带规则的缓存
            for (int i = 0; i < rule.Length; i++)
            {
                if (bfd != "")  //如果当前是在处理规则
                {
                    if (rule[i] == '}')
                    {
                        //string s = SpeToStr(bfd + "}");
                       // bfs += s;
                        bfd = "";
                    }
                    else
                    {
                        bfd += rule[i].ToString();
                    }
                }
                else
                {
                    if (rule[i] == '{')
                    {
                        bfd += "{";
                    }
                    else
                    {
                        bfs += rule[i].ToString();
                    }
                }
            }
            return new NVBarcode.CodeRule(bfs);
        }

        //private static string SpeToStr(string rule)
        //{
        //    DateTime tm = DateTime.Now;
        //    string speName = "";
        //    speName = rule.Substring(3, rule.Length - 4);
        //    if (rule[1] == 'Y')         //此处分开方便日后升级
        //    {
        //        //[Y:yyyy]
        //        return GetSpeByFormat(speName, "年", tm.Year.ToString());
        //    }
        //    else if (rule[1] == 'M')
        //    {
        //        return GetSpeByFormat(speName, "月", tm.Month.ToString());
        //    }
        //    else if (rule[1] == 'D')
        //    {
        //        return GetSpeByFormat(speName, "日", tm.Day.ToString());
        //    }
        //    return "";
        //}


        /// <summary>
        /// 通过特殊规则名称，类型，原始值，获取对应的特殊值
        /// </summary>
        /// <param name="speName"></param>
        /// <param name="type"></param>
        /// <param name="originalStr"></param>
        /// <returns></returns>
        //private static string GetSpeByFormat(string speName,string type, string originalStr)
        //{
        //    string sql = "select newStr from SpecialList where speName='" + speName + "' and type='" + type + "' and originalStr='" + originalStr + "'";
        //    DataTable dt = DB.Database.getDataTable(sql);
        //    if (dt.Rows.Count < 1)
        //        return null;
        //    return dt.Rows[0][0].ToString();
        //}
    }
}
