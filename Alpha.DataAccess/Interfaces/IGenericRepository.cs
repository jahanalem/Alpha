using Alpha.Models.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alpha.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class, IBaseEntity
    {
        Task<bool> ExistsAsync(int id);
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeRelatedTables);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, object>> orderBy = null, bool ascending = false);
        EntityEntry<T> Add(T entity);
        Task<EntityEntry<T>> AddAsync(T entity);
        void Update(T entity);
        Task UpdateAsync(T entity);
        Task<T> AddOrUpdateAsync(T entity);
        void UpdatePartial(T entity, params Expression<Func<T, object>>[] properties);
        void Delete(T entity);
        Task DeleteAsync(T entity);
        Task DeleteByCriteriaAsync(Expression<Func<T, bool>> criteria);
        Task DeleteByIdAsync(int id);
        IQueryable<T> GetByCriteria(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties);
    }
}
