using Alpha.Models;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface IAboutUsService
    {
        Task<AboutUs> GetAboutUsAsync();
    }
}