using System.Collections.Generic;
using Alpha.Infrastructure.PaginationUtility;


namespace Alpha.ViewModels
{
    public class ArticleTagListViewModel : Pagination
    {
        public List<ArticleViewModel> ArticleViewModelList { get; set; }
        /// <summary>
        /// Filter Articles by TagId. TagId is a querystring
        /// </summary>
        public int? TagId { get; set; }
    }
}
