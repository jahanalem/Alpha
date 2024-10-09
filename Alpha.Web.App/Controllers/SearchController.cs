using Alpha.Services.Interfaces;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Alpha.Web.App.Controllers
{
    //[ApiController]
    //[Route("[controller]")]
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

            ViewModels.Searches.SearchResultsViewModel searchResult =
                await _articleService.Search(searchVal, pageNumber, _appSettings.Value.DefaultItemsPerPage);
            ViewBag.SearchTerm = searchVal;
            string x = Newtonsoft.Json.JsonConvert.SerializeObject(searchResult);
            return View(searchResult);
        }

        [HttpPost]
        public async Task<IActionResult> SearchLive(string find = null)
        {
            if (string.IsNullOrEmpty(find))
                return View();
            var searchVal = find.Trim();

            ViewModels.Searches.SearchResultsViewModel searchResult =
                await _articleService.Search(searchVal);
            ViewBag.SearchTerm = searchVal;
            //var x = Newtonsoft.Json.JsonConvert.SerializeObject(searchResult.Articles[0]);
            var y = Json(searchResult.Articles);
            return y;
        }
    }
}