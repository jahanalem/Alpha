using Alpha.Models;
using Alpha.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IContactUsService
    {
        Task<bool> CreateAsync(ContactUsViewModel viewModel);
        Task<ContactUs> GetByIdAsync(int id);
        Task<List<ContactUs>> GetAllAsync();
        Task<int> GetCountAsync();
        Task<List<ContactUs>> GetByCriteria(int itemsPerPage, int pageNumber);
    }
}