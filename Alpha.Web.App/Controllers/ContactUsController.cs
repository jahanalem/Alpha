﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Infrastructure.Captcha;
using Alpha.Infrastructure.Email;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Alpha.ViewModels.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;

namespace Alpha.Web.App.Controllers
{
    public class ContactUsController : BaseController
    {
        private IContactUsService _contactUsService;
        private IEmailSender _emailSender;
        private IConfiguration _configuration;
        public ContactUsController(IContactUsService contactUsService,
            IEmailSender emailSender,
            IConfiguration config)
        {
            _contactUsService = contactUsService;
            _emailSender = emailSender;
            _configuration = config;
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

                    var forwardMessageTo = _configuration.GetSection("EmailConfiguration");
                    var sentResult = _emailSender.SendEmailAsync(forwardMessageTo["ForwardMessageTo"],
                        contactObject.Email,
                        contactObject.Title,
                        contactObject.Description);

                    if (!sentResult.IsCompletedSuccessfully)
                    {
                        // Sending operation is failed!
                        //TODO
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