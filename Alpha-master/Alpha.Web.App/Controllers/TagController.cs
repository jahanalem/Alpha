using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Controllers
{
    public class TagController : BaseController
    {
        //private readonly ApplicationDbContext _context;
        private readonly ITagRepository _tagRepository;
        public TagController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        // GET: Tag
        public async Task<IActionResult> Index()
        {
            return View(await _tagRepository.FetchByCriteria(null).ToListAsync());
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
                Tag tag = await _tagRepository.FindByIdAsync(idValue);
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
