using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Alpha.DataAccess.Helper;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure;
using Alpha.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Alpha.DataAccess
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext context;
        private DbSet<TEntity> entities;
        string errorMessage = string.Empty;

        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TEntity>();
        }
        
        #region Insert

        public virtual async Task<int> InsertAsync(TEntity entity)
        {
            if (!entity.CreatedDate.HasValue)
                entity.CreatedDate = DateTime.UtcNow;
            dynamic obj = await entities.AddAsync(entity);
            //SaveChangeAsync();
            return obj.Id;
        }

        #endregion

        #region Delete

        public virtual int Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            EntityEntry<TEntity> obj = entities.Remove(entity);
            return obj.Entity.Id;
        }

        public virtual void Delete(int id)
        {
            var x = FindByIdAsync(id).Result;
            Delete(x);
        }

        #endregion

        #region Update

        public virtual void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            context.Entry(entity).State = EntityState.Modified;
            entities.Update(entity);
        }

        public virtual void UpdatePartial(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            if (properties != null)
            {
                if (properties.Exist("Id") != true)
                    throw new Exception("There is no Id in array of Expression<Func<TEntity, object>>[]");
                var entry = context.Entry(entity);
                entities.Attach(entity);
                foreach (var prop in properties)
                {
                    entry.Property(prop).IsModified = true;
                }
            }
        }

        public virtual async Task<int> AddOrUpdateAsync(TEntity entity)
        {
            context.Entry(entity).State = entities.AddOrUpdate(entity);
            await context.SaveChangesAsync();
            return context.Entry(entity).Entity.Id;
        }

        #endregion

        #region Get

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await entities.AsQueryable().ToListAsync();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return entities.AsQueryable().AsEnumerable();
        }

        public IAsyncEnumerable<TEntity> GetAllAsyncEnumerable()
        {
            return entities.AsAsyncEnumerable();
        }

        #region Search

        public virtual async Task<bool> ExistsAsync(object primaryKey)
        {
            return await entities.FindAsync(primaryKey) != null;
        }

        public virtual IQueryable<TEntity> FindAll(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = entities;

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items;
        }

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = entities;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            return items.Where(predicate);
        }

        public virtual async Task<TEntity> FindByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FindAll(includeProperties).SingleOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task<TEntity> FindAsync(object primaryKey)
        {
            var dbResult = entities.FindAsync(primaryKey);
            return await dbResult;
        }

        #endregion

        #endregion

        public virtual async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }
    }
}
