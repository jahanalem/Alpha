using Alpha.DataAccess.Interfaces;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class CommentLikeRepository : Repository<CommentLike>, ICommentLikeRepository
    {
        public CommentLikeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
