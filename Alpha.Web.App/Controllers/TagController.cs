using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Controllers
{
    public class TagController : Controller
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
            return View(await _tagRepository.FindAll().ToListAsync());
        }

        #region Create Tag

        // GET: Tag/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Size,Title,Description,IsActive,Id,CreatedDate,ModifiedDate")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _tagRepository.AddOrUpdate(tag);
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        #endregion

        #region Edit Tag

        // GET: Tag/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Size,Title,Description,IsActive,Id,CreatedDate,ModifiedDate")] Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _tagRepository.Update(tag);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TagExists(tag.Id))
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
            return View(tag);
        }

        #endregion

        #region Delete Tag

        // GET: Tag/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (id is int idValue)
            {
                var tag = await _tagRepository.FindByIdAsync(idValue);
                if (tag == null)
                {
                    return NotFound();
                }
                return View(tag);
            }

            return View();
        }

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id is int idValue)
            {
                var tag = await _tagRepository.FindByIdAsync(idValue);
                if (tag == null)
                {
                    return NotFound();
                }
                var i = _tagRepository.Delete(tag);
            }

            return RedirectToAction(nameof(Index));
        }

        #endregion

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

        private async Task<bool> TagExists(int id)
        {
            return await _tagRepository.ExistsAsync(id);// _context.Tags.Any(e => e.Id == id);
        }
    }
}
