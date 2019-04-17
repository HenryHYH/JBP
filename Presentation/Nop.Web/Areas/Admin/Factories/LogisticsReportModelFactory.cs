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
        private readonly ITripService tripService;
        private readonly IFeeService feeService;

        #endregion

        #region Ctor

        public LogisticsReportModelFactory(
            IBaseAdminModelFactory baseAdminModelFactory,
            ITripService tripService,
            IFeeService feeService)
        {
            this.baseAdminModelFactory = baseAdminModelFactory;
            this.tripService = tripService;
            this.feeService = feeService;
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
                tripCTimeFrom: searchModel.TripCTimeFrom,
                tripCTimeTo: searchModel.TripCTimeTo,
                orderCTimeFrom: searchModel.OrderCTimeFrom,
                orderCTimeTo: searchModel.OrderCTimeTo,
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
                                    tripCTimeFrom: searchModel.TripCTimeFrom,
                                    tripCTimeTo: searchModel.TripCTimeTo);

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

        #endregion
    }
}
