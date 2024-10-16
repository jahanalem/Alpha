using Alpha.DataAccess.UnitOfWork;
using Alpha.Models.Interfaces;
using Microsoft.Extensions.Logging;

namespace Alpha.Services
{
    public class BaseService<TEntity> where TEntity : class, IBaseEntity
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILogger _logger;

        public BaseService(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
    }
}
