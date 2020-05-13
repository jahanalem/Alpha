using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.Models.Identity;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.AspNetCore.Identity;
namespace Alpha.Services
{
    public class CommentService : BaseService<ICommentRepository, Comment>, ICommentService
    {
        private ICommentRepository _commentRepository;
        private ApplicationDbContext _dbContext;
        private UserManager<User> _userManager;

        public CommentService(ICommentRepository commentRepository,
            ApplicationDbContext applicationDbContext,
            UserManager<User> userManager) : base(commentRepository)
        {
            _commentRepository = commentRepository;
            _dbContext = applicationDbContext;
            _userManager = userManager;

        }

        public async Task<List<Comment>> GetComments(int articleId)
        {
            List<Comment> comments = new List<Comment>();
            //var comments = await _commentRepository.FetchByCriteria(c => c.ArticleId == articleId).ToListAsync();
            var q0 = from c in _commentRepository.Instance()
                     join u in _dbContext.Users on c.UserId equals u.Id into cu
                     from u in cu.DefaultIfEmpty()
                     where c.ArticleId == articleId
                     select new
                     {
                         Comment = c,
                         PublicUserName = u.DisplayName,
                     };
            var queryResults = await q0.OrderByDescending(t => t.Comment.CreatedDate).ToListAsync();
            foreach (var r in queryResults)
            {
                r.Comment.PublicUserName = r.PublicUserName ?? "Anonymous";
                //var user = await _userManager.FindByIdAsync(r.Comment.UserId.ToString());
                var claims = await _userManager.GetClaimsAsync(new User { Id = r.Comment.UserId.Value });
                var claim = claims.FirstOrDefault(x => x.Type == Alpha.Infrastructure.AdditionalClaimTypes.Picture);
                var picture = claim != null ? claim.Value : null;
                r.Comment.UserPictureUrl = picture;
                comments.Add(r.Comment);
            }
            //var query = await _commentRepository.Instance().Where(c => c.ArticleId == articleId)
            //    .Join(_dbContext.Users,
            //        c => c.UserId,
            //        u => u.Id,
            //        (c, u) => (c)).ToListAsync();

            return comments;
        }
    }
}
