using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ConsignmentUserModel : BaseNopEntityModel
    {
        [NopResourceDisplayName("Admin.Logistics.ConsignmentUser.Fields.Name")]
        public string Name { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentUser.Fields.Address")]
        public string Address { get; set; }

        [NopResourceDisplayName("Admin.Logistics.ConsignmentUser.Fields.Phone")]
        public string Phone { get; set; }
    }
}
