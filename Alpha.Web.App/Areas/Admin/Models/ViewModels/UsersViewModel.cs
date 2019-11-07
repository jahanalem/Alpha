using System.Collections.Generic;

namespace Alpha.Web.App.Areas.Admin.Models.ViewModels
{
    public class UsersViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> RoleNames { get; set; }
    }
}
