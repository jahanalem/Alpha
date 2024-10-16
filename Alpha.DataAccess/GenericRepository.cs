using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Alpha.DataAccess
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        string errorMessage = string.Empty;

        public GenericRepository() { }

        public GenericRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _context.Set<T>().AnyAsync(c => c.Id == id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _context.Set<T>().AsNoTracking();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includeRelatedTables)
        {
            IQueryable<T> query = _context.Set<T>().AsQueryable();
            foreach (var item in includeRelatedTables)
            {
                query = query.Include(item);
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, object>> orderBy = null, bool ascending = false)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (orderBy != null)
            {
                query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public virtual EntityEntry<T> Add(T entity)
        {
            return _context.Set<T>().Add(entity);
        }

        public virtual async Task<EntityEntry<T>> AddAsync(T entity)
        {
            return await _context.Set<T>().AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public virtual Task UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            // No SaveChangesAsync() call here to allow use within a transaction
            return Task.CompletedTask;
        }

        public virtual async Task<T> AddOrUpdateAsync(T entity)
        {
            T existingEntity = await _context.Set<T>().FindAsync(entity.Id);
            if (existingEntity != null)
            {
                var createdDate = existingEntity.CreatedDate; // Preserve the existing CreatedDate value

                _context.Entry(existingEntity).CurrentValues.SetValues(entity);

                // Ensure that CreatedDate is not overwritten
                existingEntity.CreatedDate = createdDate;

                _context.Entry(existingEntity).State = EntityState.Modified;
                return existingEntity;
            }
            else
            {
                var addedEntity = await _context.Set<T>().AddAsync(entity);
                return addedEntity.Entity;
            }
        }

        public virtual void UpdatePartial(T entity, params Expression<Func<T, object>>[] properties)
        {
            var entry = _context.Entry(entity);
            _context.Set<T>().Attach(entity);

            foreach (var prop in properties)
            {
                entry.Property(prop).IsModified = true;
            }
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public virtual async Task DeleteByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
            }
            else
            {
                throw new ArgumentException("Entity with the given ID does not exist.");
            }
        }

        public virtual IQueryable<T> GetByCriteria(Expression<Func<T, bool>> predicate = null,
                                             params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return query;
        }

        public virtual async Task DeleteByCriteriaAsync(Expression<Func<T, bool>> criteria)
        {
            var item = await GetByCriteria(criteria).SingleOrDefaultAsync();
            await DeleteAsync(item);
        }
    }
}
