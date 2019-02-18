using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Data.OleDb;
using System.Web;
using System.Text;
using System.Collections.Specialized;

namespace DB
{
    /// <summary>
    /// ͨ�õ����ݿ⴦���࣬ͨ��ado.net�����ݿ�����
    /// </summary>
    /// 
    [Obsolete]
    public class Database : IDisposable
    {

        //ini��ȡ�����ַ���
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retval, int size, string filePath);
        [System.Runtime.InteropServices.DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, byte[] retVal, int size, string filePath);

        //private string strFilePath = Application.StartupPath + "\\FileConfig.ini";//��ȡINI�ļ�·��
        private static string strFilePath = HttpContext.Current.Request.PhysicalApplicationPath;//��ȡINI�ļ�·��
        private string strSec = ""; //INI�ļ���
        //
        private static void GetStringsFromBuffers(Byte[] Buffers, int bufLen, StringCollection Strings)
        {
            Strings.Clear();
            if (bufLen != 0)
            {
                int start = 0;
                for (int i = 0; i < bufLen; i++)
                {
                    if ((Buffers[i] == 0) && ((i - start) > 0))
                    {
                        String s = Encoding.GetEncoding(0).GetString(Buffers, start, i - start);
                        Strings.Add(s);
                        start = i + 1;
                    }
                }
            }
        }

        // ��������Դ
        //public static string dbName="";      //���ݿ���
        private static OleDbConnection con = null;
        public static OleDbDataAdapter oda;

        public string GetDefaultValue(string key)
        {
            OleDbDataReader dr = RunQuery("select statename from userstate where statetype='" + key + "'");
            if (dr.Read()) return dr[0].ToString();
            return null;
        }


        public OleDbDataAdapter getAdapter()
        {
            Open();

            OleDbDataAdapter adapter = new OleDbDataAdapter();

            return adapter;
        }

        public static OleDbDataAdapter getAdapter(string query)
        {
            Open();
            OleDbCommand selectCmd = new OleDbCommand(query, con);
            selectCmd.Transaction = null;
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCmd);
            OleDbCommandBuilder scb = new OleDbCommandBuilder(adapter);//�Զ�������������
            return adapter;
        }

        public static OleDbDataAdapter getAdapter(string query, OleDbParameter[] prams)
        {
            Open();
            OleDbCommand selectCmd = new OleDbCommand(query, con);
            OleDbDataAdapter adapter = new OleDbDataAdapter(selectCmd);
            OleDbCommandBuilder scb = new OleDbCommandBuilder(adapter);//�Զ�������������
            return adapter;
        }


        public static DataTable getDataTable(string query, OleDbParameter[] prams)
        {
            OleDbDataAdapter ad = getAdapter(query);
            oda = ad;
            if (prams != null)
            {
                foreach (OleDbParameter pra in prams)
                {
                    ad.SelectCommand.CommandType = CommandType.StoredProcedure;
                    ad.SelectCommand.Parameters.Add(pra);
                }
            }
            DataTable dt = new DataTable();
            ad.Fill(dt);
            return dt;
        }

        public static DataTable getDataTable(string query)
        {
            OleDbDataAdapter ad = getAdapter(query);
            oda = ad;
            DataTable dt = new DataTable();
            //log.Info(dt.Rows[0][0].ToString());
            ad.Fill(dt);
            return dt;
        }

        public static OleDbDataReader getDataReader(string query)
        {
            OleDbCommand cd = getCommand(query);

            OleDbDataReader dr = cd.ExecuteReader();
            return dr;
        }

        public void updateDataTable(DataSet dataset, string tablename)
        {
            OleDbDataAdapter adapter = new OleDbDataAdapter();
            adapter.SelectCommand = CreateCommand("select * from " + tablename);
            adapter.Update(dataset, tablename);
        }


        /// <summary>
        /// ִ�в�ѯ
        /// </summary>
        /// <param name="procName">��ѯ���</param>
        /// <param name="dataReader">���ش洢���̷���ֵ</param>
        public static OleDbDataReader RunQuery(string OleDb)
        {
            return RunQuery(OleDb, null);
        }

        public static OleDbDataReader RunQuery(string OleDb, OleDbParameter[] prams)
        {
            OleDbCommand cmd;
            if (prams == null)
                cmd = CreateCommand(OleDb);
            else cmd = CreateCommand(OleDb, prams);
            //cmd.CommandType=CommandType.Text;
            return cmd.ExecuteReader();
            //return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);  //������reader�رն��ر�
            //return (int)cmd.Parameters["ReturnValue"].Value;
        }

        /// <summary>
        /// ִ�з��ر�Ĵ洢���̣���������
        /// </summary>
        /// <param name="OleDb"></param>
        /// <returns></returns>
        public static DataTable RunTableProc(string OleDb)
        {
            return RunTableProc(OleDb, null);
        }
        /// <summary>
        /// ִ�з��ر�Ĵ洢���̣�������
        /// </summary>
        /// <param name="OleDb"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public static DataTable RunTableProc(string OleDb, OleDbParameter[] prams)
        {
            OleDbDataAdapter oda = getAdapter(OleDb);
            if (prams != null)
            {
                foreach (OleDbParameter pram in prams)
                {
                    oda.SelectCommand.Parameters.Add(pram);
                }
            }
            oda.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable table = new DataTable();
            oda.Fill(table);
            return table;
        }

        /// <summary>
        /// ִ�з���ֵ�Ĵ洢����
        /// </summary>
        /// <param name="OleDb"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        /// 
        public static object RunObjProc(string OleDb)
        {
            return RunObjProc(OleDb, null);
        }

        /// <summary>
        /// ִ�з���ֵ�Ĵ洢����
        /// </summary>
        /// <param name="OleDb"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        /// 
        public static object RunObjProc(string OleDb, OleDbParameter[] prams)
        {
            OleDbCommand oda = getCommand(OleDb);
            OleDbCommand cmd;
            if (prams == null)
                cmd = CreateCommand(OleDb);
            else
                cmd = CreateCommand(OleDb, prams);
            cmd.ExecuteNonQuery();
            object obj = cmd.Parameters["ReturnValue"].Value;
            return obj;
        }

        /// <summary>
        /// ִ�зǲ�ѯ���
        /// </summary>
        /// <param name="procName">�ǲ�ѯ���</param>
        /// <param name="dataReader">���طǲ�ѯ��䷵��ֵ</param>
        public static int RunNoneQuery(string OleDb)
        {
            return RunNoneQuery(OleDb, null);
            /*
            OleDbCommand cmd = CreateCommand(OleDb);
            return cmd.ExecuteNonQuery();*/
        }
        public static int RunNoneQuery(string OleDb, OleDbParameter[] prams)
        {
            OleDbCommand cmd;
            if (prams != null) cmd = CreateCommand(OleDb, prams);
            else cmd = CreateCommand(OleDb);
            int ret = cmd.ExecuteNonQuery();
            return ret;
        }




        public static OleDbCommand getCommand(string query)
        {
            Open();
            OleDbCommand cmd = new OleDbCommand(query, con);
            return cmd;
        }

        /// <summary>
        /// ����һ��OleDbCommand�����Դ���ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢���̵�����</param>
        /// <param name="prams">�洢�����������</param>
        /// <returns>����OleDbCommand����</returns>
        private static OleDbCommand CreateCommand(string procName, OleDbParameter[] prams)
        {
            OleDbCommand cmd = CreateCommand();
            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            // ���ΰѲ�������洢����
            if (prams != null)
            {
                foreach (OleDbParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // ���뷵�ز���
            //cmd.Parameters.Add(
            //    new OleDbParameter("ReturnValue", OleDbType.Integer, 4,
            //    ParameterDirection.ReturnValue, false, 0, 0,
            //    string.Empty, DataRowVersion.Default, null));
            return cmd;
        }

        private static OleDbCommand CreateCommand()
        {
            // ȷ�ϴ�����
            Open();
            OleDbCommand cmd = new OleDbCommand();
            cmd.Connection = con;
            return cmd;
        }


        private static OleDbCommand CreateCommand(String OleDb)
        {
            OleDbCommand cmd = CreateCommand();
            cmd.CommandText = OleDb;
            return cmd;
        }

        /// <summary>
        /// �����ݿ�����.
        /// </summary>
        public static void Open()
        {
            // �����ݿ�����
            if (con == null)
            {
                //if (dbName == "")
                //    return;

                string fileName = "DeiveConfig.ini";
                string strfilename = strFilePath + "\\" + fileName;
                //��ȡ����keyֵ����һ
                Byte[] Buffers = new Byte[16384];
                StringCollection Idents = new StringCollection();
                int bufLen = GetPrivateProfileString("Conifg", null, null, Buffers, Buffers.GetUpperBound(0), strfilename);
                //��Section���н��� 
                GetStringsFromBuffers(Buffers, bufLen, Idents);

                for (int i = 0; i < Idents.Count; i++)
                {
                    string strvalue = "";
                    StringBuilder temp = new StringBuilder(1024);
                    GetPrivateProfileString("Conifg", Idents[i].ToString().Trim(), "", temp, 1024, strfilename);
                    strvalue = temp.ToString().Trim();
                }


                string dbName = System.Configuration.ConfigurationSettings.AppSettings["DBName"].ToString();
                string s = System.Configuration.ConfigurationSettings.AppSettings["constring"].ToString();

                //s = s.Replace("####", dbName);      //��̬��ֵ���ݿ���
                //string s = "Provider=SQLOLEDB;server=127.0.0.1;database=Test;User Id=lwl;pwd=890914;";
                s = Idents[0].ToString().Trim();
                con = new OleDbConnection(s);
                //�����ӳ�ʱ��
            }
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }

        }

        public static OleDbConnection getConnection()
        {
            Open();
            return con;
        }



        /// <summary>
        /// �ر����ݿ�����
        /// </summary>
        public static void Close()
        {
            try
            {
                if (con != null)
                    con.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// �ͷ���Դ
        /// </summary>
        public void Dispose()
        {
            // ȷ�������Ƿ��Ѿ��ر�
            if (con != null)
            {
                con.Dispose();
                con = null;
            }
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param></param>
        /// <param name="Size">������С</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public static OleDbParameter MakeInParam(string ParamName, OleDbType DbType, int Size, object Value)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// ���뷵��ֵ����
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <returns>�µ� parameter ����</returns>
        public static OleDbParameter MakeOutParam(string ParamName, OleDbType DbType, int Size)
        {
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }


        /// <summary>
        /// ���ɴ洢���̲���
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С</param>
        /// <param name="Direction">��������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public static OleDbParameter MakeParam(string ParamName, OleDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            OleDbParameter param;

            if (Size > 0)
                param = new OleDbParameter(ParamName, DbType, Size);
            else
                param = new OleDbParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }
        /// <summary>
        /// �¼��������Sql���
        /// </summary>
        /// <param name="strs"></param>
        public static int ExecTrans(string[] strs)
        {

            Open();
            int OK = -1;
            OleDbCommand cmd = CreateCommand();
            OleDbTransaction tran = con.BeginTransaction();
            cmd.Transaction = tran;
            try
            {
                foreach (string sql in strs)
                {
                    OK++;
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();

                }
                tran.Commit();
            }
            catch (Exception e)
            {      //ִ��ʧ�ܷ����������sql����
                tran.Rollback();
                return OK;
            }
            finally
            {
                con.Close();
            }
            if (OK == strs.Length - 1)  //ִ�гɹ��Ļ�����-1;
            {
                OK = -1;
            }
            return OK;
        }
    }

}
