﻿using System;
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
    public abstract class BaseService<TIRepository, TEntity> where TEntity : Entity, new()
        where TIRepository : IRepository<TEntity>
    {
        private TIRepository _repository;
        public BaseService(TIRepository obj)
        {
            _repository = obj;
        }

        #region Insert

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            return await _repository.InsertAsync(entity);
        }


        #endregion

        #region Delete

        public virtual int Delete(TEntity entity)
        {
            return _repository.Delete(entity);
        }

        public virtual void Delete(int id)
        {
            _repository.Delete(id);
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

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }
        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual IAsyncEnumerable<TEntity> GetAllAsyncEnumerable()
        {
            return _repository.GetAllAsyncEnumerable();
        }

        #region Search

        public virtual async Task<bool> ExistsAsync(object primaryKey)
        {
            return await _repository.ExistsAsync(primaryKey);
        }

        //public virtual IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties)
        //{
        //    return _repository.FindAll(null, includeProperties);
        //}


        public virtual IQueryable<TEntity> GetByCriteria(int? itemsPerPage = null,
            int? pageNumber = null,
            Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            if (itemsPerPage.HasValue && pageNumber.HasValue)
            {
                if (itemsPerPage > 0 && pageNumber > 0)
                {
                    return _repository.FindAll(predicate, includeProperties)
                        .Skip((pageNumber.Value - 1) * itemsPerPage.Value).Take(itemsPerPage.Value);
                }
            }
            return _repository.FindAll(predicate, includeProperties);
        }


        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FindAll(predicate, includeProperties);
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
