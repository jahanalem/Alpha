using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Alpha.Infrastructure.PaginationUtility
{
    //[HtmlTargetElement("a", Attributes = "asp - action")]
    //[HtmlTargetElement("a", Attributes = "asp-controller")]
    //[HtmlTargetElement("a", Attributes = "asp-area")]
    //[HtmlTargetElement("a", Attributes = "asp-page")]
    [HtmlTargetElement("pageLink")]
    public class LinkTagHelper : TagHelper
    {
        public LinkTagHelper()
        {
            QueryStrings = new List<QueryString>();
            AspArea = string.Empty;
        }

        public string? AspArea { get; set; }
        public string? AspController { get; set; }
        public string? AspAction { get; set; }
        public int? AspRoutePagenumber { get; set; }
        public List<QueryString> QueryStrings { get; set; }

        //private const string ActionAttributeName = "asp-action";
        //private const string ControllerAttributeName = "asp-controller";
        //private const string AreaAttributeName = "asp-area";
        //private const string PageAttributeName = "asp-page";

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("class", "page-link");
            string href = string.Empty;
            
            if (AspArea != null)
            {
                output.Attributes.SetAttribute("asp-area", AspArea);
            }
            if (AspController != null)
            {
                output.Attributes.SetAttribute("asp-controller", AspController);
            }
            if (AspAction != null)
            {
                output.Attributes.SetAttribute("asp-action", AspAction);
            }
            
            if (AspRoutePagenumber != null)
            {
                output.Attributes.SetAttribute("asp-route-pagenumber", AspRoutePagenumber.Value);
            }
            if (QueryStrings.Any())
            {
                foreach (var q in QueryStrings)
                {
                    output.Attributes.SetAttribute($"asp-route-{q.Key}", q.Value);
                }
            }
            return base.ProcessAsync(context, output);
        }
    }
}
