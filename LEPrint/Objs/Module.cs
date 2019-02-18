using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace NV_SNP.Objs
{
    class Module
    {
        public string ModuleID;
        public string ModuleName;
        public string customerID;
        public string RuleStr;
        public string Inputdate;
        public int CurrentSN;
        public int MaxSN;
        public string SetZero;
        public string ZeroDate;
        public string modulePath;
        public int faxType;    //填充方式
        public int CheckCode;

        /// <summary>
        /// 获取指定式样对象
        /// </summary>
        /// <param name="mid">式样ID</param>
        /// <returns></returns>
        public static Module GetModule(string mid)
        {
            string sql = "select * from moduleList where moduleID='" + mid + "'";
            DataTable dt= DB.Database.getDataTable(sql);
            if (dt.Rows.Count < 1)
            {
                return null;
            }
            Module module = new Module();
            module.ModuleID = dt.Rows[0]["moduleID"].ToString();
            module.ModuleName = dt.Rows[0]["moduleName"].ToString();
            module.customerID = dt.Rows[0]["customerID"].ToString();
            module.RuleStr = dt.Rows[0]["RuleStr"].ToString();
            module.Inputdate = dt.Rows[0]["inputDate"].ToString();
            module.CurrentSN = int.Parse(dt.Rows[0]["currentSN"].ToString());
            module.MaxSN = int.Parse(dt.Rows[0]["maxSN"].ToString());
            module.SetZero = dt.Rows[0]["setZero"].ToString();
            module.ZeroDate = dt.Rows[0]["zeroDate"].ToString();
            module.modulePath = dt.Rows[0]["modulePath"].ToString();
            module.faxType = int.Parse(dt.Rows[0]["faxType"].ToString());
            module.CheckCode = int.Parse(dt.Rows[0]["checkCode"].ToString());
            return module;
        }

        /// <summary>
        /// 获取指定客户的式样表
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static DataTable GetModuleByCustomer(string cid)
        {
            string sql = "select * from moduleList where customerID='" + cid + "'";
            return DB.Database.getDataTable(sql);
        }

        /// <summary>
        /// 新增式样
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        public static int AddModule(Module mdl)
        {
            string sql = "insert into moduleList values('" + mdl.ModuleID + "','" + mdl.ModuleName + "','" + mdl.customerID + "','" + mdl.RuleStr + "','" + mdl.Inputdate + "'," + mdl.CurrentSN + "," + mdl.MaxSN + ",'" + mdl.SetZero + "','" + mdl.ZeroDate+ "','"+mdl.modulePath+"',"+mdl.faxType+","+mdl.CheckCode+")";
            return DB.Database.RunNoneQuery(sql);
        }

        public static int UpdateModule(Module mdl)
        {
            string sql = "update moduleList set moduleName='" + mdl.ModuleName + "',customerID='" + mdl.customerID + "',ruleStr='" + mdl.RuleStr + "',inputdate='" + mdl.Inputdate + "',currentsn=" + mdl.CurrentSN + ",maxSN=" + mdl.MaxSN + ",setZero='" + mdl.SetZero + "',modulePath='"+mdl.modulePath+"',faxType="+mdl.faxType+",checkCode="+mdl.CheckCode+"  where moduleID='" + mdl.ModuleID + "'";
            return DB.Database.RunNoneQuery(sql);
        }

        public static int DeleteModule(string mid)
        {
            string sql = "delete moduleList where moduleID='"+mid+"'";
            return DB.Database.RunNoneQuery(sql);
        }

        public static int UpdateCurrent(string mid, int num)
        {
            string sql = "update moduleList set currentSN=" + num + " where moduleID='"+mid+"'";
            return DB.Database.RunNoneQuery(sql);
        }
        
        /// <summary>
        /// 归零
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static int SetModuleZero(string mid)
        {
            string sql = "update moduleList set currentSN=0,zeroDate='" + DateTime.Now.ToShortDateString() + "' where moduleID='" + mid + "'";
            return DB.Database.RunNoneQuery(sql);
        }
        
        /// <summary>
        /// 判断模板是否可以归零的方法
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public static bool CanBeSetZero(string mid)
        {
            Module mdl = Module.GetModule(mid);
            if (mdl.SetZero == "最大值")
            {
                if (mdl.CurrentSN == mdl.MaxSN)
                    return true;

                return false;
            }
            else if(mdl.SetZero=="每月")
            {
                DateTime time = DateTime.Parse(mdl.ZeroDate);
                int zeroMonth = time.Month;
                int thisMonth = DateTime.Now.Month;
                if (zeroMonth != thisMonth)
                    return true;
                return false;
            }
            //如果不在定义范围内，则可随时归零
            return true;    
        }
    }
}
