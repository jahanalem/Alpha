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
            var model =await _articleService.GetAllAsync();//.GetAllOfArticleViewModel();
            return View(model);
        }

        // GET: Article/Create
        [HttpGet]//[HttpGet("Create")]
        public IActionResult Create()
        {
            var result = _tagService.FindAllTags(c => c.IsActive == true);
            return View(result);
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost("insert")]
        public async Task<IActionResult> Insert(ArticleViewModel articleViewModel)
        {
            if (ModelState.IsValid)
            {
                //using (_context)
                //{
                //    using (var transaction = _context.Database.BeginTransactionAsync())
                //    {
                        
                //        var articleId = _articleService.AddOrUpdate(articleViewModel.Article);
                //        foreach (var tag in articleViewModel.Tags.Where(t => t.IsActive == true))
                //        {
                //            var at = new ArticleTag()
                //            {
                //                ArticleId = articleId,
                //                TagId = tag.Id
                //            };
                //            _articleTagService.AddOrUpdate(at);
                //        }
                //        await transaction.Result.CommitAsync();
                //    }
                //}
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
    }
}