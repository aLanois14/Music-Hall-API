using MusicHall.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicHall.Services.Shared
{
    public class CRUDService<T> : ICRUDService<T> where T : BaseEntity
    {
        #region Fields
        protected readonly IRepository<T> _repository;
        #endregion

        #region Ctor

        public CRUDService(IRepository<T> repository)
        {
            _repository = repository;
        }

        #endregion

        #region Methods

        public virtual IQueryable<T> GetAll(bool tracking = true)
        {
            IQueryable<T> query;
            if (tracking)
            {
                query = _repository.Table;
            }
            else
            {
                query = _repository.TableNoTracking;
            }

            return query;
        }

        public virtual T GetById(int id)
        {
            if (id == 0)
                return null;

            var query = _repository
                .Table
                .FirstOrDefault(x => x.Id == id);

            return query;
        }

        public virtual void Create(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.CreatedAtUtc = DateTime.UtcNow;
            entity.UpdatedAtOnUtc = DateTime.UtcNow;

            _repository.Insert(entity);
        }

        public virtual void Create(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _repository.Insert(entities);
        }

        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.UpdatedAtOnUtc = DateTime.UtcNow;

            _repository.Update(entity);
        }

        public virtual void Update(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _repository.Update(entities);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _repository.Delete(entity);
        }

        public virtual void Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            _repository.Delete(entities);
        }

        #endregion
    }
}
