using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using QueryString = Microsoft.AspNetCore.Http.QueryString;


namespace Alpha.ViewModels
{
    public class ContactUsListViewModel
    {
        public Pagination Pagination { get; set; } = new Pagination();
        public List<ContactUs> ContactUsList { get; set; }
    }
}
