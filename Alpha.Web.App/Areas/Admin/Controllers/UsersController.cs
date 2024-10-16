using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models.Identity;
using Alpha.Services;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class UsersController : BaseController
    {
        private UserManager<User> _userManager;
        private IUserValidator<User> _userValidator;
        private IPasswordValidator<User> _passwordValidator;
        private IPasswordHasher<User> _passwordHasher;

        private IUserService _userService;
        private IOptions<AppSettingsModel> _appSettings;
        public UsersController(UserManager<User> usrMgr,
                                IUserValidator<User> userValid,
                                IPasswordValidator<User> passValid,
                                IPasswordHasher<User> passwordHash,
                                IOptions<AppSettingsModel> appSettings)
        {
            _userManager = usrMgr;
            _userValidator = userValid;
            _passwordValidator = passValid;
            _passwordHasher = passwordHash;
            _appSettings = appSettings;
            _userService = new UserService(this.ModelState, _userManager, _userValidator, _passwordValidator, _passwordHasher);
        }
        public async Task<IActionResult> Index(int pageNumber = 1)
        {
            UsersViewModel result = new UsersViewModel();
            string key = "totalUsers";
            var usersQuery = _userManager.Users;

            if (TempData[key] == null)
            {
                TempData[key] = await usersQuery.CountAsync();
            }
            var itemsPerPage = _appSettings.Value.DefaultItemsPerPage;// PagingInfo.DefaultItemsPerPage;
            result.Users = await usersQuery.OrderByDescending(c => c.Id)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync();
            var totalUsers = int.Parse(TempData[key].ToString());
            result.Pagination.Init(new Pagination
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = totalUsers,
                    ItemsPerPage = itemsPerPage,
                    CurrentPage = pageNumber
                },
                Url = Url.Action(action: "Index", controller: "Users", new { area = "Admin", pageNumber = pageNumber })
            });

            return View(result);
        }

        #region Edit

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Users");
            }
            User user = _userManager.Users.FirstOrDefault(u => u.Id == int.Parse(id));

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User userObj)
        {
            if (ModelState.IsValid)
            {
                IdentityResult consequence = await _userService.EditUser(userObj);
                if (consequence.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(consequence);
                }
            }

            return View();
        }

        #endregion

        #region Delete

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(data: true);
            }
            var result = _userService.DeleteAsync(id);

            if (result.Result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return Json(data: false);
        }

        #endregion
    }
}