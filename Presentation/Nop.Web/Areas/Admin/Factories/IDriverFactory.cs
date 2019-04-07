using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IDriverFactory
    {
        DriverSearchModel PrepareSearchModel(DriverSearchModel searchModel);

        DriverListModel PrepareListModel(DriverSearchModel searchModel);

        DriverModel PrepareModel(DriverModel model, Driver entity, bool excludeProperties = false);
    }
}
