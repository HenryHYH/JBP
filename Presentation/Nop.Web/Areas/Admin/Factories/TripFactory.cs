﻿using Nop.Core;
using Nop.Core.Domain.Logistics;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logistics;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class TripFactory : ITripFactory
    {
        #region Fields

        private readonly ITripService tripService;
        private readonly IBaseAdminModelFactory baseAdminModelFactory;
        private readonly ILocalizationService localizationService;
        private readonly IConsignmentOrderService consignmentOrderService;
        private readonly IDateTimeHelper dateTimeHelper;

        #endregion

        #region Ctor

        public TripFactory(
            ITripService tripService,
            IBaseAdminModelFactory baseAdminModelFactory,
            ILocalizationService localizationService,
            IConsignmentOrderService consignmentOrderService,
            IDateTimeHelper dateTimeHelper)
        {
            this.tripService = tripService;
            this.baseAdminModelFactory = baseAdminModelFactory;
            this.localizationService = localizationService;
            this.consignmentOrderService = consignmentOrderService;
            this.dateTimeHelper = dateTimeHelper;
        }

        #endregion

        #region Methods

        public virtual TripSearchModel PrepareSearchModel(TripSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            baseAdminModelFactory.PrepareLogisticsShippingStatus(searchModel.AvailableShippingStatuses);
            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual TripListModel PrepareListModel(TripSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = tripService.GetAll(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                carLicense: searchModel.SearchCarLicense,
                driverName: searchModel.SearchDriverName,
                serialNum: searchModel.SearchSerialNum,
                shippingStatuses: (searchModel.SearchShippingStatuses?.Contains(0) ?? false) ? null : searchModel.SearchShippingStatuses,
                startAtFrom: searchModel.SearchStartAtFrom,
                startAtTo: searchModel.SearchStartAtTo,
                endAtFrom: searchModel.SearchEndAtFrom,
                endAtTo: searchModel.SearchEndAtTo);

            var model = new TripListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<TripModel>();
                    PrepareModel(ref modelItem);

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual TripModel PrepareModel(TripModel model, Trip entity, bool excludeProperties = false)
        {
            if (null != entity)
            {
                model = model ?? entity.ToModel<TripModel>();

                PrepareConsignmentOrderSearchModel(model.ConsignmentOrderSearchModel, entity);
            }

            if (null == model)
                model = new TripModel
                {
                    SerialNum = CommonHelper.GenerateSerialNumber()
                };

            PrepareModel(ref model);
            baseAdminModelFactory.PrepareCars(model.AvailableCars, defaultItemText: localizationService.GetResource("Admin.Common.Select"));
            baseAdminModelFactory.PrepareDrivers(model.AvailableDrivers, defaultItemText: localizationService.GetResource("Admin.Common.Select"));
            baseAdminModelFactory.PrepareFeeCategories(model.AvaliableFeeCategories, withSpecialDefaultItem: false);

            if (model.Fees.Count != model.AvaliableFeeCategories.Count)
            {
                model.Fees.Clear();
                foreach (var item in model.AvaliableFeeCategories)
                    model.Fees.Add(new FeeModel { CategoryId = int.Parse(item.Value) });
            }

            return model;
        }

        #region ConsignmentOrder

        protected virtual ConsignmentOrderSearchModel PrepareConsignmentOrderSearchModel(ConsignmentOrderSearchModel model, Trip entity)
        {
            if (null == model)
                throw new ArgumentNullException(nameof(model));

            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            model.TripId = entity.Id;
            model.SetGridPageSize();

            return model;
        }

        public virtual ConsignmentOrderListModel PrepareConsignmentOrderListModel(ConsignmentOrderSearchModel searchModel, Trip entity)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = entity?.Orders
                            .OrderByDescending(x => x.CTime)
                            .ToList() ?? new List<ConsignmentOrder>();

            var model = new ConsignmentOrderListModel
            {
                Data = list.PaginationByRequestModel(searchModel).Select(x =>
                {
                    var modelItem = x.ToModel<ConsignmentOrderModel>();
                    modelItem.CTime = dateTimeHelper.ConvertToUserTime(modelItem.CTime, DateTimeKind.Utc);

                    return modelItem;
                }),
                Total = list.Count
            };

            return model;
        }

        public virtual void AddOrderToTrip(int tripId, int[] orderIds)
        {
            if (null == orderIds || !orderIds.Any())
                throw new NopException(localizationService.GetResource("Admin.Logistics.Trip.Orders.AddNew.OrderIds.Required"));

            var trip = tripService.Get(tripId);
            if (null == trip)
                throw new NopException(localizationService.GetResource("Admin.Logistics.Trip.TripNotExists"));

            foreach (var orderId in orderIds)
            {
                var order = consignmentOrderService.Get(orderId);
                if (null == order)
                    throw new NopException(localizationService.GetResource("Admin.Logistics.Trip.ConsignmentOrderNotExists"));

                if (trip.Orders.Any(x => x.Id == order.Id))
                    throw new NopException(localizationService.GetResource("Admin.Logistics.Trip.TripExistsConsignmentOrder"));

                order.TripId = tripId;
                consignmentOrderService.Update(order);
            }
        }

        #endregion

        #endregion

        #region Utilities

        protected virtual void PrepareModel(ref TripModel model)
        {
            if (null == model)
                throw new ArgumentNullException(nameof(model));

            model.CTime = dateTimeHelper.ConvertToUserTime(model.CTime, DateTimeKind.Utc);
            model.ShippingStatusName = localizationService.GetLocalizedEnum(model.ShippingStatus);
        }

        #endregion
    }
}
