using System.Linq;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Controllers
{
    public class AboutUsController : BaseController
    {
        private IAboutUsService _aboutUsService;
        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
            ViewBag.Titel = "About Us";
        }

        public ViewResult AboutUs()
        {
            var result = _aboutUsService.GetByCriteria(null, null).ToList();
            return View(result);
        }
    }
}