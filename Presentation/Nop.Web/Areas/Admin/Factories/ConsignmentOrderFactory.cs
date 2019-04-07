using Nop.Core.Domain.Logistics;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logistics;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Extensions;
using System;
using System.Linq;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class ConsignmentOrderFactory : IConsignmentOrderFactory
    {
        #region Fields

        private readonly IConsignmentOrderService consignmentOrderService;
        private readonly IBaseAdminModelFactory baseAdminModelFactory;
        private readonly ILocalizationService localizationService;
        private readonly IDateTimeHelper dateTimeHelper;

        #endregion

        #region Ctor

        public ConsignmentOrderFactory(
            IConsignmentOrderService consignmentOrderService,
            IBaseAdminModelFactory baseAdminModelFactory,
            ILocalizationService localizationService,
            IDateTimeHelper dateTimeHelper)
        {
            this.consignmentOrderService = consignmentOrderService;
            this.baseAdminModelFactory = baseAdminModelFactory;
            this.localizationService = localizationService;
            this.dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        public virtual ConsignmentOrderSearchModel PrepareSearchModel(ConsignmentOrderSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            baseAdminModelFactory.PrepareShipmentMethods(searchModel.AvailableShipmentMethods);

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual ConsignmentOrderListModel PrepareListModel(ConsignmentOrderSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = consignmentOrderService.GetAll(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                shipmentMethod: searchModel.SearchShipmentMethod,
                startPoint: searchModel.SearchStartPoint,
                terminal: searchModel.SearchTerminal,
                consignor: searchModel.SearchConsignor,
                consignee: searchModel.SearchConsignee,
                carLicense: searchModel.SearchCarLicense,
                driverName: searchModel.SearchDriverName);

            var model = new ConsignmentOrderListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<ConsignmentOrderModel>();

                    modelItem.ShipmentMethodName = localizationService.GetLocalizedEnum(modelItem.ShipmentMethod);

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual ConsignmentOrderModel PrepareModel(ConsignmentOrderModel model, ConsignmentOrder entity, bool excludeProperties = false)
        {
            if (null != entity)
            {
                model = model ?? entity.ToModel<ConsignmentOrderModel>();

                PrepareGoodsSearchModel(model.GoodsSearchModel, entity);
            }

            if (null == model)
                model = new ConsignmentOrderModel();

            baseAdminModelFactory.PrepareCars(model.AvailableCars, defaultItemText: localizationService.GetResource("Admin.Common.Select"));
            baseAdminModelFactory.PrepareDrivers(model.AvailableDrivers, defaultItemText: localizationService.GetResource("Admin.Common.Select"));

            return model;
        }

        #region Goods

        protected virtual GoodsSearchModel PrepareGoodsSearchModel(GoodsSearchModel model, ConsignmentOrder entity)
        {
            if (null == model)
                throw new ArgumentNullException(nameof(model));

            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            model.OrderId = entity.Id;
            model.SetGridPageSize();

            return model;
        }

        public virtual GoodsListModel PrepareGoodsListModel(GoodsSearchModel searchModel, ConsignmentOrder entity)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            var list = entity.Goods
                            .OrderByDescending(x => x.CTime)
                            .ToList();

            var model = new GoodsListModel
            {
                Data = list.PaginationByRequestModel(searchModel).Select(x =>
                {
                    var modelItem = x.ToModel<GoodsModel>();
                    modelItem.CTime = dateTimeHelper.ConvertToUserTime(modelItem.CTime, DateTimeKind.Utc);

                    return modelItem;
                }),
                Total = list.Count
            };

            return model;
        }

        #endregion

        #endregion
    }
}
