using Alpha.Infrastructure.PaginationUtility;
using Alpha.Models;
using System.Collections.Generic;

namespace Alpha.ViewModels.Searches
{
    public class SearchResultsViewModel : BaseViewModel
    {
        public List<Article> Articles { get; set; }
        public Pagination Pagination { get; set; }
    }
}
