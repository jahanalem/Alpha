using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        public static int PageSize = 3;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Show(int Id)
        {
            var result = await _articleService.FindByIdAsync(Id);
            return View(result);
        }

        // GET: Article
        [HttpGet]
        public async Task<IActionResult> Index(int? tagId = null, int pageNumber = 1)
        {
            var key = $"TotalItems-TagId-{tagId}";

            if (TempData[key] == null)
            {
                TempData[key] = await _articleService.FilterByTag(tagId).CountAsync();
            }

            var result = await _articleService.FilterByTagAsync(tagId, pageNumber);

            result.Pagination.Init(pageNumber,
                PagingInfo.DefaultItemsPerPage,
                int.Parse(TempData[key].ToString()),
                "Article",
                "Index", "");
            TempData.Keep(key);
            return View(result);
        }
    }
}
