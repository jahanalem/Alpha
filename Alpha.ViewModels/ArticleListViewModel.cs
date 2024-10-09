using Alpha.Infrastructure.PaginationUtility;
using System.Collections.Generic;


namespace Alpha.ViewModels
{
    public class ArticleListViewModel
    {
        public List<ArticleViewModel> ArticleViewModelList { get; set; }
        public Pagination Pagination { get; set; } = new Pagination();
    }
}
