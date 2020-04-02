using System;
using System.Collections.Generic;
using System.Text;
using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;

namespace Alpha.ViewModels.Searches
{
    public class SearchResultsViewModel : BaseViewModel
    {
        public List<Article> Articles { get; set; }
        public Pagination Pagination { get; set; }
    }
}
