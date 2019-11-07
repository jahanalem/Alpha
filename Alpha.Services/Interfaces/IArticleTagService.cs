using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Models;

namespace Alpha.Services.Interfaces
{
    public interface IArticleTagService : IBaseService<ArticleTag>
    {
        List<Tag> GetTagsByArticleId(int articleId);
    }
}