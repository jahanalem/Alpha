using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Article", new { area = "Admin" });//RedirectToAction("Index", "Article", new { AreaConstants.AdminArea });

            //return View();
        }
    }
}