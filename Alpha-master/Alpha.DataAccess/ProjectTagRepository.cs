using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class ProjectTagRepository : Repository<ProjectTag>, IProjectTagRepository
    {
        public ProjectTagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
