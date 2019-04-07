using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Logistics;
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
    public partial class ConsignmentOrderController : BaseAdminController
    {
        #region Fields

        private readonly IPermissionService permissionService;
        private readonly IConsignmentOrderFactory consignmentOrderFactory;
        private readonly IConsignmentOrderService consignmentOrderService;
        private readonly ICustomerActivityService customerActivityService;
        private readonly ILocalizationService localizationService;

        #endregion

        #region Ctor

        public ConsignmentOrderController(
            IPermissionService permissionService,
            IConsignmentOrderFactory consignmentOrderFactory,
            IConsignmentOrderService consignmentOrderService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService)
        {
            this.permissionService = permissionService;
            this.consignmentOrderFactory = consignmentOrderFactory;
            this.consignmentOrderService = consignmentOrderService;
            this.customerActivityService = customerActivityService;
            this.localizationService = localizationService;
        }

        #endregion

        #region Methods

        #region Consignment Orders

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var model = consignmentOrderFactory.PrepareSearchModel(new ConsignmentOrderSearchModel());

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult List(ConsignmentOrderSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var model = consignmentOrderFactory.PrepareListModel(searchModel);

            return Json(model);
        }

        public virtual IActionResult Create()
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var model = consignmentOrderFactory.PrepareModel(new ConsignmentOrderModel(), null);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Create(ConsignmentOrderModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = model.ToEntity<ConsignmentOrder>();
                consignmentOrderService.Insert(entity);

                // activity log
                customerActivityService.InsertActivity("AddNewConsignmentOrder",
                    string.Format(localizationService.GetResource("ActivityLog.AddNewConsignmentOrder"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.ConsignmentOrder.Added"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = consignmentOrderFactory.PrepareModel(model, null, true);

            return View(model);
        }

        public virtual IActionResult Edit(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var entity = consignmentOrderService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            var model = consignmentOrderFactory.PrepareModel(null, entity);

            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual IActionResult Edit(ConsignmentOrderModel model, bool continueEditing)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var entity = consignmentOrderService.Get(model.Id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            if (ModelState.IsValid)
            {
                entity = model.ToEntity(entity);
                consignmentOrderService.Update(entity);

                customerActivityService.InsertActivity("EditConsignmentOrder",
                    string.Format(localizationService.GetResource("ActivityLog.EditConsignmentOrder"), entity.Id),
                    entity);

                SuccessNotification(localizationService.GetResource("Admin.Logistics.ConsignmentOrder.Updated"));

                if (!continueEditing)
                    return RedirectToAction("List");

                SaveSelectedTabName();

                return RedirectToAction("Edit", new { id = entity.Id });
            }

            model = consignmentOrderFactory.PrepareModel(model, entity, true);

            return View(model);
        }

        [HttpPost]
        public virtual IActionResult Delete(int id)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var entity = consignmentOrderService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            consignmentOrderService.Delete(entity);

            customerActivityService.InsertActivity("DeleteConsignmentOrder",
                string.Format(localizationService.GetResource("ActivityLog.DeleteConsignmentOrder"), entity.Id),
                entity);

            SuccessNotification(localizationService.GetResource("Admin.Logistics.ConsignmentOrder.Deleted"));

            return RedirectToAction("List");
        }

        #endregion

        #region Goods

        [HttpPost]
        public virtual IActionResult GetGoods(GoodsSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedKendoGridJson();

            var entity = consignmentOrderService.Get(searchModel.OrderId)
                ?? throw new ArgumentException("Not found");

            var model = consignmentOrderFactory.PrepareGoodsListModel(searchModel, entity);

            return Json(model);
        }

        [HttpPost]
        public virtual IActionResult AddGoods(GoodsModel model)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            if (ModelState.IsValid)
            {
                var entity = consignmentOrderService.Get(model.OrderId);
                if (null == entity)
                    return Json(new { Result = false });

                var entityItem = model.ToEntity<Goods>();
                entityItem.CTime = DateTime.UtcNow;
                entity.Goods.Add(entityItem);
                consignmentOrderService.Update(entity);

                return Json(new { Result = true });
            }
            else
            {
                var errors = ModelState.Where(x => x.Value.Errors.Any())
                    .ToDictionary(
                        x => x.Key,
                        y => string.Join(Environment.NewLine,
                                    y.Value.Errors.Select(e =>
                                            string.IsNullOrWhiteSpace(e.ErrorMessage) ? e.Exception.Message : e.ErrorMessage))
                    );

                return Json(new { Result = false, Error = string.Join(Environment.NewLine, errors.Values) });
            }
        }

        [HttpPost]
        public virtual IActionResult DeleteGoods(int id, int orderId)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var entity = consignmentOrderService.Get(orderId)
                ?? throw new ArgumentException("Not found");

            var entityItem = entity.Goods.FirstOrDefault(x => x.Id == id)
                ?? throw new ArgumentException("Not found");

            consignmentOrderService.DeleteGoods(entityItem);

            return new NullJsonResult();
        }

        #endregion

        #endregion
    }
}