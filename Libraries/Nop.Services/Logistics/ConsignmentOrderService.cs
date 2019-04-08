using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using Nop.Services.Events;
using System;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class ConsignmentOrderService : IConsignmentOrderService
    {
        #region Fields

        private readonly IRepository<ConsignmentOrder> repository;
        private readonly IRepository<Goods> goodsRepository;
        private readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctor

        public ConsignmentOrderService(
            IRepository<ConsignmentOrder> repository,
            IRepository<Goods> goodsRepository,
            IEventPublisher eventPublisher)
        {
            this.repository = repository;
            this.goodsRepository = goodsRepository;
            this.eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public virtual IPagedList<ConsignmentOrder> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            ShipmentMethod? shipmentMethod = null,
            string startPoint = null,
            string terminal = null,
            string consignor = null,
            string consignee = null,
            string carLicense = null,
            string driverName = null)
        {
            var query = repository.Table.Where(x => !x.Deleted);

            if (shipmentMethod.HasValue)
                query = query.Where(x => x.ShipmentMethod == shipmentMethod.Value);
            if (!string.IsNullOrWhiteSpace(startPoint))
            {
                startPoint = startPoint.Trim();
                query = query.Where(x => x.StartPoint.Contains(startPoint));
            }
            if (!string.IsNullOrWhiteSpace(terminal))
            {
                terminal = terminal.Trim();
                query = query.Where(x => x.Terminal.Contains(terminal));
            }
            if (!string.IsNullOrWhiteSpace(consignee))
            {
                consignee = consignee.Trim();
                query = query.Where(x => x.Consignee.Name.Contains(consignee));
            }
            if (!string.IsNullOrWhiteSpace(consignor))
            {
                consignor = consignor.Trim();
                query = query.Where(x => x.Consignor.Name.Contains(consignor));
            }
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

            query = query.OrderByDescending(x => x.UTime ?? x.CTime);

            var list = new PagedList<ConsignmentOrder>(query, pageIndex, pageSize);

            return list;
        }

        public virtual ConsignmentOrder Get(int id)
        {
            if (id <= 0)
                return null;

            return repository.GetById(id);
        }

        public virtual void Insert(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.UTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityUpdated(entity);
        }

        public virtual void Delete(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.Deleted = true;
            entity.DTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityDeleted(entity);
        }

        #region Goods

        public virtual void DeleteGoods(Goods entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            goodsRepository.Delete(entity);

            eventPublisher.EntityDeleted(entity);
        }

        public virtual void InsertGoods(Goods entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            goodsRepository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        #endregion

        #endregion
    }
}
