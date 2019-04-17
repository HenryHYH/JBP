using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportTripSearchModel : BaseSearchModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchTripShippingTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? TripShippingTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchTripShippingTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? TripShippingTimeTo { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchOrderConsignmentTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? OrderConsignmentTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchOrderConsignmentTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? OrderConsignmentTimeTo { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchDriverName")]
        public string DriverName { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchCarLicense")]
        public string CarLicense { get; set; }

        #endregion
    }
}
