namespace Nop.Core.Domain.Logistics
{
    public partial class BalanceReportFee
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public FeeType Type { get; set; }

        public decimal? Amount { get; set; }
    }
}
