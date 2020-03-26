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

        public async Task<bool> InsertAsync(ContactUsViewModel viewModel)
        {
            var result = Validate(viewModel);
            if (result.isValid)
            {
                var newId = InsertAsync(result.contactUs);
                var x = await SaveChangesAsync();
                return true;
            }

            return false;
        }

        private (ContactUs contactUs, bool isValid) Validate(ContactUsViewModel obj)
        {
            var result = new ContactUs();
            var isVaild = true;
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