using Alpha.Models;
using Alpha.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Alpha.Services
{
    public class ArticleService : BaseService<IArticleRepository, Article>, IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTagRepository _articleTagRepository;
        private IArticleTagService _articleTagService;
        private ITagRepository _tagRepository;
        private IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork uow, IArticleRepository articleRepository, IArticleTagRepository articleTagRepository, ITagRepository tagRepository)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _articleTagRepository = articleTagRepository;
            _articleTagService = new ArticleTagService(_articleTagRepository);
            _tagRepository = tagRepository;
            _unitOfWork = uow;
        }

        public IQueryable<Article> FilterByTag(int? tagId)
        {
            IQueryable<Article> query;
            if (tagId != null)
            {
                query = _articleRepository.Instance().Where(c => c.IsActive).Join(_articleTagRepository.Instance()
                    .Where(x => x.TagId == tagId.Value), a => a.Id, at => at.ArticleId, (a, at) => a);
            }
            else
            {
                query = _articleRepository.Instance().Where(c => c.IsActive);
            }

            return query;
        }

        public async Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int pageNumber)
        {
            var itemsPerPage = PagingInfo.DefaultItemsPerPage;
            var articleViewModelList = new List<ArticleViewModel>();

            IQueryable<Article> query = FilterByTag(tagId);

            var articleList = await query.OrderByDescending(k => k.CreatedDate)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();

            foreach (var article in articleList)
            {
                var tags = _articleTagService.GetTagsByArticleId(article.Id);
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
            if (tagId != null)
            {
                result.Pagination.QueryStrings.TryAdd(QueryStringParameters.TagId, tagId.Value.ToString());// = new Dictionary<string, string> { { "tagId", tagId.Value.ToString() } };
            }
            return result;
        }

        public virtual List<Tag> GetTagsByArticleId(int articleId)
        {
            return _articleTagService.GetTagsByArticleId(articleId);
        }

        public virtual async Task<ArticleViewModel> GetArticleById(int articleId)
        {
            var vm = new ArticleViewModel();
            vm.Article = await FindByIdAsync(articleId);
            vm.Tags = GetTagsByArticleId(articleId);
            //vm.AllOfTags = _tagRepository.GetAll().Where(c => c.IsActive == true).ToList();
            return vm;
        }

        /// <summary>
        /// IsActive property is "true" in general set
        /// </summary>
        /// <param name="tagList">return related tags along with the general set tags</param>
        /// <returns></returns>
        public virtual List<Tag> SpecifyRelatedTagsInTheGeneralSet(List<Tag> tagList)
        {
            List<Tag> allAvailableTags = _tagRepository.GetAll().Where(c => c.IsActive == true).ToList();
            for (int t = 0; t < allAvailableTags.Count; t++)
            {
                allAvailableTags[t].IsActive = false;
            }
            foreach (var tag in tagList)
            {
                for (int t = 0; t < allAvailableTags.Count; t++)
                {
                    if (allAvailableTags[t].Id == tag.Id)
                    {
                        allAvailableTags[t].IsActive = true;
                    }
                }
            }
            return allAvailableTags;
        }

        public virtual async Task<List<ArticleViewModel>> GetAllOfArticleViewModel()
        {
            var result = new List<ArticleViewModel>();
            List<int> articleIds = _articleTagService.GetAll().Select(c => c.ArticleId).Distinct().ToList();
            foreach (var id in articleIds)
            {
                result.Add(await GetArticleById(id));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>It returns Id from new Article. If operation was not successfully it returns -1.</returns>
        public virtual async Task<int> InsertAsync(ArticleViewModel viewModel)
        {
            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var articleId = await _unitOfWork.Article.AddOrUpdateAsync(viewModel.Article);
                    if (viewModel.AllTags != null)
                        foreach (var tag in viewModel.AllTags.Where(t => t.IsActive == true))
                        {
                            var at = new ArticleTag()
                            {
                                ArticleId = articleId,
                                TagId = tag.Id
                            };
                            await _unitOfWork.ArticleTag.AddOrUpdateAsync(at);
                        }

                    await transaction.Result.CommitAsync();
                    return articleId;
                }
                catch (Exception)
                {
                    await transaction.Result.RollbackAsync();
                    return -1;
                }
            }
        }
    }
}
