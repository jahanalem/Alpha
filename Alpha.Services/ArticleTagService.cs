using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Services
{
    public class ArticleTagService : BaseService<ArticleTag>, IArticleTagService
    {
        public ArticleTagService(IUnitOfWork unitOfWork, ILogger<ArticleTagService> logger) : base(unitOfWork, logger)
        {
        }

        public virtual List<Tag> GetTagsByArticleId(int articleId)
        {
            List<Tag> tags = _unitOfWork.Repository<ArticleTag>().GetByCriteria(a => a.ArticleId == articleId)
                .Select(b => b.Tag)
                .Distinct()
                .ToList();

            return tags;
        }
    }
}