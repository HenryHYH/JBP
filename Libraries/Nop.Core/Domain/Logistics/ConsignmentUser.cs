using System;

namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 物流参与人
    /// </summary>
    public partial class ConsignmentUser : BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }

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
