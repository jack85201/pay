using Common.Payment.UtilRSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Payment
{
    /// <summary>
    /// 隆发支付
    /// 注意：金额单位：分，
    /// </summary>
    public class lfpay : PaymentBase, IPayment
    {
        #region 充值[Recharge]
        /// <summary>
        /// 常量：充值请求待签模板（签名用）
        /// </summary>
        protected override string RechargeRequestSignTemplate
        {
            get { return "{\"amount\":\"{amount}\",\"goodsName\":\"{goodsName}\",\"merchNo\":\"{merchNo}\",\"netwayType\":\"{netwayType}\",\"notifyUrl\":\"{notifyUrl}\",\"notifyViewUrl\":\"{notifyViewUrl}\",\"orderNo\":\"{orderNo}\",\"randomNo\":\"{randomNo}\"}{key}"; }
        }
        //RSA 支付公钥
        private string publicKey
        {
            get { return "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCUuVL8UTGsOOaDLMwy9MIZGL2npsG78dA9kL7Hdr6UfHmHZSzr0Xfdv55daNJmGHrJfBHjHCF1XS4IKLEb2hymyhpZ0HZGP18Zi6jnvCKRN5oq0pIAoInAltw0wZKd0JhC8E3JwE4xcbbNYgjrxx / LmLDx34mcNXk / DjrGe9bWhQIDAQAB"; }
        }
        //RSA 私钥
        private string privateKey
        {
            get { return "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBAJRIaxgy1vUGJ7dwDkf/wCTqZ+P3R09uGroBHh8nxjK887gLGDslZRGEvzmRF0teNjtXpFwqrBquB9NJ2hM6SuPyrGSwhFLyb4fDr7q2xypJXRdu1CNnQyQ7KW / 3wrPKJMq1Ju0zu576feqXIcJyNZYhy24yTcG8M8jgfCgGCoa5AgMBAAECgYEAj9doPAMlTm74HBQRZnqLk8Pnn9aCUGUIfCMVazeUJifxbDkm + kkeL85MCbXbmPr9NLeh8t5aMU9cu / PKHhjQB6znQgzRK3n8 + LPsDBr6wUf5pJJJWgc0ePP9yawghYG3eeq6f8q4VrkzJKtcFd93uFBcWmyety5DGTne8C2usfkCQQDkMMS4xldUuVXDXwEyJSJ5xxxn1umb7Meqt1f75rInj106KTYvpWdFOTKQGMQfjm6IUzxHTFvs3aZHUf1KJzAvAkEAplqkOlD + AulZWwlD0YypykZaNIIqVlEBWvNdDUlgNzeUmJH + 7gT2AjR / 2f8O8oK4lpd415mH + Ci5EWeG9ArVlwJBAJhXeq5FND6K8Rfa0GiS5B5a8Lreft2rSW3Os32n + Z5xlvLiWpuamIRdeEU9U4ohw + ddcmvDLcfH / l + 0 / B3KZd8CQFyzFeEJUQL +VbTNLOWQpOz61zl7b2w6J68u / iNxSBR8 / Gkosg6g5RXFe5lW8FVjUslYDxbVj1dSGg8AycskXnECQHAN6vhLjYA / FHzzssobj1kiVU8usFEQzR5av / f0uxJxY3WqSST0id9fu + ryCy2t8eLNychjGIlv4oCaGHtuyro = "; }
        }
        /// <summary>
        /// 常量：充值请求数据模板（提交用）
        /// </summary>
        protected override string RechargeRequestDataTemplate
        {
            get { return "{\"amount\":\"{amount}\",\"goodsName\":\"{goodsName}\",\"merchNo\":\"{merchNo}\",\"netwayType\":\"{netwayType}\",\"notifyUrl\":\"{notifyUrl}\",\"notifyViewUrl\":\"{notifyViewUrl}\",\"orderNo\":\"{orderNo}\",\"randomNo\":\"{randomNo}\",\"sign\":\"{sign}\"}"; }
        }

        /// <summary>
        /// 常量：充值通知待签模板（签名用）
        /// </summary>
        protected override string RechargeNotifySignTemplate
        {
            get { return "{\"amount\":\"{amount}\",\"goodsName\":\"{goodsName}\",\"merchNo\":\"{merchNo}\",\"netwayType\":\"{netwayType}\",\"orderNo\":\"{orderNo}\",\"payDate\":\"{payDate}\",\"payStateCode\":\"{payStateCode}\"}{shop}"; }
        }
        // 存入内存 商务号
        private static string lfpayshop = "未加载参数";
        /// <summary>
        /// 常量：充值通知成功标签
        /// </summary>
        public string RechargeNotifySuccess
        {
            get { return "SUCCESS"; }
        }

        /// <summary>
        /// 方法：充值
        /// </summary>
        /// <param name="shop">商户编号</param>
        /// <param name="bank">银行编号</param>
        /// <param name="order">商户订单编号</param>
        /// <param name="time">商户订单时间</param>
        /// <param name="amount">支付金额</param>
        /// <param name="notify">通知地址</param>
        /// <param name="redirect">跳转地址</param>
        /// <param name="referer">商户支付页面地址</param>
        /// <param name="ip">客户IP地址</param>
        /// <param name="key">密钥</param>
        /// <param name="info">订单描述</param>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        public RechargeResult Recharge(string shop, string bank, string order, DateTime time, decimal amount, string notify, string redirect, string referer, string ip, string key, string info, string url)
        {

            #region 准备数据
            lfpayshop = shop;//存放 静态数据
            string randomNo = PaymentHelper.GetRandomCode(4);
            var sign = RechargeRequestSignTemplate
                .Replace("{amount}", (amount * 100).ToString())
                .Replace("{goodsName}", info)
                .Replace("{merchNo}", shop)
                .Replace("{netwayType}", bank)
                .Replace("{notifyUrl}", notify)
                .Replace("{notifyViewUrl}", redirect)
                .Replace("{orderNo}", order)
                .Replace("{randomNo}", randomNo)
                .Replace("{key}", key);
            sign = PaymentHelper.SignByMD5(sign).ToUpper();

            var data = RechargeRequestDataTemplate
                .Replace("{amount}", (amount * 100).ToString())
                .Replace("{goodsName}", info)
                .Replace("{merchNo}", shop)
                .Replace("{netwayType}", bank)
                .Replace("{notifyUrl}", notify)
                .Replace("{notifyViewUrl}", redirect)
                .Replace("{orderNo}", order)
                .Replace("{randomNo}", randomNo)
                .Replace("{sign}", sign);
            //获取到data RSA加密  rsa支付公钥
            // string publicKey = "RSA支付公钥";// base.GateUserEmail;
            string cipher_data = "";
            string publickey = RSAEncodHelper.RSAPublicKeyJava2DotNet(publicKey);

            byte[] cdatabyte = RSAEncodHelper.RSAPublicKeySignByte(data, publickey);
            cipher_data = Convert.ToBase64String(cdatabyte);
            //RSA 加密瓶装data后提交 post 获取json数据
            string paramstr = "data=" + System.Web.HttpUtility.UrlEncode(cipher_data) + "&merchNo=" + shop + "&version=" + "V3.6.0.0";

            #endregion

            #region 提交请求
            var postback = PaymentHelper.Post(url, paramstr);
            PaymentHelper.Log($"lfpay.Recharge\r\nDATA\t{data}\r\nURL\t{url}\r\nPOSTBACK\t{postback}");
            try
            {
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(postback);
                if (obj["stateCode"].ToString() == "00")
                {
                    //在MD5 验签  获取sign
                    string sign2 = obj["sign"].ToString();
                    string datajson = "{\"merchNo\":\"{merchNo}\",\"msg\":\"{msg}\",\"orderNo\":\"{orderNo}\",\"qrcodeUrl\":\"{qrcodeUrl}\",\"stateCode\":\"{stateCode}\"}{key}";
                    datajson = datajson
                    .Replace("{merchNo}", obj["merchNo"].ToString())
                    .Replace("{msg}", obj["msg"].ToString())
                    .Replace("{orderNo}", obj["orderNo"].ToString())
                    .Replace("{qrcodeUrl}", obj["qrcodeUrl"].ToString())
                    .Replace("{stateCode}", obj["stateCode"].ToString())
                    .Replace("{key}", key);
                    sign = PaymentHelper.SignByMD5(datajson).ToUpper();
                    if (sign == obj["sign"].ToString())
                    {
                        return new RechargeResult() { Status = true, Result = obj["qrcodeUrl"].ToString() };
                    }
                    else
                    {
                        return new RechargeResult() { Status = false, Result = "验签失败!" };
                    }
                }
                else
                {
                    return new RechargeResult() { Status = false, Result = obj["msg"].ToString() };
                }
            }
            catch
            {
                return new RechargeResult() { Status = false, Result = postback };
            }
            #endregion
        }

        /// <summary>
        /// 方法：充值通知
        /// </summary>
        /// <param name="form">接收到的原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public ModelQuery RechargeNotify(string form, string key)
        {
            try
            {


                PaymentHelper.Log("lfpay.FormToList");
                //获取 表单参数
                var data = PaymentHelper.StringToList(form);
                //获取 data加密数据
                string dataJson = data.Find(m => m.Key == "data").Value;
                //获取 data数据解密
                string resultJson = RSAHelper.decryptData(dataJson, privateKey, "utf-8");
                //转换 json数据
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(resultJson);
                #region 验签

                var tmp = RechargeNotifySignTemplate
                    .Replace("{amount}", obj["amount"].ToString())
                    .Replace("{goodsName}", obj["goodsName"].ToString())
                    .Replace("{merchNo}", obj["merchNo"].ToString())
                    .Replace("{netwayType}", obj["netwayType"].ToString())
                    .Replace("{orderNo}", obj["orderNo"].ToString())
                    .Replace("{payDate}", obj["payDate"].ToString())
                    .Replace("{payStateCode}", obj["payStateCode"].ToString())
                    .Replace("{shop}", key);
                var sign = PaymentHelper.SignByMD5(tmp).ToUpper();
                var from = obj["sign"].ToString();
                PaymentHelper.Log($"lfpay.CheckSign\tmine:{sign}\tfrom:{from}");
                if (sign != from)
                {
                    return new ModelQuery()
                    {
                        Errors = { "验签失败" }
                    };
                }
                #endregion

                #region 封装数据
                var datetime = DateTime.Now;
                var result = new ModelQuery();
                result.OrderNo = obj["orderNo"].ToString();// data.Find(p => p.Key == "MerOrderNo").Value;
                result.OrderTime = datetime;// Convert.ToDateTime(datetime);
                result.Amount = Convert.ToDecimal(obj["payStateCode"].ToString()) / 100;// Convert.ToDecimal(data.Find(p => p.Key == "AmountReal").Value) / 100;
                result.TradeNo = obj["merchNo"].ToString();// data.Find(p => p.Key == "PayOrderNo").Value;
                result.TradeTime = datetime;// Convert.ToDateTime(datetime);
                result.QueryStatus = true;
                result.TradeStatus = EnumTradeStatus.Success;
                //var status = data.Find(p => p.Key == "PayStatus").Value;
                //switch (status)
                //{
                //    case "success":
                //        result.TradeStatus = EnumTradeStatus.Success;
                //        break;
                //    default:
                //        result.TradeStatus = EnumTradeStatus.Fail;
                //        break;
                //}
                #endregion

                return result;
            }
            catch (Exception err)
            {
                PaymentHelper.Log($"lfpay.Exception\r\n{err.Message}");
                return new ModelQuery()
                {
                    Errors = { err.Message }
                };
            }
        }
        #endregion


        #region 充值查询[RechargeQuery]
        /// <summary>
        /// 常量：查询请求待签模板（签名用）
        /// </summary>
        protected override string RechargeQueryRequestSignTemplate
        {
            get { return "{sys_order_id}"; }
        }

        /// <summary>
        /// 常量：查询请求数据模板（提交用）
        /// </summary>
        protected override string RechargeQueryRequestDataTemplate
        {
            get { return "{sys_order_id}"; }
        }

        /// <summary>
        /// 常量：查询回复待签模板（验签用）
        /// </summary>
        protected override string RechargeQueryResponseSignTemplate
        {
            get { return "不需要签名"; }
        }

        /// <summary>
        /// 方法：充值查询
        /// </summary>
        /// <param name="shop">商户编号</param>
        /// <param name="order">商户订单编号</param>
        /// <param name="key">密钥</param>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        public ModelQuery RechargeQuery(string shop, string order, string key, string url)
        {
            throw new Exception("功能暂不开放");

            var result = new ModelQuery();

            #region 准备数据
            var sign = PaymentHelper.SignByMD5(RechargeQueryRequestSignTemplate
                .Replace("{version}", "1")
                .Replace("{appId}", shop)
                .Replace("{orderNo}", order), "GBK").ToUpper();

            var data = RechargeQueryRequestDataTemplate
                .Replace("{version}", "1")
                .Replace("{appId}", shop)
                .Replace("{orderNo}", order)
                .Replace("{sign}", sign);
            #endregion

            #region 提交请求
            var postback = PaymentHelper.Post(url, data);
            #endregion

            #region 解析结果
            //（以Json为例）
            Newtonsoft.Json.Linq.JObject response = null;
            try
            {
                response = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postback);
            }
            catch (Exception err)
            {
                //result.Errors.Add(err.Message);
                result.Errors.Add(postback);
                return result;
            }

            // 错误信息
            result.QueryStatus = Convert.ToString(response["code"]) != "fail";
            if (!result.QueryStatus)
            {
                result.Errors.Add(Convert.ToString(response["message"]));
                return result;
            }
            #endregion

            #region 验签
            /*var tmp = RechargeQueryResponseSignTemplate
                .Replace("{amount}", Convert.ToString(response["amount"]))
                .Replace("{memberid}", Convert.ToString(response["memberid"]))
                .Replace("{orderid}", Convert.ToString(response["orderid"]))
                .Replace("{returncode}", Convert.ToString(response["returncode"]))
                .Replace("{time_end}", Convert.ToString(response["time_end"]))
                .Replace("{trade_state}", Convert.ToString(response["trade_state"]))
                .Replace("{transaction_id}", Convert.ToString(response["transaction_id"]))
                .Replace("{key}", key);
            var sign_resp = PaymentHelper.SignByMD5(tmp).ToUpper();
            if (sign_resp != Convert.ToString(response["sign"]))
            {
                return new ModelQuery()
                {
                    Errors = { "验签失败" }
                };
            }*/
            #endregion

            #region  查询状态
            /*result.QueryStatus = response["resultcode"].ToString() == "00";
            if (!result.QueryStatus)
            {
                result.Errors.Add(postback);
                return result;
            }*/
            #endregion

            #region 封装交易信息
            result.TradeNo = Convert.ToString(response["payTrxNo"]);
            var trade_time = DateTime.Now;
            var order_tmp = Convert.ToString(response["orderid"]);
            var order_time = Convert.ToDateTime(order_tmp.Substring(0, 4)
                + "-" + order_tmp.Substring(4, 2)
                + "-" + order_tmp.Substring(6, 2) // 年-月-日
                + " " + order_tmp.Substring(8, 2)
                + ":" + order_tmp.Substring(10, 2)
                + ":" + order_tmp.Substring(12, 2)); // 时:分:秒
            if (trade_time < order_time) trade_time = DateTime.Now;
            result.OrderNo = Convert.ToString(response["orderNo"]);
            result.OrderTime = order_time;
            result.TradeTime = trade_time;
            result.Amount = Convert.ToDecimal(response["amount"]) * 100;
            #endregion

            #region 交易状态
            switch (Convert.ToString(response["resultcode"]))
            {
                case "0000":
                    result.TradeStatus = EnumTradeStatus.Success;
                    break;
                case "P888":    // 交易失败
                case "P333":    // 订单已取消
                case "J003":    // 审核失败
                    result.TradeStatus = EnumTradeStatus.Fail;
                    break;
                default:
                    if (order_time.AddDays(1) < DateTime.Now)
                    {
                        // 超过一天未支付的订单自动以支付失败结束
                        result.TradeStatus = EnumTradeStatus.Fail;
                    }
                    else
                    {
                        result.TradeStatus = EnumTradeStatus.Paying;
                    }
                    break;
            }
            #endregion

            return result;
        }
        #endregion


        #region 代付[Withdraw]
        /// <summary>
        /// 常量：代付请求待签模板（签名用）
        /// </summary>
        protected override string WithdrawRequestSignTemplate
        {
            get { return "account_name={account_name}&account_number={account_number}&bank_code={bank_code}&merchant_code={merchant_code}&order_amount={order_amount}&order_time={order_time}&trade_no={trade_no}&key={key}"; }
        }

        /// <summary>
        /// 常量：代付请求数据模板（提交用）
        /// </summary>
        protected override string WithdrawRequestDataTemplate
        {
            get { return "account_name={account_name}&account_number={account_number}&bank_code={bank_code}&merchant_code={merchant_code}&order_amount={order_amount}&order_time={order_time}&trade_no={trade_no}&sign={sign}"; }
        }

        /// <summary>
        /// 常量：代付通知待签模板（签名用）
        /// </summary>
        protected override string WithdrawNotifySignTemplate
        {
            get { return "bank_status={bank_status}&is_success={is_success}&order_id={order_id}&key={key}"; }
        }

        /// <summary>
        /// 方法：代付
        /// </summary>
        /// <param name="shop">商户编号</param>
        /// <param name="order">商户订单编号</param>
        /// <param name="amount">商户订单金额</param>
        /// <param name="time">商户订单时间</param>
        /// <param name="bank">银行编号</param>
        /// <param name="user">用户在银行开户名</param>
        /// <param name="card">银行卡号（或账号）</param>
        /// <param name="key">签名密钥</param>
        /// <param name="pwd">支付密码</param>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        public string Withdraw(string shop, string order, decimal amount, DateTime time, string bank, string user, string card, string key, string pwd, string url)
        {
            throw new Exception("功能暂不开放");

            #region 准备数据
            var sign = PaymentHelper.SignByMD5(RechargeRequestSignTemplate
                .Replace("{account_name}", bank)
                .Replace("{account_number}", card)
                .Replace("{bank_code}", bank)
                .Replace("{merchant_code}", shop)
                .Replace("{order_amount}", amount.ToString("0.00"))
                .Replace("{order_time}", time.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{trade_no}", order)
                .Replace("{key}", key));

            var data = RechargeRequestDataTemplate
                .Replace("{account_name}", bank)
                .Replace("{account_number}", card)
                .Replace("{bank_code}", bank)
                .Replace("{merchant_code}", shop)
                .Replace("{order_amount}", amount.ToString("0.00"))
                .Replace("{order_time}", time.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{trade_no}", order)
                .Replace("{sign}", sign);
            #endregion

            #region 封装表单
            return PaymentHelper.BuildForm(data, url);
            #endregion
        }

        /// <summary>
        /// 方法：代付通知
        /// </summary>
        /// <param name="form">接收到的原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public ModelQuery WithdrawNotify(string form, string key)
        {
            throw new Exception("功能暂不开放");

            var data = PaymentHelper.StringToList(form);

            if (!Convert.ToBoolean(data.Find(p => p.Key == "is_success").Value))
            {
                return new ModelQuery()
                {
                    Errors = { data.Find(p => p.Key == "errror_msg").Value }
                };
            }

            #region 验签
            var sign = PaymentHelper.SignByMD5(RechargeRequestSignTemplate
                .Replace("{bank_status}", data.Find(p => p.Key == "bank_status").Value)
                .Replace("{is_success}", data.Find(p => p.Key == "is_success").Value)
                .Replace("{order_id}", data.Find(p => p.Key == "order_id").Value)
                .Replace("{key}", key));
            if (sign != data.Find(p => p.Key == "sign").Value)
            {
                return new ModelQuery()
                {
                    Errors = { "验签失败" }
                };
            }
            #endregion

            #region 封装数据
            var result = new ModelQuery();
            result.OrderNo = data.Find(p => p.Key == "order_id").Value;
            result.TradeNo = data.Find(p => p.Key == "order_id").Value;
            result.QueryStatus = true;

            var status = data.Find(p => p.Key == "bank_status").Value;
            switch (status)
            {
                case "2":
                    result.TradeStatus = EnumTradeStatus.Success;
                    break;
                case "3":
                    result.TradeStatus = EnumTradeStatus.Fail;
                    break;
                case "1":
                    result.TradeStatus = EnumTradeStatus.Paying;
                    break;
                default:
                    result.TradeStatus = EnumTradeStatus.Unknow;
                    break;
            }
            #endregion

            return result;
        }
        #endregion


        #region 代付查询[WithdrowQuery]
        /// <summary>
        /// 常量：查询请求待签模板（签名用）
        /// </summary>
        protected override string WithdrawQueryRequestSignTemplate
        {
            get { return "merchant_code={merchant_code}&order_no={order_no}&now_date={now_date}&key={key}"; }
        }

        /// <summary>
        /// 常量：查询请求数据模板（提交用）
        /// </summary>
        protected override string WithdrawQueryRequestDataTemplate
        {
            get { return "merchant_code={merchant_code}&order_no={order_no}&now_date={now_date}&sign={sign}"; }
        }

        /// <summary>
        /// 常量：查询回复待签模板（验签用）
        /// </summary>
        protected override string WithdrawQueryResponseSignTemplate
        {
            get { return "bank_status={bank_status}&is_success={is_success}&order_id={order_id}&key={key}"; }
        }

        /// <summary>
        /// 方法：代付查询
        /// </summary>
        /// <param name="shop">商户编号</param>
        /// <param name="order">商户订单编号</param>
        /// <param name="key">密钥</param>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        public ModelQuery WithdrawQuery(string shop, string order, string key, string url)
        {
            throw new Exception("功能暂不开放");

            var result = new ModelQuery();

            #region 准备数据
            var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var sign = PaymentHelper.SignByMD5(WithdrawQueryRequestSignTemplate
                .Replace("{merchant_code}", shop)
                .Replace("{order_no}", order)
                .Replace("{now_date}", date)
                .Replace("{key}", key));

            var data = WithdrawQueryRequestDataTemplate
                .Replace("{merchant_code}", shop)
                .Replace("{order_no}", order)
                .Replace("{now_date}", date)
                .Replace("{sign}", sign);
            #endregion

            #region 提交请求
            var postback = PaymentHelper.Post(url, data);
            #endregion

            #region 解析结果（以Json为例）
            Newtonsoft.Json.Linq.JObject response = null;
            try
            {
                response = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(postback);
            }
            catch (Exception err)
            {
                result.Errors.Add(err.Message);
                return result;
            }
            #endregion

            #region 验签
            #endregion

            #region  查询状态
            result.QueryStatus = Convert.ToBoolean(response["is_success"]);
            if (!result.QueryStatus)
            {
                result.Errors.Add(Convert.ToString(response["errror_msg"]));
                return result;
            }
            #endregion

            #region 交易状态
            switch (Convert.ToString(response["bank_status"]))
            {
                case "2":
                    result.TradeStatus = EnumTradeStatus.Success;
                    break;
                case "3":
                    result.TradeStatus = EnumTradeStatus.Fail;
                    break;
                case "1":
                    result.TradeStatus = EnumTradeStatus.Paying;
                    break;
                default:
                    result.TradeStatus = EnumTradeStatus.Unknow;
                    break;
            }
            #endregion

            #region 封装交易信息
            result.OrderNo = Convert.ToString(response["order_id"]);
            result.OrderTime = Convert.ToDateTime(date);
            result.TradeNo = Convert.ToString(response["order_id"]);
            result.TradeTime = Convert.ToDateTime(date);
            #endregion

            return result;
        }
        #endregion
    }
}
