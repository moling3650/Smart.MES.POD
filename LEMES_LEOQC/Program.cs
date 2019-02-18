using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LEMES_LEOQC
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
            //Application.Run(new OQCWin("2", "1234567890", "10000"));
            Application.Run(new Main());
        }
    }
}
