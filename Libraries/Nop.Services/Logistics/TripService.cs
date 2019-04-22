using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Data.Extensions;
using Nop.Core.Domain.Logistics;
using Nop.Data;
using Nop.Services.Common;
using Nop.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class TripService : ITripService
    {
        #region Fields

        private readonly IDataProvider dataProvider;
        private readonly IDbContext dbContext;
        private readonly IRepository<Trip> repository;
        private readonly IRepository<ConsignmentOrder> consignmentOrderRepository;
        private readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctor

        public TripService(
            IDataProvider dataProvider,
            IDbContext dbContext,
            IRepository<Trip> repository,
            IRepository<ConsignmentOrder> consignmentOrderRepository,
            IEventPublisher eventPublisher)
        {
            this.dataProvider = dataProvider;
            this.dbContext = dbContext;
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
            string[] serialNums = null,
            string serialNum = null,
            IList<int> shippingStatuses = null,
            DateTime? startAtFrom = null,
            DateTime? startAtTo = null,
            DateTime? endAtFrom = null,
            DateTime? endAtTo = null)
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
            if (serialNums?.Any() ?? false)
                query = query.Where(x => serialNums.Contains(x.SerialNum));
            if (!string.IsNullOrWhiteSpace(serialNum))
                query = query.Where(x => x.SerialNum.Contains(serialNum));
            if (shippingStatuses?.Any() ?? false)
                query = query.Where(x => shippingStatuses.Contains((int)x.ShippingStatus));
            if (startAtFrom.HasValue)
                query = query.Where(x => x.StartAt >= startAtFrom.Value);
            if (startAtTo.HasValue)
                query = query.Where(x => x.StartAt < startAtTo.Value.AddDays(1));
            if (endAtFrom.HasValue)
                query = query.Where(x => x.EndAt >= endAtFrom.Value);
            if (endAtTo.HasValue)
                query = query.Where(x => x.EndAt < endAtTo.Value.AddDays(1));

            query = query.OrderByDescending(x => x.UTime ?? x.CTime);

            var list = new PagedList<Trip>(query, pageIndex, pageSize);

            return list;
        }

        public virtual IList<Trip> Get(int[] ids)
        {
            if (null == ids || !ids.Any())
                return new List<Trip>();

            var list = repository.Table.Where(x => !x.Deleted)
                            .Where(x => ids.Contains(x.Id))
                            .ToList();
            var sortedList = new List<Trip>();
            foreach (var id in ids)
            {
                var item = list.Find(x => x.Id == id);
                if (null != item)
                    sortedList.Add(item);
            }

            return sortedList;
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

            entity.ShippingStatus = GetShippingStatus(entity);

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(Trip entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.ShippingStatus = GetShippingStatus(entity);
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

        #region Consignment Orders

        public virtual void DeleteConsignmentOrder(ConsignmentOrder entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.TripId = null;
            consignmentOrderRepository.Update(entity);

            eventPublisher.EntityDeleted(entity);
        }

        #endregion

        #region Statistics

        public virtual IPagedList<Trip> GetStatistics(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            DateTime? tripShippingTimeFrom = null,
            DateTime? tripShippingTimeTo = null,
            DateTime? orderConsignmentTimeFrom = null,
            DateTime? orderConsignmentTimeTo = null,
            string driverName = null,
            string carLicense = null)
        {
            var query = repository.Table.Where(x => !x.Deleted);

            if (tripShippingTimeFrom.HasValue)
                query = query.Where(x => x.EndAt >= tripShippingTimeFrom.Value.Date);
            if (tripShippingTimeTo.HasValue)
                query = query.Where(x => x.EndAt < tripShippingTimeTo.Value.Date.AddDays(1));
            if (orderConsignmentTimeFrom.HasValue)
                query = query.Where(x => x.Orders.Any(y => y.CTime >= orderConsignmentTimeFrom.Value.Date));
            if (orderConsignmentTimeTo.HasValue)
                query = query.Where(x => x.Orders.Any(y => y.CTime < orderConsignmentTimeTo.Value.Date.AddDays(1)));
            if (!string.IsNullOrWhiteSpace(driverName))
            {
                driverName = driverName.Trim();
                query = query.Where(x => x.Driver.Name.Contains(driverName));
            }
            if (!string.IsNullOrWhiteSpace(carLicense))
            {
                carLicense = carLicense.Trim();
                query = query.Where(x => x.Car.License.Contains(carLicense));
            }

            return new PagedList<Trip>(query, pageIndex, pageSize);
        }

        public virtual IPagedList<GroupBalanceReport> StatisticsBalance(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            StatisticsFrequency frequency = StatisticsFrequency.Daily,
            DateTime? tripShippingTimeFrom = null,
            DateTime? tripShippingTimeTo = null)
        {
            var pFrequency = dataProvider.GetInt32Parameter("Frequency", (int)frequency);
            var pTripShippingTimeFrom = dataProvider.GetDateTimeParameter("TripShippingTimeFrom", tripShippingTimeFrom);
            var pTripShippingTimeTo = dataProvider.GetDateTimeParameter("TripShippingTimeTo", tripShippingTimeTo);
            var pPageIndex = dataProvider.GetInt32Parameter("PageIndex", pageIndex);
            var pPageSize = dataProvider.GetInt32Parameter("PageSize", pageSize);
            var pTotalRecords = dataProvider.GetOutputInt32Parameter("TotalRecords");
            var list = dbContext.QueryFromSql<BalanceReport>("EXEC [TripBalanceReport]",
                                                pFrequency,
                                                pTripShippingTimeFrom,
                                                pTripShippingTimeTo,
                                                pPageIndex,
                                                pPageSize,
                                                pTotalRecords).ToList();

            var totalRecords = pTotalRecords.Value != DBNull.Value ? Convert.ToInt32(pTotalRecords.Value) : 0;

            var group = list.GroupBy(x => x.StatisticsTime)
                            .Select(x => new GroupBalanceReport
                            {
                                StatisticsTime = x.Key,
                                Fees = x.Select(f => new BalanceReportFee { CategoryId = f.CategoryId, Name = f.Category, Type = f.FeeType, Amount = f.Amount })
                                        .ToDictionary(k => k.CategoryId, v => v)
                            })
                            .ToList();

            return new PagedList<GroupBalanceReport>(group, pageIndex, pageSize, totalRecords);
        }

        #endregion

        #endregion

        #region Utilities

        protected virtual ShippingStatus GetShippingStatus(Trip entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            if (entity.EndAt.HasValue)
                return ShippingStatus.已完成;
            else if (entity.StartAt.HasValue)
                return ShippingStatus.进行中;

            return ShippingStatus.未开始;
        }

        #endregion
    }
}
