using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class ProjectStateRepository : Repository<ProjectState>, IProjectStateRepository
    {
        public ProjectStateRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
