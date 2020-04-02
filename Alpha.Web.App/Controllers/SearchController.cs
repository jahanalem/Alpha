using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels.Searches;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Controllers
{
    public class SearchController : BaseController
    {
        private IArticleService _articleService;
        public SearchController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        public async Task<IActionResult> Index(string search = null, int pageNumber = 1)
        {
            if (string.IsNullOrEmpty(search))
                return View();
            string searchVal = search.Trim();
           
            var searchResult = await _articleService.Search(searchVal, pageNumber);

            return View(searchResult);
        }
    }
}