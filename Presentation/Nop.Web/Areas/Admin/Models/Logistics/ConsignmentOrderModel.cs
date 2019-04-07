using FluentValidation.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(ConsignmentOrderValidator))]
    public partial class ConsignmentOrderModel : BaseNopEntityModel
    {
        #region Ctor

        public ConsignmentOrderModel()
        {
            AvailableCars = new List<SelectListItem>();
            AvailableDrivers = new List<SelectListItem>();
            GoodsSearchModel = new GoodsSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.StartPoint")]
        public string StartPoint { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Terminal")]
        public string Terminal { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.ShipmentMethod")]
        public ShipmentMethod ShipmentMethod { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.ShipmentMethod")]
        public string ShipmentMethodName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Consignor")]
        public int ConsignorId { get; set; }

        public virtual ConsignmentUserModel Consignor { get; set; }

        [NopResourceDisplayName("Admin.Logisitcs.ConsignmentOrder.Fields.Consignee")]
        public int ConsigneeId { get; set; }

        public virtual ConsignmentUserModel Consignee { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Car")]
        public int CarId { get; set; }

        public virtual CarModel Car { get; set; }

        public virtual IList<SelectListItem> AvailableCars { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Driver")]
        public int DriverId { get; set; }

        public virtual DriverModel Driver { get; set; }

        public virtual IList<SelectListItem> AvailableDrivers { get; set; }

        #region Goods

        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Name")]
        public string AddGoodsName { get; set; }

        [NopResourceDisplayName("Admin.Logistics.Goods.Fields.Price")]
        [UIHint("DecimalNullable")]
        public decimal? AddGoodsPrice { get; set; }

        public GoodsSearchModel GoodsSearchModel { get; set; }

        #endregion

        #endregion
    }
}