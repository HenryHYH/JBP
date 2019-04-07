using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using Nop.Services.Events;
using System;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class DriverService : IDriverService
    {
        #region Fields

        private readonly IRepository<Driver> repository;
        private readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctor

        public DriverService(
            IRepository<Driver> repository,
            IEventPublisher eventPublisher)
        {
            this.repository = repository;
            this.eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Driver> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string name = null,
            bool? enabled = null)
        {
            var query = repository.TableNoTracking.Where(x => !x.Deleted);

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                query = query.Where(x => x.Name.Contains(name));
            }
            if (enabled.HasValue)
                query = query.Where(x => x.Enabled == enabled.Value);

            query = query.OrderByDescending(x => x.Name)
                        .ThenByDescending(x => x.CTime);

            var list = new PagedList<Driver>(query, pageIndex, pageSize);

            return list;
        }

        public virtual Driver Get(int id)
        {
            if (id <= 0)
                return null;

            return repository.GetById(id);
        }

        public virtual void Insert(Driver entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(Driver entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.UTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityUpdated(entity);
        }

        public virtual void Delete(Driver entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.Deleted = true;
            entity.DTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityDeleted(entity);
        }

        #endregion
    }
}
