using Alpha.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface ITagService
    {
        Task<List<Tag>> GetTagsByIsActiveAsync(bool isActive);
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetByIdAsync(int id);
        Task<Tag> CreateAsync(Tag tag);
        Task UpdateAsync(Tag tag);
        Task DeleteAsync(Tag tag);
        Task<bool> ExistsAsync(int id);
    }
}