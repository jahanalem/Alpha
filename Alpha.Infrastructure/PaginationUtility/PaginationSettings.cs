using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.Infrastructure.PaginationUtility
{
    public static class PaginationSettings
    {
        public static Pagination Init(this Pagination pagination, Pagination pSettings)
        {
            var pageInfo = new PagingInfo
            {
                TotalItems = pSettings.PagingInfo.TotalItems,
                ItemsPerPage = pSettings.PagingInfo.ItemsPerPage,
                CurrentPage = pSettings.PagingInfo.CurrentPage
            };

            pagination.PagingInfo = pageInfo;
            pagination.TargetController = pSettings.TargetController;
            pagination.TargetAction = pSettings.TargetAction;
            pagination.TargetArea = pSettings.TargetArea;
            if (pSettings.QueryStrings != null)
            {
                pagination.QueryStrings = pSettings.QueryStrings;
                //QueryStrings = new Dictionary<string, string> { { "pageNumber", "1" } };
            }
            return pagination;
        }

    }
}
