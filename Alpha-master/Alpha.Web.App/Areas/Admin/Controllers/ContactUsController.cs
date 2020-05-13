using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ContactUsController : BaseController
    {
        private IContactUsService _contactUsService;
        private IOptions<AppSettingsModel> _appSettings;
        public ContactUsController(IContactUsService contactUsService, IOptions<AppSettingsModel> appSettings)
        {
            _contactUsService = contactUsService;
            _appSettings = appSettings;
        }
        public async Task<IActionResult> Index(int? pageNumber = 1)
        {
            if (pageNumber.HasValue)
            {
                var viewModel = new ContactUsListViewModel();
                if (TempData.Peek("TotalItems") == null)
                {
                    TempData["TotalItems"] = await _contactUsService.GetByCriteria().CountAsync();
                }

                viewModel.ContactUsList = await _contactUsService
                    .GetByCriteria(_appSettings.Value.DefaultItemsPerPage, pageNumber.Value, null)
                    .OrderByDescending(c => c.CreatedDate).ToListAsync();

                viewModel.Pagination.Init(new Pagination
                {
                    PagingInfo = new PagingInfo
                    {
                        TotalItems = int.Parse(TempData.Peek("TotalItems").ToString()),
                        ItemsPerPage = _appSettings.Value.DefaultItemsPerPage,// PagingInfo.DefaultItemsPerPage,
                        CurrentPage = pageNumber.Value
                    },
                    Url = Url.Action(action: "Index", controller: "ContactUs", new { area = "Admin", pageNumber = pageNumber })
                });

                return View(viewModel);
            }

            return View();
        }

        //public async Task<ActionResult> ContactUsList(int? pageNumber = null)
        //{
        //    if (pageNumber.HasValue)
        //    {
        //        var viewModel = new ContactUsListViewModel();
        //        viewModel.ContactUsList = await _contactUsService
        //            .FindAll(3, 1, null)
        //            .OrderByDescending(c => c.CreatedDate).ToListAsync();
        //        viewModel.PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = pageNumber.Value,
        //            ItemsPerPage = 3,
        //            TotalItems = (int)TempData.Peek("TotalItems")
        //        };
        //        viewModel.TargetController = "ContactUs";
        //        viewModel.TargetAction = "ContactUsList";
        //        return View(viewModel);
        //    }
        //}

        #region Read

        public async Task<IActionResult> Details(int id)
        {
            if (ModelState.IsValid)
            {
                ContactUs contact = await _contactUsService.FindByIdAsync(id);
                return View(contact);
            }
            return View();
        }

        #endregion

        #region Insert

        // /ContactUs/ContactForm

        #endregion

        #region Delete

        #endregion

        #region Update

        #endregion
    }
}