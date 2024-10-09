using Alpha.Models;
using System.Collections.Generic;

namespace Alpha.ViewModels
{
    public class RecursionArticleCategory
    {
        public List<ArticleCategory> ArticleCategories { get; set; }
        public ArticleCategory Item { get; set; }
        public int? ParentId { get; set; }
    }
}
