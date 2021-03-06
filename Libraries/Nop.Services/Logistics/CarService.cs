﻿using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using Nop.Services.Events;
using System;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class CarService : ICarService
    {
        #region Fields

        private readonly IRepository<Car> repository;
        private readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctor

        public CarService(
            IRepository<Car> repository,
            IEventPublisher eventPublisher)
        {
            this.repository = repository;
            this.eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Car> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string license = null,
            bool? enabled = null,
            string[] licenses = null)
        {
            var query = repository.TableNoTracking.Where(x => !x.Deleted);

            if (!string.IsNullOrWhiteSpace(license))
            {
                license = license.Trim();
                query = query.Where(x => x.License.Contains(license));
            }
            if (null != licenses && licenses.Any())
                query = query.Where(x => licenses.Contains(x.License));
            if (enabled.HasValue)
                query = query.Where(x => x.Enabled == enabled.Value);

            query = query.OrderByDescending(x => x.License)
                        .ThenByDescending(x => x.CTime);

            var list = new PagedList<Car>(query, pageIndex, pageSize);

            return list;
        }

        public virtual Car Get(int id)
        {
            if (id <= 0)
                return null;

            return repository.GetById(id);
        }

        public virtual void Insert(Car entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(Car entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.UTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityUpdated(entity);
        }

        public virtual void Delete(Car entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.Deleted = true;
            entity.DTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityDeleted(entity);
        }

        public virtual string[] GetNotExistings(params string[] idOrLicenses)
        {
            if (null == idOrLicenses)
                throw new ArgumentNullException(nameof(idOrLicenses));

            var query = repository.TableNoTracking;
            var queryFilter = idOrLicenses.Distinct().ToArray();

            // License
            var filter = query.Where(x => x.Enabled).Select(x => x.License).Where(x => queryFilter.Contains(x)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            if (!queryFilter.Any())
                return queryFilter;

            // ID
            filter = query.Where(x => x.Enabled).Select(x => x.Id.ToString()).Where(x => queryFilter.Contains(x)).ToList();
            queryFilter = queryFilter.Except(filter).ToArray();

            return queryFilter;
        }

        #endregion
    }
}
