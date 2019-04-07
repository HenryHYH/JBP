using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IConsignmentOrderFactory
    {
        ConsignmentOrderSearchModel PrepareSearchModel(ConsignmentOrderSearchModel searchModel);

        ConsignmentOrderListModel PrepareListModel(ConsignmentOrderSearchModel searchModel);

        ConsignmentOrderModel PrepareModel(ConsignmentOrderModel model, ConsignmentOrder entity, bool excludeProperties = false);

        GoodsListModel PrepareGoodsListModel(GoodsSearchModel searchModel, ConsignmentOrder entity);
    }
}
