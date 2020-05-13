using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Alpha.Infrastructure
{
    [HtmlTargetElement("td", Attributes = "identity-role")]
    public class RoleUsersTagHelper : TagHelper
    {
        private UserManager<User> userManager;
        private RoleManager<Role> roleManager;
        public RoleUsersTagHelper(UserManager<User> usermgr,
        RoleManager<Role> rolemgr)
        {
            userManager = usermgr;
            roleManager = rolemgr;
        }
        [HtmlAttributeName("identity-role")]
        public string Role { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            List<string> names = new List<string>();
            var role = await roleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in userManager.Users)
                {
                    if (user != null && await userManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "No Users" : string.Join(", ", names));
        }
    }
}
