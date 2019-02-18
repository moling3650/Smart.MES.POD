using System;
using System.Collections.Generic;

using System.Text;
using System.Data;

namespace NV_SNP.Objs
{
    class Customer
    {

        /// <summary>
        /// 获取所有客户
        /// </summary>
        /// <returns></returns>
        public static DataTable GetAllCustomer()
        {
            string sql = "select * from Customerlist";
            return DB.Database.getDataTable(sql);
        }
    }
}
