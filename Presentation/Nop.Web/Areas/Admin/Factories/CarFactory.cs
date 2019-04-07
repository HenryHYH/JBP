using Nop.Core.Domain.Logistics;
using Nop.Services.Logistics;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using System;
using System.Linq;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class CarFactory : ICarFactory
    {
        private readonly ICarService carService;
        private readonly IBaseAdminModelFactory baseAdminModelFactory;

        public CarFactory(
            ICarService carService,
            IBaseAdminModelFactory baseAdminModelFactory)
        {
            this.carService = carService;
            this.baseAdminModelFactory = baseAdminModelFactory;
        }

        public virtual CarSearchModel PrepareSearchModel(CarSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            baseAdminModelFactory.PrepareEnabledOptions(searchModel.AvailableEnabledOptions);

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual CarListModel PrepareListModel(CarSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = carService.GetAll(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                license: searchModel.SearchLicense,
                enabled: searchModel.SearchEnabled);

            var model = new CarListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<CarModel>();

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual CarModel PrepareModel(CarModel model, Car entity, bool excludeProperties = false)
        {
            if (null != entity)
            {
                model = model ?? entity.ToModel<CarModel>();
            }

            if (null == model)
                model = new CarModel();

            if (null == entity)
            {
                model.Enabled = true;
            }

            return model;
        }
    }
}
