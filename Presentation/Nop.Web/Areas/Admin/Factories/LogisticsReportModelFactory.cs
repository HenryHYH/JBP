using Nop.Services.Localization;
using Nop.Services.Logistics;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using System;
using System.Linq;

namespace Nop.Web.Areas.Admin.Factories
{
    public class LogisticsReportModelFactory : ILogisticsReportModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory baseAdminModelFactory;
        private readonly ILocalizationService localizationService;
        private readonly ITripService tripService;
        private readonly IConsignmentOrderService consignmentOrderService;

        #endregion

        #region Ctor

        public LogisticsReportModelFactory(
            IBaseAdminModelFactory baseAdminModelFactory,
            ILocalizationService localizationService,
            ITripService tripService,
            IConsignmentOrderService consignmentOrderService)
        {
            this.baseAdminModelFactory = baseAdminModelFactory;
            this.localizationService = localizationService;
            this.tripService = tripService;
            this.consignmentOrderService = consignmentOrderService;
        }

        #endregion

        #region Methods

        public virtual ReportTripSearchModel PrepareReportTripSearchModel(ReportTripSearchModel model = null)
        {
            if (null == model)
                model = new ReportTripSearchModel();

            return model;
        }

        public virtual ReportTripListModel PrepareReportTripListModel(ReportTripSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = tripService.GetStatistics(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                tripShippingTimeFrom: searchModel.TripShippingTimeFrom,
                tripShippingTimeTo: searchModel.TripShippingTimeTo,
                orderConsignmentTimeFrom: searchModel.OrderConsignmentTimeFrom,
                orderConsignmentTimeTo: searchModel.OrderConsignmentTimeTo,
                driverName: searchModel.DriverName,
                carLicense: searchModel.CarLicense);

            var model = new ReportTripListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<ReportTripModel>();

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual ReportBalanceSearchModel PrepareReportBalanceSearchModel(ReportBalanceSearchModel model = null)
        {
            if (null == model)
                model = new ReportBalanceSearchModel();

            baseAdminModelFactory.PrepareStatisticsFrequency(model.AvailableStatisticsFrequencies, false);
            baseAdminModelFactory.PrepareFeeCategories(model.AvailableFeeCategories, false);

            return model;
        }

        public virtual ReportBalanceListModel PrepareReportBalanceListModel(ReportBalanceSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = tripService.StatisticsBalance(
                                    pageIndex: searchModel.Page - 1,
                                    pageSize: searchModel.PageSize,
                                    frequency: searchModel.Frequency,
                                    tripShippingTimeFrom: searchModel.TripShippingTimeFrom,
                                    tripShippingTimeTo: searchModel.TripShippingTimeTo);

            var model = new ReportBalanceListModel
            {
                Data = list.GroupBy(x => x.StatisticsTime)
                            .Select(x => new ReportBalanceModel
                            {
                                StatisticsTime = x.Key,
                                Fees = x.Select(f => new ReportFeeModel { Id = f.CategoryId, Name = f.Category, Type = f.FeeType, Amount = f.Amount })
                                        .ToDictionary(k => k.Id, v => v)
                            }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual ReportStatementSearchModel PrepareReportStatementSearchModel(ReportStatementSearchModel model = null)
        {
            if (null == model)
                model = new ReportStatementSearchModel();

            baseAdminModelFactory.PrepareLogisticsOrderStatus(model.AvailableOrderStatuses);
            if (model.AvailableOrderStatuses.Any())
            {
                if (model.OrderStatuses?.Any() ?? false)
                {
                    var statuses = model.OrderStatuses.Select(x => x.ToString());
                    model.AvailableOrderStatuses.Where(x => statuses.Contains(x.Value)).ToList()
                        .ForEach(x => x.Selected = true);
                }
                else
                    model.AvailableOrderStatuses.FirstOrDefault().Selected = true;
            }
            baseAdminModelFactory.PrepareLogisticsPaymentStatus(model.AvailablePaymentStatuses);
            if (model.AvailablePaymentStatuses.Any())
            {
                if (model.PaymentStatuses?.Any() ?? false)
                {
                    var statuses = model.PaymentStatuses.Select(x => x.ToString());
                    model.AvailablePaymentStatuses.Where(x => statuses.Contains(x.Value)).ToList()
                        .ForEach(x => x.Selected = true);
                }
                else
                    model.AvailablePaymentStatuses.FirstOrDefault().Selected = true;
            }
            baseAdminModelFactory.PrepareLogisticsShippingStatus(model.AvailableShippingStatuses);
            if (model.AvailableShippingStatuses.Any())
            {
                if (model.ShippingStatuses?.Any() ?? false)
                {
                    var statuses = model.ShippingStatuses.Select(x => x.ToString());
                    model.AvailableShippingStatuses.Where(x => statuses.Contains(x.Value)).ToList()
                        .ForEach(x => x.Selected = true);
                }
                else
                    model.AvailableShippingStatuses.FirstOrDefault().Selected = true;
            }

            return model;
        }

        public virtual ReportStatementListModel PrepareReportStatementListModel(ReportStatementSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = consignmentOrderService.GetStatistics(
                                                pageIndex: searchModel.Page - 1,
                                                pageSize: searchModel.PageSize,
                                                consigneeName: searchModel.ConsigneeName,
                                                orderConsignmentTimeFrom: searchModel.OrderConsignmentTimeFrom,
                                                orderConsignmentTimeTo: searchModel.OrderConsignmentTimeTo,
                                                orderStatuses: (searchModel.OrderStatuses?.Contains(0) ?? false) ? null : searchModel.OrderStatuses,
                                                paymentStatuses: (searchModel.PaymentStatuses?.Contains(0) ?? false) ? null : searchModel.PaymentStatuses,
                                                shippingStatuses: (searchModel.ShippingStatuses?.Contains(0) ?? false) ? null : searchModel.ShippingStatuses);

            var model = new ReportStatementListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<ReportStatementModel>();

                    modelItem.ShippingStatus = x.Trip?.ShippingStatus;
                    modelItem.ShippingStatusName = modelItem.ShippingStatus.HasValue ? localizationService.GetLocalizedEnum(modelItem.ShippingStatus.Value) : string.Empty;
                    modelItem.PaymentStatusName = localizationService.GetLocalizedEnum(modelItem.PaymentStatus);
                    modelItem.OrderStatusName = localizationService.GetLocalizedEnum(modelItem.OrderStatus);

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual ReportStatementAggrModel PrepareReportStatementAggrModel(ReportStatementSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var aggr = consignmentOrderService.GetAggregates(
                                                consigneeName: searchModel.ConsigneeName,
                                                orderConsignmentTimeFrom: searchModel.OrderConsignmentTimeFrom,
                                                orderConsignmentTimeTo: searchModel.OrderConsignmentTimeTo,
                                                orderStatuses: (searchModel.OrderStatuses?.Contains(0) ?? false) ? null : searchModel.OrderStatuses,
                                                paymentStatuses: (searchModel.PaymentStatuses?.Contains(0) ?? false) ? null : searchModel.PaymentStatuses,
                                                shippingStatuses: (searchModel.ShippingStatuses?.Contains(0) ?? false) ? null : searchModel.ShippingStatuses);

            return new ReportStatementAggrModel
            {
                Receivable = aggr.Receivable,
                Receipts = aggr.Receipts
            };
        }

        #endregion
    }
}
