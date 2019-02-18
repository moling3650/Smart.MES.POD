using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
namespace LEMES_POD
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
           

            
            //SystemConsole main = new  SystemConsole();
            //QualityTest main = new QualityTest("INR26650-50A-01-JX");
            LEMES_POD.Main main = new LEMES_POD.Main();
            Application.Run(main);

        }
      
    }
}
