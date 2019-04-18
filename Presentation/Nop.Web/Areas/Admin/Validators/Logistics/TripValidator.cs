using FluentValidation;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Logistics
{
    public partial class TripValidator : BaseNopValidator<TripModel>
    {
        public TripValidator(ILocalizationService localizationService)
        {
            RuleFor(x => x.EndAt)
                .GreaterThan(x => x.StartAt.Value)
                .WithMessage(localizationService.GetResource("Admin.Logistics.Trip.Fields.EndAt.GreaterThanStartAt"))
                .When(x => x.StartAt.HasValue && x.EndAt.HasValue);
        }
    }
}
