using Common.Payment.UtilsRSA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Payment
{
    public class hhpay : PaymentBase, IPayment
    {
        #region 充值[Recharge]
        // 服务端公钥
        private static string serverPublicKey
        {
            get
            {
                return @"MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA6eByWpXB/G2ceG/SJcFm
gGkSuNh4E5ZJG+GoDzbNO9kODnaHCWY6EDw6a7hL8vWsnHG/U2b+sxnDHZtkY74B
oGDf1txl/Mzb6Abqtzd+Ik8Axt3LAlNMz/Y/FBjmP5bmJOFTq1MU5vD2RNe3/Bi3
JnlBzv8Cn5Dg3dZ8OhgdiRmRmLN50flARkX3tlPedOz363rUGF11Fhu6TtkhUh79
7X1Gd2vurSCFtINsKnBmw3wy2EXL2dU/x5eH1NyWUyGSZAsoYYFfPAbyKwLLlCgk
8dqMi3Kh7xvqe28/BvwPdmrMnv0PG63ydHRW7xvvgva+f6s2sb2ChzB/JJxecsqs
yqvbJRfT7jdL7ec0LVb4PsggTV1zkC5ib/6FqHtcGHiWI1BoP7NYBcdeTJF6mBZl
pM7ohaX6V4nUbvti8z7GQKBboUsKTxdOc+rLEEtBL5mmCpeBNbWfwQX309b2gPqi
mTkyY8xXNYF5P/YJIuxUX1jzjjYyVOgVJwORpkGDOx3m/fN/fxJoi3csAxXqvX+Z
KGJBf4aJcZdLDsPzaZVhGNSNxYVJ2rlnAbzi1LdhHeVLmq6ZBRv7mUnToiO8sUAn
xYXDBPNrS+NNStEOjYhgdNij9afAdWAr6ORtI2QjYFhmQFNlfo0Ctd+Xud69GGsz
0MAhNty6JV3qj9Tr308WyQUCAwEAAQ==";
            }
        }
        // 商户公钥
        private static string publicKey
        {
            get
            {
                return @"MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAsEjVhVD1BVRQhT2TlpMG
8atNHAHMNsJqrn9k18hUq/OQmARnle49s0Va7JDpO72R9HuYRzRq1SZSrUclC2Vp
Toq1zlNhg8+yRv5dLfCRU9r2JbRTWYoWiZoQn7sRPWVrW9QNXexKdXepHzRNvYlh
fXmURegN9F34uAE4I6n/aVAnJMCSUfRS2AEjdBCOokBeTv6t9a0uBBxwNLLWNDtM
j46032uFsoc2QduLqa0zbbmUeFzIEkmnGg9dsn67gVs1lE/j8WnFHG+I6g89AwPj
MVjBiGbio46kloqRojKQf+RuiV7GrSES+ctFbN3jyWmFkXur0Ks3+Nx5C31HtHNv
4VOe6ly1E5CFMsVyh1gfnOQSgJMhFpBQrHfGOD6ZyvCuUOepU7+NsQh2+Mi5rr4D
BVlqyZRcRzeJnNHK9XE42fuI5tDe5bSuJePgEhL7loWQyj4ZYxKgQW3Tl0z2AhAh
thliBnGWhOEyJ9XeSyNuo1N+vxeoqcSzEk4VPdLUShgrRL9IaWrGnghDCSekcg77
fYXZDFYiybTQnubkCear7ZuFp92anOdgB4EsAYxJRKbT7rgj2MjrV5JrhHzvnzPn
H+7XL8xFuVDwbIHVJtihH5Z04Ak1D77U4VWOPUXyZO0uw5+WyZmnfsfzDdfB42NU
X5kae+G2OGBEO2zejE5yjWECAwEAAQ==";
            }
        }
        // 商户私钥
        private static string privateKey
        {
            get
            {
                return
                    @"MIIJKgIBAAKCAgEAsEjVhVD1BVRQhT2TlpMG8atNHAHMNsJqrn9k18hUq/OQmARn
le49s0Va7JDpO72R9HuYRzRq1SZSrUclC2VpToq1zlNhg8+yRv5dLfCRU9r2JbRT
WYoWiZoQn7sRPWVrW9QNXexKdXepHzRNvYlhfXmURegN9F34uAE4I6n/aVAnJMCS
UfRS2AEjdBCOokBeTv6t9a0uBBxwNLLWNDtMj46032uFsoc2QduLqa0zbbmUeFzI
EkmnGg9dsn67gVs1lE/j8WnFHG+I6g89AwPjMVjBiGbio46kloqRojKQf+RuiV7G
rSES+ctFbN3jyWmFkXur0Ks3+Nx5C31HtHNv4VOe6ly1E5CFMsVyh1gfnOQSgJMh
FpBQrHfGOD6ZyvCuUOepU7+NsQh2+Mi5rr4DBVlqyZRcRzeJnNHK9XE42fuI5tDe
5bSuJePgEhL7loWQyj4ZYxKgQW3Tl0z2AhAhthliBnGWhOEyJ9XeSyNuo1N+vxeo
qcSzEk4VPdLUShgrRL9IaWrGnghDCSekcg77fYXZDFYiybTQnubkCear7ZuFp92a
nOdgB4EsAYxJRKbT7rgj2MjrV5JrhHzvnzPnH+7XL8xFuVDwbIHVJtihH5Z04Ak1
D77U4VWOPUXyZO0uw5+WyZmnfsfzDdfB42NUX5kae+G2OGBEO2zejE5yjWECAwEA
AQKCAgEAiJMFCdJc4hIeh/va2i9yk80ZYndqFYquSB7eq+bC4q1C+uN+tUPsfXVg
KiNi7yvBZl5S7eeIVTbpmuGhq6CX44fHruAejpZdEm+DFVJp5UOgrDl20coQB+9a
rcWqZ5ypfm1dJcUpMrTQTCKjkBJde260FyzfktEzHqujKM5N2POGQA0Jz4CwpjTL
mOIy/zVLW3wonkvbeMfnjox3M+Q1PotbSjTtUhE7Ue6b14seawX6Jv4K61AwrBn3
h5B0CJX3fr4eZSyCz7MBqgTAShO68sJGvjynnfunPBKDHdx64vrBQKAy4HLdVrXL
Rbrq/WJJXxsnU/Uy2hVFmkcnRICize/MK0yRIK+UJwpQvB+qHrtQEPoz7biWVvS4
MNRbPjtJDv6IeIkEhASlspyDERqpBt6l6t7QYFtbkP+tplmcdpds8ekHbxyAQEre
21V4VfjGyANnHLTTkeQDoxcalcx4NQGVp9NCcpjo9/DSkn4lZeEClbUgdHjnJmAW
x4FlYK5u4N5bzAcfm52QRC8JIy1LRjbMdheZtNrgNYZtNTJFtw4IYJ20f0WRGTvT
tvnuZLrxNapbfUbiPUjETckD2VYTYw+R0XaSwUJ5G0ZCQCRasoy+kPbGnMQ91PiK
okqYI6RQubN7tUJR7IKgmygfUc/RPH0PFXF+rfZZMX2XUiKvJbkCggEBAOGpAbzZ
AKJ0CBUfomhutE7cq94NPdHncXpOU3ErX/g9OfG/25M3UxFEMq9IVUMLJ7q+mtG/
n7pgaJbkQRKngU8kXvpxWAEv41VRJuuHNmCQLT1hn2vd1zRGKqBf6NyHLNWjQ7UY
QzNaPd6ebvvNTxuc8TMpXrc1kfZrPqeOaufbXi0GH495wgVpdYf8Lf/GapQnlPG2
JIi5c3AG4pNSn9cFbHK66mnpFmxf1347GtOAEy9Wv6NGq9Y+ulexwyNVZ7ywyBPX
Rs8r90UgJpybceZ+cn4dhTQEE3pCTQj+LHIngrErPJIdD4Gc+GI85xLh3fjRxu3d
Q5BSbiD+SQBnCCMCggEBAMf8Xe/VOxIFo0J1+VWByNJI0UHdSHXypWPdVHqaDtnz
nUKSj5xaqFavKZifX9g7yxEr23vREIEGAmjKLVRb1xX0DEzEDXG/13hjWFIfCc+H
Bxg3vQ6NN/++aqdVkSeDVfVzTkr6MtGSp0Wec08Roh1HQ84ZnoU34r63OALXlXTI
tZZrlINharFC/CWVwsWDC5KJwvXyOW7SBb9QIdUdLUdNU0aLxinY15/zWKTxpIzH
6P0Necd43ZMgHxh/V7vr2pEtIDvS1VfWMmJSh5PZRkUJkZD7q+z9TvBS1OT62DUQ
evaPkR0A4ECDITQAgO6We/KHzJ3UyMfhtXbUUJsCSqsCggEBALLujiAueaBXHSmS
wfbJAVQfCnCyUzijqolunh94Y0q1UcLtfLCJB+cDCqWOV30n1ULwbFaw+XiRhZ1Q
NIEULSaSEnXHdh7K2BoIlTHhLy72Y8juJbkkWMZsdNRh+IUU8mocYU2xhWeswK5L
NnmXXIjqsx78SMxBQBIDC9VUylQ6z7wnxNLBmyUrcDR5tAfCTzRXBnV7FhEL7AsC
ipqCw5d+B3YZ3FJKrqsUfAzt1OGsBopc4OwY15y0dJGgtLjJuc4W5y9EXjNcC1/W
lIdNgs1fqm1x/vM14DPraqYc1SVXSvws6oR5YphF3XYYEKoaSyZHQNPPxQklM4oZ
18zn5b0CggEAMrgJZuLwSUDOx/M9mfcBZHt9PrQNRYSVd39Rhop9y77iGlkrPN8t
JFbmOMPqvd9kP+Wck4lRIzfP8p2b43IrWXD8kZeDRV7/GZQweRtQFKbNINI6C2+7
TQwQ5oFn/9pqOYbVy2Qq8UW9UIW//sSdFcnWI3YR4v1qby3ucaIVUHn6u7xq/Or+
8Rfo9OtHa2oxzCdOH6wf7sArHvsGqZLtwdCVlyWpy0EoZnOD/skeD2o/57W8Nd+s
ajzSRhc9u/Y0B2+nZiSxYMZ9HryJj/Chxq6HkXqSftc7Zu3K+ou3u7WDSfbdObrY
JORczmB2UowtwTO5rCmkBjVzHUD5dGafYwKCAQEAzm3SrFzqHjctNOPcNtH99jT/
Mu2gNodGca2Rui4NPFZre+NKS35IFdn9l2HhtvLB/s0iGJQyMht/XHS+YFD8FYDI
2xO+zHUZzWlQrGwqxpgWa2I4emBi4oAyFRn/vZRoLFhINXi2Z1khABhxSCmalO6G
4489Q7kcD6ei0RB6C+S/B/003LI7FaBvV8QJnhZKB7BymFPpD35Nhkct/cZifoU5
waanwsamPI7c6qbxZazg7qcz04ZPzyLLVLV6vgA6fGal37B29I6goa5sZXmzDVTz
14qL9rUKYObihzQkBS+WaWaC7Ivd/8xw7mGUlt97nPteqMWVeWV4Ay+vfQmwrQ==";
            }
        }
        /// <summary>
        /// 常量：充值请求待签模板（请求前 签名）
        /// </summary>
        protected override string RechargeRequestSignTemplate
        {
            get
            {
                return "app_id={app_id}&biz_content={biz_content}&charset={charset}&format={format}&method={method}&notify_url={notify_url}&sign_type={sign_type}&stamptime={stamptime}&version={version}";
            }
        }

        /// <summary>
        /// 常量：充值请求数据模板（提交用）
        /// </summary>
        protected override string RechargeRequestDataTemplate
        {
            get
            {
                return "app_id={app_id}&method={method}&format={format}&charset={charset}&sign_type={sign_type}&sign={sign}&stamptime={stamptime}&version={version}&notify_url={notify_url}&biz_content={biz_content}";
            }
        }
        /// <summary>
        /// 业务参数
        /// </summary>
        private string biz_Content
        {
            get
            {
                return "{\"out_trade_no\":\"{out_trade_no}\",\"pay_type\":\"{pay_type}\",\"total_amount\":\"{total_amount}\",\"timeout_express\":\"{timeout_express}\",\"time_expire\":\"{time_expire}\",\"passback_params\":\"{passback_params}\"}";

            }
        }


        /// <summary>
        /// 常量：充值通知待签模板（签名用 异步验证）
        /// </summary>
        protected override string RechargeNotifySignTemplate
        {
            get
            {
                return "app_id={app_id}&biz_content={\"out_trade_no\":\"{out_trade_no}\",\"trade_no\":\"{trade_no}\",\"total_amount\":\"{total_amount}\",\"commission\":\"{commission}\",\"balance\":\"{balance}\",\"status\":\"{status}\",\"success_time\":\"{success_time}\",\"drawee\":\"{drawee}\",\"source\":\"{source}\",\"payee\":\"{payee}\",\"payee_card\":\"{payee_card}\",\"passback_params\":\"{passback_params}\"}&charset={charset}&format={format}&method={method}&sign_type={sign_type}&stamptime={stamptime}&version={version}";
            }
        }

        /// <summary>
        /// 常量：充值通知成功标签(异步验证返回)
        /// </summary>
        public string RechargeNotifySuccess
        {
            get { return "success"; }
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
        public string jsondata = "{\"{out_trade_no}\":\"{out_trade_no}\"}";

        private string biz_Contents
        {
            get
            {
                return "{\"out_trade_no\":\"{out_trade_no}\"}";
            }
        }
        public RechargeResult Recharge(string shop, string bank, string order, DateTime time, decimal amount, string notify, string redirect, string referer, string ip, string key, string info, string url)
        {
            string jsondata = biz_Contents
                .Replace("{out_trade_no}", shop);
            #region 准备数据
            //业务参数
            string content = biz_Content
                .Replace("{out_trade_no}", order)
                .Replace("{pay_type}", bank)
                .Replace("{total_amount}", amount.ToString("0.00"))
                .Replace("{timeout_express}", "1h")
                .Replace("{time_expire}", time.ToString("yyyy-MM-dd HH:mm"))
                .Replace("{passback_params}", info);
            //加密
            string signData = RechargeRequestSignTemplate
                 .Replace("{app_id}", shop)
                 .Replace("{biz_content}", content)
                 .Replace("{charset}", "utf-8")
                 .Replace("{format}", "JSON")
                 .Replace("{method}", "trade.wap.pay")
                 .Replace("{notify_url}", notify)
                 .Replace("{sign_type}", "RSA2")
                 .Replace("{stamptime}", time.ToString("yyyy-MM-dd HH:mm:ss"))
                 .Replace("{version}", "1.0");
            string sign=SHA256WithRSA.RSASignCharSet(signData, privateKey, "UTF-8",false,"RSA2");
            string tijiaoData = RechargeRequestDataTemplate
                .Replace("{app_id}",shop)
                .Replace("{method}", "trade.wap.pay")
                .Replace("{format}", "JSON")
                .Replace("{charset}", "utf-8")
                .Replace("{sign_type}","RSA2")
                .Replace("{sign}", sign)
                .Replace("{stamptime}",time.ToString("yyyy-MM-dd HH:mm:ss"))
                .Replace("{version}","1.0")
                .Replace("{notify_url}",notify)
                .Replace("{biz_content}", content);
            PaymentHelper.Log($"hhpay.Recharge\r\nDATA\t{tijiaoData}\r\nURL\t{url}\r\nPOSTBACK\t{signData}");
            try
            {
                string jsonData = PaymentHelper.Post(url, tijiaoData);
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(jsonData);
                if (obj["code"].ToString() == "1")
                {
                    string dataUrl = SHA256WithRSA.RSADecrypt(obj["data"].ToString(), privateKey, "UTF-8", "RSA2", false);
                    obj = Newtonsoft.Json.Linq.JObject.Parse(dataUrl);
                    return new RechargeResult() { Status = true, Result = obj["cashier_url"].ToString() };
                }
                else
                {
                    return new RechargeResult() { Status = false, Result = obj["msg"].ToString() };
                }
            }
            catch(Exception ex)
            {
                return new RechargeResult() { Status = false, Result = ex.ToString() };
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
           
            //var data = PaymentHelper.JsonToList(form);
            Newtonsoft.Json.Linq.JObject data = Newtonsoft.Json.Linq.JObject.Parse(form);
            try
            {
                Newtonsoft.Json.Linq.JObject obj = Newtonsoft.Json.Linq.JObject.Parse(data["biz_content"].ToString());
                #region 验签
                string SignTemplate = RechargeNotifySignTemplate
                    .Replace("{app_id}", data["app_id"].ToString())
                    .Replace("{charset}", data["charset"].ToString())
                    .Replace("{format}", data["format"].ToString())
                    .Replace("{method}", data["method"].ToString())
                    .Replace("{sign_type}", data["sign_type"].ToString())
                    .Replace("{stamptime}", data["stamptime"].ToString())
                    .Replace("{version}", data["version"].ToString())
                    .Replace("{out_trade_no}", obj["out_trade_no"].ToString())
                    .Replace("{trade_no}", obj["trade_no"].ToString())
                    .Replace("{total_amount}", obj["total_amount"].ToString())
                    .Replace("{commission}", obj["commission"].ToString())
                    .Replace("{balance}", obj["balance"].ToString())
                    .Replace("{status}", obj["status"].ToString())
                    .Replace("{success_time}", obj["success_time"].ToString())
                    .Replace("{drawee}", obj["drawee"].ToString())
                    .Replace("{source}", obj["source"].ToString())
                    .Replace("{payee}", obj["payee"].ToString())
                    .Replace("{payee_card}", obj["payee_card"].ToString())
                    .Replace("{passback_params}", obj["passback_params"].ToString());
                //坑获取到sign   包含"+"  转 " "  用替换方式解决
                string sign =data["sign"].ToString().Replace(" ","+");

               // string sign = data["sign"].ToString();

                //验签部分
                bool flag = SHA256WithRSA.RSACheckContent(SignTemplate, sign, serverPublicKey, "UTF-8", "RSA2", false);
                PaymentHelper.Log($"hhpay.CheckSign\tmine:{sign}\tfrom:{SignTemplate}\tFlag:{flag.ToString()}");
                if (!flag)
                {
                    PaymentHelper.Log($"hhpay.flag\tmine:{flag.ToString()}\tfrom:{"验签失败"}");
                    return new ModelQuery()
                    {
                        Errors = { "验签失败" }
                    };
                }
                #endregion

                #region 封装数据
                var datetime = System.DateTime.Now;// 
                var result = new ModelQuery();
                result.OrderNo = obj["out_trade_no"].ToString();
                result.OrderTime = datetime;
                result.Amount = Convert.ToDecimal(obj["total_amount"].ToString()); //单位元
                result.TradeNo = obj["trade_no"].ToString();//平台订单号(平台)
                result.TradeTime = datetime;
                result.QueryStatus = true;
                result.TradeStatus = EnumTradeStatus.Success;
                //var status = obj["code"].ToString();// data.Find(p => p.Key == "code").Value;
                //switch (status)
                //{
                //    case "SUCCESS":
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
                PaymentHelper.Log($"hhpay.Notify\tmine:{data}");
                PaymentHelper.Log($"hhpay.Exception\r\n{err.Message}");

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
            get { return "pay_memberid={pay_memberid}&pay_orderid={pay_orderid}&key={key}"; }
        }

        /// <summary>
        /// 常量：查询请求数据模板（提交用）
        /// </summary>
        protected override string RechargeQueryRequestDataTemplate
        {
            get { return "pay_memberid={pay_memberid}&pay_orderid={pay_orderid}&pay_md5sign={sign}"; }
        }

        /// <summary>
        /// 常量：查询回复待签模板（验签用）
        /// </summary>
        protected override string RechargeQueryResponseSignTemplate
        {
            get { return "amount={amount}&memberid={memberid}&orderid={orderid}&returncode={returncode}&time_end={time_end}&trade_state={trade_state}&transaction_id={transaction_id}&key={key}"; }
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
            var result = new ModelQuery();

            #region 准备数据
            var sign = PaymentHelper.SignByMD5(RechargeQueryRequestSignTemplate
                .Replace("{pay_memberid}", shop)
                .Replace("{pay_orderid}", order)
                .Replace("{key}", key)).ToUpper();

            var data = RechargeQueryRequestDataTemplate
                .Replace("{pay_memberid}", shop)
                .Replace("{pay_orderid}", order)
                .Replace("{key}", key)
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
            result.QueryStatus = Convert.ToString(response["status"]) != "error";
            if (!result.QueryStatus)
            {
                result.Errors.Add(Convert.ToString(response["msg"]));
                return result;
            }
            #endregion

            #region 验签
            var tmp = RechargeQueryResponseSignTemplate
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
            }
            #endregion

            #region  查询状态
            result.QueryStatus = response["returncode"].ToString() == "00";
            if (!result.QueryStatus)
            {
                result.Errors.Add(postback);
                return result;
            }
            #endregion

            #region 封装交易信息
            result.TradeNo = Convert.ToString(response["transaction_id"]);
            var trade_time = Convert.ToDateTime(response["time_end"]);
            var order_time = Convert.ToDateTime(result.TradeNo.Substring(0, 4)
                + "-" + result.TradeNo.Substring(4, 2)
                + "-" + result.TradeNo.Substring(6, 2) // 年-月-日
                + " " + result.TradeNo.Substring(8, 2)
                + ":" + result.TradeNo.Substring(10, 2)
                + ":" + result.TradeNo.Substring(12, 2)); // 时:分:秒
            if (trade_time < order_time) trade_time = DateTime.Now;
            result.OrderNo = Convert.ToString(response["orderid"]);
            result.OrderTime = order_time;
            result.TradeTime = trade_time;
            result.Amount = Convert.ToDecimal(response["amount"]);
            #endregion

            #region 交易状态
            switch (Convert.ToString(response["trade_state"]))
            {
                case "SUCCESS":
                    result.TradeStatus = EnumTradeStatus.Success;
                    break;
                case "NOTPAY":
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
                default:
                    result.TradeStatus = EnumTradeStatus.Unknow;
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
