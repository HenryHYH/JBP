using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportStatementSearchModel : BaseSearchModel
    {
        #region Ctor

        public ReportStatementSearchModel()
        {
            OrderStatuses = new List<int>();
            AvailableOrderStatuses = new List<SelectListItem>();
            PaymentStatuses = new List<int>();
            AvailablePaymentStatuses = new List<SelectListItem>();
            ShippingStatuses = new List<int>();
            AvailableShippingStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.LogisticsReports.Orders.Statement.SearchConsigneeName")]
        public string ConsigneeName { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Orders.Statement.SearchOrderConsignmentTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? OrderConsignmentTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Orders.Statement.SearchOrderConsignmentTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? OrderConsignmentTimeTo { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Orders.Statement.SearchOrderStatus")]
        public IList<int> OrderStatuses { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Orders.Statement.SearchPaymentStatus")]
        public IList<int> PaymentStatuses { get; set; }

        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Orders.Statement.SearchShippingStatus")]
        public IList<int> ShippingStatuses { get; set; }

        public IList<SelectListItem> AvailableShippingStatuses { get; set; }

        #endregion
    }
}
