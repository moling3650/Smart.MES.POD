using System;
using System.Collections.Generic;

using System.Windows.Forms;

namespace NV_SNP
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
            //try
            //{
            //    DB.Database.Open();
            //}
            //catch
            //{
            //    MessageBox.Show("打开指定的数据库失败,请检查配置文件和数据服务器是否正常");
            //    return;
            //}
            Application.Run(new Main());
        }
    }
}
