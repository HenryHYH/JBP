using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Logistics;
using Nop.Services.ExportImport;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Logistics;
using Nop.Services.Security;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using System;
using System.Collections.Generic;
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
        private readonly IImportManager importManager;
        private readonly IExportManager exportManager;

        #endregion

        #region Ctor

        public ConsignmentOrderController(
            IPermissionService permissionService,
            IConsignmentOrderFactory consignmentOrderFactory,
            IConsignmentOrderService consignmentOrderService,
            ICustomerActivityService customerActivityService,
            ILocalizationService localizationService,
            IImportManager importManager,
            IExportManager exportManager)
        {
            this.permissionService = permissionService;
            this.consignmentOrderFactory = consignmentOrderFactory;
            this.consignmentOrderService = consignmentOrderService;
            this.customerActivityService = customerActivityService;
            this.localizationService = localizationService;
            this.importManager = importManager;
            this.exportManager = exportManager;
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

            var model = consignmentOrderFactory.PrepareModel(null, null);

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

        [HttpPost, ActionName("Edit"), FormValueRequired("StartOrder")]
        public virtual IActionResult StartOrder(int id)
        {
            return ChangeOrderStatus(id, OrderStatus.进行中);
        }

        [HttpPost, ActionName("Edit"), FormValueRequired("CompleteOrder")]
        public virtual IActionResult CompleteOrder(int id)
        {
            return ChangeOrderStatus(id, OrderStatus.已完成);
        }

        [HttpPost, ActionName("Edit"), FormValueRequired("CancelOrder")]
        public virtual IActionResult CancelOrder(int id)
        {
            return ChangeOrderStatus(id, OrderStatus.已取消);
        }

        protected virtual IActionResult ChangeOrderStatus(int id, OrderStatus orderStatus)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var entity = consignmentOrderService.Get(id);
            if (null == entity || entity.Deleted)
                return RedirectToAction("List");

            try
            {
                consignmentOrderService.ChangeOrderStatus(entity, orderStatus);
            }
            catch (Exception ex)
            {
                ErrorNotification(ex, false);
            }

            return RedirectToAction("Edit", new { id });
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

        [HttpPost]
        public virtual IActionResult ImportFromXlsx(IFormFile importexcelfile)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            try
            {
                if (null != importexcelfile && importexcelfile.Length > 0)
                {
                    importManager.ImportConsignmentOrdersFromXlsx(importexcelfile.OpenReadStream());
                }
                else
                {
                    ErrorNotification(localizationService.GetResource("Admin.Common.UploadFile"));
                    return RedirectToAction("List");
                }

                SuccessNotification(localizationService.GetResource("Admin.Logistics.ConsignmentOrders.Imported"));

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex);
                return RedirectToAction("List");
            }
        }

        [HttpPost, ActionName("List")]
        [FormValueRequired("exportexcel-all")]
        public virtual IActionResult ExportExcelAll(ConsignmentOrderSearchModel searchModel)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var list = consignmentOrderService.GetAll(
                shipmentMethod: searchModel.SearchShipmentMethod,
                startPoint: searchModel.SearchStartPoint,
                terminal: searchModel.SearchTerminal,
                consignor: searchModel.SearchConsignor,
                consignee: searchModel.SearchConsignee,
                tripId: searchModel.TripId,
                noRelatedTrip: searchModel.SearchNoRelatedTrip);

            try
            {
                var bytes = exportManager.ExportConsignmentOrdersToXlsx(list);

                return File(bytes, MimeTypes.TextXlsx, $"{localizationService.GetResource("Admin.Logistics.ConsignmentOrder")}.xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex);
                return RedirectToAction("List");
            }
        }

        [HttpPost]
        public virtual IActionResult ExportExcelSelected(string selectedIds)
        {
            if (!permissionService.Authorize(StandardPermissionProvider.ManageConsignmentOrders))
                return AccessDeniedView();

            var list = new List<ConsignmentOrder>();
            if (!string.IsNullOrEmpty(selectedIds))
            {
                var ids = selectedIds
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x))
                    .ToArray();
                list.AddRange(consignmentOrderService.Get(ids));
            }

            try
            {
                var bytes = exportManager.ExportConsignmentOrdersToXlsx(list);

                return File(bytes, MimeTypes.TextXlsx, $"{localizationService.GetResource("Admin.Logistics.ConsignmentOrder")}.xlsx");
            }
            catch (Exception ex)
            {
                ErrorNotification(ex);
                return RedirectToAction("List");
            }
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