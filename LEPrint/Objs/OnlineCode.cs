using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace NV_SNP.Objs
{
    class OnlineCode
    {
        public static void InsertOnLineCodes(string orderNO,NVBarcode.BarCode barcode,int current,int num)
        {
            string[] ss = { "", "0", "00", "000", "0000", "00000","000000","0000000","00000000"};
            for(int i=1;i<=num-current;i++)
            {
                int crt = current + i;
                string sn = ss[barcode.snStr.Length - crt.ToString().Length] + crt.ToString();
                //写入onLineCodes表
                AddCode(orderNO,barcode.prefix+sn+barcode.suffix);
            }
        }

        public static void InsertOnLineCodes(string orderNO, NVBarcode.BarCode barcode, int current, int num,int checkCode)
        {
            string[] ss = { "", "0", "00", "000", "0000", "00000", "000000", "0000000", "00000000" };
            for (int i = 1; i <= num - current; i++)
            {
                int crt = current + i;
                string sn = ss[barcode.snStr.Length - crt.ToString().Length] + crt.ToString();
                barcode.sn=sn;
                string suffix=barcode.suffix + NVBarcode.CheckCode.ModCheckCode(barcode);
                //写入onLineCodes表
                AddCode(orderNO, barcode.prefix + sn + suffix);
            }
        }

        public static int AddCode(string orderNO,string code)
        {
            string sql="insert into onlineCodes values('"+orderNO+"','"+code+"',0,'')";
            return DB.Database.RunNoneQuery(sql);
        }

        /// <summary>
        /// 免检
        /// </summary>
        /// <param name="orderNO"></param>
        /// <returns></returns>
        public static int PassAllOnLineCodes(string odNO)
        {
            DataTable dt = DB.Database.getDataTable("select * from onlineCodes where orderNO='" + odNO + "' and state=0");

            string customer = Objs.Order.GetCustomerByOrder(odNO);

            DateTime time = DateTime.Now;
            foreach (DataRow row in dt.Rows)
            {
                try
                {
                    Objs.BarCodeList.AddBarCodeList(customer, odNO, row["barcode"].ToString(), time.AddSeconds(1));
                }
                catch
                {
                    Objs.BarCodeList.AddNGList(odNO, row["barcode"].ToString(), "重码", time.AddSeconds(1));
                }
            }

            string sql = "update onlineCodes set state=1,checkTime='" + DateTime.Now.ToString() + "' where orderNO='" + odNO + "';update orderlist set completed=qty where orderNO='"+odNO+"'";
            return DB.Database.RunNoneQuery(sql);
        }
    }
}
