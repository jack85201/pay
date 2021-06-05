using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using Common.Payment.UtilRSA;
using System.IO;

namespace Common.Payment.UtilRSA
{
    /**
     * 签名类
     */
    public class Signature
    {

        /** 默认编码字符集 */
        public static string DEFAULT_CHARSET = "UTF-8";

        /**
         * 获取签名字符串
         */
        public static string GetSignContent(IDictionary<string, string> parameters)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();


            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");
            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value) && !"sign".Equals(key))
                {
                    query.Append(key).Append("=").Append(value).Append("&");
                }
            }
            string content = query.ToString().Substring(0, query.Length - 1);

            return content;
        }

        /**
         * Md5签名
         */
        public static string Md5Sign(IDictionary<string, string> parameters, string apiKey)
        {
            string plaintext = GetSignContent(parameters) + "&key=" + apiKey;

            return Md5(plaintext).ToUpper();
        }

        /**
         * Md5 加密
         */
        public static string Md5(string content)
        {
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(content));
            string result = "";
            for (int i = 0; i < s.Length; i++)
            {

                result = result + s[i].ToString("x2");

            }
            return result;
        }

        /**
        * RSA2验证签名
        */
        public static bool RsaVerifySign(IDictionary<string, string> parameters, string publicKeyPem, string charset)
        {
            string sign = "";
            parameters.TryGetValue("sign", out sign);
            String plaintext = Md5(GetSignContent(parameters));
            return RsaVerifySign(plaintext, sign, publicKeyPem, charset);
        }


        /**
        * RSA2验证签名
        */
        public static bool RsaVerifySign(IDictionary<string, object> parameters, string publicKeyPem, string charset)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            foreach (var item in parameters)
            {
                data.Add(item.Key, item.Value.ToString());
            }
            return RsaVerifySign(data, publicKeyPem, charset);
        }

        /**
         * RSA2验证签名
         */
        public static bool RsaVerifySign(string signContent, string sign, string publicKeyPem, string charset)
        {

            try
            {
                if (string.IsNullOrEmpty(charset))
                {
                    charset = DEFAULT_CHARSET;
                }

                //string sPublicKeyPEM = File.ReadAllText(publicKeyPem);
                string sPublicKeyPEM = publicKeyPem;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.PersistKeyInCsp = false;
                RSACryptoServiceProviderExtension.LoadPublicKeyPEM(rsa, sPublicKeyPEM);

                bool bVerifyResultOriginal = rsa.VerifyData(Encoding.GetEncoding(charset).GetBytes(signContent), "SHA256", Convert.FromBase64String(sign));
                return bVerifyResultOriginal;

                
            }
            catch
            {
                return false;
            }

        }


    }

   
}