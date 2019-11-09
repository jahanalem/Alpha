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
        //[HttpGet("Details/{id}")]
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

    }
}
