using System;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure;
using Alpha.Models;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Controllers
{
    [Authorize]
    public class CommentController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Comments.Include(c => c.Article).Include(c => c.Parent);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Comment/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Article)
                .Include(c => c.Parent)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comment/Create
        public IActionResult Create()
        {
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Description");
            ViewData["ParentId"] = new SelectList(_context.Comments, "Id", "Id");
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Description", comment.ArticleId);
            ViewData["ParentId"] = new SelectList(_context.Comments, "Id", "Id", comment.ParentId);
            return View(comment);
        }

        [HttpPost]
        public async Task<JsonResult> EditComment([FromBody] EditCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Comment cmmt = await _commentRepository.FindByIdAsync(obj.CommentId);
                cmmt.Description = obj.Dsc;
                cmmt.ModifiedDate = DateTime.UtcNow;

                var id = await _commentRepository.AddOrUpdateAsync(cmmt);
                if (Request.IsAjaxRequest())
                {
                    return Json(id);
                }
            }
            return Json(new { code = 0 });
            //return RedirectToAction("Show", "Article", new { Id = obj.ArticleId });//Ok(obj.Dsc);
        }
        [HttpPost]
        public async Task<JsonResult> DeleteComment([FromBody] DeleteCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var comment = await _commentRepository.FetchByCriteria(m => m.Id == obj.CommentId).SingleOrDefaultAsync();
                if (comment != null)
                {
                    await RemoveChildren(comment.Id);
                    _commentRepository.Remove(comment);
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(1);
                }
            }
            return Json(new { code = 0 });
            //return RedirectToAction("Show", "Article", new { Id = obj.ArticleId });//Ok(obj.Dsc);
        }

        async Task RemoveChildren(int i)
        {
            var children = _commentRepository.FetchByCriteria(c => c.ParentId == i).ToList();
            foreach (var child in children)
            {
                await RemoveChildren(child.Id);
                _commentRepository.Remove(child);
            }
        }

        [HttpPost]
        //[Route("Test/{des}")]
        //[ValidateAntiForgeryToken]
        public async Task<JsonResult> Save(SendCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Description = obj.Dsc,
                    ArticleId = obj.ArticleId,
                    ParentId = obj.ParentId
                };
                var id = await _commentRepository.AddOrUpdateAsync(comment);
                if (Request.IsAjaxRequest())
                {
                    return Json(id);
                }
            }
            return Json(new { code = 0 });
            //return RedirectToAction("Show", "Article", new { Id = obj.ArticleId });//Ok(obj.Dsc);
        }
        [HttpPost]
        public async Task<ActionResult> SavebyPageLoad(SendCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var comment = new Comment
                {
                    Description = obj.Dsc,
                    ArticleId = obj.ArticleId,
                    ParentId = obj.ParentId
                };
                var id = await _commentRepository.AddOrUpdateAsync(comment);
            }
            return RedirectToAction("Show", "Article", new { Id = obj.ArticleId });
        }
        // GET: Comment/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Description", comment.ArticleId);
            ViewData["ParentId"] = new SelectList(_context.Comments, "Id", "Id", comment.ParentId);
            return View(comment);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParentId,ArticleId,UserId,Description,LikeCounter,Id,CreatedDate,ModifiedDate")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Description", comment.ArticleId);
            ViewData["ParentId"] = new SelectList(_context.Comments, "Id", "Id", comment.ParentId);
            return View(comment);
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Article)
                .Include(c => c.Parent)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _context.Comments.SingleOrDefaultAsync(m => m.Id == id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}