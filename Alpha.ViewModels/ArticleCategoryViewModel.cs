using Alpha.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.ViewModels
{
    public class ArticleCategoryViewModel : BaseViewModel
    {
        public ArticleCategory Category { get; set; }
        public List<ArticleCategory> Parents { get; set; }
    }
}
