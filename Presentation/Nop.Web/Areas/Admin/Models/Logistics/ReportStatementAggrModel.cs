using Nop.Web.Framework.Models;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportStatementAggrModel : BaseNopModel
    {
        public decimal? Receivable { get; set; }

        public decimal? Receipts { get; set; }

        public decimal Balance { get => (Receivable ?? 0) - (Receipts ?? 0); }
    }
}
