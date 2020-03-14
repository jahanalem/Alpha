using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUsForm(ContactUsViewModel contactObject)
        {
            return View();
        }
    }
}