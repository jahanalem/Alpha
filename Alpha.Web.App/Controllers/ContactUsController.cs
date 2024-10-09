using Alpha.Infrastructure.Email;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.Web.App.Resources.AppSettingsFileModel;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Alpha.Web.App.Controllers
{
    public class ContactUsController : BaseController
    {
        private IContactUsService _contactUsService;
        private IEmailSender _emailSender;
        private IConfiguration _configuration;
        private IOptions<EmailConfigurationSettingsModel> _emailConfigurationSettings;
        public ContactUsController(IContactUsService contactUsService,
            IEmailSender emailSender,
            IConfiguration config,
            IOptions<EmailConfigurationSettingsModel> emailConfigurationSettings)
        {
            _contactUsService = contactUsService;
            _emailSender = emailSender;
            _configuration = config;
            ViewBag.TitleHtmlMetaTag = "Contact Us";
            ViewBag.DescriptionHtmlMetaTag = "You can contact me by this page.";
            _emailConfigurationSettings = emailConfigurationSettings;
        }

        [HttpGet, AllowAnonymous]
        public ActionResult ContactUsForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUsForm(ContactUsViewModel contactObject)
        {
            if (ModelState.IsValid)
            {
                if (!contactObject.IsResultCorrect(contactObject.FirstNumber,
                            contactObject.SecondNumber,
                            contactObject.Result))
                {
                    return Json(new
                    {
                        status = Messages.FailedStatus,
                        message = Messages.CaptchaIsNotCorrect,
                        type = Messages.ErrorAlertType
                    });
                }

                var result = await _contactUsService.CreateAsync(contactObject);
                if (result)
                {
                    var senderName = string.Empty;
                    if (!string.IsNullOrEmpty(contactObject.FirstName))
                        senderName = contactObject.FirstName.Trim();
                    if (!string.IsNullOrEmpty(contactObject.LastName))
                        senderName = $"{senderName} {contactObject.LastName.Trim()}";

                    var fullName = $"{contactObject.FirstName} {contactObject.LastName}";
                    var sentResult = _emailSender.ForwardIncomingMessageToAdmin(contactObject.Email,
                        fullName,
                        contactObject.Title,
                        contactObject.Description);

                    if (!sentResult.IsCompletedSuccessfully)
                    {
                        // Sending operation is failed!
                        //TODO:
                    }
                    return Json(new
                    {
                        status = Messages.SuccessStatus,
                        message = Messages.SendingMessageSuccessfully,
                        type = Messages.SuccessAlertType
                    });
                }
                else
                {
                    return Json(new
                    {
                        status = Messages.FailedStatus,
                        message = Messages.SendingMessageFailed,
                        type = Messages.ErrorAlertType
                    });
                }
            }

            return View();
        }
    }
}