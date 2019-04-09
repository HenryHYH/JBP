using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ConsignmentOrderSearchModel : BaseSearchModel
    {
        #region Ctor

        public ConsignmentOrderSearchModel()
        {
            AvailableShipmentMethods = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.List.SearchShipmentMethod")]
        public ShipmentMethod? SearchShipmentMethod { get; set; }

        public IList<SelectListItem> AvailableShipmentMethods { get; set; }

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
