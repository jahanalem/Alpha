using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ArticleController : BaseController
    {
        private readonly IArticleCategoryService _articleCategoryService;
        private readonly IArticleTagService _articleTagService;
        private readonly IArticleService _articleService;
        private readonly ITagService _tagService;
        private IOptions<AppSettingsModel> _appSettings;

        public ArticleController(IArticleService articleService,
            IArticleTagService articleTagService,
            ITagService tagService,
            IOptions<AppSettingsModel> appSettings,
            IArticleCategoryService articleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
            _articleTagService = articleTagService;
            _articleService = articleService;
            _tagService = tagService;
            _appSettings = appSettings;
        }

        [Route("Admin/Article/pageNumber/{pageNumber:int}")]
        [Route("Admin/Article/")]
        public async Task<IActionResult> Index(int? tagId = null, int pageNumber = 1)
        {
            var key = $"admin_TotalItems-TagId-{tagId}";

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
                    ItemsPerPage = _appSettings.Value.DefaultItemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = Url.Action(action: "Index", controller: "Article", new { area = "Admin", tagId = tagId, pageNumber = pageNumber })
            });

            return View(result);
        }

        // GET: Article/Details/5
        //[HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            var article = await _articleService.GetArticleByIdAsync(id.Value);
            if (article == null)
                return NotFound();

            return View(article);
        }

        #region Create

        // GET: Admin/Article/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _tagService.GetTagsByIsActiveAsync(true);
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i].IsActive = false;
            }
            var articleViewModel = new ArticleViewModel()
            {
                Article = new Article(),
                AllTags = tags,
                CategoryList = await _articleCategoryService.GetByIsActiveAsync(true)
            };

            return View(articleViewModel);
        }

        // POST: Admin/Article/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel articleViewModel)
        {
            if (ModelState.IsValid)
            {
                await _articleService.InsertAsync(articleViewModel);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();
            }
            return RedirectToAction("Index", "Article", new { area = "Admin" });
        }

        #endregion

        #region Edit

        // GET: Article/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            ArticleViewModel article = await _articleService.GetArticleByIdAsync(id);

            article.AllTags = await _articleService.SpecifyRelatedTagsInTheGeneralSet(article.Tags);
            return View(article);
        }

        // POST: Article/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Alpha.ViewModels.ArticleViewModel obj)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _articleService.InsertAsync(obj);
                }
                catch (DbUpdateConcurrencyException)
                {
                    bool x = await ArticleExists(obj.Article.Id);
                    if (x)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Error();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        #endregion

        #region Delete

        // GET: Admin/Article/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _articleService.GetArticleByIdAsync(id.Value);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Admin/Article/Delete/5
        [HttpPost]//, ActionName("DeleteConfirmed/{id}")
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleViewModel = await _articleService.GetArticleByIdAsync(id);
            await _articleService.DeleteAsync(articleViewModel.Article);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        private async Task<bool> ArticleExists(int id)
        {
            return await _articleService.ArticleExists(id);
        }
    }
}