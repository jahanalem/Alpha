using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.Services;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Controllers
{
    [Route("[controller]")]
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        public static int PageSize = 3;
        public ArticleController(IArticleService articleService,
            IArticleTagRepository articleTagRepository,
            ITagRepository tagRepository,
            ApplicationDbContext context)
        {
            //_context = context;
            _articleService = articleService;
            //_articleTagRepository = articleTagRepository;
            //_tagRepository = tagRepository;
        }

        [HttpGet]
        [Route("Show/{Id}")]
        public async Task<IActionResult> Show(int Id)
        {
            var result = await _articleService.FindByIdAsync(Id);
            //result.Comments.Where(p => p.ArticleId == Id).ToList();
            return View(result);
        }

        [HttpGet]
        [Route("FilterByTagAsync/{tagId}/{articlePage}")]
        [Route("FilterByTagAsync/{tagId}")] // Matches GET Article/FilterByTag/2
        [Route("FilterByTagAsync/{articlePage}")]
        //[ActionName("Introduction-To-AspNet")]
        public async Task<IActionResult> FilterByTagAsync(int? tagId = null, int articlePage = 1)
        {
            Task<ArticleTagListViewModel> result = _articleService.FilterByTagAsync(tagId, articlePage, PageSize);
            return View(await result);
        }

        // GET: Article

        public async Task<IActionResult> Index()
        {
            var result = _articleService.FilterByTagAsync(null, 1, 3);
            return View(await result);
        }

        //public ActionResult ArticleList()
        //{
        //    List<Article> result = _articleRepository.GetAll().ToList();
        //    return View(result);
        //}
        // GET: Article/Details/5
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int? id)
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


        // GET: Article/Edit/5
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article =  _articleService.GetAll().SingleOrDefault(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [FromBody] Article article)
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
        [HttpGet("Delete/{id}")]
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
        [HttpPost("delete"), ActionName("DeleteConfirmed/{id}")]
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
