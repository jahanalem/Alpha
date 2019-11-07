using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Infrastructure;
using Alpha.Models.Identity;
using Alpha.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alpha.Services
{
    public class UserService : IUserService
    {
        private ModelStateDictionary _modelState;
        private UserManager<User> _userManager;

        public UserService(ModelStateDictionary modelState, UserManager<User> userManager)
        {
            _modelState = modelState;
            _userManager = userManager;
        }
        public bool CreateUser(User userToCreate)
        {
            // Validation logic
            if (!ValidateUser(userToCreate))
                return false;
            // Database logic
            IdentityResult result = _userManager.CreateAsync(userToCreate).Result;
            return result.Succeeded;
        }

        public async Task<IdentityResult> EditUser(UserEditViewModel userViewModel)
        {
            IdentityResult result = new IdentityResult();
            User user = _userManager.FindByIdAsync(userViewModel.Id.ToString()).Result;
            if (user != null && ValidateUser(userViewModel))
            {
                user.IsActive = userViewModel.IsActive;
                user.UserName = userViewModel.UserName.Trim();
                user.Email = userViewModel.Email.Trim();
                user.PhoneNumber = userViewModel.PhoneNumber.Trim();
                user.BirthDate = userViewModel.BirthDate;
                user.FirstName = userViewModel.FirstName.Trim();
                user.LastName = userViewModel.LastName.Trim();
                user.PhotoFileName = userViewModel.PhotoFileName.Trim();
                user.IsEmailPublic = userViewModel.IsEmailPublic;
                user.Location = userViewModel.Location.Trim();
                result = await _userManager.UpdateAsync(user);
            }
            return result;
        }
        public IEnumerable<User> ListUsers()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            IdentityResult result = await _userManager.DeleteAsync(user);
            return result;
        }

        protected bool ValidateUser(object obj)
        {
            Dictionary<string, object> properties = obj.GetProperties();
            foreach (var prop in properties)
            {
                switch (prop.Key)
                {
                    case "FirstName":
                        if (prop.Value.ToString().Trim().Length > 450)
                            _modelState.AddModelError("FirstName", "The first name is too long!");
                        break;
                    case "LastName":
                        if (prop.Value.ToString().Trim().Length > 450)
                            _modelState.AddModelError("LastName", "The first name is too long!");
                        break;
                    case "Email":
                        if (!prop.Value.ToString().Trim().IsValidEmailAddress())
                            _modelState.AddModelError("Email", "Email is not valid!");
                        break;
                    case "BirthDate":
                        if (prop.Value.ToDateTime() == DateTime.MinValue)
                            _modelState.AddModelError("BirthDate", "BirthDate is not valid!");
                        break;
                }
            }
            return _modelState.IsValid;
        }
    }
}
