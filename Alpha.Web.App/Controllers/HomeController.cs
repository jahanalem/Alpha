using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Alpha.Web.App.Models;
using Microsoft.AspNetCore.Http;

namespace Alpha.Web.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            GetCurrentUserInfo();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}