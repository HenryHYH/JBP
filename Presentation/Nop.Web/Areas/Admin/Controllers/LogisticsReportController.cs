using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class LogisticsReportController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly ILogisticsReportModelFactory logisticsReportModelFactory;

        #endregion

        #region Ctor

        public LogisticsReportController(
            IPermissionService permissionService,
            ILogisticsReportModelFactory logisticsReportModelFactory)
        {
            this.permissionService = permissionService;
            this.logisticsReportModelFactory = logisticsReportModelFactory;
        }

        #endregion

        #region Methods

        public virtual IActionResult TripReport()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var model = logisticsReportModelFactory.PrepareReportTripSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult TripReport(ReportTripSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedKendoGridJson();

            var model = logisticsReportModelFactory.PrepareReportTripListModel(searchModel);

            return Json(model);
        }

        #endregion
    }
}
