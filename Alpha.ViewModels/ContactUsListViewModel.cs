using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using System.Collections.Generic;


namespace Alpha.ViewModels
{
    public class ContactUsListViewModel
    {
        public Pagination Pagination { get; set; } = new Pagination();
        public List<ContactUs> ContactUsList { get; set; }
    }
}
