using Nop.Core.Domain.Logistics;
using System.Collections.Generic;

namespace Nop.Services.Logistics
{
    public partial interface IFeeService
    {
        IList<FeeCategory> GetFeeCategories();
    }
}
