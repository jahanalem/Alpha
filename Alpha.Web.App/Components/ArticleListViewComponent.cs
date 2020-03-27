using System;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Components
{
    public class ArticleListViewComponent : ViewComponent
    {
        public static int PageSize = 3;
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int pageNumber = 1;
        public int ArticlePage = 1;
        public ArticleListViewComponent(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
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
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            Task<ArticleTagListViewModel> result = _articleService.FilterByTagAsync(null, 1, 3);
            return View(await result);
        }
    }
}
