using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Payment
{
    public interface IPayment
    {
        #region 充值

        /// <summary>
        /// 充值通知成功标志
        /// </summary>
        string RechargeNotifySuccess { get; }

        /// <summary>
        /// 充值
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
        RechargeResult Recharge(string shop, string bank, string order, DateTime time, decimal amount, string notify, string redirect, string referer, string ip, string key, string info, string url);

        /// <summary>
        /// 充值通知
        /// </summary>
        /// <param name="form">接收到的原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        ModelQuery RechargeNotify(string form, string key);

        /// <summary>
        /// 充值查询
        /// </summary>
        /// <param name="shop">商户编号</param>
        /// <param name="order">商户订单编号</param>
        /// <param name="key">密钥</param>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        ModelQuery RechargeQuery(string shop, string order, string key, string url);

        #endregion

        #region 代付

        /// <summary>
        /// 代付
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
        string Withdraw(string shop, string order, decimal amount, DateTime time, string bank, string user, string card, string key, string pwd, string url);

        /// <summary>
        /// 代付通知
        /// </summary>
        /// <param name="form">接收到的原始数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        ModelQuery WithdrawNotify(string form, string key);

        /// <summary>
        /// 代付查询
        /// </summary>
        /// <param name="shop">商户编号</param>
        /// <param name="order">商户订单编号</param>
        /// <param name="key">密钥</param>
        /// <param name="url">API地址</param>
        /// <returns></returns>
        ModelQuery WithdrawQuery(string shop, string order, string key, string url);

        #endregion
    }


    public struct RechargeResult
    {
        /// <summary>
        /// 充值状态：true成功；false失败；
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 充值结果：
        /// 当Status=true时，可以是网页跳转路径或二维码；
        /// 当Status=false时，表示错误信息；
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 本地订单号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 第三方订单号
        /// </summary>
        public string TradeNo { get; set; }

    }

}
