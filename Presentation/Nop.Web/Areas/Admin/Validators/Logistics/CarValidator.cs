using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Logistics
{
    public partial class CarValidator : BaseNopValidator<CarModel>
    {
        public CarValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.License).NotEmpty().WithMessage(localizationService.GetResource("Admin.Logistics.Car.Fields.License.Required"));
        }
    }
}
