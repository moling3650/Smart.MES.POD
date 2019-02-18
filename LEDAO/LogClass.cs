using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LEDAO
{
    public partial class LogClass
    {
        /**/
        /// <summary>
        /// 写入日志文件
        /// </summary>
        /// <param name="input"></param>
        public static void WriteLogFile(string input, string parstrfilepath = null)
        {
            try
            {
                string strfilepath = "";
                /**/
                ///指定日志文件的目录
                string fname = "";
                if (string.IsNullOrWhiteSpace(parstrfilepath))
                {
                    strfilepath = Directory.GetCurrentDirectory() + "\\Log\\";
                    //fname = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_LogFile.txt";
                }
                else
                {
                    strfilepath = parstrfilepath + "\\";
                    //fname = parstrfilepath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_LogFile.txt";
                }
                string strfilepathbackup = strfilepath + "log_backup\\";
                //判断目录是否存在
                if (!Directory.Exists(strfilepath))
                {
                    Directory.CreateDirectory(strfilepath);
                }
                //判断目录是否存在
                if (!Directory.Exists(strfilepathbackup))
                {
                    Directory.CreateDirectory(strfilepathbackup);
                }
                fname = strfilepath + DateTime.Now.ToString("yyyyMMdd") + "_LogFile.txt";
                /**/
                ///定义文件信息对象

                FileInfo finfo = new FileInfo(fname);

                //判断文件是否存在
                if (!finfo.Exists)
                {
                    FileStream fs;
                    fs = File.Create(fname);
                    fs.Close();
                    finfo = new FileInfo(fname);
                }

                /**/
                ///判断文件是否存在以及是否大于15MB
                if (finfo.Length > 1024 * 1024 * 15)
                {
                    /**/
                    ///文件超过15MB则重命名
                    string strfilebackup = strfilepathbackup + DateTime.Now.ToString("yyyyMMddHHmmss") + "_LogFile.txt";
                    File.Move(fname, strfilebackup);
                    /**/
                    ///删除该文件
                    //finfo.Delete();
                }
                //finfo.AppendText();
                /**/
                ///创建只写文件流

                using (FileStream fs = finfo.OpenWrite())
                {
                    /**/
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter w = new StreamWriter(fs, Encoding.UTF8);

                    /**/
                    ///设置写数据流的起始位置为文件流的末尾
                    w.BaseStream.Seek(0, SeekOrigin.End);

                    /**/
                    ///写入日志内容并换行
                    w.Write("{0} {1}\r\n", DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss]"), input);

                    /**/
                    ///写入------------------------------------“并换行
                    //w.Write("------------------------------------\n\r");

                    /**/
                    ///清空缓冲区内容，并把缓冲区内容写入基础流
                    w.Flush();

                    /**/
                    ///关闭写数据流
                    w.Close();
                }
            }
            catch (Exception exp)
            {
            }
        }

        /// <summary>
        /// 写入缓存文件
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parstrfilepath"></param>
        public static void WriteCacheFile(string input, string sfc, string order, string parstrfilepath = null)
        {
            try
            {
                string strfilepath = "";
                /**/
                ///指定日志文件的目录
                string fname = "";
                if (string.IsNullOrWhiteSpace(parstrfilepath))
                {
                    strfilepath = Directory.GetCurrentDirectory() + "\\SFCBUFF\\";
                    //fname = Directory.GetCurrentDirectory() + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_LogFile.txt";
                }
                else
                {
                    strfilepath = parstrfilepath + "\\";
                    //fname = parstrfilepath + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_LogFile.txt";
                }
                //string strfilepathbackup = sfc + "\\";
                //判断目录是否存在
                if (!Directory.Exists(strfilepath))
                {
                    Directory.CreateDirectory(strfilepath);
                }
                ////判断目录是否存在
                //if (!Directory.Exists(strfilepathbackup))
                //{
                //    Directory.CreateDirectory(strfilepathbackup);
                //}
                fname = strfilepath + order + "_" + sfc + ".txt";
                /**/
                ///定义文件信息对象

                FileInfo finfo = new FileInfo(fname);

                FileStream fs;
                //判断文件是否存在
                if (!finfo.Exists)
                {
                    fs = File.Create(fname);
                    fs.Close();
                    finfo = new FileInfo(fname);
                }
                else
                {
                    File.Delete(fname);
                    fs = File.Create(fname);
                    fs.Close();
                    finfo = new FileInfo(fname);
                }

                /**/
                ///判断文件是否存在以及是否大于15MB
                //if (finfo.Length > 1024 * 1024 * 15)
                //{
                //    /**/
                //    ///文件超过15MB则重命名
                //    string strfilebackup = strfilepathbackup + DateTime.Now.ToString("yyyyMMddHHmmss") + "_LogFile.txt";
                //    File.Move(fname, strfilebackup);
                //    /**/
                //    ///删除该文件
                //    //finfo.Delete();
                //}
                //finfo.AppendText();
                /**/
                ///创建只写文件流

                using (FileStream fs1 = finfo.OpenWrite())
                {
                    /**/
                    ///根据上面创建的文件流创建写数据流
                    StreamWriter w = new StreamWriter(fs1, Encoding.UTF8);

                    /**/
                    ///设置写数据流的起始位置为文件流的末尾
                    w.BaseStream.Seek(0, SeekOrigin.End);

                    /**/
                    ///写入缓存内容
                    w.Write("{0}", input);

                    /**/
                    ///写入------------------------------------“并换行
                    //w.Write("------------------------------------\n\r");

                    /**/
                    ///清空缓冲区内容，并把缓冲区内容写入基础流
                    w.Flush();

                    /**/
                    ///关闭写数据流
                    w.Close();
                }
            }
            catch (Exception exp)
            {
            }
        }

    }
}
