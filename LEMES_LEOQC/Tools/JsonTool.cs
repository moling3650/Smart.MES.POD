using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace LEMES_LEOQC.Tools
{
    public class JsonToolsNet
    {

        public static string ObjectToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        // 从一个Json串生成对象信息
        public static object JsonToObject(string jsonString, object obj)
        {
            return JsonConvert.DeserializeObject(jsonString, obj.GetType());
        }

        ///// <summary>
        ///// 条用服务返回json 数据
        ///// </summary>
        ///// <param name="serverPath"></param>
        ///// <param name="jsonData"></param>
        ///// <returns></returns>
        //public static string GetJsonData(string FunctionName, string JsonData)
        //{
        //    //   string serverPath = "http://172.16.108.54:8080/swd-web/swd/mesforerp/" + FunctionName;
        //    string serverPath = "http://" + UserHelper.serverip + "/swd/mesforerp/" + FunctionName;
        //    return DoPost(serverPath, JsonData);
        //}

        //public static string GetJsonDataNew(string FunctionName, string JsonData)
        //{
        //    //   string serverPath = "http://172.16.108.54:8080/swd-web/swd/http/pack/" + FunctionName;
        //    string serverPath = "http://" + UserHelper.serverip + "/swd/http/pack/" + FunctionName;
        //    return DoPost(serverPath, JsonData);
        //}

        public static string DoPost(string url, string data)
        {
            try
            {
                HttpWebRequest webRequest = GetWebRequest(url, "POST");
                webRequest.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                //webRequest.ContentType = "application/json; charset=utf-8";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
                // txt_EndTime.Text = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss:fff");
                Encoding encoding = Encoding.GetEncoding(httpWebResponse.CharacterSet);
                return GetResponseAsString(httpWebResponse, encoding);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            Stream stream = null;
            StreamReader streamReader = null;
            string result;
            try
            {
                stream = rsp.GetResponseStream();
                streamReader = new StreamReader(stream, encoding);
                result = streamReader.ReadToEnd();
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
                if (stream != null)
                {
                    stream.Close();
                }
                if (rsp != null)
                {
                    rsp.Close();
                }
            }
            return result;
        }

        public static HttpWebRequest GetWebRequest(string url, string method)
        {
            HttpWebRequest httpWebRequest;
            if (url.Contains("https"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpWebRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            //httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.Method = method;
            //httpWebRequest.KeepAlive = true;
            httpWebRequest.Timeout = 100000;
            return httpWebRequest;
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
