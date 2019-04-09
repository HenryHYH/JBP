using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(TripValidator))]
    public partial class TripModel : BaseNopEntityModel
    {
        #region Ctor

        public TripModel()
        {
            AvailableCars = new List<SelectListItem>();
            AvailableDrivers = new List<SelectListItem>();
            ConsignmentOrderSearchModel = new ConsignmentOrderSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.Car")]
        public int CarId { get; set; }

        public virtual CarModel Car { get; set; }

        public virtual IList<SelectListItem> AvailableCars { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.Driver")]
        public int DriverId { get; set; }

        public virtual DriverModel Driver { get; set; }

        public virtual IList<SelectListItem> AvailableDrivers { get; set; }

        public virtual ConsignmentOrderSearchModel ConsignmentOrderSearchModel { get; set; }

        #endregion
    }
}