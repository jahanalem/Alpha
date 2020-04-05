using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Infrastructure.Email;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;

namespace Alpha.Services
{
    public class ContactUsService : BaseService<IContactUsRepository, ContactUs>, IContactUsService
    {
        public ContactUsService(IContactUsRepository obj) : base(obj)
        {
        }

        public async Task<bool> CreateAsync(ContactUsViewModel viewModel)
        {
            var result = Validate(viewModel);
            result.contactUs.IsActive = true;
            if (result.isValid)
            {
                var newId = CreateAsync(result.contactUs);
                var x = await SaveChangesAsync();
                return true;
            }

            return false;
        }

        private (ContactUs contactUs, bool isValid) Validate(ContactUsViewModel obj)
        {
            var result = new ContactUs();
            var isVaild = true;
            //if (!obj.NumericCaptcha.IsResultCorrect(obj.NumericCaptcha.FirstNumber,
            //    obj.NumericCaptcha.SecondNumber,
            //    obj.NumericCaptcha.Result))
            //{
            //    isVaild = false;
            //    return (contactUs: result, isVaild);
            //}
            if (!string.IsNullOrEmpty(obj.FirstName))
            {
                result.FirstName = obj.FirstName.Trim();
            }
            if (!string.IsNullOrEmpty(obj.LastName))
            {
                result.LastName = obj.LastName.Trim();
            }

            if (!string.IsNullOrEmpty(obj.Description))
            {
                if (obj.Description.Length <= 1024)
                {
                    result.Description = obj.Description.Trim();
                }
                else
                {
                    // too much dsc.
                    isVaild = false;
                }
            }
            else
            {
                // Description is null or empty
                isVaild = false;
            }
            result.LastName = obj.LastName?.Trim();
            if (!string.IsNullOrEmpty(obj.Email))
            {
                if (obj.Email.IsValidEmail())
                {
                    result.Email = obj.Email;
                }
                else
                {
                    // the email format is wrong!
                    isVaild = false;
                }
            }
            else
            {
                // the email is null or empty
                isVaild = false;
            }

            return (contactUs: result, isVaild);
        }
    }
}