using Nop.Core.Domain.Logistics;
using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportFeeCategoryModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public FeeType Type { get; set; }
    }
}
