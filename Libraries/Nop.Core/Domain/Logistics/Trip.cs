using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.Logistics
{
    public partial class Trip : BaseEntity
    {
        private ICollection<ConsignmentOrder> orders;

        /// <summary>
        /// 托运单
        /// </summary>
        public virtual ICollection<ConsignmentOrder> Orders
        {
            get => orders ?? (orders = new List<ConsignmentOrder>());
            protected set => orders = value;
        }

        /// <summary>
        /// 司机ID
        /// </summary>
        public int DriverId { get; set; }

        /// <summary>
        /// 司机
        /// </summary>
        public virtual Driver Driver { get; set; }

        /// <summary>
        /// 车辆ID
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// 车辆
        /// </summary>
        public virtual Car Car { get; set; }

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
