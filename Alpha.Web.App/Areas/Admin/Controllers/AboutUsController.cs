﻿using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class AboutUsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}