using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using ChanHamDIS;

namespace LEDIS
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    public class Service1 : IService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public DataTable GetAllData(string sql)
        {
            if (sql == null)
                return null;

            DataTable dt= DB.Database.getDataTable(sql);
            dt.TableName = "tb1";
            return dt;
        }

        public string RunServerAPI(string spc, string APIName, string jsonData)
        {
            string strClass = spc;  //命名空间+类名
            string strMethod = APIName;//方法名
            Type tp;
            object obj;
            tp = Type.GetType(strClass);//通过string类型的strClass获得同名类“t”
            System.Reflection.MethodInfo method = tp.GetMethod(strMethod);//通过string类型的strMethod获得同名的方法“method”
            obj = System.Activator.CreateInstance(tp);//创建t类的实例 "obj"

            //上面的方法是无参的,下面是有参的情况.
            object[] objs = new object[] { jsonData };
            object rec = method.Invoke(null, objs);//t类实例obj,调用方法"method(testcase)"
            return rec.ToString();
        }
    }
}
