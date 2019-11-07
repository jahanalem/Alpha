using System.Linq;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Controllers
{
    public class AboutUsController : BaseController
    {
        private IAboutUsRepository _repository;

        public AboutUsController(IAboutUsRepository repoAboutUs)
        {
            this._repository = repoAboutUs;
            ViewBag.Titel = "About Us";
        }

        public ViewResult AboutUs()
        {
            var result =_repository.GetAll().ToList();
            return View(result);
        }
    }
}
