using System;
using System.Collections.Generic;

using System.Text;

namespace NVBarcode
{
    class CodeRuleCenter
    {
        public static CheckResult CheckCode(CodeRule rule)
        {
            return null;
        }
    }

    //检测结果
    class CheckResult
    {
        public bool Result{get;set;}  //结果
        public string ResultInfo { get; set; }  //内容
    }
}
