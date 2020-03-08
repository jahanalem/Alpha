using System.Collections.Generic;
using Alpha.Models;
//using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Alpha.ViewModels
{
    public class ArticleViewModel : BaseViewModel
    {
        //[BindRequired]// Microsoft.AspNetCore.Mvc.Core
        public Article Article { get; set; }
        
        /// <summary>
        /// Just only related tags
        /// </summary>
        //[BindRequired]
        public List<Tag> Tags { get; set; }

        /// <summary>
        /// Related tags in the set of all tags
        /// </summary>
        //[BindRequired]
        public List<Tag> AllTags { get; set; }
    }
}
