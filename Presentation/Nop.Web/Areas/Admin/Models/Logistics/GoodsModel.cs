using FluentValidation.Attributes;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(GoodsValidator))]
    public partial class GoodsModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Price")]
        [UIHint("DecimalNullable")]
        public decimal? Price { get; set; }
    }
}