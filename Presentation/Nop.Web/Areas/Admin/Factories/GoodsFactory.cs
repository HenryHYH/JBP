using Nop.Core.Domain.Logistics;
using Nop.Services.Logistics;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using System;
using System.Linq;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class GoodsFactory : IGoodsFactory
    {
        private readonly IGoodsService goodsService;

        public GoodsFactory(
            IGoodsService goodsService)
        {
            this.goodsService = goodsService;
        }

        public virtual GoodsSearchModel PrepareSearchModel(GoodsSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual GoodsListModel PrepareListModel(GoodsSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = goodsService.GetAll(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                name: searchModel.SearchName);

            var model = new GoodsListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<GoodsModel>();

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual GoodsModel PrepareModel(GoodsModel model, Goods entity, bool excludeProperties = false)
        {
            if (null != entity)
            {
                model = model ?? entity.ToModel<GoodsModel>();
            }

            if (null == model)
                model = new GoodsModel();

            return model;
        }
    }
}
