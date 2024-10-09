using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Alpha.Models.Identity
{
    public class Role : IdentityRole<int>
    {
        public Role() : base()
        {

        }
        public Role(string roleName) : this()
        {
            Name = roleName;
        }
        public Role(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Description { get; set; }

        public virtual ICollection<UserRole> Users { get; set; }
        public virtual ICollection<RoleClaim> Claims { get; set; }
    }
}
