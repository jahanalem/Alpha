using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class ArticleLikeRepository : Repository<ArticleLike>, IArticleLikeRepository
    {
        public ArticleLikeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
