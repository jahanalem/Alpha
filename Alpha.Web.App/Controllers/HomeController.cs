using Alpha.Infrastructure.Email;
using Alpha.LoggerService;
//using Microsoft.Extensions.Logging;
using Alpha.Web.App.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

//using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Alpha.Web.App.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILoggerManager _logger;
        private IEmailSender _emailSender;
        public HomeController(ILoggerManager logger, IEmailSender emailSender)
        {
            _logger = logger;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
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

        public IActionResult SendReportError(ErrorViewModel errorViewModel)
        {
            if (ModelState.IsValid)
            {
                var cuserInfo = GetCurrentUserInfo();
                if (cuserInfo.IsAuthenticated)
                {
                    errorViewModel.ReporterEmail = cuserInfo.Email;
                }
                var result = _emailSender.SendErrorMessageToSupportTeam(errorViewModel);
                if (result.IsCompletedSuccessfully)
                {
                    TempData["ErrorReport_Sent_Successfully"] = Resources.Constants.Messages.SendingMessageSuccessfully;
                    return RedirectToAction("MailSentSuccessfully", "Home");
                }
                else
                {
                    TempData["ErrorReport_Sent_Failed"] = Resources.Constants.Messages.SendingMessageFailed;
                    return RedirectToAction("MailSentFailed", "Home");
                }
            }
            return View();
        }
        //public IActionResult Privacy()
        //{
        //    return View();
        //}
    }
}