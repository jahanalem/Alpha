using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class Comment : Entity
    {
        public Comment()
        {
        }

        public virtual int? ParentId { get; set; }
        public Comment Parent { get; set; }
        /// <summary>
        /// Replies:  جواب هایی که به نظر داده شده است.
        /// </summary>
        public virtual IList<Comment> Replies { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual int? ArticleId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int? UserId { get; set; }

        /// <summary>
        /// Description: توضیحات
        /// </summary>
        [StringLength(1024)]
        public virtual string Description { get; set; }

        /// <summary>
        /// LikeCounter: شمارنده لایک
        /// </summary>
        public virtual int? LikeCounter { get; set; }

        /// <summary>
        /// Article: این نظر مربوط به کدام مقاله است
        /// </summary>
        public virtual Article Article { get; set; }
    }
}
