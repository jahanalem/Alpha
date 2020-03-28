using System.Collections.Generic;
using Alpha.Infrastructure.PaginationUtility;


namespace Alpha.ViewModels
{
    public class ArticleListViewModel
    {
        public List<ArticleViewModel> ArticleViewModelList { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
