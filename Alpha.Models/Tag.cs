using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class Tag : Entity
    {
        [Range(1, 4)]
        public virtual int Size { get; set; }

        [Required]
        [StringLength(25)]
        public virtual string Title { get; set; }

        [StringLength(256)]
        public virtual string Description { get; set; }

        public virtual bool IsActive { get; set; }

        public virtual ISet<ArticleTag> ArticleTags { get; set; }

        public virtual ISet<ProjectTag> ProjectTags { get; set; }
    }
}
