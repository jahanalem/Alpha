using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    public class ArticleTag : BaseEntity
    {
        //[Key, Column(Order = 0)]
        [ForeignKey("Tag")]
        public virtual int TagId { get; set; }

        //[Key, Column(Order = 1)]
        [ForeignKey("Article")]
        public virtual int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual int? Extra { get; set; }
    }
}
