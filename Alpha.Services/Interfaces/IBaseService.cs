using Alpha.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IBaseService<TEntity> where TEntity : Entity, new()
    {
        int Delete(TEntity entity);

        void Delete(int id);

        Task<bool> ExistsAsync(object primaryKey);

        IQueryable<TEntity> GetByCriteria(int? itemsPerPage = null,
            int? pageNumber = null,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> GetByCriteria(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        Task<TEntity> FindByIdAsync(int? id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<int> CreateAsync(TEntity entity);

        Task<TEntity> FindAsync(object primaryKey);

        void Update(TEntity entity);

        void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        Task<int> AddOrUpdateAsync(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}