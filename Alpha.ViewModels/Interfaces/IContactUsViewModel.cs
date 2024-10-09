using Alpha.Infrastructure.Captcha;

namespace Alpha.ViewModels.Interfaces
{
    public interface IContactUsViewModel
    {
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        string Description { get; set; }
        string Title { get; set; }
        NumericCaptcha NumericCaptcha { get; set; }
    }
}
