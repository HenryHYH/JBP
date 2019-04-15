using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportTripSearchModel : BaseSearchModel
    {
        #region Properties

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchTripCTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? TripCTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchTripCTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? TripCTimeTo { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchOrderCTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? OrderCTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchOrderCTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? OrderCTimeTo { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchDriverName")]
        public string DriverName { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.ByDriver.SearchCarLicense")]
        public string CarLicense { get; set; }

        #endregion
    }
}
