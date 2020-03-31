using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Alpha.Models;
using Microsoft.EntityFrameworkCore;

namespace Alpha.DataAccess.Interfaces
{
    public interface IBaseRepository<TEntity, TId> where TEntity : Entity
    {
        DbSet<TEntity> Instance();
        Task<int> InsertAsync(TEntity entity);

        int Remove(TEntity entity);
        void Remove(TId id);

        void Update(TEntity entity);
        void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        Task<int> AddOrUpdateAsync(TEntity entity);

        Task<TEntity> FindByIdAsync(TId id, params Expression<Func<TEntity, object>>[] includeProperties);

        IQueryable<TEntity> FetchByCriteria(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        
        Task<TEntity> FindAsync(object primaryKey);
        
        Task<bool> ExistsAsync(object primaryKey);
        
        Task<int> SaveChangesAsync();
    }
}
