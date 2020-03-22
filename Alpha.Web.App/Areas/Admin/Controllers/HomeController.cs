using Alpha.Web.App.Constants;
using Alpha.Web.App.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            ViewBag.CurrentUser = CurrentUser;
            return RedirectToAction("Index", "Article", new { area = "Admin" });//RedirectToAction("Index", "Article", new { AreaConstants.AdminArea });

            //return View();
        }
    }
}