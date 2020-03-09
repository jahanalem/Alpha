﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Alpha.Web.App.AppTagHelpers
{
    [HtmlTargetElement("LocalDate")]
    public class LocalDateTagHelper : TagHelper
    {
        public DateTime Utc { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";// Replaces <LocalDate> with <span> tag
            output.Attributes.SetAttribute("class", "mytime");
            output.Attributes.Add("utc", Utc.ToString("o"));
            //output.Content.SetContent(xxx);
            //String.Format("<span class=\"mytime\" utc =\"{0}\"></span>", DateTimeUtc.ToString("o"));
            base.Process(context, output);
        }

        //public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        //{
        //    return base.ProcessAsync(context, output);
        //}
    }
}
