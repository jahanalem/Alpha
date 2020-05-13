using System.ComponentModel.DataAnnotations;

namespace Alpha.Models.Interfaces
{
    public interface IHtmlMetaTags
    {
        string TitleHtmlMetaTag { get; set; }
        string DescriptionHtmlMetaTag { get; set; }
        string KeywordsHtmlMetaTag { get; set; }
    }
}