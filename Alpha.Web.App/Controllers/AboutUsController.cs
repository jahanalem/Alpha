using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Alpha.Web.App.Controllers
{
    public class AboutUsController : BaseController
    {
        private IAboutUsService _aboutUsService;
        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
            ViewBag.Titel = "About Us";
            ViewBag.TitleHtmlMetaTag = "About Us";
            ViewBag.DescriptionHtmlMetaTag = "I am a web developer.";
        }

        public async Task<ViewResult> Index()
        {
            var result = await _aboutUsService.GetAboutUsAsync();

            return View(result);
        }
    }
}