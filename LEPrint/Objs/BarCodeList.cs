using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using DB;

namespace NV_SNP.Objs
{
    class BarCodeList
    {
        /// <summary>
        /// 添加完成记录
        /// </summary>
        /// <returns></returns>
        public static int AddBarCodeList(string user, string orderNo, string barCode, DateTime time)
        {
            string sql = "insert into BarCodeList values "
                + "('"+user+"','"+orderNo+"','"+barCode+"','"+time.ToString()+"')";
      
            return Database.RunNoneQuery(sql);
        }

        /// <summary>
        /// 添加NG记录
        /// </summary>
        public static int AddNGList(string orderNo, string barCode, string ngtype, DateTime time)
        {
            string sql = "insert into NGList values ('" + orderNo + "','" + barCode + "','" + ngtype + "','" + time.ToString() + "')";
            
            return Database.RunNoneQuery(sql);
        }
    }
}
