using Alpha.Models;
using Alpha.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IArticleCategoryService
    {
        Task<ArticleCategory> CreateOrUpdateAsync(ArticleCategoryViewModel model);
        Task<List<ArticleCategory>> GetSelfAndDescendants(int id);
        Task<List<ArticleCategory>> GetAllAsync();
        Task<ArticleCategory> GetByIdAsync(int id);
        Task<List<ArticleCategory>> GetByIsActiveAsync(bool isActive);
        Task DeleteAsync(ArticleCategory articleCategory);
    }
}
