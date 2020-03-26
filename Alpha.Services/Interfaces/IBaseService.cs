using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.Services.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : Entity, new()
    {
        int Delete(TEntity entity);
        void Delete(int id);
        Task<bool> ExistsAsync(object primaryKey);
        IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> FindByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
        IEnumerable<TEntity> GetAll();

        Task<List<TEntity>> GetAllAsync();
        Task<int> InsertAsync(TEntity entity);
        Task<TEntity> SingleAsync(object primaryKey);
        TEntity SingleOrDefault(object primaryKey);
        void Update(TEntity entity);
        void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties);
        int AddOrUpdate(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}