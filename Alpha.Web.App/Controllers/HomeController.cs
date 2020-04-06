using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Alpha.LoggerService;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using Alpha.Web.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Fluent;

//using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Alpha.Web.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILoggerManager _logger;
        public HomeController(ILoggerManager logger)
        {
            _logger = logger;
            
        }

        public IActionResult Index()
        {
            int t = 1;
            var l = 2 /(t-1);
            GetCurrentUserInfo();
            return View();
        }

        public IActionResult MailSentSuccessfully()
        {
            return View();
        }

        public IActionResult MailSentFailed()
        {
            return View();
        }

        public IActionResult Error(string requestId, string timeOfError)
        {
            var t = DateTime.ParseExact(timeOfError, "yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture);
            return View(new ErrorViewModel { RequestId = requestId, TimeOfError = t });
        }
        //public IActionResult Privacy()
        //{
        //    return View();
        //}
    }
}