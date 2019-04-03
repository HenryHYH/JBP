using Nop.Core;
using Nop.Core.Data;
using Nop.Core.Domain.Logistics;
using Nop.Services.Events;
using System;
using System.Linq;

namespace Nop.Services.Logistics
{
    public partial class GoodsService : IGoodsService
    {
        #region Fields

        private readonly IRepository<Goods> repository;
        private readonly IEventPublisher eventPublisher;

        #endregion

        #region Ctor

        public GoodsService(
            IRepository<Goods> repository,
            IEventPublisher eventPublisher)
        {
            this.repository = repository;
            this.eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Goods> GetAll(
            int pageIndex = 0,
            int pageSize = int.MaxValue,
            string name = null)
        {
            var query = repository.TableNoTracking.Where(x => !x.Deleted);

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                query = query.Where(x => x.Name.Contains(name));
            }

            query = query.OrderByDescending(x => x.UTime ?? x.CTime);

            var list = new PagedList<Goods>(query, pageIndex, pageSize);

            return list;
        }

        public virtual Goods Get(int id, bool loadCacheableCopy = true)
        {
            if (id <= 0)
                return null;

            return repository.GetById(id);
        }

        public virtual void Insert(Goods entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            repository.Insert(entity);

            eventPublisher.EntityInserted(entity);
        }

        public virtual void Update(Goods entity)
        {
            if (null == entity)
                throw new ArgumentNullException(nameof(entity));

            entity.UTime = DateTime.UtcNow;

            repository.Update(entity);

            eventPublisher.EntityUpdated(entity);
        }

        public virtual void Delete(Goods entity)
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
