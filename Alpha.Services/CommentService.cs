using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Models.Identity;
using Alpha.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class CommentService : BaseService<Comment>, ICommentService
    {
        private readonly UserManager<User> _userManager;

        public CommentService(UserManager<User> userManager, IUnitOfWork unitOfWork, ILogger<CommentService> logger)
            : base(unitOfWork, logger)
        {
            _userManager = userManager;
        }

        public async Task<Comment> AddOrUpdateAsync(Comment comment)
        {
            return await _unitOfWork.Repository<Comment>().AddOrUpdateAsync(comment);
        }

        public async Task<Comment> GetCommentById(int id)
        {
            return await _unitOfWork.Repository<Comment>().GetByIdAsync(id);
        }
        public async Task<List<Comment>> GetCommentsByParentId(int id)
        {
            return await _unitOfWork.Repository<Comment>().GetByCriteria(c => c.ParentId == id).ToListAsync();
        }

        public Task<Comment> FindByIdAsync(int commentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetComments(int articleId)
        {
            var userRepository = _unitOfWork.Repository<User>();
            var commentRepository = _unitOfWork.Repository<Comment>();

            var commentsQuery = from comment in commentRepository.GetByCriteria(c => c.ArticleId == articleId)
                                join user in userRepository.GetAll() on comment.UserId equals user.Id into commentUserGroup
                                from user in commentUserGroup.DefaultIfEmpty()
                                select new
                                {
                                    Comment = comment,
                                    PublicUserName = user.DisplayName,
                                };

            var queryResults = await commentsQuery.OrderByDescending(t => t.Comment.CreatedDate).ToListAsync();
            var comments = new List<Comment>();

            foreach (var result in queryResults)
            {
                result.Comment.PublicUserName = result.PublicUserName ?? "Anonymous";
                var user = new User { Id = result.Comment.UserId.Value };
                var claims = await _userManager.GetClaimsAsync(user);
                var claim = claims.FirstOrDefault(x => x.Type == Alpha.Infrastructure.AdditionalClaimTypes.Picture);
                result.Comment.UserPictureUrl = claim?.Value;
                comments.Add(result.Comment);
            }

            return comments;
        }

        Task<Comment> ICommentService.AddOrUpdateAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveCommentAsync(Comment comment)
        {
            await _unitOfWork.Repository<Comment>().DeleteAsync(comment);
        }
    }
}