using System;
using System.Linq.Expressions;
using Alpha.Models;
using Alpha.ViewModels;

namespace Alpha.Services.Interfaces
{
    public interface ITagService : IBaseService<Tag>
    {
        ArticleViewModel FindAllTags(Expression<Func<Tag, bool>> predicate,
            params Expression<Func<Tag, object>>[] includeProperties);
    }
}