using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.ViewModels;

namespace Alpha.Services.Interfaces
{
    public interface IArticleService : IBaseService<Article>
    {
        //Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int articlePage, int pageSize,
        //    IArticleRepository articleRepository, IArticleTagRepository articleTagRepository);
        List<Tag> GetTagsByArticleId(int articleId);
        Task<List<ArticleViewModel>> GetAllOfArticleViewModel();
        Task<ArticleViewModel> GetArticleById(int articleId);

        IQueryable<Article> FilterByTag(int? tagId);
        Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int articlePage);

        Task<int> InsertAsync(ArticleViewModel viewModel);
        List<Tag> SpecifyRelatedTagsInTheGeneralSet(List<Tag> tagList);
    }
}