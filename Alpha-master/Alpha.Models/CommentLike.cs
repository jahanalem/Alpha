using Alpha.Models.Identity;

namespace Alpha.Models
{
    public class CommentLike : Entity
    {
        public virtual int? CommentId { get; set; }
        public virtual int? UserId { get; set; }

        /// <summary>
        /// IsLiked: 
        /// </summary>
        public virtual bool? IsLiked { get; set; }

        /// <summary>
        /// Comment: 
        /// </summary>
        public virtual Comment Comment { get; set; }

        /// <summary>
        /// UserMemberShip:
        /// </summary>
        public virtual User User { get; set; }
    }
}
