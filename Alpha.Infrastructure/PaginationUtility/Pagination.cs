using System;

namespace Alpha.Infrastructure.PaginationUtility
{
    public class Pagination
    {
        public virtual PagingInfo PagingInfo { get; set; } = new PagingInfo();

        public string Url { get; set; }
    }

    public static class UpdateUrl
    {
        public static string UpdatePageNumber(this string url, int value)
        {
            var pageNumber = QueryStringParameters.PageNumber;
            var pos = url.IndexOf(pageNumber, StringComparison.Ordinal);
            url = url.Remove(pos + pageNumber.Length);
            url = $"{url}/{value}";
            return url;
        }
    }
}
//System.Uri uri = new Uri("http://www.somesite.com/what/test.aspx?hello=1");
//string fixedUri = uri.AbsoluteUri.Replace(uri.Query, string.Empty);