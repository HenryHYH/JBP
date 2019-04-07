using FluentValidation.Attributes;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(GoodsValidator))]
    public partial class GoodsModel : BaseNopEntityModel
    {
        public int OrderId { get; set; }

        [NopResourceDisplayName("Admin.Logisitcs.Goods.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Price")]
        [UIHint("DecimalNullable")]
        public decimal? Price { get; set; }

        [NopResourceDisplayName("Admin.Logisitcs.Goods.Fields.CTime")]
        public DateTime CTime { get; set; }
    }
}