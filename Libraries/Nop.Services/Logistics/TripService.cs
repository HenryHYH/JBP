﻿using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Data.Extensions;
using Nop.Core.Domain.Logistics;
using Nop.Data;
using Nop.Services.Common;
using Nop.Services.Events;
using System;
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
            DateTime? tripCTimeFrom = null,
            DateTime? tripCTimeTo = null,
            DateTime? orderCTimeFrom = null,
            DateTime? orderCTimeTo = null,
            string driverName = null,
            string carLicense = null)
        {
            var query = repository.Table.Where(x => !x.Deleted);

            if (tripCTimeFrom.HasValue)
                query = query.Where(x => x.CTime >= tripCTimeFrom.Value.Date);
            if (tripCTimeTo.HasValue)
                query = query.Where(x => x.CTime < tripCTimeTo.Value.Date.AddDays(1));
            if (orderCTimeFrom.HasValue)
                query = query.Where(x => x.Orders.Any(y => y.CTime >= orderCTimeFrom.Value.Date));
            if (orderCTimeTo.HasValue)
                query = query.Where(x => x.Orders.Any(y => y.CTime < orderCTimeTo.Value.Date.AddDays(1)));
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

        public virtual IPagedList<BalanceReport> StatisticsBalance(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            StatisticsFrequency frequency = StatisticsFrequency.Daily,
            DateTime? tripCTimeFrom = null,
            DateTime? tripCTimeTo = null)
        {
            var pFrequency = dataProvider.GetInt32Parameter("Frequency", (int)frequency);
            var pTripCTimeFrom = dataProvider.GetDateTimeParameter("TripCTimeFrom", tripCTimeFrom);
            var pTripCTimeTo = dataProvider.GetDateTimeParameter("TripCTimeTo", tripCTimeTo);
            var pPageIndex = dataProvider.GetInt32Parameter("PageIndex", pageIndex);
            var pPageSize = dataProvider.GetInt32Parameter("PageSize", pageSize);
            var pTotalRecords = dataProvider.GetOutputInt32Parameter("TotalRecords");
            var list = dbContext.QueryFromSql<BalanceReport>("EXEC [TripBalanceReport]",
                                                pFrequency,
                                                pTripCTimeFrom,
                                                pTripCTimeTo,
                                                pPageIndex,
                                                pPageSize,
                                                pTotalRecords).ToList();

            var totalRecords = pTotalRecords.Value != DBNull.Value ? Convert.ToInt32(pTotalRecords.Value) : 0;
            return new PagedList<BalanceReport>(list, pageIndex, pageSize, totalRecords);
        }

        #endregion

        #endregion
    }
}
