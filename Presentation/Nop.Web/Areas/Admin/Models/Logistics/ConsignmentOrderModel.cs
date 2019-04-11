using FluentValidation.Attributes;
using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(ConsignmentOrderValidator))]
    public partial class ConsignmentOrderModel : BaseNopEntityModel
    {
        #region 

        private static readonly IDictionary<OrderStatus, IList<OrderStatus>> NEXT_AVAIABLE_STATUS = new Dictionary<OrderStatus, IList<OrderStatus>>
        {
            { OrderStatus.未开始, new[] { OrderStatus.进行中, OrderStatus.已取消 } },
            { OrderStatus.进行中, new[] { OrderStatus.已完成, OrderStatus.已取消 } },
            { OrderStatus.已完成, new[] { OrderStatus.已取消 } },
            { OrderStatus.已取消, new OrderStatus[] { } }
        };

        #endregion

        #region Ctor

        public ConsignmentOrderModel()
        {
            OrderStatus = OrderStatus.未开始;
            GoodsSearchModel = new GoodsSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.SerialNum")]
        public string SerialNum { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.StartPoint")]
        public string StartPoint { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Terminal")]
        public string Terminal { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Receivable")]
        [UIHint("DecimalNullable")]
        public decimal? Receivable { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Receipts")]
        [UIHint("DecimalNullable")]
        public decimal? Receipts { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.PaymentStatus")]
        public PaymentStatus PaymentStatus { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.PaymentStatus")]
        public string PaymentStatusName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.OrderStatus")]
        public OrderStatus OrderStatus { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.OrderStatus")]
        public string OrderStatusName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.ShipmentMethod")]
        public ShipmentMethod ShipmentMethod { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.ShipmentMethod")]
        public string ShipmentMethodName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Consignor")]
        public int ConsignorId { get; set; }

        public virtual ConsignmentUserModel Consignor { get; set; }

        [NopResourceDisplayName("Admin.Logisitcs.ConsignmentOrder.Fields.Consignee")]
        public int ConsigneeId { get; set; }

        public virtual ConsignmentUserModel Consignee { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.CTime")]
        public DateTime CTime { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Trip")]
        public int TripId { get; set; }

        public virtual TripModel Trip { get; set; }

        #region Goods

        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Name")]
        public string AddGoodsName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Price")]
        [UIHint("DecimalNullable")]
        public decimal? AddGoodsPrice { get; set; }

        public GoodsSearchModel GoodsSearchModel { get; set; }

        #endregion

        #endregion

        #region Methods

        public virtual IList<OrderStatus> GetNextAvailableStatus()
        {
            if (!NEXT_AVAIABLE_STATUS.TryGetValue(this.OrderStatus, out IList<OrderStatus> status))
                status = new List<OrderStatus>();

            return status;
        }

        public virtual PaymentStatus GetPaymentStatus()
        {
            if (!Receivable.HasValue || !Receipts.HasValue)
                return PaymentStatus.未知;
            if (0 == Receivable.Value || Receivable.Value <= Receipts.Value)
                return PaymentStatus.已付全款;
            if (0 == Receipts.Value)
                return PaymentStatus.未支付;
            else
                return PaymentStatus.部分支付;
        }

        #endregion
    }
}