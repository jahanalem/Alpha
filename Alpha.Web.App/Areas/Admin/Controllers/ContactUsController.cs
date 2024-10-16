﻿using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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
                    TempData["TotalItems"] = await _contactUsService.GetCountAsync();
                }

                viewModel.ContactUsList = await _contactUsService.GetByCriteria(_appSettings.Value.DefaultItemsPerPage, pageNumber.Value);

                viewModel.Pagination.Init(new Pagination
                {
                    PagingInfo = new PagingInfo
                    {
                        TotalItems = int.Parse(TempData.Peek("TotalItems").ToString()),
                        ItemsPerPage = _appSettings.Value.DefaultItemsPerPage,
                        CurrentPage = pageNumber.Value
                    },
                    Url = Url.Action(action: "Index", controller: "ContactUs", new { area = "Admin", pageNumber = pageNumber })
                });

                return View(viewModel);
            }

            return View();
        }

        #region Read

        public async Task<IActionResult> Details(int id)
        {
            if (ModelState.IsValid)
            {
                ContactUs contact = await _contactUsService.GetByIdAsync(id);

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