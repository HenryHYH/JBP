using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface ITripFactory
    {
        TripSearchModel PrepareSearchModel(TripSearchModel searchModel);

        TripListModel PrepareListModel(TripSearchModel searchModel);

        TripModel PrepareModel(TripModel model, Trip entity, bool excludeProperties = false);

        ConsignmentOrderListModel PrepareConsignmentOrderListModel(ConsignmentOrderSearchModel searchModel, Trip entity);

        void AddOrderToTrip(int tripId, int[] orderIds);
    }
}
