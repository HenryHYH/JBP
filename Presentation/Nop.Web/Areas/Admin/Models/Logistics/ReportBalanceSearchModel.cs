using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Services.Common;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportBalanceSearchModel : BaseSearchModel
    {
        #region

        public ReportBalanceSearchModel()
        {
            AvailableStatisticsFrequencies = new List<SelectListItem>();
            AvailableFeeCategories = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.Balance.SearchTripShippingTimeFrom")]
        [UIHint("DateNullable")]
        public DateTime? TripShippingTimeFrom { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.Balance.SearchTripShippingTimeTo")]
        [UIHint("DateNullable")]
        public DateTime? TripShippingTimeTo { get; set; }

        [NopResourceDisplayName("Admin.LogisticsReports.Trips.Balance.SearchFrequency")]
        public StatisticsFrequency Frequency { get; set; }

        public IList<SelectListItem> AvailableStatisticsFrequencies { get; set; }

        public IList<SelectListItem> AvailableFeeCategories { get; set; }

        #endregion
    }
}
