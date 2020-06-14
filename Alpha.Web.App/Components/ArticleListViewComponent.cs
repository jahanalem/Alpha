using System.Threading.Tasks;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Services.Interfaces;
using Alpha.ViewModels.Searches;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Alpha.Web.App.Components
{
    public class ArticleListViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private IOptions<AppSettingsModel> _appSettings;
        private string _niceUrl = string.Empty;
        private int _pageNumber = 1;
        private int? _tagId;
        private int? _artCatId;

        public ArticleListViewComponent(IArticleService articleService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AppSettingsModel> appSettings)
        {
            _articleService = articleService;
            _httpContextAccessor = httpContextAccessor;
            _appSettings = appSettings;
            //var result = 1;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string queryString = _httpContextAccessor.HttpContext.Request.Query[QueryStringParameters.PageNumber];
                string tId = _httpContextAccessor.HttpContext.Request.Query[QueryStringParameters.TagId];
                //string artCatId = _httpContextAccessor.HttpContext.Request.Query[QueryStringParameters.ArtCatId];
                if (queryString == null && tId == null)
                {
                    var routeValues = _httpContextAccessor.HttpContext.Request.RouteValues;
                    if (routeValues["tagId"] != null) _tagId = int.Parse(routeValues["tagId"].ToString());
                    if (routeValues["pageNumber"] != null) _pageNumber = int.Parse(routeValues["pageNumber"].ToString());
                   // if (routeValues["artCatId"] != null) _artCatId = int.Parse(routeValues["artCatId"].ToString());
                }

                if (queryString != null) int.TryParse(queryString, out _pageNumber);

                if (tId != null) _tagId = int.Parse(tId);
                ViewBag.PageNumber = _pageNumber;
            }
        }

        public async Task<IViewComponentResult> InvokeAsync(ViewModels.ArticleTagListViewModel model = null)
        {
            if (model == null)
            {
                var key = $"TotalItems-TagId-{_tagId}";
                if (TempData[key] == null) TempData[key] = await _articleService.FilterByTag(_tagId).CountAsync();

                model =
                    await _articleService.FilterByTagAsync(_tagId, _pageNumber, _appSettings.Value.DefaultItemsPerPage);
                var x = Url.Action(_niceUrl);
                model.Pagination.Init(new Pagination
                {
                    PagingInfo = new PagingInfo
                    {
                        TotalItems = int.Parse(TempData[key].ToString()),
                        ItemsPerPage = _appSettings.Value.DefaultItemsPerPage, // PagingInfo.DefaultItemsPerPage,
                        CurrentPage = _pageNumber
                    },
                    Url = Url.Action("Index", "Article", new { tagId = _tagId, pageNumber = _pageNumber })
                });
            }

            return View(model);
        }
    }
}