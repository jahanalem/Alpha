using Microsoft.AspNetCore.Identity;

namespace Alpha.Models.Identity
{
    public class UserToken : IdentityUserToken<int>
    {
        public virtual User User { get; set; }
    }
}
