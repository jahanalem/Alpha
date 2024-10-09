using Alpha.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Alpha.DataAccess.Interfaces
{
    public interface IBaseAsyncRepository<TEntity, TId> where TEntity : Entity
    {
        TEntity FindByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindAllAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> GetAllAsync();
        //void Add(T entity);
        //void Remove(T entity);
        TEntity SingleAsync(object primaryKey);
        TEntity SingleOrDefaultAsync(object primaryKey);
        bool ExistsAsync(object primaryKey);
        int InsertAsync(TEntity entity);
        void UpdateAsync(TEntity entity);
        void UpdatePartialAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        int DeleteAsync(TEntity entity);
        void DeleteAsync(TId id);
        TId AddOrUpdateAsync(TEntity entity);
    }
}
