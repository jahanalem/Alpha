using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Models;

namespace Alpha.ViewModels
{
    public class RecursionArticleCategory
    {
        public List<ArticleCategory> ArticleCategories { get; set; }
        public ArticleCategory Item { get; set; }
        public int? ParentId { get; set; }
    }
}
