using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class TagController : BaseController
    {
        private readonly ITagService _tagService;
        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var tagList = await _tagService.GetByCriteria(null, null).ToListAsync();
            return View(tagList);
        }

        #region Create

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
        public async Task<IActionResult> Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                await _tagService.AddOrUpdateAsync(tag);
                await _tagService.SaveChangesAsync();
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
                Tag tag = await _tagService.FindByIdAsync(idValue);
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
        public async Task<IActionResult> Edit(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _tagService.Update(tag);
                    await _tagService.SaveChangesAsync();
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
                var tag = await _tagService.FindByIdAsync(idValue);
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
                var tag = await _tagService.FindByIdAsync(idValue);
                if (tag == null)
                {
                    return NotFound();
                }
                _tagService.Delete(tag);
                await _tagService.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TagExists(int id)
        {
            return await _tagService.ExistsAsync(id);// _context.Tags.Any(e => e.Id == id);
        }
        #endregion
    }
}