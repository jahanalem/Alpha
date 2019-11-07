using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Models.Identity;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Alpha.Services
{
    public interface IUserService
    {
        bool CreateUser(User userToCreate);
        Task<IdentityResult> EditUser(UserEditViewModel userViewModel);
        Task<IdentityResult> DeleteAsync(string id);
        IEnumerable<User> ListUsers();
    }
}
