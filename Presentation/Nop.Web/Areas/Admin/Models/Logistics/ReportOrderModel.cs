using Nop.Web.Framework.Models;
using System;
using System.Collections.Generic;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportOrderModel : BaseNopEntityModel
    {
        public ReportOrderModel()
        {
            Goods = new List<ReportGoodsModel>();
        }

        public string SerialNum { get; set; }

        public virtual ReportUserModel Consignor { get; set; }

        public virtual ReportUserModel Consignee { get; set; }

        public DateTime CTime { get; set; }

        public virtual IList<ReportGoodsModel> Goods { get; set; }
    }
}
