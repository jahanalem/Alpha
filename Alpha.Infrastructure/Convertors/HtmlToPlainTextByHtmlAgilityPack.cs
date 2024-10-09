using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;

namespace Alpha.Infrastructure.Convertors
{
    public static class HtmlToPlainTextByHtmlAgilityPack //: IHtmlToPlainText
    {
        public static string ToPlainText(this string html, bool AsStripHtml = true)
        {
            if (AsStripHtml)
                return StripHtml(html);
            else
            {
                string result = string.Empty;
                foreach (var text in GetTextsFromHtml(html))
                {
                    result += $"{text}'\n'";
                }
                return result;
            }
        }

        private static ICollection<string> GetTextsFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return new List<string> { html };
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return GetTextsFromNode(htmlDoc.DocumentNode.ChildNodes);
        }

        private static ICollection<string> GetTextsFromNode(HtmlNodeCollection nodes)
        {
            var texts = new List<string>();
            foreach (var node in nodes)
            {
                if (node.Name.ToLowerInvariant() == "style")
                    continue;
                if (node.HasChildNodes)
                {
                    texts.AddRange(GetTextsFromNode(node.ChildNodes));
                }
                else
                {
                    var innerText = node.InnerText;
                    if (!string.IsNullOrWhiteSpace(innerText))
                    {
                        texts.Add(innerText);
                    }
                }
            }

            return texts;
        }

        public static string StripHtml(string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;
            //get rid of HTML tags
            var output = Regex.Replace(source, "<[^>]*>", string.Empty);
            //get rid of multiple blank lines
            output = Regex.Replace(output, @"^\s*$\n", string.Empty, RegexOptions.Multiline);
            return HttpUtility.HtmlDecode(output);
        }
    }
}
