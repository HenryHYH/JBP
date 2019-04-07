using FluentValidation.Attributes;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(CarValidator))]
    public partial class CarModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Logistics.Car.Fields.License")]
        public string License { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Car.Fields.Enabled")]
        public bool Enabled { get; set; }
    }
}