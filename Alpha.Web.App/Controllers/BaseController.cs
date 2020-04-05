using System;
using System.Diagnostics;
using System.Security.Claims;
using Alpha.Infrastructure;
using Alpha.Web.App.Models;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

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
            //x = _configuration.GetValue<string>("appSettings:DefaultItemsPerPage");
            //ViewBag.CurrentUser = CurrentUserInfo;
        }

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);
        //}

        protected CurrentUser GetCurrentUserInfo()
        {
            var uId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? userId = null;
            if (uId != null)
                userId = int.Parse(uId);

            CurrentUser cUser = new CurrentUser
            {
                IsAuthenticated = HttpContext.User.Identity.IsAuthenticated,
                AuthenticationType = HttpContext.User.Identity.AuthenticationType,
                UserName = HttpContext.User.Identity.Name,
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
