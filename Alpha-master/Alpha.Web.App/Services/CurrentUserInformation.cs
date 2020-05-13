using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Alpha.Infrastructure;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Http;

namespace Alpha.Web.App.Services
{
    public class CurrentUserInformation : ICurrentUserInformation
    {
        private IHttpContextAccessor _httpContext;
        
        public CurrentUserInformation(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor;
        }

        public CurrentUser GetCurrentUserInfo()
        {
            var uId = _httpContext.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            int? userId = null;
            if (uId != null)
                userId = int.Parse(uId);

            CurrentUser cUser = new CurrentUser
            {
                IsAuthenticated = _httpContext.HttpContext.User.Identity.IsAuthenticated,
                AuthenticationType = _httpContext.HttpContext.User.Identity.AuthenticationType,
                UserName = _httpContext.HttpContext.User.Identity.Name,
                UserId = userId,// var userId = _userManager.GetUserId(HttpContext.User);
                IsInRoleOfUsers = _httpContext.HttpContext.User.IsInRole(RoleTypes.Users),
                IsInRoleOfAdmins = _httpContext.HttpContext.User.IsInRole(RoleTypes.Admins),
                UserIp = GetClientIpAddress()
            };
            return cUser;
        }

        protected string GetClientIpAddress()
        {
            string ip = string.Empty;
            if (_httpContext.HttpContext != null &&
                _httpContext.HttpContext.Connection != null &&
                _httpContext.HttpContext.Connection.RemoteIpAddress != null)
                ip = _httpContext.HttpContext.Connection.RemoteIpAddress.ToString();
            return ip;
        }
    }
}