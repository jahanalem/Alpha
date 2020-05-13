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
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        private readonly ApplicationDbContext context;
        private DbSet<TEntity> entities;
        string errorMessage = string.Empty;

        protected Repository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<TEntity>();
        }

        public DbSet<TEntity> Instance()
        {
            return entities;
        }

        public ApplicationDbContext DbContext()
        {
            return this.context;
        }
        #region Insert

        public virtual async Task<EntityState> InsertAsync(TEntity entity)
        {
            if (!entity.CreatedDate.HasValue)
                entity.CreatedDate = DateTime.UtcNow;
            await entities.AddAsync(entity);
            context.Entry(entity).State = EntityState.Added;
            //await context.SaveChangesAsync();
            //context.Entry(entity).Entity.Id
            return EntityState.Added;
        }

        #endregion

        #region Delete

        public virtual int Remove(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            EntityEntry<TEntity> obj = entities.Remove(entity);
            return obj.Entity.Id;
        }

        public virtual void Remove(int id)
        {
            var x = FindByIdAsync(id).Result;
            Remove(x);
        }

        public virtual async Task RemoveWhile(Expression<Func<TEntity, bool>> predicate)
        {
            var candidates = await FetchByCriteria(predicate).ToListAsync();
            foreach (var item in candidates)
            {
                Remove(item);
            }
        }
        public virtual async Task Remove(Expression<Func<TEntity, bool>> predicate)
        {
            var item = await FetchByCriteria(predicate).SingleOrDefaultAsync();
            Remove(item);
        }
        #endregion

        #region Update

        public virtual void Update(TEntity entity)
        {
            entity.ModifiedDate = DateTime.UtcNow;
            context.Entry(entity).State = EntityState.Modified;
            entities.Update(entity);
        }

        public virtual void UpdatePartial(TEntity entity,
            params Expression<Func<TEntity, object>>[] properties)
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
        public virtual async Task<int> AddOrUpdateAsync(TEntity entity,
            Expression<Func<TEntity, bool>> predicate)
        {
            context.Entry(entity).State = entities.AddOrUpdate(entity, predicate);
            await context.SaveChangesAsync();
            return context.Entry(entity).Entity.Id;
        }

        #endregion

        #region Get

        #region Search

        public virtual async Task<bool> ExistsAsync(object primaryKey)
        {
            return await entities.FindAsync(primaryKey) != null;
        }

        public virtual IQueryable<TEntity> FetchByCriteria(Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> items = entities;
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    items = items.Include(includeProperty);
                }
            }
            if (predicate != null)
                items = items.Where(predicate);
            return items;
        }

        public virtual async Task<TEntity> FindByIdAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await FetchByCriteria(null, includeProperties).SingleOrDefaultAsync(x => x.Id == id);
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
