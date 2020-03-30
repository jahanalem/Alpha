using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Infrastructure.PaginationUtility;

namespace Alpha.ViewModels
{
    public class UsersViewModel
    {
        public IList<Alpha.Models.Identity.User> Users { get; set; }
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
