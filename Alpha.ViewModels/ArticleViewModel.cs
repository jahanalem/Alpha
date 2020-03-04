using System.Collections.Generic;
using Alpha.Models;

namespace Alpha.ViewModels
{
    public class ArticleViewModel : BaseViewModel
    {
        public Article Article { get; set; }
        public List<Tag> Tags { get; set; }
        public List<Tag> AllOfTags { get; set; }
    }
}
