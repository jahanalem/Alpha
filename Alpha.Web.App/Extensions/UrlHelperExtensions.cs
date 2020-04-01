using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Web.App.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper,
            string email,
            string token,
            string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { token, email = email },
                protocol: scheme);
        }

        public static string AccountActivationLink(this IUrlHelper urlHelper,
            string userId,
            string code,
            string scheme,
            string externalHostName)
        {
            return urlHelper.Action(
                action: nameof(AccountController.AccountActivation),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme,
                host: externalHostName
            );
        }

        public static string PasswordResetLink(this IUrlHelper urlHelper,
            string userId,
            string code,
            string scheme,
            string externalHostName)
        {
            return urlHelper.Action(
                action: nameof(AccountController.PasswordReset),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme,
                host: externalHostName);
        }
    }
}
