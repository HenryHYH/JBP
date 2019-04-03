using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Models.Logistics;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial interface IGoodsFactory
    {
        GoodsSearchModel PrepareSearchModel(GoodsSearchModel searchModel);

        GoodsListModel PrepareListModel(GoodsSearchModel searchModel);

        GoodsModel PrepareModel(GoodsModel model, Goods entity, bool excludeProperties = false);
    }
}
