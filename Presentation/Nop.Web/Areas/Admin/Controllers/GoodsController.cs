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
    public partial class GoodsController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly IGoodsFactory goodsFactory;
        private readonly IGoodsService goodsService;
        private readonly ICustomerActivityService customerActivityService;
        private readonly ILocalizationService localizationService;

        #endregion

        #region Ctor

        public GoodsController(
            IPermissionService permissionService,
            IGoodsFactory goodsFactory,
            IGoodsService goodsService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            this.permissionService = permissionService;
            this.goodsFactory = goodsFactory;
            this.goodsService = goodsService;
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
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            var model = goodsFactory.PrepareSearchModel(new GoodsSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(GoodsSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            var model = goodsFactory.PrepareListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            var model = goodsFactory.PrepareModel(new GoodsModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(GoodsModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity<Goods>();
                goodsService.Insert(entity);

                // activity log
                customerActivityService.InsertActivity("AddNewGoods",
                    string.Format(localizationService.GetResource("ActivityLog.AddNewGoods"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Goods.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = goodsFactory.PrepareModel(model, null, true);

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            var entity = goodsService.Get(id, false);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            var model = goodsFactory.PrepareModel(null, entity);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(GoodsModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            var entity = goodsService.Get(model.Id, false);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                entity = model.ToEntity(entity);
                goodsService.Update(entity);

                customerActivityService.InsertActivity("EditGoods",
                    string.Format(localizationService.GetResource("ActivityLog.EditGoods"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.Goods.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = goodsFactory.PrepareModel(model, entity, true);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageGoods))
                return AccessDeniedView();

            var entity = goodsService.Get(id, false);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            goodsService.Delete(entity);

            customerActivityService.InsertActivity("DeleteGoods",
                string.Format(localizationService.GetResource("ActivityLog.DeleteGoods"), entity.Id),
                entity);

            SuccessNotification(localizationService.GetResource("Admin.Logistics.Goods.Deleted"));

            return RedirectToAction("List");
        }

        #endregion
    }
}