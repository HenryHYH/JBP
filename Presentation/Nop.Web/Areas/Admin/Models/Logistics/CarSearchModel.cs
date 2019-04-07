using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class CarSearchModel : BaseSearchModel
    {
        #region Ctor

        public CarSearchModel()
        {
            AvailableEnabledOptions = new List<SelectListItem>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.Car.List.SearchLicense")]
        public string SearchLicense { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Car.List.SearchEnabled")]
        public bool? SearchEnabled { get; set; }

        public IList<SelectListItem> AvailableEnabledOptions { get; set; }

        #endregion
    }
}
