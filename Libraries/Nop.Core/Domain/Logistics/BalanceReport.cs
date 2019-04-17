namespace Nop.Core.Domain.Logistics
{
    public partial class BalanceReport
    {
        public string StatisticsTime { get; set; }

        public int CategoryId { get; set; }

        public string Category { get; set; }

        public FeeType FeeType { get; set; }

        public decimal? Amount { get; set; }
    }
}
