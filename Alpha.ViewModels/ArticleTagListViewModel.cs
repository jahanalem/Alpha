﻿using Alpha.Infrastructure.PaginationUtility;
using System.Collections.Generic;


namespace Alpha.ViewModels
{
    public class ArticleTagListViewModel
    {
        public List<ArticleViewModel> ArticleViewModelList { get; set; }
        public Pagination Pagination { get; set; } = new Pagination();
        /// <summary>
        /// Filter Articles by TagId. TagId is a querystring
        /// </summary>
        public int? TagId { get; set; }
    }
}
