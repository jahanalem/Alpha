using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Alpha.Models;

namespace Alpha.DataAccess.Interfaces
{
    public interface IBaseRepository<TEntity, TId> where TEntity : Entity
    {
        int InsertAsync(TEntity entity);

        int Delete(TEntity entity);
        void Delete(TId id);

        void Update(TEntity entity);
        void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        TId AddOrUpdate(TEntity entity);

        Task<TEntity> FindByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAll();
        IAsyncEnumerable<TEntity> GetAllAsyncEnumerable();
        Task<List<TEntity>> GetAllAsync();
        //void Add(T entity);
        //void Remove(T entity);
        Task<TEntity> SingleAsync(object primaryKey);
        TEntity SingleOrDefault(object primaryKey);
        Task<bool> ExistsAsync(object primaryKey);
        
        Task<int> SaveChangesAsync();
    }
}
