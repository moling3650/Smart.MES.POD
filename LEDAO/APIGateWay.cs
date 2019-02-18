using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LEDAO
{
    public class APIGateWay
    {
        public static string aaa;
        private static LEMESEntity _entityContext;

        /// <summary>
        /// 获取唯一的EF环境对象
        /// </summary>
        /// <returns></returns>
        public static LEMESEntity GetEntityContext()
        {
            return  new LEMESEntity();
        }

        /// <summary>
        /// 获取唯一的EF环境对象
        /// </summary>
        /// <returns></returns>
        public static LEMESEntity GetConnEntityContext()
        {
            /*
            if (_entityContext == null)
            {
                _entityContext = new LEMESEntity();
                return _entityContext;
            }
            */
            return new LEMESEntity(System.Configuration.ConfigurationManager.ConnectionStrings["connstr"].ToString());
        }

        /// <summary>
        /// 销毁EF对象
        /// </summary>
        public static void CloseContext()
        {
            _entityContext.Dispose();
        }
    }
}
