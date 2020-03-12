using System;
using Alpha.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Controllers
{
    public class BaseController : Controller
    {
        public CurrentUser CurrentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseController() : base()
        {
            //ViewBag.CurrentUser = CurrentUserInfo;
        }

        public BaseController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            //ViewBag.ArticlePage = ArticlePage;
            //ViewBag.CurrentUser = CurrentUserInfo;
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
                IsInRoleOfAdmins = HttpContext.User.IsInRole("Admins")
            };
            ViewBag.CurrentUser = cUser;
            return cUser;
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
