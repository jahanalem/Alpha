using System;
using System.Collections.Generic;
using System.Linq;
using Alpha.Models;
using Microsoft.EntityFrameworkCore;

namespace Alpha.DataAccess.Helper
{
    public static class RepositoryUtility
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, IEnumerable<T> records)
        where T : Entity
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
        where TEntity : Entity
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
    }
}
