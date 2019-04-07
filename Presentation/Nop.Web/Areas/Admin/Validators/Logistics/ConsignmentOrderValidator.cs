using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Logistics
{
    public partial class ConsignmentOrderValidator : BaseNopValidator<ConsignmentOrderModel>
    {
        public ConsignmentOrderValidator(ILocalizationService localizationService)
        {
            //RuleFor(x => x.Name).NotEmpty().WithMessage(localizationService.GetResource("Admin.Logistics.ConsignmentOrder.Fields.Name.Required"));
        }
    }
}
