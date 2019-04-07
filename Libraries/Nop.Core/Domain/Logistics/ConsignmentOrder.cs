using System;
using System.Collections.Generic;

namespace Nop.Core.Domain.Logistics
{
    /// <summary>
    /// 运货单
    /// </summary>
    public partial class ConsignmentOrder : BaseEntity
    {
        private ICollection<Goods> goods;

        /// <summary>
        /// 货物
        /// </summary>
        public virtual ICollection<Goods> Goods
        {
            get => goods ?? (goods = new List<Goods>());
            protected set => goods = value;
        }

        /// <summary>
        /// 发货人ID
        /// </summary>
        public int ConsignorId { get; set; }

        /// <summary>
        /// 发货人
        /// </summary>
        public virtual ConsignmentUser Consignor { get; set; }

        /// <summary>
        /// 收货人ID
        /// </summary>
        public int ConsigneeId { get; set; }

        /// <summary>
        /// 收货人
        /// </summary>
        public virtual ConsignmentUser Consignee { get; set; }

        /// <summary>
        /// 发货方式
        /// </summary>
        public ShipmentMethod ShipmentMethod { get; set; }

        /// <summary>
        /// 始发站
        /// </summary>
        public string StartPoint { get; set; }

        /// <summary>
        /// 终点站
        /// </summary>
        public string Terminal { get; set; }

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
