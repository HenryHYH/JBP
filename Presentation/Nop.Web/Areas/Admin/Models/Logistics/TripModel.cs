using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
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
            AvaliableFeeCategories = new List<SelectListItem>();
            ConsignmentOrderSearchModel = new ConsignmentOrderSearchModel();
            Fees = new List<FeeModel>();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.SerialNum")]
        public string SerialNum { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.CTime")]
        public DateTime CTime { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.Car")]
        public int CarId { get; set; }

        public virtual CarModel Car { get; set; }

        public virtual IList<SelectListItem> AvailableCars { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.Driver")]
        public int DriverId { get; set; }

        public virtual DriverModel Driver { get; set; }

        public virtual IList<SelectListItem> AvailableDrivers { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Trip.Fields.ShippingTime")]
        [UIHint("DateNullable")]
        public DateTime? ShippingTime { get; set; }

        public virtual ConsignmentOrderSearchModel ConsignmentOrderSearchModel { get; set; }

        public virtual IList<SelectListItem> AvaliableFeeCategories { get; set; }

        public virtual IList<FeeModel> Fees { get; set; }

        #endregion
    }
}