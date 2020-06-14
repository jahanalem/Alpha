using System.Threading.Tasks;
using Alpha.Services.Interfaces;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Alpha.Web.App.Controllers
{
    public class SearchController : BaseController
    {
        private IOptions<AppSettingsModel> _appSettings;
        private IArticleService _articleService;

        public SearchController(IArticleService articleService, IOptions<AppSettingsModel> appSettings)
        {
            _articleService = articleService;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Index(string search = null, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(search))
                return View();
            var searchVal = search.Trim();

            var searchResult =
                await _articleService.Search(searchVal, pageNumber, _appSettings.Value.DefaultItemsPerPage);
            ViewBag.SearchTerm = searchVal;
            return View(searchResult);
        }
    }
}