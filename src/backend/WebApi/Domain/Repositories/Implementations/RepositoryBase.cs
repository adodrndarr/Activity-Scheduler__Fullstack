using ActivityScheduler.Domain.DataAccess;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Domain.Repositories.Implementations
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public RepositoryBase(ActivitySchedulerDbContext dbContext)
        {
            this.DbContext = dbContext;
        }


        public ActivitySchedulerDbContext DbContext { get; set; }

        public IQueryable<T> GetAll()
        {
            return DbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return DbContext.Set<T>().Where(expression).AsNoTracking();
        }

        public void Create(T entity)
        {
            DbContext.Set<T>().Add(entity);
        }

        public void CreateMany(IEnumerable<T> entities)
        {
            DbContext.Set<T>().AddRange(entities);
        }

        public void Update(T entity)
        {
            DbContext.Set<T>().Update(entity);
        }

        public void Delete(T entity)
        {
            DbContext.Set<T>().Remove(entity);
        }

        public void DeleteMany(IEnumerable<T> entities)
        {
            DbContext.Set<T>().RemoveRange(entities);
        }
    }
}
