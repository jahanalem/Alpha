using Alpha.Infrastructure.PaginationUtility;
using System.Collections.Generic;

namespace Alpha.ViewModels
{
    public class UsersViewModel
    {
        public IList<Alpha.Models.Identity.User> Users { get; set; }
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
