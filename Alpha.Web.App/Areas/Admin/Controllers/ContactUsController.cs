using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.ViewModels.Helper;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ContactUsController : BaseController
    {
       private IContactUsService _contactUsService;

        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
        }
        public async Task<IActionResult> Index(int? pageNumber = 1)
        {
            if (pageNumber.HasValue)
            {
                var viewModel = new ContactUsListViewModel();
                if (TempData.Peek("TotalItems") == null)
                {
                    TempData["TotalItems"] = await _contactUsService.FindAll().CountAsync();
                }
                viewModel.ContactUsList = await _contactUsService
                    .FindAll(3, pageNumber.Value, null)
                    .OrderByDescending(c => c.CreatedDate).ToListAsync();
                var pageInfo = new PagingInfo
                {
                    CurrentPage = pageNumber.Value,
                    ItemsPerPage = 3,
                    TotalItems = int.Parse(TempData.Peek("TotalItems").ToString())
                };

                viewModel.PagingInfo = pageInfo;
                viewModel.TargetController = "ContactUs";
                viewModel.TargetAction = "Index";
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