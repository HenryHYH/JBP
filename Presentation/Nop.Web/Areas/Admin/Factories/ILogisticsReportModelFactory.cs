using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface ILogisticsReportModelFactory
    {
        ReportTripSearchModel PrepareReportTripSearchModel(ReportTripSearchModel model = null);

        ReportTripListModel PrepareReportTripListModel(ReportTripSearchModel searchModel);
    }
}
