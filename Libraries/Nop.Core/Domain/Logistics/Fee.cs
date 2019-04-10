namespace Nop.Core.Domain.Logistics
{
    public partial class Fee : BaseEntity
    {
        /// <summary>
        /// 行程ID
        /// </summary>
        public int TripId { get; set; }

        /// <summary>
        /// 行程
        /// </summary>
        public virtual Trip Trip { get; set; }

        /// <summary>
        /// 分类ID
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public virtual FeeCategory Category { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal? Amount { get; set; }
    }
}
