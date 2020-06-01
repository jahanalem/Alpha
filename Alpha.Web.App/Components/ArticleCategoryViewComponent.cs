using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Components
{
    public class ArticleCategoryViewComponent : ViewComponent
    {
        private IArticleCategoryService _articleCategoryService;

        public ArticleCategoryViewComponent(IArticleCategoryService articleCategoryService)
        {
            _articleCategoryService = articleCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<ArticleCategory> catList = await _articleCategoryService.GetByCriteria(c => c.IsActive).ToListAsync();
            return View(catList);
        }
    }
}
