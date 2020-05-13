using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class AboutUsRepository : Repository<AboutUs>, IAboutUsRepository
    {
        public AboutUsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
