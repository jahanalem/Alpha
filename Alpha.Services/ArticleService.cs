﻿using Alpha.DataAccess.UnitOfWork;
using Alpha.Infrastructure.Convertors;
using Alpha.Infrastructure.LinqUtility;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.ViewModels.Searches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class ArticleService : BaseService<Article>, IArticleService
    {
        private IArticleTagService _articleTagService;
        private IArticleCategoryService _articleCategoryService;
        private IUrlHelper _urlHelper;

        public ArticleService(IArticleCategoryService articleCategoryService,
                              IArticleTagService articleTagService,
                              IUrlHelper urlHelper,
                              IUnitOfWork uow,
                              ILogger<ArticleService> logger) : base(uow, logger)
        {
            _articleTagService = articleTagService;
            _articleCategoryService = articleCategoryService;
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
                query = _unitOfWork.Repository<Article>().GetByCriteria(c => c.IsActive && c.IsPublished);
            }

            return query;
        }

        public async Task<IQueryable<Article>> FilterByCriteriaQueryableAsync(int tagId, int catId)
        {
            List<ArticleCategory> selectedCategories = await _articleCategoryService.GetSelfAndDescendants(catId);
            IQueryable<Article> query1 = GetArticlesByCategories(selectedCategories);
            IQueryable<Article> query = query1
               .Where(c => c.IsActive && c.IsPublished)
               .Join(_unitOfWork.Repository<ArticleTag>().GetByCriteria(x => x.TagId == tagId), a => a.Id, at => at.ArticleId,
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
                query = _unitOfWork.Repository<Article>()
                    .GetByCriteria(c => c.IsActive && c.IsPublished)
                    .Join(_unitOfWork.Repository<ArticleTag>()
                                     .GetByCriteria(x => x.TagId == tagId.Value), a => a.Id, at => at.ArticleId, (a, at) => a);
            }
            else
            {
                query = _unitOfWork.Repository<Article>().GetByCriteria(a => a.IsActive && a.IsPublished);
            }
            return query;
        }

        private IQueryable<Article> FilterByTag(IQueryable<Article> query, int tagId)
        {
            query = query.Join(_unitOfWork.Repository<ArticleTag>()
                .GetByCriteria(x => x.TagId == tagId), a => a.Id, at => at.ArticleId, (a, at) => a);

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
            query = _unitOfWork.Repository<Article>().GetByCriteria(pr);

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

        public virtual async Task<ArticleViewModel> GetArticleByIdAsync(int articleId)
        {
            var vm = new ArticleViewModel();
            vm.Article = await _unitOfWork.Repository<Article>().GetByIdAsync(articleId);
            vm.Tags = GetTagsByArticleId(articleId);
            vm.CategoryList = await _unitOfWork.Repository<ArticleCategory>().GetByCriteria(c => c.IsActive).ToListAsync();

            return vm;
        }

        /// <summary>
        /// IsActive property is "true" in general set
        /// </summary>
        /// <param name="tagList">return related tags along with the general set tags</param>
        /// <returns></returns>
        public virtual async Task<List<Tag>> SpecifyRelatedTagsInTheGeneralSet(List<Tag> tagList)
        {
            List<Tag> allAvailableTags = await _unitOfWork.Repository<Tag>().GetByCriteria(c => c.IsActive).ToListAsync();
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
            List<int> articleIds = await _unitOfWork.Repository<ArticleTag>().GetByCriteria(null, null)
                .Select(p => p.ArticleId)
                .Distinct().ToListAsync();
            foreach (var id in articleIds)
            {
                result.Add(await GetArticleByIdAsync(id));
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
            var oldTags = await _unitOfWork.Repository<ArticleTag>().GetByCriteria(c => c.ArticleId == viewModel.Article.Id)
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

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                viewModel.Article.DescriptionAsPlainText = viewModel.Article.Description.ToPlainText();
                var article = await _unitOfWork.Repository<Article>().AddOrUpdateAsync(viewModel.Article);

                foreach (var tag in deletedList)
                {
                    await _unitOfWork.Repository<ArticleTag>().DeleteByCriteriaAsync(c => c.ArticleId == article.Id && c.TagId == tag.Id);
                }
                foreach (var tag in addedList)
                {
                    var addArticleTag = new ArticleTag() { ArticleId = article.Id, TagId = tag.Id };
                    await _unitOfWork.Repository<ArticleTag>().AddAsync(addArticleTag);
                }
                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                return article.Id;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message);
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

            var query = _unitOfWork.Repository<Article>().GetByCriteria(pr).OrderByDescending(o => o.CreatedDate);
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

        public async Task DeleteAsync(Article article)
        {
            await _unitOfWork.Repository<Article>().DeleteAsync(article);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<bool> ArticleExists(int id)
        {
            return await _unitOfWork.Repository<Article>().ExistsAsync(id);
        }
    }
}