using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface ICarFactory
    {
        CarSearchModel PrepareSearchModel(CarSearchModel searchModel);

        CarListModel PrepareListModel(CarSearchModel searchModel);

        CarModel PrepareModel(CarModel model, Car entity, bool excludeProperties = false);
    }
}
