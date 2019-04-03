using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class GoodsSearchModel : BaseSearchModel
    {
        [NopResourceDisplayName("Admin.Logistics.Goods.List.SearchName")]
        public string SearchName { get; set; }
    }
}
