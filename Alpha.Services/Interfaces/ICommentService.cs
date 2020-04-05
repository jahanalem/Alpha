using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Models;

namespace Alpha.Services.Interfaces
{
    public interface ICommentService : IBaseService<Comment>
    {
        Task<List<Comment>> GetComments(int articleId);
    }
}