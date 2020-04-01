using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Alpha.Models.Identity;
using Alpha.ViewModels;
using Alpha.Web.App.Helpers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using Alpha.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Alpha.Web.App.Extensions;

namespace Alpha.Web.App.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        public AccountController(UserManager<User> userMgr,
            SignInManager<User> signinMgr,
            IWebHostEnvironment environment,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _userManager = userMgr;
            _signInManager = signinMgr;
            _environment = environment;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        #region Sign up

        [HttpGet, AllowAnonymous]
        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupUserViewModel signupObj)
        {
            if (ModelState.IsValid)
            {
                Alpha.Models.Identity.User user = new User()
                {
                    FirstName = signupObj.FirstName,
                    LastName = signupObj.LastName,
                    UserName = signupObj.UserName,
                    Email = signupObj.Email,
                    IpAddress = GetClientIpAddress()
                };
                IdentityResult result = await _userManager.CreateAsync(user, signupObj.Password);
                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail),
                        "Account",
                        new { email = user.Email, token },
                        Request.Scheme);

                    //var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
                    await SendActivationEmail(user.UserName, user.Email, confirmationLink);
                    await _userManager.AddToRoleAsync(user, PolicyTypes.OrdinaryUsers);
                    return RedirectToAction(nameof(SuccessRegistration));
                    //var callbackUrl = Url.AccountActivationLink()
                    //await SendActivationEmail(user.UserName, user.Email, callbackUrl);
                    //return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(signupObj);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        private Task SendActivationEmail(string userName, string emailAddress, string activationLink)
        {
            var messageBody = EmailHelper.GetEmailTemplate(_environment, EmailSettings.AccountActivation.HtmlTemplateName);

            var imageUrl = EmailHelper.GetImagesUrl(_configuration, HttpContext.Request);
            var impressum = EmailHelper.GetEmailTemplate(_environment, EmailSettings.HtmlTemplateImpressum);

            messageBody = messageBody.Replace(EmailSettings.ReplaceToken.Impressum, impressum); //immer zuerst replacen !!!

            var tokenValues = new Dictionary<string, string>
            {
                { EmailSettings.ReplaceToken.ImagesUrl, imageUrl },
                { EmailSettings.ReplaceToken.UserName, userName},
                { EmailSettings.ReplaceToken.Link, HtmlEncoder.Default.Encode(activationLink)}
            };

            foreach (var key in tokenValues.Keys)
            {
                messageBody = messageBody.Replace(key, tokenValues[key]);
            }

            var senderAddress = EmailSettings.AccountActivation.SenderAddressPrefix + "@" + EmailSettings.EmailSenderDomain;

            return _emailSender.SendEmailAsync(emailAddress,
                senderAddress,
                EmailSettings.AccountActivation.Subject,
                messageBody);
        }

        public async Task<IActionResult> AccountActivation()
        {
            return null;
        }

        //public async Task<IActionResult> EmailConfirmation()
        //{
        //    return null;
        //}

        public async Task<IActionResult> PasswordReset()
        {
            return null;
        }

        #endregion

        #region Login

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                                                 await _signInManager.PasswordSignInAsync(
                                                      user, details.Password, false, false);
                    //ViewBag.CurrentUser = user;
                    ViewData["CurrentUser"] = user;
                    if (result.Succeeded)
                    {
                        if (await _userManager.IsInRoleAsync(user, RoleTypes.Admins))
                        {
                            returnUrl = "/Admin/Article/";
                        }
                        return Redirect(returnUrl ?? "/");
                    }
                    else
                    {
                        if (!await _userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError("", "Your email has not confirmed yet!");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Login Attempt");
                        }
                        return View();
                    }
                }
                ModelState.AddModelError(nameof(LoginUserViewModel.Email),
                    "Invalid user or password");
            }

            return View(details);
        }

        #endregion

        #region Logout

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            ViewBag.CurrentUser = null;
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Forgot Password

        [HttpGet, AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordUserViewModel forgotPasswordObject)
        {
            return View();
        }

        #endregion

        #region Google Login

        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("GoogleResponse", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                User user = new User
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };
                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return Redirect(returnUrl);
                    }
                }
                return AccessDenied();
            }
        }

        #endregion

        #region Facebook Login

        [AllowAnonymous]
        public IActionResult FacebookLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("FacebookResponse", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> FacebookResponse(string returnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                User user = new User
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    IsActive = true
                };

                var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                var alreadyUser = await _userManager.FindByEmailAsync(email);
                if (alreadyUser != null)
                {
                    // The user has already registered through filling out the sign up form.
                    // It means the user can login but without using third-party authentication.
                    TempData["SpecialMessage"] =
                        $"There is already {email} in the database. You can login with it through entering password.";
                    return RedirectToAction("Login");
                }

                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, false);
                        return Redirect(returnUrl);
                    }
                }
                return AccessDenied();
            }
        }

        #endregion

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}