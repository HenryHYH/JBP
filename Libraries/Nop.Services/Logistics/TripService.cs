using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using Nop.Services.Events;
using System;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class TripService : ITripService
    {
        #region Fields

        private readonly IRepository<Trip> repository;
        private readonly IRepository<ConsignmentOrder> consignmentOrderRepository;
        private readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctor

        public TripService(
            IRepository<Trip> repository,
            IRepository<ConsignmentOrder> consignmentOrderRepository,
            IEventPublisher eventPublisher)
        {
            this.repository = repository;
            this.consignmentOrderRepository = consignmentOrderRepository;
            this.eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Trip> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string carLicense = null,
            string driverName = null,
            string[] serialNums = null)
        {
            var query = repository.Table.Where(x => !x.Deleted);

            if (!string.IsNullOrWhiteSpace(carLicense))
            {
                carLicense = carLicense.Trim();
                query = query.Where(x => x.Car.License.Contains(carLicense));
            }
            if (!string.IsNullOrWhiteSpace(driverName))
            {
                driverName = driverName.Trim();
                query = query.Where(x => x.Driver.Name.Contains(driverName));
            }
            if (null != serialNums && serialNums.Any())
                query = query.Where(x => serialNums.Contains(x.SerialNum));

            query = query.OrderByDescending(x => x.UTime ?? x.CTime);

            var list = new PagedList<Trip>(query, pageIndex, pageSize);

            return list;
        }

        public virtual Trip Get(int id)
        {
            if (id <= 0)
                return null;

            return repository.GetById(id);
        }

        public virtual void Insert(Trip entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(Trip entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.UTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityUpdated(entity);
        }

        public virtual void Delete(Trip entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.Deleted = true;
            entity.DTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityDeleted(entity);
        }

        #region Trips

        public virtual void DeleteConsignmentOrder(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.TripId = null;
            consignmentOrderRepository.Update(entity);

            eventPublisher.EntityDeleted(entity);
        }

        #endregion

        #endregion
    }
}
