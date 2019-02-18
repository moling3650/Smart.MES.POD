using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;

namespace LEMES_POD.Component
{
    public class Process
    {
        public static Image GetISOP(string process)
        {
            string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Process", "GetESOP", process);
            byte[] bt = JsonConvert.DeserializeObject<byte[]>(str);
            Image img=null;
            if (bt != null)
            {
                img = Image.FromStream(new MemoryStream(bt));
            }
            return img;
        }

        public static Image GetPIDISOP(int pid)
        {
            string strpid = pid.ToString();
            string str = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Process", "GetPIDESOP", strpid);
            byte[] bt = JsonConvert.DeserializeObject<byte[]>(str);
            Image img = null;
            if (bt != null)
            {
                img = Image.FromStream(new MemoryStream(bt));
            }
            return img;
        }
       
    }
}
