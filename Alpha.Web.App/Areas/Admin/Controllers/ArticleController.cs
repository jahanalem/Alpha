using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class ArticleController : Controller
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
            var model = await _articleService.GetAllAsync();//.GetAllOfArticleViewModel();
            return View(model);
        }

        // GET: Article/Create
        [HttpGet]//[HttpGet("Create")]
        public IActionResult Create()
        {
            var tags = _tagService.GetAll().Where(c => c.IsActive == true).ToList();
            var articleViewModel = new ArticleViewModel()
            {
                AllOfTags = tags
            };
            return View(articleViewModel);
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
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

        // GET: Article/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ArticleViewModel article = await _articleService.GetArticleById(id);
            var allTags = _tagService.GetAll().Where(c => c.IsActive == true).ToList();
            for (int t = 0; t < allTags.Count; t++)
            {
                allTags[t].IsActive = false;
            }
            foreach (var tag in article.Tags)
            {
                for (int t = 0; t < allTags.Count; t++)
                {
                    if (allTags[t].Id == tag.Id)
                    {
                        allTags[t].IsActive = true;
                    }
                }
            }

            article.AllOfTags = allTags;
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _articleService.Update(article);
                    await _articleService.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
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
            return View(article);
        }

        // GET: Article/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _articleService.GetAll().SingleOrDefault(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        //[ValidateAntiForgeryToken]
        [HttpPost, ActionName("DeleteConfirmed/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = _articleService.GetAll().SingleOrDefault(m => m.Id == id);
            _articleService.Delete(article);
            await _articleService.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _articleService.GetAll().Any(e => e.Id == id);
        }
    }
}