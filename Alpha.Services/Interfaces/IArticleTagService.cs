using Alpha.Models;
using System.Collections.Generic;

namespace Alpha.Services.Interfaces
{
    public interface IArticleTagService
    {
        List<Tag> GetTagsByArticleId(int articleId);
    }
}