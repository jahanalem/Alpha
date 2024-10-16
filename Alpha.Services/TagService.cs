using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class TagService : BaseService<Tag>, ITagService
    {
        public TagService(IUnitOfWork unitOfWork, ILogger<TagService> logger) : base(unitOfWork, logger)
        {
        }

        public async Task<List<Tag>> GetTagsByIsActiveAsync(bool isActive)
        {
            return await _unitOfWork.Repository<Tag>().GetByCriteria(c => c.IsActive == isActive).ToListAsync();
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _unitOfWork.Repository<Tag>().GetAll().ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Tag>().GetByIdAsync(id);
        }

        public async Task<Tag> CreateAsync(Tag tag)
        {
            var newTag = await _unitOfWork.Repository<Tag>().AddOrUpdateAsync(tag);
            await _unitOfWork.CompleteAsync();

            return newTag;
        }

        public async Task UpdateAsync(Tag tag)
        {
            await _unitOfWork.Repository<Tag>().UpdateAsync(tag);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(Tag tag)
        {
            await _unitOfWork.Repository<Tag>().DeleteAsync(tag);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Repository<Tag>().ExistsAsync(id);
        }
    }
}
