using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Alpha.Web.App.AppTagHelpers
{
    // There is a function in javascript file that convert the utc to local time zone: function convertUTCDateToLocalDate(date) {}
    [HtmlTargetElement("LocalDate")]
    public class LocalDateTagHelper : TagHelper
    {
        public DateTime? Utc { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";// Replaces <LocalDate> with <span> tag
            output.Attributes.SetAttribute("class", "mytime");
            if (Utc != null)
                output.Attributes.Add("utc", Utc.Value.ToString("o"));
            else
            {
                output.Attributes.Add("utc", null);
            }
            // DateTime x = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, TimeZoneInfo.Local);
            // output.Content.SetContent(xxx);
            base.Process(context, output);
        }

        //public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    return base.ProcessAsync(context, output);
        //}
    }
}
