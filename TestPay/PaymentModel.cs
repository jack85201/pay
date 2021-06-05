using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Payment
{
    public class ModeBase
    {
        public List<string> Errors = new List<string>();
    }

    /// <summary>
    /// 查询模型
    /// </summary>
    public class ModelQuery : ModeBase
    {
        public string OrderNo { get; set; }

        public DateTime OrderTime { get; set; }

        public string TradeNo { get; set; }

        public DateTime TradeTime { get; set; }

        public decimal Amount { get; set; }

        public bool QueryStatus { get; set; }

        public EnumTradeStatus TradeStatus { get; set; }
    }

    /// <summary>
    /// 交易状态
    /// </summary>
    public enum EnumTradeStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success,
        /// <summary>
        /// 失败
        /// </summary>
        Fail,
        /// <summary>
        /// 交易中
        /// </summary>
        Paying,
        /// <summary>
        /// 未知状态
        /// </summary>
        Unknow
    }
}
