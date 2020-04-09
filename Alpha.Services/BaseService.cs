using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Alpha.Services
{
    public abstract class BaseService<TIRepository, TEntity> : IBaseService<TEntity> where TEntity : Entity, new()
        where TIRepository : IRepository<TEntity>
    {
        private TIRepository _repository;
        public BaseService(TIRepository obj)
        {
            _repository = obj;
        }

        #region Create/Insert

        public virtual async Task<int> CreateAsync(TEntity entity)
        {
            return await _repository.InsertAsync(entity);
        }


        #endregion

        #region Delete/Remove

        public virtual int Delete(TEntity entity)
        {
            return _repository.Remove(entity);
        }

        public virtual void Delete(int id)
        {
            _repository.Remove(id);
        }

        #endregion

        #region Update

        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public virtual void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            _repository.UpdatePartial(entity, properties);
        }

        public virtual async Task<int> AddOrUpdateAsync(TEntity entity)
        {
            return await _repository.AddOrUpdateAsync(entity);
        }

        #endregion

        #region Get

        #region Search

        public virtual async Task<bool> ExistsAsync(object primaryKey)
        {
            return await _repository.ExistsAsync(primaryKey);
        }

        public virtual IQueryable<TEntity> GetByCriteria(int? itemsPerPage = null,
            int? pageNumber = null,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (itemsPerPage.HasValue && pageNumber.HasValue)
            {
                if (itemsPerPage > 0 && pageNumber > 0)
                {
                    return _repository.FetchByCriteria(predicate, includeProperties)
                        .OrderByDescending(c=>c.CreatedDate)
                        .Skip((pageNumber.Value - 1) * itemsPerPage.Value).Take(itemsPerPage.Value);
                }
            }
            return _repository.FetchByCriteria(predicate, includeProperties);
        }

        public virtual IQueryable<TEntity> GetByCriteria(Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FetchByCriteria(predicate, includeProperties);
        }

        public virtual async Task<TEntity> FindByIdAsync(int? id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (id != null)
                return await _repository.FindByIdAsync(id.Value, includeProperties);
            return null;
        }

        public virtual async Task<TEntity> FindAsync(object primaryKey)
        {
            return await _repository.FindAsync(primaryKey);
        }

        #endregion

        #endregion

        public virtual Task<int> SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }
    }
}
