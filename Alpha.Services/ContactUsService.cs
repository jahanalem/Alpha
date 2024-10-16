using Alpha.DataAccess.UnitOfWork;
using Alpha.Infrastructure.Email;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class ContactUsService : BaseService<ContactUs>, IContactUsService
    {
        public ContactUsService(IUnitOfWork unitOfWork, ILogger<ContactUsService> logger) : base(unitOfWork, logger)
        {
        }

        public async Task<bool> CreateAsync(ContactUsViewModel viewModel)
        {
            var result = Validate(viewModel);
            result.contactUs.IsActive = true;
            if (result.isValid)
            {
                await _unitOfWork.Repository<ContactUs>().AddAsync(result.contactUs);
                await _unitOfWork.CompleteAsync();
                return true;
            }

            return false;
        }

        public async Task<ContactUs> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ContactUs>().GetByIdAsync(id);
        }

        public async Task<List<ContactUs>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ContactUs>().GetAll().ToListAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _unitOfWork.Repository<ContactUs>().GetAll().CountAsync();
        }

        public async Task<List<ContactUs>> GetByCriteria(int itemsPerPage, int pageNumber)
        {
            return await _unitOfWork.Repository<ContactUs>().GetByCriteria()
                .OrderByDescending(c => c.CreatedDate)
                .Skip((pageNumber - 1) * itemsPerPage)
                .Take(itemsPerPage).ToListAsync();
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
            if (!string.IsNullOrEmpty(obj.Title))
            {
                result.Title = obj.Title.Trim();
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