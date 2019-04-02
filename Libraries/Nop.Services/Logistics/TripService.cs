using Nop.Core;
using Nop.Core.Domain.Logistics;
using System;

namespace Nop.Services.Logistics
{
    public partial class TripService : ITripService
    {
        public IPagedList<Trip> GetAll(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            throw new NotImplementedException();
        }

        public void Insert(Trip entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Trip entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Trip entity)
        {
            throw new NotImplementedException();
        }
    }
}
