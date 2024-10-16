using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Web.App.Components
{
    public class TagListViewComponent : ViewComponent
    {
        private readonly ITagService _tagService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TagListViewComponent(ITagService tagService, IHttpContextAccessor httpContextAccessor)
        {
            _tagService = tagService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Tag> result = await _tagService.GetTagsByIsActiveAsync(true);
            return View(result);
        }
    }
}
