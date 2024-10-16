using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class AboutUsService : BaseService<AboutUs>, IAboutUsService
    {
        public AboutUsService(IUnitOfWork unitOfWork, ILogger<AboutUsService> logger) : base(unitOfWork, logger)
        {
        }

        public async Task<AboutUs> GetAboutUsAsync()
        {
            return await _unitOfWork.Repository<AboutUs>().GetAll().FirstOrDefaultAsync();
        }
    }
}
