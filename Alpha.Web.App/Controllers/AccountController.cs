using Alpha.Infrastructure;
using Alpha.Infrastructure.Email;
using Alpha.Models.Identity;
using Alpha.ViewModels;
using Alpha.Web.App.Extensions;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

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
                    await _emailSender.SendEmailConfirmationLink(confirmationLink, user.UserName, user.Email);

                    await _userManager.AddToRoleAsync(user, PolicyTypes.OrdinaryUsers);

                    return RedirectToAction(nameof(SuccessRegistration));
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
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordObject)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordObject);
            }

            var user = await _userManager.FindByEmailAsync(forgotPasswordObject.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var senderEmail = _configuration.GetSection("EmailConfiguration");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.PasswordResetLink(user.Email, token, Request.Scheme);
            await _emailSender.SendResetPasswordLink(callback, user.Email, user.Email);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet, AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost, AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(resetPasswordViewModel);
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordViewModel.Email);
            if (user == null)
            {
                RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var resetPasswordResult = await _userManager.ResetPasswordAsync(user,
                resetPasswordViewModel.Token,
                resetPasswordViewModel.Password);

            if (!resetPasswordResult.Succeeded)
            {
                foreach (var error in resetPasswordResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(ResetPasswordConfirmation));
        }

        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        #endregion

        #region Google Login

        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        #endregion

        #region Facebook Login

        [AllowAnonymous]
        public IActionResult FacebookLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);

            return new ChallengeResult("Facebook", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return RedirectToAction(nameof(Login));

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            if (signInResult.Succeeded)
                return Redirect(returnUrl);
            else
            {
                var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
                if (await _userManager.FindByEmailAsync(email) != null)
                {
                    // The user has already registered through filling out the sign up form.
                    // It means the user can login but without using third-party authentication.
                    TempData["SpecialMessage"] = $"There is already {email} in the database.";
                    return RedirectToAction("Login");
                }
                User user = new User
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    IsActive = true,
                    EmailConfirmed = true,
                    IpAddress = GetClientIpAddress(),
                    FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                    LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                    LoginProvider = info.LoginProvider
                };
                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, PolicyTypes.OrdinaryUsers);
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        if (info.LoginProvider == "Google")
                        {
                            await _userManager.AddClaimAsync(user, info.Principal.FindFirst(AdditionalClaimTypes.Locale));
                            await _userManager.AddClaimAsync(user, info.Principal.FindFirst(AdditionalClaimTypes.Picture));
                        }
                        else if (info.LoginProvider == "Facebook")
                        {
                            var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                            var thumbnailUrl = $"https://graph.facebook.com/{identifier}/picture?type=album";
                            await _userManager.AddClaimAsync(user, new Claim(AdditionalClaimTypes.Picture, thumbnailUrl));
                        }
                        // Include the access token in the properties
                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);
                        props.IsPersistent = true;

                        await _signInManager.SignInAsync(user, props);
                        return LocalRedirect(returnUrl);
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