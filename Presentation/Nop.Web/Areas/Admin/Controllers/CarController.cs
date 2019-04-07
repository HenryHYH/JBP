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
    public partial class CarController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly ICarFactory carFactory;
        private readonly ICarService carService;
        private readonly ICustomerActivityService customerActivityService;
        private readonly ILocalizationService localizationService;

        #endregion

        #region Ctor

        public CarController(
            IPermissionService permissionService,
            ICarFactory carFactory,
            ICarService carService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            this.permissionService = permissionService;
            this.carFactory = carFactory;
            this.carService = carService;
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
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            var model = carFactory.PrepareSearchModel(new CarSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(CarSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            var model = carFactory.PrepareListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            var model = carFactory.PrepareModel(new CarModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(CarModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity<Car>();
                carService.Insert(entity);

                // activity log
                customerActivityService.InsertActivity("AddNewCar",
                    string.Format(localizationService.GetResource("ActivityLog.AddNewCar"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Car.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = carFactory.PrepareModel(model, null, true);

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            var entity = carService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            var model = carFactory.PrepareModel(null, entity);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(CarModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            var entity = carService.Get(model.Id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                entity = model.ToEntity(entity);
                carService.Update(entity);

                customerActivityService.InsertActivity("EditCar",
                    string.Format(localizationService.GetResource("ActivityLog.EditCar"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Car.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = carFactory.PrepareModel(model, entity, true);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageCars))
                return AccessDeniedView();

            var entity = carService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            carService.Delete(entity);

            customerActivityService.InsertActivity("DeleteCar",
                string.Format(localizationService.GetResource("ActivityLog.DeleteCar"), entity.Id),
                entity);

            SuccessNotification(localizationService.GetResource("Admin.Logistics.Car.Deleted"));

            return RedirectToAction("List");
        }

        #endregion
    }
}