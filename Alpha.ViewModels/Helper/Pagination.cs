using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Alpha.ViewModels.Helper
{
    public class Pagination
    {
        public PagingInfo PagingInfo { get; set; } = new PagingInfo();

        //[HtmlAttributeName("asp-controller")]
        public string TargetController { get; set; }

        //[HtmlAttributeName("asp-action")]
        public string TargetAction { get; set; }
    }
}
