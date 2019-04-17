using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class FeeService : IFeeService
    {
        #region Fields

        private readonly IRepository<FeeCategory> feeCategoryRepository;

        #endregion

        #region Ctor

        public FeeService(IRepository<FeeCategory> feeCategoryRepository)
        {
            this.feeCategoryRepository = feeCategoryRepository;
        }

        #endregion

        #region Methods

        public IList<FeeCategory> GetFeeCategories()
        {
            return feeCategoryRepository.TableNoTracking
                                .OrderBy(x => x.DisplayOrder)
                                .ToList();
        }

        #endregion
    }
}
