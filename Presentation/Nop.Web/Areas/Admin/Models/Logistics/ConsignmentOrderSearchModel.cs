using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ConsignmentOrderSearchModel : BaseSearchModel
    {
        #region Ctor

        public ConsignmentOrderSearchModel()
        {
            AvailableShipmentMethods = new List<SelectListItem>();
            AvailableOrderStatuses = new List<SelectListItem>();
            AvailablePaymentStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchSerialNum")]
        public string SearchSerialNum { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchConsignmentTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? SearchConsignmentTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchConsignmentTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? SearchConsignmentTimeTo { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchShipmentMethod")]
        public ShipmentMethod? SearchShipmentMethod { get; set; }

        public IList<SelectListItem> AvailableShipmentMethods { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchOrderStatus")]
        public IList<int> SearchOrderStatuses { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchPaymentStatus")]
        public IList<int> SearchPaymentStatuses { get; set; }

        public IList<SelectListItem> AvailablePaymentStatuses { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchStartPoint")]
        public string SearchStartPoint { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchTerminal")]
        public string SearchTerminal { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchConsignor")]
        public string SearchConsignor { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchConsignee")]
        public string SearchConsignee { get; set; }

        public int TripId { get; set; }

        public bool? SearchNoRelatedTrip { get; set; }

        #endregion
    }
}
