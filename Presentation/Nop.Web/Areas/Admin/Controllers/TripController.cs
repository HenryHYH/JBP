using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Logistics;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Logistics;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Linq;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class TripController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly ITripFactory tripFactory;
        private readonly ITripService tripService;
        private readonly ICustomerActivityService customerActivityService;
        private readonly ILocalizationService localizationService;
        private readonly IImportManager importManager;
        private readonly IConsignmentOrderFactory consignmentOrderFactory;

        #endregion

        #region Ctor

        public TripController(
            IPermissionService permissionService,
            ITripFactory tripFactory,
            ITripService tripService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IImportManager importManager,
            IConsignmentOrderFactory consignmentOrderFactory)
        {
            this.permissionService = permissionService;
            this.tripFactory = tripFactory;
            this.tripService = tripService;
            this.customerActivityService = customerActivityService;
            this.localizationService = localizationService;
            this.importManager = importManager;
            this.consignmentOrderFactory = consignmentOrderFactory;
        }

        #endregion

        #region Methods

        #region Trip

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var model = tripFactory.PrepareSearchModel(new TripSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(TripSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var model = tripFactory.PrepareListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var model = tripFactory.PrepareModel(null, null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(TripModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity<Trip>();
                tripService.Insert(entity);

                // activity log
                customerActivityService.InsertActivity("AddNewTrip",
                    string.Format(localizationService.GetResource("ActivityLog.AddNewTrip"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Trip.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = tripFactory.PrepareModel(model, null, true);

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var entity = tripService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            var model = tripFactory.PrepareModel(null, entity);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(TripModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var entity = tripService.Get(model.Id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                entity = model.ToEntity(entity);
                tripService.Update(entity);

                customerActivityService.InsertActivity("EditTrip",
                    string.Format(localizationService.GetResource("ActivityLog.EditTrip"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Trip.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = tripFactory.PrepareModel(model, entity, true);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var entity = tripService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            tripService.Delete(entity);

            customerActivityService.InsertActivity("DeleteTrip",
                string.Format(localizationService.GetResource("ActivityLog.DeleteTrip"), entity.Id),
                entity);

            SuccessNotification(localizationService.GetResource("Admin.Logistics.Trip.Deleted"));

            return RedirectToAction("List");
        }

        [HttpPost]
        public virtual IActionResult ImportFromXlsx(IFormFile importexcelfile)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            try
            {
                if (null != importexcelfile && importexcelfile.Length > 0)
                {
                    importManager.ImportTripsFromXlsx(importexcelfile.OpenReadStream());
                }
                else
                {
                    ErrorNotification(localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("List");
                }

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Trips.Imported"));

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex);
                return RedirectToAction("List");
            }
        }

        #endregion

        #region Consignment Order

        [HttpPost]
        public virtual IActionResult GetConsignmentOrder(ConsignmentOrderSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedKendoGridJson();

            var entity = tripService.Get(searchModel.TripId);

            var model = tripFactory.PrepareConsignmentOrderListModel(searchModel, entity);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult DeleteConsignmentOrder(int id, int tripId)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageTrips))
                return AccessDeniedView();

            var entity = tripService.Get(tripId)
                ?? throw new ArgumentException("Not found");

            var entityItem = entity.Orders.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException("Not found");

            tripService.DeleteConsignmentOrder(entityItem);

            return new NullJsonResult();
        }

        public virtual IActionResult AddOrderToTrip(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var model = consignmentOrderFactory.PrepareSearchModel(new ConsignmentOrderSearchModel { TripId = id });

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult AddOrderToTrip(int id, string selectedIds)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            try
            {
                var orderIds = selectedIds.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Where(x => int.TryParse(x, out int orderId))
                                        .Select(x => int.Parse(x))
                                        .ToArray();
                tripFactory.AddOrderToTrip(id, orderIds);

                SuccessNotification(string.Format(localizationService.GetResource("Admin.Logistics.Trip.Orders.AddNew.Success"), orderIds.Length));
                return RedirectToAction("Edit", new { id = id });
            }
            catch (Exception ex)
            {
                ErrorNotification(ex);
                return RedirectToAction("AddOrderToTripId", new { id = id });
            }
        }

        #endregion

        #endregion
    }
}