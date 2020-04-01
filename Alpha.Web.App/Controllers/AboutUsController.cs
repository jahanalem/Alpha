﻿using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Web.App.Controllers
{
    public class AboutUsController : BaseController
    {
        private IAboutUsService _aboutUsService;
        public AboutUsController(IAboutUsService aboutUsService)
        {
            _aboutUsService = aboutUsService;
            ViewBag.Titel = "About Us";
        }

        public async Task<ViewResult> AboutUs()
        {
            var result =await _aboutUsService.GetByCriteria(null, null).FirstOrDefaultAsync();
            return View(result);
        }
    }
}