using Alpha.Models.Identity;

namespace Alpha.Models
{
    public class ArticleLike : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual int? ArticleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool? IsLiked { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Article Article { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual User User { get; set; }
    }
}
