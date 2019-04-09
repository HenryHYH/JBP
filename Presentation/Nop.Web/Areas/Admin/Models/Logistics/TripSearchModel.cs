using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class TripSearchModel : BaseSearchModel
    {
        #region Ctor

        public TripSearchModel()
        {
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchCarLicense")]
        public string SearchCarLicense { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.List.SearchDriverName")]
        public string SearchDriverName { get; set; }

        #endregion
    }
}
