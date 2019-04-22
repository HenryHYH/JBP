using System.Collections.Generic;
using System.Linq;

namespace Nop.Core.Domain.Logistics
{
    public partial class GroupBalanceReport
    {
        #region Ctor

        public GroupBalanceReport()
        {
            Fees = new Dictionary<int, BalanceReportFee>();
        }

        #endregion

        #region Properties

        public string StatisticsTime { get; set; }

        public decimal Income
        {
            get
            {
                return Fees.Values
                            .Where(x => x.Type == FeeType.Income)
                            .Sum(x => x.Amount) ?? 0;
            }
        }

        public decimal Expense
        {
            get
            {
                return Fees.Values
                            .Where(x => x.Type == FeeType.Expense)
                            .Sum(x => x.Amount) ?? 0;
            }
        }

        public decimal Balance
        {
            get
            {
                return Income - Expense;
            }
        }

        public virtual IDictionary<int, BalanceReportFee> Fees { get; set; }

        #endregion
    }
}
