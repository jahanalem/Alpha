using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure;
using Alpha.Services.Interfaces;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Components
{
    public class CommentListViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentRepository _commentRepository;
        private ICommentService _commentService;
        private int _articleId;
        public CommentListViewComponent(ICommentRepository commentRepository, IHttpContextAccessor httpContextAccessor, ICommentService commentService)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
            _commentService = commentService;
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string queryString = (_httpContextAccessor.HttpContext.Request.Query["articleId"]);
                if (queryString != null)
                {
                    Int32.TryParse(queryString, out _articleId);
                }
            }
        }

        public async Task<IViewComponentResult> InvokeAsync(int Id)
        {
            var comments = await _commentService.GetComments(Id); 
            //var comments = await _commentRepository.FetchByCriteria(c => c.ArticleId == Id).ToListAsync();
            return View(comments);
        }
    }
}
