using Alpha.Models;
using Alpha.ViewModels;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IContactUsService : IBaseService<ContactUs>
    {
        Task<bool> CreateAsync(ContactUsViewModel viewModel);
    }
}