using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.Infrastructure.PaginationUtility
{
    public static class PaginationSettings
    {
        public static Pagination Init(this Pagination pagination,
            int currentPage,
            int itemsPerPage,
            int totalItems,
            string targetController,
            string targetAction,
            string targetArea = "",
            Dictionary<string, string> queryStrings = null)
        {
            var test = new Pagination
            {
                PagingInfo = new PagingInfo
                {
                    TotalItems = 1,
                    ItemsPerPage = 1,
                    CurrentPage = 1
                },
                TargetAction = "",
                TargetController = "",
                TargetArea = "",
                QueryStrings = null
            };
            var pageInfo = new PagingInfo
            {
                CurrentPage = currentPage,
                ItemsPerPage = itemsPerPage,
                TotalItems = totalItems
            };

            pagination.PagingInfo = pageInfo;
            pagination.TargetController = targetController;
            pagination.TargetAction = targetAction;
            pagination.TargetArea = targetArea;
            if (queryStrings != null)
            {
                pagination.QueryStrings = queryStrings;
                //QueryStrings = new Dictionary<string, string> { { "pageNumber", "1" } };
            }
            return pagination;
        }

    }
}
