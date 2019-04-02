using System;

namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 车辆
    /// </summary>
    public partial class Car : BaseEntity
    {
        /// <summary>
        /// 车牌
        /// </summary>
        public string License { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool Enabled { get; set; }

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
