using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using System.Threading.Tasks;

namespace Alpha.DataAccess
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly IArticleRepository _repository;
        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<int> AddOrUpdateAsync(Article entity)
        {
            return await base.AddOrUpdateAsync(entity);
        }
    }
}
