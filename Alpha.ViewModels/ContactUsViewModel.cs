using Alpha.Infrastructure.Captcha;


namespace Alpha.ViewModels
{
    public class ContactUsViewModel : NumericCaptcha
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Description { get; set; }
        public virtual string Title { get; set; }
    }
}
