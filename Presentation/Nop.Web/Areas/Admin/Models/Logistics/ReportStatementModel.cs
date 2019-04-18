using Nop.Core.Domain.Logistics;
using Nop.Web.Framework.Models;
using System;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportStatementModel : BaseNopEntityModel
    {
        #region Properties

        public string SerialNum { get; set; }

        public DateTime ConsignmentTime { get; set; }

        public ReportUserModel Consignee { get; set; }

        public decimal? Receivable { get; set; }

        public decimal? Receipts { get; set; }

        public decimal Balance
        {
            get
            {
                return (Receivable ?? 0) - (Receipts ?? 0);
            }
        }

        public PaymentStatus PaymentStatus { get; set; }

        public string PaymentStatusName { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public string OrderStatusName { get; set; }

        public ShippingStatus? ShippingStatus { get; set; }

        public string ShippingStatusName { get; set; }

        #endregion
    }
}
