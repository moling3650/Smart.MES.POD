using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace NV_SNP.Objs
{
    class Order
    {
        public string OrderNO;
        public string CustonerID;
        public string ModuleID;
        public int Qty;
        public int Completed;
        public string InputTime;
        public string FinishTime;
        public int State;

        /// <summary>
        /// 获取指定客户的在制工单
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static DataTable GetOnLineOrdersByCustomer(string cid)
        {
            string sql = "select a.orderNO,b.customerName,c.moduleName,a.qty,a.completed,a.inputTime,'在制' state from orderList a left join customerList b on a.customerID=b.customerID left join moduleList c on a.moduleID=c.moduleID "+
                         "where a.customerID='"+cid+"' and a.state=0 order by a.inputTime desc";

            return DB.Database.getDataTable(sql);
        }

        /// <summary>
        /// 获取指定工单号的工单
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static DataTable GetOrderByNO(string orderNO)
        {
            string sql = "select a.orderNO,a.moduleID,c.moduleName,c.ruleStr,a.qty,a.completed from orderList a left join moduleList c on a.moduleID=c.moduleID " +
                         "where a.orderNO='" + orderNO + "'";

            return DB.Database.getDataTable(sql);
        }

        /// <summary>
        /// 获取指定工单的客户编号
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public static string GetCustomerByOrder(string orderNO)
        {
            string sql = "select customerID from orderList where orderNO='" + orderNO + "'";
            DataTable dt = DB.Database.getDataTable(sql);
            if (dt.Rows.Count < 1)
                return "";
            return dt.Rows[0]["customerID"].ToString();
        }

        /// <summary>
        /// 新增工单
        /// </summary>
        /// <param name="ord"></param>
        /// <returns></returns>
        public static int AddOrder(Order ord)
        {
            string sql = "insert into orderList values ('" + ord.OrderNO + "','" + ord.CustonerID + "','" + ord.ModuleID + "'," + ord.Qty + "," + ord.Completed + ",'" + ord.InputTime + "','" + ord.FinishTime + "'," + 0 + ")";
            return DB.Database.RunNoneQuery(sql);
        }


        /// <summary>
        /// 修改工单完成数的方法
        /// </summary>
        /// <param name="orderNO"></param>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int ComleteOrder(string orderNO, int num)
        {
            string sql = "update orderList set completed=completed+" + num + " where orderNO='" + orderNO + "'";
            return DB.Database.RunNoneQuery(sql);
        }


        /// <summary>
        /// 工单结案
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public static int FinishOrder(string orderNO)
        {
            string sql = "update orderList set state=1 where orderNO='" + orderNO + "'; delete onlineCodes where orderNO='"+orderNO+"'";
            return DB.Database.RunNoneQuery(sql);
        }

        /// <summary>
        /// 删除新工单
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public static int DeleteOrder(string orderNO)
        {
            string sql = "delete orderList where orderNO='" + orderNO + "'";
            return DB.Database.RunNoneQuery(sql);
        }
        
        /// <summary>
        /// 检查工单是否可以被删除
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public static bool CanBeDelete(string orderNO)
        {
            DataTable dt = Order.GetOrderByNO(orderNO);
            if (dt.Rows.Count>0 & dt.Rows[0]["completed"].ToString()=="0")
            {
                return true;
            }
            return false;
        }
    }
}
