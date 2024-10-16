using Alpha.DataAccess;
using Alpha.Infrastructure;
using Alpha.Models;
using Alpha.Models.Identity;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Web.App.Controllers
{
    [Authorize]
    public class CommentController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommentService _commentService;
        private UserManager<User> _userManager;
        public CommentController(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        // GET: Comment
        public async Task<IActionResult> Index()
        {
            var query = _context.Comments
                .Include(c => c.Article)
                .Include(c => c.Parent).OrderByDescending(t => t.CreatedDate);
            List<Comment> result = await query.ToListAsync();
            return View(result);
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


        #region Create

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
                var userInfo = GetCurrentUserInfo();
                comment.UserId = userInfo.UserId;
                await _commentService.AddOrUpdateAsync(comment);

                return RedirectToAction(nameof(Index));
            }
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Description", comment.ArticleId);
            ViewData["ParentId"] = new SelectList(_context.Comments, "Id", "Id", comment.ParentId);
            return View(comment);
        }

        #endregion

        #region Edit

        [HttpPost]
        public async Task<JsonResult> EditComment([FromBody] EditCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                Comment cmmt = await _commentService.FindByIdAsync(obj.CommentId);
                cmmt.Description = obj.Dsc;
                cmmt.ModifiedDate = DateTime.UtcNow;

                var comment = await _commentService.AddOrUpdateAsync(cmmt);
                if (Request.IsAjaxRequest())
                {
                    return Json(comment.Id);
                }
            }
            return Json(new { code = 0 });
        }

        #endregion

        #region Delete

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

        [HttpPost]
        public async Task<JsonResult> DeleteComment([FromBody] DeleteCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var comment = await _commentService.GetCommentById(obj.CommentId);
                if (comment != null)
                {
                    await RemoveChildren(comment.Id);
                    await _commentService.RemoveCommentAsync(comment);
                }
                if (Request.IsAjaxRequest())
                {
                    return Json(1);
                }
            }
            return Json(new { code = 0 });
        }

        async Task RemoveChildren(int i)
        {
            var children = await _commentService.GetCommentsByParentId(i);
            foreach (var child in children)
            {
                await RemoveChildren(child.Id);
                await _commentService.RemoveCommentAsync(child);
            }
        }

        #endregion

        [HttpPost]
        public async Task<JsonResult> Save(SendCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                if (!GetCurrentUserInfo().IsAuthenticated)
                {
                    return Json(new { code = 0 });
                }
                var CurrentUserInfo = GetCurrentUserInfo();
                var comment = new Comment
                {
                    Description = obj.Dsc,
                    ArticleId = obj.ArticleId,
                    ParentId = obj.ParentId,
                    UserId = CurrentUserInfo.UserId
                };
                var commentId = await _commentService.AddOrUpdateAsync(comment);
                var user = HttpContext.User.Identity.Name;

                if (Request.IsAjaxRequest())
                {
                    string data = JsonConvert.SerializeObject(new { user = user, id = commentId });
                    object dataObj = JsonConvert.DeserializeObject(data);
                    return Json(dataObj);
                }
            }

            return Json(new { code = 0 });
        }

        [HttpPost]
        public async Task<ActionResult> SavebyPageLoad(SendCommentViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var userInfo = GetCurrentUserInfo();

                var comment = new Comment
                {
                    Description = obj.Dsc,
                    ArticleId = obj.ArticleId,
                    ParentId = obj.ParentId,
                    UserId = userInfo.UserId
                };
                var id = await _commentService.AddOrUpdateAsync(comment);
            }
            return RedirectToAction("Show", "Article", new { Id = obj.ArticleId });
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}