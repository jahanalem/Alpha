using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Components
{
    public class TagListViewComponent : ViewComponent
    {
        private readonly ITagRepository _tagRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TagListViewComponent(ITagRepository tagRepository, IHttpContextAccessor httpContextAccessor)
        {
            _tagRepository = tagRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Tag> result = await _tagRepository.FindAll(p => p.IsActive == true).ToListAsync();
            return View(result);
        }
    }
}
