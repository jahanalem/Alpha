using Microsoft.AspNetCore.Identity;

namespace Alpha.Models.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}
