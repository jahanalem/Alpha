using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IQueryable<Article> Slice(this IQueryable<Article> query, int sliceNumber, int size)
        {
            IQueryable<Article> sliceQuery = query.OrderByDescending(k => k.CreatedDate)
                .Skip((sliceNumber - 1) * size)
                .Take(size);

            return sliceQuery;
        }

        public static async Task<ArticleTagListViewModel> MapToViewModel(
            this IQueryable<Article> query,
            IArticleTagService articleTagService,
            int? tagId = null)
        {
            var articles = await query.ToListAsync();
            var articleViewModelList = new List<ArticleViewModel>();
            foreach (var article in articles)
            {
                var tags = articleTagService.GetTagsByArticleId(article.Id);
                articleViewModelList.Add(new ArticleViewModel
                {
                    Article = article,
                    Tags = tags,
                });
            }

            var result = new ArticleTagListViewModel
            {
                ArticleViewModelList = articleViewModelList,
                TagId = tagId
            };

            return result;
        }
    }
}
