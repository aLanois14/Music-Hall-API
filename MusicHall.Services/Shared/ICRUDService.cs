using MusicHall.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicHall.Services.Shared
{
    public interface ICRUDService<T> where T : BaseEntity
    {
        IQueryable<T> GetAll(bool tracking = true);

        T GetById(int id);

        void Create(T entity);

        void Create(IEnumerable<T> entities);

        void Update(T entity);

        void Update(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(IEnumerable<T> entities);
    }
}
