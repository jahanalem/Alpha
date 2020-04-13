using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
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

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleTagService _articleTagService;
        private readonly ITagService _tagService;
        private IOptions<AppSettingsModel> _appSettings;

        public ArticleController(IArticleService articleService,
            IArticleTagService articleTagService,
            ITagService tagService,
            IOptions<AppSettingsModel> appSettings
            )
        {
            _articleService = articleService;
            _articleTagService = articleTagService;
            _tagService = tagService;
            _appSettings = appSettings;
        }
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
                    ItemsPerPage = _appSettings.Value.DefaultItemsPerPage,// PagingInfo.DefaultItemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = Url.Action(action: "Index", controller: "Article", new { area = "Admin", tagId = tagId, pageNumber = pageNumber })
            });

            //TempData.Keep(key);
            return View(result);
        }

        // GET: Article/Details/5
        //[HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            var article = await _articleService.FindByIdAsync(id);
            if (article == null)
                return NotFound();

            return View(article);
        }

        #region Create

        // GET: Admin/Article/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _tagService.GetByCriteria(c => c.IsActive == true).ToListAsync();
            for (int i = 0; i < tags.Count; i++)
            {
                tags[i].IsActive = false;
            }
            var articleViewModel = new ArticleViewModel()
            {
                AllTags = tags
            };
            //StringLengthAttribute strLenAttr = typeof(Article).GetProperty("Summary")?.GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
            //if (strLenAttr != null)
            //{
            //    int maxLen = strLenAttr.MaximumLength;
            //}
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

            ArticleViewModel article = await _articleService.GetArticleById(id);

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

            var article = await _articleService.FindByIdAsync(id.Value);
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
            var article = await _articleService.GetByCriteria(m => m.Id == id).SingleOrDefaultAsync();
            _articleService.Delete(article);
            await _articleService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        private async Task<bool> ArticleExists(int id)
        {
            return await _articleService.ExistsAsync(id);
        }
    }
}