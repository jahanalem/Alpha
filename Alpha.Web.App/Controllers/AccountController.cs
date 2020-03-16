﻿using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Alpha.Models.Identity;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        public AccountController(UserManager<User> userMgr, SignInManager<User> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
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
                IdentityResult result = await userManager.CreateAsync(user, signupObj.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
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
                User user = await userManager.FindByEmailAsync(details.Email);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result =
                        await signInManager.PasswordSignInAsync(
                            user, details.Password, false, false);
                    //ViewBag.CurrentUser = user;
                    ViewData["CurrentUser"] = user;
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
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
            await signInManager.SignOutAsync();
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

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}