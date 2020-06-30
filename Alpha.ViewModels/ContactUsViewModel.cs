﻿using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Infrastructure.Captcha;
using Alpha.Models;
using Alpha.ViewModels.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Converters;


namespace Alpha.ViewModels
{
    public class ContactUsViewModel: NumericCaptcha
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Description { get; set; }
        public virtual string Title { get; set; }

        public virtual IFormFile Attachment { get; set; }

    }
}
