using Alpha.Models.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public interface IUserService
    {
        bool CreateUser(User userToCreate);
        Task<IdentityResult> EditUser(User userObj);
        Task<IdentityResult> DeleteAsync(string id);
        IEnumerable<User> ListUsers();
    }
}
