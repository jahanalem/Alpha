using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Infrastructure.Email;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Alpha.Web.App.Resources.Constants;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alpha.Web.App.Controllers
{
    public class ContactUsController : BaseController
    {
        private IContactUsService _contactUsService;
        public ContactUsController(IContactUsService contactUsService)
        {
            _contactUsService = contactUsService;
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
                var result = await _contactUsService.CreateAsync(contactObject);
                if (result)
                {
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