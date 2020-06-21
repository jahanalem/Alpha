using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services;
using Alpha.Services.Extensions;
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
        private IArticleTagService _articleTagService;
        public ArticleController(IArticleService articleService, IArticleTagService articleTagService,
            IOptions<AppSettingsModel> appSettings)
        {
            this._articleService = articleService;
            _appSettings = appSettings;
            _articleTagService = articleTagService;
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
        [Route("Article/pageNumber/{pageNumber:int}")]
        [Route("Article/artCatId/{artCatId:int}/tagId/{tagId:int}/pageNumber/{pageNumber:int}")]
        [Route("Article/artCatId/{artCatId:int}/tagId/{tagId:int}")]
        [Route("Article/artCatId/{artCatId:int}/pageNumber/{pageNumber:int}")]
        [Route("Article/artCatId/{artCatId:int}")]
        [Route("Article/tagId/{tagId:int}/pageNumber/{pageNumber:int}")]
        public async Task<IActionResult> Index(int? artCatId = null, int? tagId = null, int pageNumber = 1)
        {
            var key = $"TotalItems-TagId-{tagId}-artCatId-{artCatId}";

            if (TempData[key] == null)
            {
                TempData[key] = await _articleService.FilterByCriteria(tagId, artCatId).CountAsync();
            }

            var result = await _articleService
                 .FilterByCriteria(tagId, artCatId)
                 .Slice(pageNumber, _appSettings.Value.DefaultItemsPerPage)
                 .MapToViewModel(_articleTagService, tagId);

            string url = Url.Action(action: "Index", controller: "Article", new { artCatId, tagId, pageNumber });

            result?.Pagination?.Init(new Pagination
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = int.Parse(TempData[key].ToString()),
                    ItemsPerPage = _appSettings.Value.DefaultItemsPerPage, //PagingInfo.DefaultItemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = url
            });

            TempData.Keep(key);

            return View(result);
        }
    }
}