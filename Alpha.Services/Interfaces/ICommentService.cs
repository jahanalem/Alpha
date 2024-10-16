using Alpha.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface ICommentService
    {
        Task<Comment> AddOrUpdateAsync(Comment comment);
        Task<Comment> GetCommentById(int id);
        Task<Comment> FindByIdAsync(int commentId);
        Task<List<Comment>> GetComments(int articleId);
        Task<List<Comment>> GetCommentsByParentId(int id);
        Task RemoveCommentAsync(Comment comment);
    }
}