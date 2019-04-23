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
            string[] serialNums = null,
            string serialNum = null,
            DateTime? consignmentTimeFrom = null,
            DateTime? consignmentTimeTo = null,
            IList<int> orderStatuses = null,
            IList<int> paymentStatuses = null)
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
            if (serialNums?.Any() ?? false)
                query = query.Where(x => serialNums.Contains(x.SerialNum));
            if (!string.IsNullOrWhiteSpace(serialNum))
                query = query.Where(x => x.SerialNum.Contains(serialNum));
            if (consignmentTimeFrom.HasValue)
                query = query.Where(x => x.ConsignmentTime >= consignmentTimeFrom.Value);
            if (consignmentTimeTo.HasValue)
                query = query.Where(x => x.ConsignmentTime < consignmentTimeTo.Value.AddDays(1));
            if (orderStatuses?.Any() ?? false)
                query = query.Where(x => orderStatuses.Contains((int)x.OrderStatus));
            if (paymentStatuses?.Any() ?? false)
                query = query.Where(x => paymentStatuses.Contains((int)x.PaymentStatus));

            query = query.OrderByDescending(x => x.UTime ?? x.CTime);

            var list = new PagedList<ConsignmentOrder>(query, pageIndex, pageSize);

            return list;
        }

        public virtual IList<ConsignmentOrder> Get(int[] ids)
        {
            if (null == ids || !ids.Any())
                return new List<ConsignmentOrder>();

            var list = repository.Table.Where(x => !x.Deleted)
                            .Where(x => ids.Contains(x.Id))
                            .ToList();
            var sortedList = new List<ConsignmentOrder>();
            foreach (var id in ids)
            {
                var item = list.Find(x => x.Id == id);
                if (null != item)
                    sortedList.Add(item);
            }

            return sortedList;
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

            entity.PaymentStatus = GetPaymentStatus(entity);

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.UTime = DateTime.UtcNow;
            entity.PaymentStatus = GetPaymentStatus(entity);

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

        public virtual OrderStatementAggregate GetAggregates(
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

            var list = query.GroupBy(x => true)
                            .Select(x => new OrderStatementAggregate
                            {
                                Receivable = x.Sum(y => y.Receivable),
                                Receipts = x.Sum(y => y.Receipts)
                            });

            return list.FirstOrDefault() ?? new OrderStatementAggregate();
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

        #region Utilities

        protected virtual PaymentStatus GetPaymentStatus(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            if (entity.PaymentStatus == PaymentStatus.Cancelled)
                return PaymentStatus.Cancelled;

            if (entity.Receivable.HasValue && entity.Receipts.HasValue)
            {
                if (entity.Receivable.Value <= entity.Receipts.Value)
                    return PaymentStatus.Paid;
                else if (entity.Receipts.Value > 0)
                    return PaymentStatus.PartiallyPaid;
                else
                    return PaymentStatus.Pending;
            }
            else if (entity.Receivable.HasValue)
                return PaymentStatus.Pending;
            else if (entity.Receipts.HasValue)
                return PaymentStatus.Paid;

            return PaymentStatus.Unknown;
        }

        #endregion
    }
}
