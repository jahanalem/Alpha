using System.Collections.Generic;
using Alpha.ViewModels.Helper;

namespace Alpha.ViewModels
{
    public class ArticleTagListViewModel
    {
        public List<ArticleViewModel> ArticleViewModelList { get; set; }
        public PagingInfo PagingInfo { get; set; }
        /// <summary>
        /// Filter Articles by TagId. TagId is a querystring
        /// </summary>
        public int? TagId { get; set; }
    }
}
