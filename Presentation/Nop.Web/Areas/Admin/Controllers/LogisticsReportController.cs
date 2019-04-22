using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logistics;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Controllers;
using System;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class LogisticsReportController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly ILogisticsReportModelFactory logisticsReportModelFactory;
        private readonly IExportManager exportManager;
        private readonly ILocalizationService localizationService;
        private readonly ITripService tripService;

        #endregion

        #region Ctor

        public LogisticsReportController(
            IPermissionService permissionService,
            ILogisticsReportModelFactory logisticsReportModelFactory,
            IExportManager exportManager,
            ILocalizationService localizationService,
            ITripService tripService)
        {
            this.permissionService = permissionService;
            this.logisticsReportModelFactory = logisticsReportModelFactory;
            this.exportManager = exportManager;
            this.localizationService = localizationService;
            this.tripService = tripService;
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

        [HttpPost, ActionName("BalanceReport")]
        [FormValueRequired("exportexcel-all")]
        public virtual IActionResult ExportExcelBalanceReport(ReportBalanceSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var list = tripService.StatisticsBalance(
                                    frequency: searchModel.Frequency,
                                    tripShippingTimeFrom: searchModel.TripShippingTimeFrom,
                                    tripShippingTimeTo: searchModel.TripShippingTimeTo);

            try
            {
                var bytes = exportManager.ExportLogisticsBalanceReportToXlsx(list);

                return File(bytes, MimeTypes.TextXlsx, $"{localizationService.GetResource("Admin.LogisticsReports.Trips.Balance")}.xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex);
                return RedirectToAction("List");
            }
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
