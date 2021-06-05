using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Common.Payment
{
    public class PaymentHelper
    {
        private static Hashtable instances = new Hashtable();




        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <returns>HTTP响应</returns>
        public static string DoPost(string url, IDictionary<string, string> parameters, string charset)
        {
            //HttpWebRequest req = GetWebRequest(url, "POST");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.ServicePoint.Expect100Continue = false;
            req.Method = "POST";
            req.KeepAlive = true;
            req.UserAgent = "Aop4Net";
            req.Timeout = 100000;

            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;

            byte[] postData = Encoding.GetEncoding(charset).GetBytes(BuildQuery(parameters, charset));
            Stream reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();

            HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
            Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }
        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;

            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);

                // 按字符读取并写入字符串缓冲
                int ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    char c = (char)ch;
                    if (c != '\0')
                    {
                        result.Append(c);
                    }
                }
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }

            return result.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, string charset)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;

            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }

                    postData.Append(name);
                    postData.Append("=");

                    string encodedValue = HttpUtility.UrlEncode(value, Encoding.GetEncoding(charset));

                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }

            return postData.ToString();
        }

        //public static HttpWebRequest GetWebRequest(string url, string method)
        //{
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        //    req.ServicePoint.Expect100Continue = false;
        //    req.Method = method;
        //    req.KeepAlive = true;
        //    req.UserAgent = "Aop4Net";
        //    req.Timeout = 100000;
        //    return req;
        //}
        /// <summary>
        /// 从字符串里随机得到，规定个数的字符串.
        /// </summary>
        /// <param name="allChar"></param>
        /// <param name="CodeCount"></param>
        /// <returns></returns>
        public static string GetRandomCode(int CodeCount)
        {
            string allChar = "1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            string[] allCharArray = allChar.Split(',');
            string RandomCode = "";
            int temp = -1;
            Random rand = new Random();
            for (int i = 0; i < CodeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
                }

                int t = rand.Next(allCharArray.Length - 1);

                while (temp == t)
                {
                    t = rand.Next(allCharArray.Length - 1);
                }

                temp = t;
                RandomCode += allCharArray[t];
            }
            return RandomCode;
        }

        /// <summary>
        /// MD5签名（格式化）。包含逻辑：1、拼接成标准QueryString字符串（a=1&b=2）；2、附加私有密钥字段；3、MD5加密
        /// </summary>
        /// <param name="data">待签名数据集</param>
        /// <param name="key">私有密钥</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <param name="sort">是否排序，默认false（不排序）</param>
        /// <returns></returns>
        public static string SignByMD5(List<KeyValue> data, KeyValue key, string charset = "UTF-8", bool sort = false)
        {
            return SignByMD5(ToQueryString(data, sort) + "&" + key.Key + "=" + key.Value, charset);
        }
        public static string SignByHmacMD5(string source, string key)
        {
            HMACMD5 hmacmd = new HMACMD5(Encoding.Default.GetBytes(key));
            byte[] inArray = hmacmd.ComputeHash(Encoding.Default.GetBytes(source));
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < inArray.Length; i++)
            {
                sb.Append(inArray[i].ToString("X2"));
            }

            hmacmd.Clear();

            return sb.ToString();
        }
        /// <summary>
        /// MD5签名（自定义）。
        /// </summary>
        /// <param name="data">待签名字符串</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <returns></returns>
        public static string SignByMD5(string data, string charset = "UTF-8")
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] array = md5.ComputeHash(Encoding.GetEncoding(charset).GetBytes(data));
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                result.Append(array[i].ToString("x2"));
            }
            return result.ToString();
        }

        /// <summary>
        /// SHA1签名（格式化）。包含逻辑：1、拼接成标准QueryString字符串（a=1&b=2）；2、SHA1加密
        /// </summary>
        /// <param name="data">待签名数据集</param>
        /// <param name="key">私有密钥</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <param name="sort">是否排序，默认false（不排序）</param>
        /// <returns></returns>
        public static string SignBySHA1(List<KeyValue> data, string key, string charset = "UTF-8", bool sort = false)
        {
            return SignBySHA1(ToQueryString(data, sort), key, charset);
        }

        /// <summary>
        /// SHA1签名（16进制）。
        /// </summary>
        /// <param name="data">待签名字符串</param>
        /// <param name="key">私有密钥</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <returns></returns>
        public static string SignBySHA1(string data, string key, string charset = "UTF-8")
        {
            Encoding encode = Encoding.GetEncoding(charset);
            HMACSHA1 sha = new HMACSHA1(encode.GetBytes(key));
            byte[] bytes = sha.ComputeHash(encode.GetBytes(data));
            return BitConverter.ToString(bytes, 0).Replace("-", string.Empty);    // 16进制
        }
        /// <summary>
        /// SHA256签名（16进制）。
        /// </summary>
        /// <param name="data">待签名字符串</param>
        /// <param name="key">私有密钥</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <returns></returns>
        public static string SignBySHA256(string data, string key, string charset = "UTF-8")
        {
            Encoding encode = Encoding.GetEncoding(charset);
            HMACSHA256 sha = new HMACSHA256(encode.GetBytes(key));
            byte[] bytes = sha.ComputeHash(encode.GetBytes(data));
            return BitConverter.ToString(bytes, 0).Replace("-", string.Empty);    // 16进制
        }


        /// <summary>
        /// 生成Web表单（包含自动提交代码）
        /// </summary>
        /// <param name="data">数据集（KeyValue数据结构）</param>
        /// <param name="action">提交地址</param>
        /// <param name="method">提交方式（get|post），默认post</param>
        /// <returns></returns>
        public static string BuildForm(List<KeyValue> data, string action, string method = "post")
        {
            StringBuilder result = new StringBuilder();
            result.Append("<form id=\"form_pay\" action=\"" + action + "\" method=\"" + method + "\">");
            foreach (var item in data)
            {
                result.Append("<input type=\"hidden\" name=\"" + item.Key + "\" value=\"" + item.Value + "\" />");
            }
            result.Append("</form>");
            result.Append("<script type=\"text/javascript\">document.getElementById('form_pay').submit();</script>");

            return result.ToString();
        }

        /// <summary>
        /// 生成Web表单（包含自动提交代码）
        /// </summary>
        /// <param name="data">数据集（key1=value1&key2=value2数据结构）</param>
        /// <param name="action">提交地址</param>
        /// <param name="method">提交方式（get|post），默认post</param>
        /// <returns></returns>
        public static string BuildForm(string data, string action, string method = "post")
        {
            return BuildForm(StringToList(data), action, method);
        }

        /// <summary>
        /// 生成二维码
        /// 注意：前端需引用jquery.min.js和jquery-qrcode.min.js
        /// </summary>
        /// <param name="data">数据(一般是链接)</param>
        /// <returns></returns>
        public static string BuildQRcode(string data)
        {
            var qr = new StringBuilder();
            qr.Append("<div style='margin:0px auto;padding:20px 0px;'>");
            qr.Append("<div id='hs_qrcode'></div>");
            qr.Append("</div>");
            qr.Append("<script>");
            qr.Append("$(function(){");
            qr.Append("$('#hs_qrcode').qrcode({text:'" + data + "',width:200,height:200});");
            qr.Append("});");
            qr.Append("</script>");
            return qr.ToString();
        }

        /// <summary>
        /// 将数据集拼接成标准QueryString字符串（a=1&b=2）
        /// </summary>
        /// <param name="data">数据集</param>
        /// <param name="sort">是否排序（仅支持按Key升序），默认false</param>
        /// <returns></returns>
        public static string ToQueryString(List<KeyValue> data, bool sort = false)
        {
            StringBuilder result = new StringBuilder();
            if (sort) data = data.OrderBy(d => d.Key).ToList();
            foreach (var item in data)
            {
                result.Append("&" + item.Key + "=" + item.Value);
            }
            return result.ToString().TrimStart('&');
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="data">待提交的数据列表</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <param name="type">内容类型，默认：application/x-www-form-urlencoded</param>
        /// <returns></returns>
        public static string Post(string url, List<KeyValue> data, string charset = "UTF-8", string type = "application/x-www-form-urlencoded")
        {
            return Post(url, ToQueryString(data), charset, type);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="data">待提交的数据列表</param>
        /// <param name="charset">字符编码，默认：UTF-8</param>
        /// <param name="type">内容类型，默认：application/x-www-form-urlencoded</param>
        /// <returns></returns>
        public static string Post(string url, string data, string charset = "UTF-8", string type = "application/x-www-form-urlencoded")
        {
            try
            {
                var encoding = System.Text.Encoding.GetEncoding(charset);
                var request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                request.Method = "post";
                request.Accept = "*/*";
                request.ContentType = type;
                byte[] buffer = encoding.GetBytes(data);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
                var response = (System.Net.HttpWebResponse)request.GetResponse();

                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception err)
            {
                return "POST异常：" + err.Message;
            }
        }
        /// <summary>
        /// get 方式
        /// </summary>
        /// <param name="p_url"></param>
        /// <returns></returns>
        public static string Get(string p_url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(p_url);
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
        public static List<KeyValue> StringToList(string data)
        {
            var result = new List<KeyValue>();
            if (!string.IsNullOrEmpty(data))
            {
                var array1 = data.Split('&');
                foreach (var item in array1)
                {
                    // 2018-12-21发现部分平台使用【=】做为值的一部分，导致数据解析错误
                    //var array2 = item.Split('=');
                    //result.Add(new KeyValue() { Key = array2[0], Value = array2[1] });
                    if (item.Length <= (item.IndexOf('=') + 1))
                    {
                        result.Add(new KeyValue() { Key = (item.IndexOf('=') > -1 ? item.Substring(0, item.IndexOf('=')) : item), Value = "" });
                    }
                    else
                    {
                        result.Add(new KeyValue() { Key = (item.IndexOf('=') > -1 ? item.Substring(0, item.IndexOf('=')) : item), Value = ((item.IndexOf('=') == -1) ? "" : item.Substring(item.IndexOf('=') + 1)) });
                    }
                }
            }

            return result;
        }

        public static string ListToString(List<KeyValue> data)
        {
            var result = new StringBuilder();
            foreach (var item in data)
            {
                result.Append($"&{item.Key}={item.Value}");
            }

            return result.ToString().TrimStart('&');
        }

        public static List<KeyValue> JsonToList(string data)
        {
            var result = new List<KeyValue>();
            if (!string.IsNullOrEmpty(data))
            {
                var reg = new Regex(@"""(\w+)"":([^,{}]*)[,}]+");
                var matchs = reg.Matches(data);
                foreach (Match item in matchs)
                {
                    result.Add(new KeyValue() { Key = item.Groups[1].Value, Value = item.Groups[2].Value.Replace("\"", "").Trim() });
                }
            }

            return result;
        }

        /// <summary>  
        /// 时间戳转为C#格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>C#格式时间</returns>  
        public static DateTime UnixToDateTime(string timeStamp)
        {
            if (string.IsNullOrWhiteSpace(timeStamp)) return DateTime.Now;
            if (timeStamp.Length > 10) timeStamp = timeStamp.Substring(0, 10);

            DateTime date = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long time = long.Parse(timeStamp + "0000000");
            TimeSpan now = new TimeSpan(time);
            return date.Add(now);
        }

        /// <summary>
        /// 时间转时间戳
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public static long DateTimeToUnix(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (time.Ticks - startTime.Ticks) / 10000000;
        }

        public static object GetInstance(string name)
        {
            if (instances.ContainsKey(name))
            {
                return instances[name];
            }
            else
            {
                var type = Type.GetType(name);
                if (type != null)
                {
                    instances.Add(name, type.Assembly.CreateInstance(name));
                }
                return instances[name];
            }
        }

        /// <summary>
        /// Unicode编码
        /// </summary>
        /// <returns>编码字符串（金额 -> \u91d1\u989d）</returns>
        /// <param name="value">需要编码的内容</param>
        public static string UnicodeEncode(string value)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(value);
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i += 2)
            {
                // 取两个字符，每个字符都是右对齐。
                result.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return result.ToString();
        }

        /// <summary>
        /// Unicode解码
        /// </summary>
        /// <returns>解码字符串（\u91d1\u989d -> 金额）</returns>
        /// <param name="value">需要解码的内容</param>
        public static string UnicodeDecode(string value)
        {
            if (!string.IsNullOrEmpty(value) && (value.StartsWith("\\") || value.StartsWith("u")))
            {
                StringBuilder result = new StringBuilder();
                string[] list = value.Replace("\\", "").Split('u');
                for (int i = 1; i < list.Length; i++)
                {
                    result.Append((char)int.Parse(list[i], System.Globalization.NumberStyles.HexNumber));
                }
                return result.ToString();
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// 支付日志（头尾换行，需要调试信息，请在站点根目录新建文件PayDebug.txt）
        /// </summary>
        /// <param name="msg">内容</param>
        public static string Log(string msg)
        {
            try
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "PayDebug.txt";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.AppendAllText(path, $"\r\n{DateTime.Now.ToString()}\t{msg}\r\n");
                }
            }
            catch { }
            return msg;
        }
    }
}

public class KeyValue
{
    public string Key { get; set; }
    public string Value { get; set; }

    public KeyValue() { }

    public KeyValue(string key, string value)
    {
        Key = key;
        Value = value;
    }

}

