using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Models.Logistics;
using Nop.Web.Framework.Validators;

namespace Nop.Web.Areas.Admin.Validators.Logistics
{
    public partial class TripValidator : BaseNopValidator<TripModel>
    {
        public TripValidator(ILocalizationService localizationService)
        {
        }
    }
}
