using System;
using System.Collections.Generic;
using System.Text;

namespace NVBarcode
{
    public class CheckCode
    {

        /// <summary>
        /// 取数字和最后一位的校验码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string ModCheckCode(BarCode code)
        {
            string codeStr = code.prefix + code.sn + code.suffix;

            int num = 0;
            foreach (char c in codeStr)
            {
                if (char.IsNumber(c))
                {
                    num += int.Parse(c.ToString());
                }
            }
            
            return num.ToString().Substring(num.ToString().Length-1);
        }
    }
}
