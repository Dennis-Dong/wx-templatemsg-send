using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PrettyNotify
{
    public class HttpHelper
    {
        #region HttpsClient
        /// <summary>
        /// 创建HttpClient
        /// </summary>
        /// <returns></returns>
        public static HttpClient CreateHttpClient(string url)
        {
            HttpClient httpclient;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                httpclient = new HttpClient();
            }
            else
            {
                httpclient = new HttpClient();
            }
            return httpclient;
        }
        #endregion

        #region Get

        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="heads"></param>
        /// <returns></returns>
        public static string HttpGet(string url, Hashtable heads = null)
        {
            HttpWebRequest request;
            var responseStr = string.Empty;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null) request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (request == null) return responseStr;

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            if (heads != null)
            {
                foreach (DictionaryEntry item in heads)
                {
                    request.Headers.Add(item.Key.ToString(), item.Value.ToString());
                }
            }

            var response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8);
            responseStr = reader.ReadToEnd();
            reader.Close();
            return responseStr;
        }

        #endregion

        #region POST
        /// <summary>
        /// body 
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="body">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string body)
        {
            HttpWebRequest request;
            var responseStr = string.Empty;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null) request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (request == null) return responseStr;

            request.Method = "POST";
            request.ContentType = "application/json";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;
            var data = Encoding.UTF8.GetBytes(body);
            request.ContentLength = data.Length;
            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            var response = request.GetResponse();

            //获取响应内容
            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
            {
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }

        /// <summary>
        /// form 表单
        /// </summary>
        /// <param name="url"></param>
        /// <param name="form"></param>
        /// <returns></returns>
        public static string HttpPost(string url, Dictionary<string, string> form)
        {
            HttpWebRequest request;
            var responseStr = string.Empty;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
                request = WebRequest.Create(url) as HttpWebRequest;
                if (request != null) request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }

            if (request == null) return responseStr;

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 15000;
            request.AllowAutoRedirect = false;

            #region 添加Post 参数

            var builder = new StringBuilder();
            foreach (var item in form)
            {
                builder.AppendFormat("&{0}={1}", item.Key, item.Value);
            }
            var data = Encoding.UTF8.GetBytes(builder.ToString().Substring(1));
            request.ContentLength = data.Length;

            using (Stream reqStream = request.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            #endregion

            var response = request.GetResponse();

            //获取响应内容
            using (StreamReader reader = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(), Encoding.UTF8))
            {
                responseStr = reader.ReadToEnd();
            }

            return responseStr;
        }

        #endregion

        /// <summary>
        /// 证书信任
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true; //总是接受  
        }
    }
}
