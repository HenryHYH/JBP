using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
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
            int? tripId = null,
            bool? noRelatedTrip = null,
            string[] serialNums = null)
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
            if (tripId.HasValue && tripId.Value > 0)
                query = query.Where(x => x.TripId == tripId.Value);
            if (noRelatedTrip.HasValue && noRelatedTrip.Value)
                query = query.Where(x => null == x.TripId || 0 == x.TripId);
            if (null != serialNums && serialNums.Any())
                query = query.Where(x => serialNums.Contains(x.SerialNum));

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

        public virtual string[] GetNotExistings(string[] serialNums)
        {
            if (null == serialNums)
                throw new ArgumentNullException(nameof(serialNums));

            var query = repository.TableNoTracking;
            var queryFilter = serialNums.Distinct().ToArray();

            var filter = query.Where(x => !x.Deleted).Select(x => x.SerialNum).Where(x => queryFilter.Contains(x)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter;
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

        public virtual void ChangeOrderStatus(ConsignmentOrder entity, OrderStatus orderStatus)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.OrderStatus = orderStatus;

            Update(entity);
        }

        public virtual IPagedList<ConsignmentOrder> GetStatistics(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string consigneeName = null,
            DateTime? orderConsignmentTimeFrom = null,
            DateTime? orderConsignmentTimeTo = null,
            IList<int> orderStatuses = null,
            IList<int> paymentStatuses = null,
            IList<int> shippingStatuses = null)
        {
            var query = repository.Table.Where(x => !x.Deleted);

            if (!string.IsNullOrWhiteSpace(consigneeName))
            {
                consigneeName = consigneeName.Trim();
                query = query.Where(x => x.Consignee.Name.Contains(consigneeName));
            }
            if (orderConsignmentTimeFrom.HasValue)
                query = query.Where(x => x.ConsignmentTime >= orderConsignmentTimeFrom.Value);
            if (orderConsignmentTimeTo.HasValue)
                query = query.Where(x => x.ConsignmentTime < orderConsignmentTimeTo.Value.AddDays(1));
            if (orderStatuses?.Any() ?? false)
                query = query.Where(x => orderStatuses.Contains((int)x.OrderStatus));
            if (paymentStatuses?.Any() ?? false)
                query = query.Where(x => paymentStatuses.Contains((int)x.PaymentStatus));
            if (shippingStatuses?.Any() ?? false)
                query = query.Where(x => shippingStatuses.Contains((int)x.Trip.ShippingStatus));

            query = query.OrderByDescending(x => x.ConsignmentTime);

            return new PagedList<ConsignmentOrder>(query, pageIndex, pageSize);
        }

        #region Goods

        public virtual void InsertGoods(Goods entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            goodsRepository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void DeleteGoods(Goods entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            goodsRepository.Delete(entity);

            eventPublisher.EntityDeleted(entity);
        }

        public virtual void DeleteGoods(ICollection<Goods> goods)
        {
            if (null == goods)
                throw new ArgumentNullException(nameof(goods));

            goodsRepository.Delete(goods);
        }

        #endregion

        #endregion
    }
}
