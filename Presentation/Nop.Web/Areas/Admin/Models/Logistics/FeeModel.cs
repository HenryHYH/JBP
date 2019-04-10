using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class FeeModel : BaseNopEntityModel
    {
        public int TripId { get; set; }

        public int CategoryId { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Fee.Fields.Amount")]
        [UIHint("DecimalNullable")]
        public decimal? Amount { get; set; }
    }
}
