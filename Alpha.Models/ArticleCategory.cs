using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class ArticleCategory : Entity
    {
        [Required]
        [StringLength(26)]
        public string Title { get; set; }

        [Display(Name = "Active")]
        public virtual bool IsActive { get; set; }
        public virtual int? ParentId { get; set; }
        public ArticleCategory Parent { get; set; }
        public virtual IList<Article> Articles { get; set; }
    }
}
