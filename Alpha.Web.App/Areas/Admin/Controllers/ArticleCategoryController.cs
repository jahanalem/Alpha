using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var list = await _articleCategoryService.GetAllAsync();

            return View(list);
        }

        // GET: ArticleCategory/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var cat = await _articleCategoryService.GetByIdAsync(id);

            return View(cat);
        }

        // GET: ArticleCategory/Create
        public async Task<ActionResult> Create()
        {
            List<Alpha.Models.ArticleCategory> list = await _articleCategoryService.GetByIsActiveAsync(true);
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
            var cat = await _articleCategoryService.GetByIdAsync(id);
            var model = new ArticleCategoryViewModel
            {
                Category = cat,
                Parents = await _articleCategoryService.GetAllAsync()
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
            var cat = await _articleCategoryService.GetByIdAsync(id);
            var model = new ArticleCategoryViewModel
            {
                Category = cat,
                Parents = await _articleCategoryService.GetAllAsync()
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
                await _articleCategoryService.DeleteAsync(model.Category);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
    }
}