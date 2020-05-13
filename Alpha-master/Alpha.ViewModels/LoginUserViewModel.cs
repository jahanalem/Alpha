using System.ComponentModel.DataAnnotations;

namespace Alpha.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
