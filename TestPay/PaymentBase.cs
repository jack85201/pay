using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Payment
{
    public abstract class PaymentBase
    {
        #region 充值[Recharge]
        /// <summary>
        /// 充值请求待签模板（签名用）
        /// </summary>
        protected abstract string RechargeRequestSignTemplate { get; }

        /// <summary>
        /// 充值请求数据模板（提交用）
        /// </summary>
        protected abstract string RechargeRequestDataTemplate { get; }

        /// <summary>
        /// 充值通知待签模板（签名用）
        /// </summary>
        protected abstract string RechargeNotifySignTemplate { get; }
        #endregion

        #region 充值查询[Query]

        /// <summary>
        /// 查询请求待签模板（签名用）
        /// </summary>
        protected abstract string RechargeQueryRequestSignTemplate { get; }

        /// <summary>
        /// 查询请求数据模板（提交用）
        /// </summary>
        protected abstract string RechargeQueryRequestDataTemplate { get; }

        /// <summary>
        /// 查询回复待签模板（验签用）
        /// </summary>
        protected abstract string RechargeQueryResponseSignTemplate { get; }
        #endregion

        #region 代付[Withdraw]
        /// <summary>
        /// 代付请求待签模板（签名用）
        /// </summary>
        protected abstract string WithdrawRequestSignTemplate { get; }

        /// <summary>
        /// 代付请求数据模板（提交用）
        /// </summary>
        protected abstract string WithdrawRequestDataTemplate { get; }

        /// <summary>
        /// 代付通知待签模板（签名用）
        /// </summary>
        protected abstract string WithdrawNotifySignTemplate { get; }
        #endregion

        #region 代付查询[Query]
        /// <summary>
        /// 查询请求待签模板（签名用）
        /// </summary>
        protected abstract string WithdrawQueryRequestSignTemplate { get; }

        /// <summary>
        /// 查询请求数据模板（提交用）
        /// </summary>
        protected abstract string WithdrawQueryRequestDataTemplate { get; }

        /// <summary>
        /// 查询回复待签模板（验签用）
        /// </summary>
        protected abstract string WithdrawQueryResponseSignTemplate { get; }
        #endregion

        public PaymentBase()
        {
        }
    }
}
