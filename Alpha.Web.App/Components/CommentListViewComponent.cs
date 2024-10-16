using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Alpha.Web.App.Components
{
    public class CommentListViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentService _commentService;
        private int _articleId;
        public CommentListViewComponent(IHttpContextAccessor httpContextAccessor, ICommentService commentService)
        {
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

            return View(comments);
        }
    }
}
