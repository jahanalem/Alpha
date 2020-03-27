using System;
using System.Diagnostics;
using Alpha.Infrastructure;
using Alpha.Web.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Alpha.Web.App.Controllers
{
    // https://stackoverflow.com/questions/13225315/pass-data-to-layout-that-are-common-to-all-pages
    // https://stackoverflow.com/questions/5453327/how-to-set-viewbag-properties-for-all-views-without-using-a-base-class-for-contr/21130867#21130867
    public class BaseController : Controller
    {
        public CurrentUser CurrentUser;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController() : base()
        {
            //ViewBag.CurrentUser = CurrentUserInfo;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }
        
        protected CurrentUser GetCurrentUserInfo()
        {
            CurrentUser cUser = new CurrentUser
            {
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated,
                AuthenticationType = HttpContext.User.Identity.AuthenticationType,
                UserName = HttpContext.User.Identity.Name,
                IsInRoleOfUsers = HttpContext.User.IsInRole("Users"),
                IsInRoleOfAdmins = HttpContext.User.IsInRole("Admins"),
                UserIp = GetClientIpAddress()
            };
            ViewBag.CurrentUser = cUser;
            return cUser;
        }

        protected string GetClientIpAddress()
        {
            string ip = string.Empty;
            if (HttpContext != null &&
                HttpContext.Connection != null &&
                HttpContext.Connection.RemoteIpAddress != null)
                ip = HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
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
