namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 物流货物
    /// </summary>
    public partial class ConsignmentGoods : BaseEntity
    {
        /// <summary>
        /// 货物ID
        /// </summary>
        public int GoodsId { get; set; }

        /// <summary>
        /// 货物
        /// </summary>
        public virtual Goods Goods { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 运货单ID
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 运货单
        /// </summary>
        public virtual ConsignmentOrder Order { get; set; }
    }
}
