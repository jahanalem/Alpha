using Alpha.Infrastructure;
using Alpha.Web.App.Models;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace Alpha.Web.App.Controllers
{
    // https://stackoverflow.com/questions/13225315/pass-data-to-layout-that-are-common-to-all-pages
    // https://stackoverflow.com/questions/5453327/how-to-set-viewbag-properties-for-all-views-without-using-a-base-class-for-contr/21130867#21130867
    public class BaseController : Controller
    {
        public CurrentUser CurrentUser;
        public BaseController() : base()
        {
        }

        protected CurrentUser GetCurrentUserInfo()
        {
            var uId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int userId;
            int.TryParse(uId, out userId);

            CurrentUser cUser = new CurrentUser
            {
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated,
                AuthenticationType = HttpContext.User.Identity.AuthenticationType,
                UserName = HttpContext.User.Identity.Name,
                Email = HttpContext.User.FindFirstValue(ClaimTypes.Email),
                UserId = userId,// var userId = _userManager.GetUserId(HttpContext.User);
                IsInRoleOfUsers = HttpContext.User.IsInRole(RoleTypes.Users),
                IsInRoleOfAdmins = HttpContext.User.IsInRole(RoleTypes.Admins),
                UserIp = GetClientIpAddress()
            };
            ViewBag.CurrentUser = cUser;
            return cUser;
        }

        protected string GetClientIpAddress()
        {
            return HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
        protected void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        protected IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
