using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Web.App.Controllers;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Alpha.Web.App.Areas.Admin.Controllers
{
    [Authorize(Policy = PolicyTypes.SuperAdmin)]
    [Area(AreaConstants.AdminArea)]
    public class ContactUsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}