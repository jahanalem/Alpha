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

using HtmlAgilityPack;
using System.Web;
using System.Text.RegularExpressions;
using Alpha.Infrastructure.Convertors;
using Alpha.Services.Extensions;

namespace Alpha.Services
{
    public class ArticleService : BaseService<IArticleRepository, Article>, IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTagRepository _articleTagRepository;
        private IArticleTagService _articleTagService;
        private IArticleCategoryService _articleCategoryService;
        private ITagRepository _tagRepository;
        private IUnitOfWork _unitOfWork;
        private IUrlHelper _urlHelper;

        public ArticleService(IUnitOfWork uow,
            IArticleRepository articleRepository,
            IArticleTagRepository articleTagRepository,
            IArticleTagService articleTagService,
            IArticleCategoryService articleCategoryService,
            ITagRepository tagRepository, IUrlHelper urlHelper)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _articleTagRepository = articleTagRepository;
            _articleTagService = articleTagService;
            _tagRepository = tagRepository;
            _articleCategoryService = articleCategoryService;
            _unitOfWork = uow;
            _urlHelper = urlHelper;
        }

        public IQueryable<Article> FilterByCriteria(int? tagId = null, int? catId = null)
        {
            IQueryable<Article> query = null;

            var articleViewModelList = new List<ArticleViewModel>();

            if (catId != null && tagId != null)
            {
                query = FilterByCriteriaQueryableAsync(tagId.Value, catId.Value).Result;
            }
            else if (catId != null && tagId == null)
            {
                query = FilterByCategoryAsync(catId.Value).Result;
            }
            else if (catId == null && tagId != null)
            {
                query = FilterByTag(tagId.Value);
            }
            else
            {
                query = _articleRepository.FetchByCriteria(c => c.IsActive && c.IsPublished);
            }


            return query;
        }

        //private ArticleTagListViewModel MapToViewModel(List<Article> articleList, int? tagId)
        //{
        //    var articleViewModelList = new List<ArticleViewModel>();
        //    foreach (var article in articleList)
        //    {
        //        var tags = _articleTagService.GetTagsByArticleId(article.Id);
        //        articleViewModelList.Add(new ArticleViewModel
        //        {
        //            Article = article,
        //            Tags = tags,
        //        });
        //    }

        //    var result = new ArticleTagListViewModel
        //    {
        //        ArticleViewModelList = articleViewModelList,
        //        TagId = tagId
        //    };

        //    return result;
        //}

        public async Task<IQueryable<Article>> FilterByCriteriaQueryableAsync(int tagId, int catId)
        {
            List<ArticleCategory> selectedCategories = await _articleCategoryService.GetSelfAndDescendants(catId);
            IQueryable<Article> query1 = GetArticlesByCategories(selectedCategories);
            IQueryable<Article> query = query1
               .Where(c => c.IsActive && c.IsPublished)
               .Join(_articleTagRepository.Instance().Where(x => x.TagId == tagId), a => a.Id, at => at.ArticleId,
                   (a, at) => a);

            return query;
        }

        private async Task<IQueryable<Article>> FilterByCategoryAsync(int catId)
        {
            List<ArticleCategory> selectedCategories = await _articleCategoryService.GetSelfAndDescendants(catId);
            IQueryable<Article> query1 = GetArticlesByCategories(selectedCategories);

            var query = query1.Where(c => c.IsActive && c.IsPublished);

            return query;
        }

        public IQueryable<Article> FilterByTag(int? tagId)
        {
            IQueryable<Article> query = null;
            if (tagId != null)
            {
                query = _articleRepository.Instance()
                    .Where(c => c.IsActive && c.IsPublished)
                    .Join(_articleTagRepository.Instance()
                    .Where(x => x.TagId == tagId.Value), a => a.Id, at => at.ArticleId, (a, at) => a);
            }
            else
            {
                query = _articleRepository.FetchByCriteria(a => a.IsActive && a.IsPublished);
            }
            return query;
        }


        private IQueryable<Article> FilterByTag(IQueryable<Article> query, int tagId)
        {
            query = query
                .Join(_articleTagRepository.Instance()
                .Where(x => x.TagId == tagId), a => a.Id, at => at.ArticleId, (a, at) => a);

            return query;
        }


        public IQueryable<Article> GetArticlesByCategories(List<ArticleCategory> categories, bool isArticleActive = true, bool isArticlePublished = true)
        {
            IQueryable<Article> query;
            var pr = PredicateBuilder.False<Article>();
            foreach (var cat in categories)
            {
                pr = pr.Or(p => p.ArticleCategoryId == cat.Id);
            }
            pr = pr.And(a => a.IsActive == isArticleActive);
            pr = pr.And(a => a.IsPublished == isArticlePublished);
            query = _articleRepository.FetchByCriteria(pr);

            return query;
        }


        public async Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int pageNumber = 1, int items = 10)
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

        public async Task<ArticleTagListViewModel> FilterByCategoryAsync(int? artCatId, int pageNumber = 1, int items = 10)
        {
            var itemsPerPage = items;
            var articleViewModelList = new List<ArticleViewModel>();
            var catList = await _articleCategoryService.GetSelfAndDescendants(artCatId.Value);

            IQueryable<Article> query = GetArticlesByCategories(catList);//FilterByCategory(artCatId);

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
                TagId = null
            };

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
            vm.CategoryList = await _articleCategoryService.GetByCriteria(c => c.IsActive).ToListAsync();
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
                    viewModel.Article.DescriptionAsPlainText = viewModel.Article.Description.ToPlainText();
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

        public virtual async Task<SearchResultsViewModel> Search(string searchTerm, int? pageNumber = 1, int? itemsNum = 10)
        {

            if (string.IsNullOrEmpty(searchTerm))
                return null;


            var searchValue = searchTerm.Trim().ToLower();

            var pr = PredicateBuilder.False<Article>();
            pr = pr.Or(a =>
                a.Title.ToLower().Contains(searchValue) ||
                a.Summary.ToLower().Contains(searchValue) ||
                a.DescriptionAsPlainText.ToLower().Contains(searchValue));

            foreach (var term in searchTerm.ToLower().Split(' '))
            {
                string temp = term.Trim();
                pr = pr.Or(a => a.Title.ToLower().Contains(temp) ||
                              a.Summary.ToLower().Contains(temp) ||
                              a.DescriptionAsPlainText.ToLower().Contains(temp));
            }

            pr = pr.And(a => a.IsActive);
            pr = pr.And(a => a.IsPublished);

            var query = _articleRepository.Instance().AsQueryable().Where(pr).OrderByDescending(o => o.CreatedDate);
            // EF.Functions.Like(a.Title.ToLower(), $"%{searchValue}%"))
            var totalItems = await query.CountAsync();

            var result = new SearchResultsViewModel();
            if (itemsNum != null && pageNumber != null)
            {
                int itemsPerPage = itemsNum.Value;
                List<Article> articles = SortedList(await query.Skip(pageNumber.Value - 1).Take(itemsPerPage).ToListAsync(), searchValue);


                var pagination = new Pagination
                {
                    PagingInfo = new PagingInfo
                    {
                        TotalItems = totalItems,
                        ItemsPerPage = itemsPerPage,
                        CurrentPage = pageNumber.Value
                    },
                    Url = _urlHelper.Action(action: "Index",
                        controller: "Search",
                        new { search = searchTerm.Trim(), pageNumber = pageNumber })
                };

                result = new SearchResultsViewModel
                {
                    Articles = articles,
                    Pagination = pagination
                };
            }
            else
            {
                var articles = SortedList(await query.ToListAsync(), searchValue);
                result = new SearchResultsViewModel
                {
                    Articles = articles
                };
            }
            return result;
        }

        private List<Article> SortedList(List<Article> articles, string searchValue)
        {
            List<Article> firstSortedList = new List<Article>();
            List<Article> secondSortedList = new List<Article>();
            List<Article> thirdSortedList = new List<Article>();
            foreach (var item in articles)
            {
                var t = item.Title.ToLower();
                var s = item.Summary.ToLower();
                if (t.Contains(searchValue))
                {
                    firstSortedList.Add(item);
                }
                else if (s.Contains(searchValue))
                {
                    secondSortedList.Add(item);
                }
                else
                {
                    thirdSortedList.Add(item);
                }
            }
            var final = firstSortedList.Concat(secondSortedList).Concat(thirdSortedList).ToList();
            return final;
        }

        public virtual async Task<SearchResultsViewModel> Search(string searchTerm)
        {
            return await Search(searchTerm, null, null);
        }

    }
}