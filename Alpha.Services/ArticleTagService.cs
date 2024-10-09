using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Services
{
    public class ArticleTagService : BaseService<IArticleTagRepository, ArticleTag>, IArticleTagService
    {
        private IArticleTagRepository _articleTagRepository;

        public ArticleTagService(IArticleTagRepository articleTagRepository) : base(articleTagRepository)
        {
            _articleTagRepository = articleTagRepository;
        }

        public virtual List<Tag> GetTagsByArticleId(int articleId)
        {
            List<Tag> tags = _articleTagRepository.FetchByCriteria(a => a.ArticleId == articleId).Select(b => b.Tag).Distinct().ToList();
            return tags;
        }
    }
}