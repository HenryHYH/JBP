using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportGoodsModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }
    }
}
