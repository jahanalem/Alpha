using System.ComponentModel.DataAnnotations;

namespace Alpha.Models
{
    public class AttachmentFile : Entity
    {
        public virtual int? ArticleId { get; set; }
        public virtual int? ProjectId { get; set; }

        /// <summary>
        /// Name: نام فایل
        /// </summary>
        [StringLength(512)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Size: اندازه فایل
        /// </summary>
        public virtual long? Size { get; set; }

        /// <summary>
        /// Extension: فرمت فایل
        /// </summary>
        [StringLength(10)]
        public virtual string Extension { get; set; }

        /// <summary>
        /// Path: مسیر ذخیره شده فایل
        /// </summary>
        [StringLength(1024)]
        public virtual string Path { get; set; }

        /// <summary>
        /// IsActive: فعال؟
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// Article: این فایل متعلق به مقاله ... است
        /// </summary>
        public virtual Article Article { get; set; }

        //public virtual Project Project { get; set; }
    }
}
