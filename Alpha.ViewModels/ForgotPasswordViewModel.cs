using System.ComponentModel.DataAnnotations;

namespace Alpha.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [UIHint("email")]
        public string Email { get; set; }
    }
}
