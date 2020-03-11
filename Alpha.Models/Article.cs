using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class Article : Entity
    {
        public Article()
        {

        }
        public virtual int? UserId { get; set; }

        /// <summary>
        /// Title = عنوان
        /// </summary>
        [Required]
        [StringLength(256)]
        public virtual string Title { get; set; }

        /// <summary>
        /// Summary = خلاصه
        /// </summary>
        [StringLength(256)]
        [Required]
        public virtual string Summary { get; set; }

        /// <summary>
        /// Description: توضیحات
        /// </summary>
        [Required]
        public virtual string Description { get; set; }

        /// <summary>
        /// RateCounter: رتبه
        /// </summary>
        [Range(0, 10)]
        [Display(Name = "Rating")]
        public virtual decimal? RateCounter { get; set; } = 0;


        /// <summary>
        /// LikeCounter: شمارنده لایک 
        /// </summary>
        [Display(Name = "Like")]
        public virtual int? LikeCounter { get; set; }

        /// <summary>
        /// IsActive: فعال؟
        /// </summary>
        [Display(Name = "Active")]
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// IsActiveNewComment: آیا امکان گذاشتن نظر جدید وجود دارد؟
        /// </summary>
        [Display(Name = "New Comment")]
        public virtual bool IsActiveNewComment { get; set; }

        /// <summary>
        /// Comments: ارتباط با جدول نظرات
        /// </summary>
        public virtual IList<Comment> Comments { get; set; }

        /// <summary>
        /// Ratings: ارتباط با جدول رتبه
        /// </summary>
        public virtual ISet<Rating> Ratings { get; set; }

        /// <summary>
        /// AttachmentFiles: ارتباط با فایل های الحاقی
        /// </summary>
        public virtual IList<AttachmentFile> AttachmentFiles { get; set; }

        /// <summary>
        /// ArticleTags:  جدول واسط مقاله-تگ
        /// </summary>
        public virtual IList<ArticleTag> ArticleTags { get; set; }

        /// <summary>
        /// ArticleLikes: جدول واسط مقاله-لایک
        /// </summary>
        public virtual ISet<ArticleLike> ArticleLikes { get; set; }
    }
}
