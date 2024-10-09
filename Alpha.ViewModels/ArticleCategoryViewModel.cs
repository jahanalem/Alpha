using Alpha.Models;
using System.Collections.Generic;

namespace Alpha.ViewModels
{
    public class ArticleCategoryViewModel : BaseViewModel
    {
        public ArticleCategory Category { get; set; }
        public List<ArticleCategory> Parents { get; set; }
    }
}
