using Alpha.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services.Interfaces
{
    public interface ICommentService : IBaseService<Comment>
    {
        Task<List<Comment>> GetComments(int articleId);
    }
}