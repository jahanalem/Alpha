using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Models;
using Alpha.ViewModels.Helper;

namespace Alpha.ViewModels
{
    public class ContactUsListViewModel : Pagination
    {
        public List<ContactUs> ContactUsList { get; set; }
    }
}
