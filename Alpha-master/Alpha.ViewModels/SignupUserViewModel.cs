using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alpha.ViewModels
{
    public class SignupUserViewModel
    {
        [StringLength(50), UIHint("first name")]
        public string FirstName { get; set; }

        [StringLength(50), UIHint("last name")]
        public string LastName { get; set; }

        [Required, StringLength(50), UIHint("user name")]
        public string UserName { get; set; }

        [Required, UIHint("email")]
        public string Email { get; set; }

        [Required, UIHint("password")]
        public string Password { get; set; }

        [Required, UIHint("confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
