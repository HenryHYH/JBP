using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface ILogisticsReportModelFactory
    {
        ReportTripSearchModel PrepareReportTripSearchModel(ReportTripSearchModel model = null);

        ReportTripListModel PrepareReportTripListModel(ReportTripSearchModel searchModel);

        ReportBalanceSearchModel PrepareReportBalanceSearchModel(ReportBalanceSearchModel model = null);

        ReportBalanceListModel PrepareReportBalanceListModel(ReportBalanceSearchModel searchModel);

        ReportStatementSearchModel PrepareReportStatementSearchModel(ReportStatementSearchModel model = null);

        ReportStatementListModel PrepareReportStatementListModel(ReportStatementSearchModel searchModel);
    }
}
