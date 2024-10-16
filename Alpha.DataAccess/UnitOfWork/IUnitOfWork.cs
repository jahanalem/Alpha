using Alpha.DataAccess.Interfaces;
using Alpha.Models.Interfaces;
using System;
using System.Threading.Tasks;

namespace Alpha.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        Task BeginTransactionAsync();
        void CommitTransaction();
        Task CommitTransactionAsync();
        int Complete();
        Task<int> CompleteAsync();
        IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class, IBaseEntity;
        void RollbackTransaction();
        Task RollbackTransactionAsync();
    }
}