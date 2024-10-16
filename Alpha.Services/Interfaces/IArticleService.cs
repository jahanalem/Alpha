using Alpha.Models;
using Alpha.ViewModels;
using Alpha.ViewModels.Searches;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IArticleService
    {
        List<Tag> GetTagsByArticleId(int articleId);
        Task<List<ArticleViewModel>> GetAllOfArticleViewModel();
        Task<ArticleViewModel> GetArticleByIdAsync(int articleId);
        IQueryable<Article> FilterByTag(int? tagId);
        IQueryable<Article> FilterByCriteria(int? tagId = null, int? catId = null);
        Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int pageNumber = 1, int items = 10);
        Task<int> InsertAsync(ArticleViewModel viewModel);
        Task<List<Tag>> SpecifyRelatedTagsInTheGeneralSet(List<Tag> tagList);
        Task<SearchResultsViewModel> Search(string search, int? pageNumber, int? items);
        Task<SearchResultsViewModel> Search(string searchTerm);
        Task DeleteAsync(Article article);
        Task<bool> ArticleExists(int id);
    }
}