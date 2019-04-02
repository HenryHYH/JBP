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
        /// 是否已删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CTime { get; set; }

        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? UTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DTime { get; set; }
    }
}
