using System.Collections.Generic;
using Alpha.Models.Identity;

namespace Alpha.ViewModels
{
    public class RoleEditViewModel
    {
        public Role Role { get; set; }
        public IEnumerable<User> Members { get; set; }
        public IEnumerable<User> NonMembers { get; set; }
    }
}
