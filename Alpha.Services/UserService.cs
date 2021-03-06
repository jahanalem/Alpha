﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        private IUserValidator<User> _userValidator;
        private IPasswordValidator<User> _passwordValidator;
        private IPasswordHasher<User> _passwordHasher;
        public UserService(ModelStateDictionary modelState,
                            UserManager<User> userManager,
                            IUserValidator<User> userValid,
                            IPasswordValidator<User> passValid,
                            IPasswordHasher<User> passwordHash)
        {
            _modelState = modelState;
            _userManager = userManager;
            _userValidator = userValid;
            _passwordValidator = passValid;
            _passwordHasher = passwordHash;

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

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                _modelState.AddModelError("", error.Description);
            }
        }
        public async Task<IdentityResult> EditUser(User userObj)
        {
            IList<IdentityError> errors = new List<IdentityError>();
            User user = await _userManager.FindByIdAsync(userObj.Id.ToString());
            if (user != null)
            {
                user.Email = userObj.Email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    errors.Add(validEmail.Errors);
                    //AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if (!string.IsNullOrEmpty(userObj.Password))
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, userObj.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, userObj.Password);
                    }
                    else
                    {
                        errors.Add(validPass.Errors);
                        //AddErrorsFromResult(validPass);
                    }
                    if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && userObj.Password != string.Empty && validPass.Succeeded))
                    {
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            errors.Add(result.Errors);
                            //AddErrorsFromResult(result);
                        }
                    }
                }
                {
                    //user.IsActive = userViewModel.IsActive;
                    //user.UserName = userViewModel.UserName.Trim();
                    //user.Email = userViewModel.Email.Trim();
                    //user.PhoneNumber = userViewModel.PhoneNumber.Trim();
                    //user.BirthDate = userViewModel.BirthDate;
                    //user.FirstName = userViewModel.FirstName.Trim();
                    //user.LastName = userViewModel.LastName.Trim();
                    //user.PhotoFileName = userViewModel.PhotoFileName.Trim();
                    //user.IsEmailPublic = userViewModel.IsEmailPublic;
                    //user.Location = userViewModel.Location.Trim();
                    //result = await _userManager.UpdateAsync(user);
                }
            }
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
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
