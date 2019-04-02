using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface ITripService
    {
        IPagedList<Trip> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);

        void Insert(Trip entity);

        void Update(Trip entity);

        void Delete(Trip entity);
    }
}
