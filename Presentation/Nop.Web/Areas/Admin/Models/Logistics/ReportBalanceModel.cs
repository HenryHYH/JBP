using Nop.Core.Domain.Logistics;
using Nop.Web.Framework.Models;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Web.Areas.Admin.Models.Logistics
{
    public partial class ReportBalanceModel : BaseNopModel
    {
        #region Ctor

        public ReportBalanceModel()
        {
            Fees = new Dictionary<int, ReportFeeModel>();
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

        public virtual IDictionary<int, ReportFeeModel> Fees { get; set; }

        #endregion
    }
}