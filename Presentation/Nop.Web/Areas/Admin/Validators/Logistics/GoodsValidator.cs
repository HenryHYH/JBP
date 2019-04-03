using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Logistics
{
    public partial class GoodsValidator : BaseNopValidator<GoodsModel>
    {
        public GoodsValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Logistics.Goods.Fields.Name.Required"));
        }
    }
}
