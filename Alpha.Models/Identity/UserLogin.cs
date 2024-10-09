using Microsoft.AspNetCore.Identity;

namespace Alpha.Models.Identity
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}
