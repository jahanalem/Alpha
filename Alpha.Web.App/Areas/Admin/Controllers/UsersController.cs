﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.Infrastructure;
using Alpha.Models.Identity;
using Alpha.Services;
using Alpha.ViewModels;
using Alpha.Web.App.Areas.Admin.Models.ViewModels;
using Alpha.Web.App.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Area(AreaConstants.AdminArea)]
    public class UsersController : BaseController
    {
        private UserManager<User> _userManager;
        private IUserValidator<User> _userValidator;
        private IPasswordValidator<User> _passwordValidator;
        private IPasswordHasher<User> _passwordHasher;
        private RoleManager<Role> _roleManager;
        private IUserRoleStore<UserRole> _userRoleStore;
        private ApplicationDbContext _applicationDbContext;

        private IUserService _userService;

        //public UsersController()
        //{
        //    _userService = new UserService(this.ModelState, _userManager);
        //}

        //public UsersController(IUserService service)
        //{
        //    _userService = service;
        //}
        public UsersController(ApplicationDbContext dbContext, UserManager<User> usrMgr, RoleManager<Role> roleMgr, IUserValidator<User> userValid, IPasswordValidator<User> passValid, IPasswordHasher<User> passwordHash)
        {
            _userManager = usrMgr;
            _userValidator = userValid;
            _passwordValidator = passValid;
            _passwordHasher = passwordHash;
            _roleManager = roleMgr;
            _applicationDbContext = dbContext;
            _userService = new UserService(this.ModelState, _userManager);
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadData()
        {

            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            // Skiping number of Rows count
            var start = Request.Form["start"].FirstOrDefault();
            // Paging Length 10,20
            var length = Request.Form["length"].FirstOrDefault();
            // Sort Column Name
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            // Sort Column Direction ( asc ,desc)
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            // Search Value from (Search box)
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            //Paging Size (10,20,50,100)
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            IQueryable<User> data = _userManager.Users;




            //using (_context)
            {
                // Getting all Customer data
                IQueryable<User> userData = data; //_context.CustomerTB.Select(tempcustomer => tempcustomer);

                //Sorting
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                {
                    if (sortColumn == string.Empty) sortColumn = "Id";
                    var order = $"{sortColumn} {sortColumnDirection}";
                    userData = data.OrderBy(order);
                }
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    userData = data.AsQueryable().Where(m => m.Email.ToLower().Contains(searchValue) || m.UserName.ToLower().Contains(searchValue));
                }
                data = userData.Skip(skip).Take(pageSize);
                //total number of rows count 
                recordsTotal = userData.Count();
                //Paging 

            }
            var result = new List<UsersViewModel>();
            foreach (User u in data.ToList())
            {
                List<string> rolesOfUser = _applicationDbContext.UserRoles.Where(c => c.User.Id == u.Id).Select(c => c.Role.Name).ToList();

                var vm = new UsersViewModel { Id = u.Id, UserName = u.UserName, Email = u.Email, RoleNames = rolesOfUser };
                result.Add(vm);
            }

            //Returning Json Data
            JsonResult jsonResult = Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = result });
            return jsonResult;
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            List<string> propertyNames = DynamicOperation.GetNamesOfProperties(typeof(UserEditViewModel));
            ViewData["PropertyList"] = propertyNames;
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index", "Users");
            }
            User user = _userManager.Users.FirstOrDefault(u => u.Id == int.Parse(id));
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel userViewModel)
        {
            IdentityResult consequence = await _userService.EditUser(userViewModel);
            if (consequence.Succeeded)
            {
                return RedirectToAction("Edit", new { id = userViewModel.Id });
            }
            else
            {
                AddErrorsFromResult(consequence);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(data: true);//RedirectToAction("Index", "Users");
            }
            var result = _userService.DeleteAsync(id);

            if (result.Result.Succeeded)
            {
                return Json(data: true);
            }
            return Json(data: false);
        }
    }
}