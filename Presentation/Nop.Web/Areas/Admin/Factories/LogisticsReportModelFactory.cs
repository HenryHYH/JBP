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

        private readonly ITripService tripService;

        #endregion

        #region Ctor

        public LogisticsReportModelFactory(
            ITripService tripService)
        {
            this.tripService = tripService;
        }

        #endregion

        #region Methods

        public virtual ReportTripSearchModel PrepareReportTripSearchModel(ReportTripSearchModel model = null)
        {
            if (null == model)
                model = new ReportTripSearchModel();

            return model;
        }

        public ReportTripListModel PrepareReportTripListModel(ReportTripSearchModel searchModel)
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

        #endregion
    }
}
