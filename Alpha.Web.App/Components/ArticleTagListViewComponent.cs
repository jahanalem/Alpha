using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Components
{
    public class ArticleTagListViewComponent : ViewComponent
    {
        public static int PageSize = 5;
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int pageNumber = 1;
        private static int _tagId = 0;
        public int ArticlePage = 1;
        public ArticleTagListViewComponent(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            _articleService = articleService;
            _httpContextAccessor = httpContextAccessor;
            //var result = 1;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string queryString = (_httpContextAccessor.HttpContext.Request.Query["articlePage"]);
                if (queryString != null)
                {
                    Int32.TryParse(queryString, out pageNumber);
                }
                ViewBag.ArticlePage = pageNumber;
                queryString = (_httpContextAccessor.HttpContext.Request.Query["tagId"]);
                if (queryString != null)
                {
                    Int32.TryParse(queryString, out _tagId);
                }
                ViewBag.TagId = _tagId;
            }
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int articlePage = pageNumber;
            var articleList = new List<ArticleViewModel>();
            List<int> articleIdList = _articleService.FindAll(p => p.Id == _tagId).Skip((articlePage - 1) * PageSize).Take(PageSize).Select(p => p.Id).ToList();
            foreach (var articleId in articleIdList)
            {
                Article art =await _articleService.FindByIdAsync(articleId);
                var tags = _articleService.GetTagsByArticleId(articleId);
                articleList.Add(new ArticleViewModel
                {
                    Article = art,
                    Tags = tags,
                });
            }

            PagingInfo pInfo = new PagingInfo
            {
                //PageSize = 5,
                CurrentPage = articlePage,
                ItemsPerPage = 5,
                TotalItems = _articleService.FindAll(p => p.Id == _tagId).Count()
            };
            var result = new ArticleTagListViewModel
            {
                ArticleViewModelList = articleList,
                PagingInfo = pInfo,
                TagId = _tagId
            };
            return View(result);
        }
    }
}
