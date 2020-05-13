using Microsoft.AspNetCore.Identity;

namespace Alpha.Models.Identity
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public virtual User User { get; set; }
    }
}
