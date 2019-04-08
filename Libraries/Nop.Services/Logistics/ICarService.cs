using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface ICarService
    {
        IPagedList<Car> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string license = null,
            bool? enabled = null,
            string[] licenses = null);

        Car Get(int id);

        void Insert(Car entity);

        void Update(Car entity);

        void Delete(Car entity);

        string[] GetNotExistings(params string[] idOrLicenses);
    }
}
