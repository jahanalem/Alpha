using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Alpha.Infrastructure.PaginationUtility
{
    public class Pagination
    {
        public virtual PagingInfo PagingInfo { get; set; } = new PagingInfo();

        public string Url { get; set; }
        //public virtual string TargetArea { get; set; } = string.Empty;
        //public virtual string TargetController { get; set; }
        //public virtual string TargetAction { get; set; }

#nullable enable
        /// <summary>
        /// asp-all-route-data
        /// </summary>
        //public virtual Dictionary<string, string>? QueryStrings { get; set; } = new Dictionary<string, string>
        //{
        //    {"pageNumber","1" }
        //};

#nullable disable
    }

    public static class UpdateUrl
    {
        public static string UpdatePageNumber(this string url, int value)
        {
            var pageNumber = QueryStringParameters.PageNumber;
            var pos = url.IndexOf(pageNumber, StringComparison.Ordinal);
            url = url.Remove(pos + 10);
            url = $"{url}={value}";
            return url;
            //System.Uri uri = new Uri("http://www.somesite.com/what/test.aspx?hello=1");
            //string fixedUri = uri.AbsoluteUri.Replace(uri.Query, string.Empty);
        }
    }
}
