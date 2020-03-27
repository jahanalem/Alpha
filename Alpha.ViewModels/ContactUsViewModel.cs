﻿using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Models;
using Alpha.ViewModels.Helper;

namespace Alpha.ViewModels
{
    public class ContactUsViewModel
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Description { get; set; }
        public virtual string Title { get; set; }
    }
}
