using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class TripSearchModel : BaseSearchModel
    {
        #region Ctor

        public TripSearchModel()
        {
            AvailableShippingStatuses = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchSerialNum")]
        public string SearchSerialNum { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchCarLicense")]
        public string SearchCarLicense { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchDriverName")]
        public string SearchDriverName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchShippingStatus")]
        public IList<int> SearchShippingStatuses { get; set; }

        public IList<SelectListItem> AvailableShippingStatuses { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchStartAtFrom")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartAtFrom { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchStartAtTo")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartAtTo { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchEndAtFrom")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndAtFrom { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchEndAtTo")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndAtTo { get; set; }

        #endregion
    }
}
