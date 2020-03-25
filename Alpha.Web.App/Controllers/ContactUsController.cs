using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.Models;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Alpha.Web.App.Resources.Constants;
namespace Alpha.Web.App.Controllers
{
    public class ContactUsController : BaseController
    {
        public ContactUsController()
        {

        }

        [HttpGet, AllowAnonymous]
        public ActionResult ContactUsForm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ContactUsForm(ContactUsViewModel contactObject)
        {
            return Json(new
            {
                status = Messages.SuccessStatus,
                message = Messages.SendingMessageSuccessfully,
                type = Messages.SuccessAlertType
            });
        }
    }
}