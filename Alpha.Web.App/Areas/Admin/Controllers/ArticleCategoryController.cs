using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ArticleCategoryController : BaseController
    {
        IArticleCategoryService _articleCategoryService;
        public ArticleCategoryController(IArticleCategoryService articleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
        }
        // GET: ArticleCategory
        public async Task<ActionResult> Index()
        {
            var list = await _articleCategoryService.GetByCriteria().ToListAsync();
            return View(list);
        }

        // GET: ArticleCategory/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var cat = await _articleCategoryService.GetByCriteria(c => c.Id == id).FirstOrDefaultAsync();
            return View();
        }

        // GET: ArticleCategory/Create
        public async Task<ActionResult> Create()
        {
            List<Alpha.Models.ArticleCategory> list = await _articleCategoryService.GetByCriteria(c => c.IsActive == true).ToListAsync();
            var model = new ArticleCategoryViewModel
            {
                Parents = list
            };
            return View(model);
        }

        // POST: ArticleCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ArticleCategoryViewModel model)
        {
            try
            {
                var x = await _articleCategoryService.CreateOrUpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: ArticleCategory/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var cat = await _articleCategoryService.FindByIdAsync(id);
            var model = new ArticleCategoryViewModel
            {
                Category = cat,
                Parents = await _articleCategoryService.GetByCriteria().ToListAsync()
            };
            return View(model);
        }

        // POST: ArticleCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ArticleCategoryViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                var x = await _articleCategoryService.CreateOrUpdateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ArticleCategory/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var cat = await _articleCategoryService.FindByIdAsync(id);
            var model = new ArticleCategoryViewModel
            {
                Category = cat,
                Parents = await _articleCategoryService.GetByCriteria().ToListAsync()
            };
            return View(model);
        }

        // POST: ArticleCategory/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(ArticleCategoryViewModel model)
        {
            try
            {
                // TODO: Add delete logic here
                _articleCategoryService.Delete(model.Category);
                await _articleCategoryService.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}