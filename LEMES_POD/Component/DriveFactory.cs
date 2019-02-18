using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;
using System.Reflection;

namespace LEMES_POD.Component
{
    class DriveFactory
    {
        public static IResult GetDCO(string drive_code,string param)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\DCDriveList\\" + drive_code + ".dll";
            IResult rec = new LEResult();
            try
            {
                Assembly ass = Assembly.LoadFrom(path);
                Type type = ass.GetType(drive_code + ".DCO");
                Object obj = Activator.CreateInstance(type,new object[]{param});
                IDCO SpoObj = (IDCO)obj;
                SpoObj.Driver_code = drive_code;
                rec.Result = true;
                rec.obj = SpoObj;
                return rec;
                
            }
            catch (Exception exp)
            {
                rec.Result = false;
                rec.ExtMessage = "加载工步驱动" + "[" + drive_code + "]失败:" + exp.Message;
                return rec;
            }
        }

        public static IResult GetTPO(string drive_code, string param)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\TPDriveList\\" + drive_code + ".dll";
            IResult rec = new LEResult();
            try
            {
                Assembly ass = Assembly.LoadFrom(path);
                Type type = ass.GetType(drive_code + ".TPO");
                Object obj = Activator.CreateInstance(type, new object[] { param });
                ITPO SpoObj = (ITPO)obj;
                SpoObj.Driver_code = drive_code;
                rec.Result = true;
                rec.obj = SpoObj;
                return rec;

            }
            catch (Exception exp)
            {
                rec.Result = false;
                rec.ExtMessage = "加载任务驱动" + "[" + drive_code + "]失败:" + exp.Message;
                return rec;
            }
        }
    }
}
