using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Alpha.Infrastructure.PaginationUtility
{
    public class Pagination
    {
        public virtual PagingInfo PagingInfo { get; set; } = new PagingInfo();
        public virtual string TargetArea { get; set; } = string.Empty;
        public virtual string TargetController { get; set; }
        public virtual string TargetAction { get; set; }

#nullable enable
        /// <summary>
        /// asp-all-route-data
        /// </summary>
        public virtual Dictionary<string, string>? QueryStrings { get; set; } = new Dictionary<string, string>
        {
            {"pageNumber","1" }
        };

#nullable disable
    }
}
