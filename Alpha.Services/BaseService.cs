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
    public class BaseService<TIRepository, TEntity> where TEntity : Entity, new()
        where TIRepository : IRepository<TEntity>
    {
        private TIRepository _repository;
        public BaseService(TIRepository obj)
        {
            _repository = obj;
        }


        public virtual int Delete(TEntity entity)
        {
            return _repository.Delete(entity);
        }

        public virtual void Delete(int id)
        {
            _repository.Delete(id);
        }

        public virtual async Task<bool> ExistsAsync(object primaryKey)
        {
            return await _repository.ExistsAsync(primaryKey);
        }

        public virtual IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FindAll(includeProperties);
        }

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return _repository.FindAll(predicate, includeProperties);
        }

        public virtual async Task<TEntity> FindByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await _repository.FindByIdAsync(id, includeProperties);
        }

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

        public virtual int InsertAsync(TEntity entity)
        {
            return _repository.InsertAsync(entity);
        }

        public virtual async Task<TEntity> SingleAsync(object primaryKey)
        {
            return await _repository.SingleAsync(primaryKey);
        }

        public virtual TEntity SingleOrDefault(object primaryKey)
        {
            return _repository.SingleOrDefault(primaryKey);
        }

        public virtual void Update(TEntity entity)
        {
            _repository.Update(entity);
        }

        public virtual void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            _repository.UpdatePartial(entity, properties);
        }

        public virtual int AddOrUpdate(TEntity entity)
        {
            return _repository.AddOrUpdate(entity);
        }

        public virtual Task<int> SaveChangesAsync()
        {
            return _repository.SaveChangesAsync();
        }
    }
}
