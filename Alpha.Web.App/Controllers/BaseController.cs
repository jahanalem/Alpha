using System;
using Alpha.Infrastructure;
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

        //public BaseController(IHttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;

        //    //ViewBag.ArticlePage = ArticlePage;
        //    //ViewBag.CurrentUser = CurrentUserInfo;
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
        }

        //public int ArticlePage
        //{
        //    get
        //    {
        //        var result = 1;
        //        if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
        //        {
        //            string queryString = (_httpContextAccessor.HttpContext.Request.Query["articlePage"]);
        //            if (queryString != null)
        //            {
        //                Int32.TryParse(queryString, out result);
        //            }
        //            ViewBag.ArticlePage = result;
        //        }

        //        return result;
        //    }
        //}

        //public CurrentUser CurrentUserInfo
        //{
        //    get
        //    {
        //        if (HttpContext != null)
        //            return new CurrentUser
        //            {
        //                IsAuthenticated = HttpContext != null && HttpContext.User.Identity.IsAuthenticated,
        //                AuthenticationType = HttpContext.User.Identity.AuthenticationType,
        //                UserName = HttpContext.User.Identity.Name,
        //                IsInRoleOfUsers = HttpContext.User.IsInRole("Users"),
        //                IsInRoleOfAdmins = HttpContext.User.IsInRole("Admins")
        //            };
        //        return new CurrentUser();
        //    }
        //}
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
    }
}
