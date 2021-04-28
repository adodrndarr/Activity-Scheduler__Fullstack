using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Domain.Repositories.Interfaces
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void CreateMany(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteMany(IEnumerable<T> entities);
    }
}
