using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Alpha.Web.App.Controllers
{
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        // GET: Tag
        public async Task<IActionResult> Index()
        {
            return View(await _tagService.GetAllAsync());
        }

        #region Details of Tag

        // GET: Tag/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id is int idValue)
            {
                Tag tag = await _tagService.GetByIdAsync(idValue);
                if (tag == null)
                {
                    return NotFound();
                }
                return View(tag);
            }

            return View();
        }

        #endregion
    }
}
