using System;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Alpha.Web.App.Components
{
    public class ArticleListViewComponent : ViewComponent
    {
        //public static int PageSize = 3;
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private int pageNumber = 1;
        //public int ArticlePage = 1;
        private int? tagId = null;
        public ArticleListViewComponent(IArticleService articleService, IHttpContextAccessor httpContextAccessor)
        {
            _articleService = articleService;
            _httpContextAccessor = httpContextAccessor;
            //var result = 1;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string queryString = (_httpContextAccessor.HttpContext.Request.Query["pageNumber"]);
                string tId = (_httpContextAccessor.HttpContext.Request.Query["tagId"]);
                if (queryString != null)
                {
                    Int32.TryParse(queryString, out pageNumber);
                }

                if (tId != null)
                {
                    tagId = int.Parse(tId);
                }
                ViewBag.ArticlePage = pageNumber;
            }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var key = $"TotalItems-TagId-{tagId}";
            if (TempData[key]== null)
            {
                TempData[key] = await _articleService.FilterByTag(null).CountAsync();
            }

            var result = await _articleService.FilterByTagAsync(tagId, pageNumber);

            result.Pagination.Init(pageNumber,
                PagingInfo.DefaultItemsPerPage,
                int.Parse(TempData[key].ToString()),
                "Article",
                "Index");
            
            return View(result);
        }
    }
}
