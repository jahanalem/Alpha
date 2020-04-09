using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Alpha.Web.App.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private IOptions<AppSettingsModel> _appSettings;
        public ArticleController(IArticleService articleService,
            IOptions<AppSettingsModel> appSettings)
        {
            _articleService = articleService;
            _appSettings = appSettings;
        }

        [HttpGet]
        public async Task<IActionResult> Show(int Id)
        {
            var result = await _articleService.FindByIdAsync(Id);
            ViewBag.TitleHtmlMetaTag = result.TitleHtmlMetaTag;
            ViewBag.DescriptionHtmlMetaTag = result.DescriptionHtmlMetaTag;
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

            var result = await _articleService.FilterByTagAsync(tagId, pageNumber, _appSettings.Value.DefaultItemsPerPage);

            result.Pagination.Init(new Pagination
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = int.Parse(TempData[key].ToString()),
                    ItemsPerPage = _appSettings.Value.DefaultItemsPerPage, //PagingInfo.DefaultItemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = Url.Action(action: "Index", controller: "Article", new { tagId = tagId, pageNumber = pageNumber })
            });

            TempData.Keep(key);
            return View(result);
        }
    }
}