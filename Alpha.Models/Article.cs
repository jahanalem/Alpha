using Alpha.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    public class Article : BaseEntity, IHtmlMetaTags
    {
        public Article()
        {

        }
        public virtual int? UserId { get; set; }

        [Display(Name = "Category")]
        public virtual int? ArticleCategoryId { get; set; }

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

        public virtual string DescriptionAsPlainText { get; set; }

        /// <summary>
        /// RateCounter: رتبه
        /// </summary>
        [Range(0, 10)]
        [Display(Name = "Rating")]
        [Column(TypeName = "decimal(18, 2)")]
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


        [Display(Name = "Publish")]
        public virtual bool IsPublished { get; set; } = true;

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

        [Display(Name = "Title Html Meta Tag"), StringLength(70)]
        public virtual string TitleHtmlMetaTag { get; set; }

        [Display(Name = "Description Html Meta Tag"), StringLength(300)]
        public virtual string DescriptionHtmlMetaTag { get; set; }
        public virtual string KeywordsHtmlMetaTag { get; set; }
    }
}