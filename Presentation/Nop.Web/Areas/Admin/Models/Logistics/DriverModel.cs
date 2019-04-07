using FluentValidation.Attributes;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(DriverValidator))]
    public partial class DriverModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Logistics.Driver.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Driver.Fields.Enabled")]
        public bool Enabled { get; set; }
    }
}