using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

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
            pagination.Url = pSettings.Url;
            return pagination;
        }
    }
}