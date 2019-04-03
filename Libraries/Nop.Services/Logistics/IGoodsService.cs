using Nop.Core;
using Nop.Core.Domain.Logistics;

namespace Nop.Services.Logistics
{
    public partial interface IGoodsService
    {
        IPagedList<Goods> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string name = null);

        void Insert(Goods entity);

        void Update(Goods entity);

        void Delete(Goods entity);

        Goods Get(int id, bool loadCacheableCopy = true);
    }
}
