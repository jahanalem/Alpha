using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Services
{
    public class TagService : BaseService<ITagRepository, Tag>, ITagService
    {
        public TagService(ITagRepository obj) : base(obj)
        {
        }

        public  ArticleViewModel FindAllTags(Expression<Func<Tag, bool>> predicate, params Expression<Func<Tag, object>>[] includeProperties)
        {
            var tagList = base.FindAll(predicate, includeProperties).ToListAsync().Result; ;
            tagList.ForEach(i => i.IsActive = false);
            ArticleViewModel viewM = new ArticleViewModel
            {
                Tags = tagList
            };
            return viewM;
        }
    }
}
