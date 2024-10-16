using Alpha.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Alpha.DataAccess.Helper
{
    public static class RepositoryUtility
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, IEnumerable<T> records)
        where T : BaseEntity
        {
            foreach (var data in records)
            {
                var exists = dbSet.AsNoTracking().Any(x => x.Id == data.Id);
                if (exists)
                {
                    dbSet.Update(data);
                    continue;
                }
                dbSet.Add(data);
            }
        }
        public static EntityState AddOrUpdate<TEntity>(this DbSet<TEntity> dbSet, TEntity data)
        where TEntity : BaseEntity
        {
            {
                var exists = dbSet.AsNoTracking().Any(x => x.Id == data.Id);
                if (exists)
                {
                    data.ModifiedDate = DateTime.UtcNow;
                    dbSet.Update(data);
                    return EntityState.Modified;
                }
                else
                {
                    data.CreatedDate = DateTime.UtcNow;
                    dbSet.Add(data);
                    return EntityState.Added;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="data"></param>
        /// <param name="predicate">if result of predicate is true, it means the object exists in database.</param>
        /// <returns></returns>
        public static EntityState AddOrUpdate<TEntity>(this DbSet<TEntity> dbSet, TEntity data,
            Expression<Func<TEntity, bool>> predicate)
        where TEntity : BaseEntity
        {
            {
                var exists = dbSet.Where(predicate).Any();
                if (exists)
                {
                    data.ModifiedDate = DateTime.UtcNow;
                    dbSet.Update(data);
                    return EntityState.Modified;
                }
                else
                {
                    data.CreatedDate = DateTime.UtcNow;
                    dbSet.Add(data);
                    return EntityState.Added;
                }
            }
        }
    }
}
