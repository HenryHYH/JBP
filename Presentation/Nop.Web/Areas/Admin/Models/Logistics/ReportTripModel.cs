using Nop.Web.Framework.Models;
using System;
using System.Collections.Generic;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportTripModel : BaseNopEntityModel
    {
        public ReportTripModel()
        {
            Orders = new List<ReportOrderModel>();
        }

        public string SerialNum { get; set; }

        public virtual ReportDriverModel Driver { get; set; }

        public virtual ReportCarModel Car { get; set; }

        public DateTime? EndAt { get; set; }

        public virtual IList<ReportOrderModel> Orders { get; set; }
    }
}