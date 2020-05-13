using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class ArticleTagRepository : Repository<ArticleTag>, IArticleTagRepository
    {
        public ArticleTagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
