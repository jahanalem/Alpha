using System.Collections.Generic;
using Alpha.Models;

namespace Alpha.ViewModels
{
    public class ArticleViewModel : BaseViewModel
    {
        public Article Article { get; set; }

        /// <summary>
        /// Just only related tags
        /// </summary>
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Related tags in the set of all tags
        /// </summary>
        public List<Tag> AllTags { get; set; }
    }
}
