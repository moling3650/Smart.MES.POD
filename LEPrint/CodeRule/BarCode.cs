using System;
using System.Collections.Generic;
using System.Text;

namespace NVBarcode
{
    public class BarCode:ICode
    {
        public string prefix { get; set; }
        public string suffix { get; set; }
        public string sn { get; set; }
        public string snStr { get; set; }
    }
}
