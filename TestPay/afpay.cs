
using System;
using System.Collections.Generic;

namespace Common.Payment
{
    /// <summary>
    /// 安付支付1
    /// 注意：请求的价格(单位：元)
    /// </summary>
    public class afpay : PaymentBase, IPayment
    {
        #region 充值[Recharge]
        /// <summary>
        /// 常量：充值请求待签模板（请求前 签名）
        /// </summary>
        protected override string RechargeRequestSignTemplate
        {
            get
            {
                return "amount={amount}&clientip={clientip}&currency={currency}&mhtorderno={mhtorderno}&mhtuserid={mhtuserid}&notifyurl={notifyurl}&opmhtid={opmhtid}&paytype={paytype}&random={random}&signkey={key}";
            }
        }

        /// <summary>
        /// 常量：充值请求数据模板（提交用）
        /// </summary>
        protected override string RechargeRequestDataTemplate
        {
            get
            {
                return "amount={amount}&clientip={clientip}&currency={currency}&mhtorderno={mhtorderno}&mhtuserid={mhtuserid}&notifyurl={notifyurl}&opmhtid={opmhtid}&paytype={paytype}&random={random}&sign={sign}";
                // return "{\"callBackUrl\": \"{callBackUrl}\",\"mchNo\": \"{mchNo}\",\"mchOrderNo\": \"{mchOrderNo}\",\"orderMoney\":{orderMoney},\"sign\":\"{sign}\",\"payWay\":{payWay}}";
            }
        }

        /// <summary>
        /// 常量：充值通知待签模板（签名用 异步验证）
        /// </summary>
        protected override string RechargeNotifySignTemplate
        {
            get
            {
                return "currency={currency}&mhtorderno={mhtorderno}&paidamount={paidamount}&pforderno={pforderno}&random={random}&signkey={key}";
            }
        }

        /// <summary>
        /// 常量：充值通知成功标签(异步验证返回)
        /// </summary>
        public string RechargeNotifySuccess
        {
            get { return "success"; }//充值成功状态
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
            var amountStr = (amount*100).ToString() ;//分
            #region 准备数据
            // 参数拼接
            string signData = RechargeRequestSignTemplate
                   .Replace("{amount}", amountStr)
                   .Replace("{clientip}", ip)
                   .Replace("{currency}", "CNY")
                   .Replace("{mhtorderno}", order)
                   .Replace("{mhtuserid}", "66666999")
                   .Replace("{notifyurl}", notify)
                   .Replace("{opmhtid}", shop)
                   .Replace("{paytype}", bank)
                   .Replace("{random}", order)
                   .Replace("{key}", key);
            // 加密
            var sign = PaymentHelper.SignByMD5(signData).ToUpper();


            var data = RechargeRequestDataTemplate
                   .Replace("{amount}", amountStr)
                   .Replace("{clientip}", ip)
                   .Replace("{currency}", "CNY")
                   .Replace("{mhtorderno}", order)
                   .Replace("{mhtuserid}", "66666999")
                   .Replace("{notifyurl}", notify)
                   .Replace("{opmhtid}", shop)
                   .Replace("{paytype}", bank)
                   .Replace("{random}", order)
                   .Replace("{sign}", sign);
            #endregion

            PaymentHelper.Log($"afpay.Recharge\r\nDATA\t{data}\r\nURL\t{url}\r\nPOSTBACK\t{data}");
            #region 封装表单
            var postback = PaymentHelper.Post(url, data);
            try
            {
               //多层JSON提取
                Dictionary<string, object> obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(postback);
                if (obj["rtCode"].ToString() == "0")
                {
                    string datas = obj["result"].ToString();//提取JSON层的{}
                    Dictionary<string, object> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(datas);
                    string pay_url = dic["payurl"].ToString();
                    return new RechargeResult() { Status = true, Result = pay_url };
                }
                else
                {
                    return new RechargeResult() { Status = false, Result = obj["msg"].ToString() };
                }

            }
            catch (Exception ex)
            {
                return new RechargeResult() { Status = false, Result = ex.Message };
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

            var data = PaymentHelper.StringToList(form);
            try
            {
                #region 验签
                var formData = RechargeNotifySignTemplate
                    .Replace("{currency}", data.Find(m => m.Key == "currency").Value)
                    .Replace("{mhtorderno}", data.Find(m => m.Key == "mhtorderno").Value)
                    .Replace("{paidamount}", data.Find(m => m.Key == "paidamount").Value)
                    .Replace("{pforderno}", data.Find(m => m.Key == "pforderno").Value)
                    .Replace("{random}", data.Find(m => m.Key == "random").Value)
                    .Replace("{key}", key);
                var sign = PaymentHelper.SignByMD5(formData).ToUpper();
                var signFrom = data.Find(m => m.Key == "sign").Value.ToUpper();
                PaymentHelper.Log($"afpay.CheckSign\tmine:{signFrom}\tfrom:{sign}");
                if (signFrom != sign)
                {
                    return new ModelQuery()
                    {
                        Errors = { "验签失败" }
                    };
                }
                #endregion

                #region 封装数据
                var datetime = System.DateTime.Now;// 
                var result = new ModelQuery();
                result.OrderNo = data.Find(m => m.Key == "mhtorderno").Value;//商户订单号(自己).
                result.OrderTime = datetime;
                result.Amount = Convert.ToDecimal(data.Find(m => m.Key == "paidamount").Value)*100; //单位元
                result.TradeNo = data.Find(m => m.Key == "mhtorderno").Value;//平台订单号(平台)
                result.TradeTime = datetime;
                result.QueryStatus = true;
                result.TradeStatus = EnumTradeStatus.Success;
                #endregion

                return result;
            }
            catch (Exception err)
            {
                PaymentHelper.Log($"afpay.Notify\tmine:{data}");
                PaymentHelper.Log($"afpay.Exception\r\n{err.Message}");

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
                .Replace("{amount}", Convert.ToString(response["amount"]))//金额
                .Replace("{memberid}", Convert.ToString(response["memberid"]))//商户号
                .Replace("{orderid}", Convert.ToString(response["orderid"]))//订单号
                .Replace("{returncode}", Convert.ToString(response["returncode"]))
                .Replace("{time_end}", Convert.ToString(response["time_end"]))
                .Replace("{trade_state}", Convert.ToString(response["trade_state"]))
                .Replace("{transaction_id}", Convert.ToString(response["transaction_id"]))
                .Replace("{key}", key);//秘钥
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
