using Alpha.Models;
using Alpha.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Infrastructure.LinqUtility;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models.Identity;
using Alpha.Services.Interfaces;
using Alpha.ViewModels.Searches;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Options;

namespace Alpha.Services
{
    public class ArticleService : BaseService<IArticleRepository, Article>, IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTagRepository _articleTagRepository;
        private IArticleTagService _articleTagService;
        private ITagRepository _tagRepository;
        private IUnitOfWork _unitOfWork;
        private IUrlHelper _urlHelper;

        public ArticleService(IUnitOfWork uow,
            IArticleRepository articleRepository,
            IArticleTagRepository articleTagRepository,
            IArticleTagService articleTagService,
            ITagRepository tagRepository, IUrlHelper urlHelper)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _articleTagRepository = articleTagRepository;
            _articleTagService = articleTagService;
            _tagRepository = tagRepository;
            _unitOfWork = uow;
            _urlHelper = urlHelper;
        }

        public IQueryable<Article> FilterByTag(int? tagId)
        {
            IQueryable<Article> query;
            if (tagId != null)
            {
                query = _articleRepository.Instance()
                    .Where(c => c.IsActive && c.IsPublished)
                    .Join(_articleTagRepository.Instance()
                    .Where(x => x.TagId == tagId.Value), a => a.Id, at => at.ArticleId, (a, at) => a);
            }
            else
            {
                query = _articleRepository.Instance().Where(c => c.IsActive && c.IsPublished);
            }

            return query;
        }

        public async Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int pageNumber, int items = 10)
        {
            var itemsPerPage = items;
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
                //result.Pagination.QueryStrings.TryAdd(QueryStringParameters.TagId, tagId.Value.ToString());// = new Dictionary<string, string> { { "tagId", tagId.Value.ToString() } };
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
        public virtual async Task<List<Tag>> SpecifyRelatedTagsInTheGeneralSet(List<Tag> tagList)
        {
            List<Tag> allAvailableTags = await _tagRepository.FetchByCriteria(c => c.IsActive).ToListAsync();
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
            List<int> articleIds = await _articleTagService.GetByCriteria(null, null)
                .Select(p => p.ArticleId)
                .Distinct().ToListAsync();
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
            var oldTags = await _articleTagRepository
                .FetchByCriteria(c => c.ArticleId == viewModel.Article.Id)
                .Select(t => t.Tag)
                .ToListAsync();

            var newTags = viewModel.AllTags.Where(c => c.IsActive).ToList();
            List<Tag> addedList = new List<Tag>();
            List<Tag> deletedList = new List<Tag>();
            bool isDeletedTag = true;
            foreach (var ot in oldTags)
            {
                isDeletedTag = true;
                foreach (var nt in newTags)
                {
                    if (ot.Id == nt.Id)
                    {
                        isDeletedTag = false;
                        newTags.Remove(nt);
                        break;
                    }
                }
                if (isDeletedTag)
                {
                    deletedList.Add(ot);
                }
            }
            addedList = newTags;
            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var articleId = await _unitOfWork.Article.AddOrUpdateAsync(viewModel.Article);

                    foreach (var tag in deletedList)
                    {
                        await _unitOfWork.ArticleTag.Remove(c => c.ArticleId == articleId && c.TagId == tag.Id);
                    }
                    foreach (var tag in addedList)
                    {
                        var addArticleTag = new ArticleTag() { ArticleId = articleId, TagId = tag.Id };
                        await _unitOfWork.ArticleTag.InsertAsync(addArticleTag);
                    }
                    await _unitOfWork.ArticleTag.SaveChangesAsync();

                    await transaction.Result.CommitAsync();
                    return articleId;
                }
                catch (Exception ex)
                {
                    await transaction.Result.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }
        }

        public virtual async Task<SearchResultsViewModel> Search(string search, int pageNumber = 1, int itemsNum = 10)
        {
            if (string.IsNullOrEmpty(search))
                return null;
            int itemsPerPage = itemsNum;

            var searchValue = search.Trim().ToLower();

            var pr = PredicateBuilder.False<Article>();
            pr = pr.Or(a =>
                a.Title.ToLower().Contains(searchValue) ||
                a.Summary.ToLower().Contains(searchValue) ||
                a.Description.ToLower().Contains(searchValue));

            foreach (var term in search.ToLower().Split(' '))
            {
                string temp = term.Trim();
                pr = pr.Or(a => a.Title.ToLower().Contains(temp) ||
                              a.Summary.ToLower().Contains(temp) ||
                              a.Description.ToLower().Contains(temp));
            }

            pr = pr.And(a => a.IsActive);
            pr = pr.And(a => a.IsPublished);

            var query = _articleRepository.Instance().AsQueryable().Where(pr).OrderByDescending(o => o.CreatedDate);
            // EF.Functions.Like(a.Title.ToLower(), $"%{searchValue}%"))
            var totalItems = await query.CountAsync();

            var articles = await query.Skip(pageNumber - 1).Take(itemsPerPage).ToListAsync();

            var pagination = new Pagination
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = totalItems,
                    ItemsPerPage = itemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = _urlHelper.Action(action: "Index",
                    controller: "Search",
                    new { search = search.Trim(), pageNumber = pageNumber })
            };

            var result = new SearchResultsViewModel
            {
                Articles = articles,
                Pagination = pagination
            };

            return result;
        }
    }
}