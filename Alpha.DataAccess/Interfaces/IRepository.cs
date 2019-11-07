using Alpha.Models;

namespace Alpha.DataAccess.Interfaces
{
    public interface IRepository<TEntity> : IBaseRepository<TEntity, int> where TEntity : Entity
    {

    }
}
