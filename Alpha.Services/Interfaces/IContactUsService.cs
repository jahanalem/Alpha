using System.Threading.Tasks;
using Alpha.Models;
using Alpha.ViewModels;

namespace Alpha.Services.Interfaces
{
    public interface IContactUsService : IBaseService<ContactUs>
    {
        Task<bool> CreateAsync(ContactUsViewModel viewModel);
    }
}