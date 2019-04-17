﻿using Nop.Core;
using Nop.Core.Domain.Logistics;
using Nop.Services.Common;
using System;

namespace Nop.Services.Logistics
{
    public partial interface ITripService
    {
        IPagedList<Trip> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string carLicense = null,
            string driverName = null,
            string[] serialNums = null);

        Trip Get(int id);

        void Insert(Trip entity);

        void Update(Trip entity);

        void Delete(Trip entity);

        void DeleteConsignmentOrder(ConsignmentOrder entity);

        IPagedList<Trip> GetStatistics(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            DateTime? tripShippingTimeFrom = null,
            DateTime? tripShippingTimeTo = null,
            DateTime? orderConsignmentTimeFrom = null,
            DateTime? orderConsignmentTimeTo = null,
            string driverName = null,
            string carLicense = null);

        IPagedList<BalanceReport> StatisticsBalance(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            StatisticsFrequency frequency = StatisticsFrequency.Daily,
            DateTime? tripShippingTimeFrom = null,
            DateTime? tripShippingTimeTo = null);
    }
}
