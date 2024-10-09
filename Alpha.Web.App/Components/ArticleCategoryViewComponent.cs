using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
