using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface IConsignmentOrderService
    {
        IPagedList<ConsignmentOrder> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);

        void Insert(ConsignmentOrder entity);

        void Update(ConsignmentOrder entity);

        void Delete(ConsignmentOrder entity);
    }
}
