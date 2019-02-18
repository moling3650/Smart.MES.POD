using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace LEMES_POD.Tools
{
    public class DynamicPopUp
    {
        public static ILE.IFormProperty PopUpLoad(ILE.IJob job, string val)
        {
            string[] strTriger = job.StepList[job.StepIdx].Triger.Split(',');
            string path = System.IO.Directory.GetCurrentDirectory() + "\\" + "DP_WinForm" + ".dll";
            try
            {
                string strClass = "DP_WinForm" + "." + strTriger[1];
                Assembly ass = Assembly.LoadFrom(path);
                Type type = ass.GetType(strClass);
                ILE.IFormProperty obj = (ILE.IFormProperty)Activator.CreateInstance(type);
                obj.Job = job;
                obj.Val = val;
                obj.Run();
                return obj;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
