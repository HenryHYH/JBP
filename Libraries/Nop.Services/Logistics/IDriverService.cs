using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface IDriverService
    {
        IPagedList<Driver> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string name = null,
            bool? enabled = null,
            string[] names = null);

        Driver Get(int id);

        void Insert(Driver entity);

        void Update(Driver entity);

        void Delete(Driver entity);

        string[] GetNotExistings(params string[] idOrNames);
    }
}
