using Nop.Core.Domain.Logistics;
using Nop.Services.Logistics;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Logistics;
using System;
using System.Linq;

namespace Nop.Web.Areas.Admin.Factories
{
    public partial class DriverFactory : IDriverFactory
    {
        #region Fields

        private readonly IDriverService driverService;
        private readonly IBaseAdminModelFactory baseAdminModelFactory;

        #endregion

        #region Ctor

        public DriverFactory(
            IDriverService driverService,
            IBaseAdminModelFactory baseAdminModelFactory)
        {
            this.driverService = driverService;
            this.baseAdminModelFactory = baseAdminModelFactory;
        }

        #endregion

        #region Methods

        public virtual DriverSearchModel PrepareSearchModel(DriverSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            baseAdminModelFactory.PrepareEnabledOptions(searchModel.AvailableEnabledOptions);

            searchModel.SetGridPageSize();

            return searchModel;
        }

        public virtual DriverListModel PrepareListModel(DriverSearchModel searchModel)
        {
            if (null == searchModel)
                throw new ArgumentNullException(nameof(searchModel));

            var list = driverService.GetAll(
                pageIndex: searchModel.Page - 1,
                pageSize: searchModel.PageSize,
                name: searchModel.SearchName,
                enabled: searchModel.SearchEnabled);

            var model = new DriverListModel
            {
                Data = list.Select(x =>
                {
                    var modelItem = x.ToModel<DriverModel>();

                    return modelItem;
                }),
                Total = list.TotalCount
            };

            return model;
        }

        public virtual DriverModel PrepareModel(DriverModel model, Driver entity, bool excludeProperties = false)
        {
            if (null != entity)
            {
                model = model ?? entity.ToModel<DriverModel>();
            }

            if (null == model)
                model = new DriverModel();

            if (null == entity)
            {
                model.Enabled = true;
            }

            return model;
        }

        #endregion
    }
}
