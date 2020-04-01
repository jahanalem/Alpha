using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Alpha.ViewModels
{
    public class PasswordForgotViewModel : BaseViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "EMail Address")]
        public string Email { get; set; }
    }
}
