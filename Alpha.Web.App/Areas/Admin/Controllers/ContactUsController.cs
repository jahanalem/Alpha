using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
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
        public async Task<IActionResult> Index(int? pagenumber = 1)
        {
            if (pagenumber.HasValue)
            {
                var viewModel = new ContactUsListViewModel();
                if (TempData.Peek("TotalItems") == null)
                {
                    TempData["TotalItems"] = await _contactUsService.GetByCriteria().CountAsync();
                }

                viewModel.ContactUsList = await _contactUsService
                    .GetByCriteria(PagingInfo.DefaultItemsPerPage, pagenumber.Value, null)
                    .OrderByDescending(c => c.CreatedDate).ToListAsync();

                viewModel.Pagination.Init(new Pagination
                {
                    PagingInfo = new PagingInfo
                    {
                        TotalItems = int.Parse(TempData.Peek("TotalItems").ToString()),
                        ItemsPerPage = PagingInfo.DefaultItemsPerPage,
                        CurrentPage = pagenumber.Value
                    },
                    TargetArea = "Admin",
                    TargetController = "ContactUs",
                    TargetAction = "Index"
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