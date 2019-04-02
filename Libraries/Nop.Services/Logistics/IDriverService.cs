using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface IDriverService
    {
        IPagedList<Driver> GetAll(int pageIndex = 0, int pageSize = int.MaxValue);

        void Insert(Driver entity);

        void Update(Driver entity);

        void Delete(Driver entity);
    }
}
