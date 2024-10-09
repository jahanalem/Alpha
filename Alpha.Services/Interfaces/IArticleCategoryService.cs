using Alpha.Models;
using Alpha.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IArticleCategoryService : IBaseService<ArticleCategory>
    {
        Task<int> CreateOrUpdateAsync(ArticleCategoryViewModel model);

        Task<List<ArticleCategory>> GetSelfAndDescendants(int id);
    }
}
