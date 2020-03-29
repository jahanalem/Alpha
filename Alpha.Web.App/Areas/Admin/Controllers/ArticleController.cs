using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleTagService _articleTagService;
        private readonly ITagService _tagService;

        public static int PageSize = 3;
        public ArticleController(IArticleService articleService,
            IArticleTagService articleTagService,
            ITagService tagService
            )
        {
            _articleService = articleService;
            _articleTagService = articleTagService;
            _tagService = tagService;
        }
        public async Task<IActionResult> Index()
        {
            var model = (await _articleService.GetAllAsync()).OrderByDescending(k => k.CreatedDate);//.GetAllOfArticleViewModel();
            return View(model);
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
        public IActionResult Create()
        {
            var tags = _tagService.GetAll().Where(c => c.IsActive == true).ToList();
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

            article.AllTags = _articleService.SpecifyRelatedTagsInTheGeneralSet(article.Tags);
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
                    await _articleService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(obj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
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
            var article = _articleService.GetAll().SingleOrDefault(m => m.Id == id);
            _articleService.Delete(article);
            await _articleService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #endregion

        private bool ArticleExists(int id)
        {
            return _articleService.GetAll().Any(e => e.Id == id);
        }
    }
}