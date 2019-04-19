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

        public virtual IActionResult BalanceReport()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var model = logisticsReportModelFactory.PrepareReportBalanceSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult BalanceReport(ReportBalanceSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedKendoGridJson();

            var model = logisticsReportModelFactory.PrepareReportBalanceListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult StatementReport()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var model = logisticsReportModelFactory.PrepareReportStatementSearchModel();

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult StatementReport(ReportStatementSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedKendoGridJson();

            var model = logisticsReportModelFactory.PrepareReportStatementListModel(searchModel);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult StatementAggregates(ReportStatementSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedKendoGridJson();

            var model = logisticsReportModelFactory.PrepareReportStatementAggrModel(searchModel);

            return Json(model);
        }

        #endregion
    }
}
