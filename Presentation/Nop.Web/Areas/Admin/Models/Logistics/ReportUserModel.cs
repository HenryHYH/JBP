using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportUserModel : BaseNopEntityModel
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }
    }
}
