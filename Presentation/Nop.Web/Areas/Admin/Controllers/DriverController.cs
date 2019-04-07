using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Logistics;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Logistics;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class DriverController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly IDriverFactory driverFactory;
        private readonly IDriverService driverService;
        private readonly ICustomerActivityService customerActivityService;
        private readonly ILocalizationService localizationService;

        #endregion

        #region Ctor

        public DriverController(
            IPermissionService permissionService,
            IDriverFactory driverFactory,
            IDriverService driverService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            this.permissionService = permissionService;
            this.driverFactory = driverFactory;
            this.driverService = driverService;
            this.customerActivityService = customerActivityService;
            this.localizationService = localizationService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            var model = driverFactory.PrepareSearchModel(new DriverSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(DriverSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            var model = driverFactory.PrepareListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            var model = driverFactory.PrepareModel(new DriverModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(DriverModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity<Driver>();
                driverService.Insert(entity);

                // activity log
                customerActivityService.InsertActivity("AddNewDriver",
                    string.Format(localizationService.GetResource("ActivityLog.AddNewDriver"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Driver.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = driverFactory.PrepareModel(model, null, true);

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            var entity = driverService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            var model = driverFactory.PrepareModel(null, entity);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(DriverModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            var entity = driverService.Get(model.Id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                entity = model.ToEntity(entity);
                driverService.Update(entity);

                customerActivityService.InsertActivity("EditDriver",
                    string.Format(localizationService.GetResource("ActivityLog.EditDriver"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Driver.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = driverFactory.PrepareModel(model, entity, true);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageDrivers))
                return AccessDeniedView();

            var entity = driverService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            driverService.Delete(entity);

            customerActivityService.InsertActivity("DeleteDriver",
                string.Format(localizationService.GetResource("ActivityLog.DeleteDriver"), entity.Id),
                entity);

            SuccessNotification(localizationService.GetResource("Admin.Logistics.Driver.Deleted"));

            return RedirectToAction("List");
        }

        #endregion
    }
}