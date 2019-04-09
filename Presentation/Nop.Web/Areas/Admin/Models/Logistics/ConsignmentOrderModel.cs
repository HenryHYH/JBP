using FluentValidation.Attributes;
using Nop.Core.Domain.Logistics;
using Nop.Web.Areas.Admin.Validators.Logistics;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    [Validator(typeof(ConsignmentOrderValidator))]
    public partial class ConsignmentOrderModel : BaseNopEntityModel
    {
        #region Ctor

        public ConsignmentOrderModel()
        {
            GoodsSearchModel = new GoodsSearchModel();
        }

        #endregion

        #region Properties

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.CTime")]
        public DateTime CTime { get; set; }

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

        [NopResourceDisplayName("Admin.Logistics.ConsignmentOrder.Fields.Trip")]
        public int TripId { get; set; }

        public virtual TripModel Trip { get; set; }

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