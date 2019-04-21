using System;

namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 货品
    /// </summary>
    public partial class Goods : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        public virtual ConsignmentOrder Order { get; set; }
    }
}
