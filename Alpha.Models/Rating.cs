using Alpha.Models.Identity;

namespace Alpha.Models
{
    public class Rating : Entity
    {
        public virtual int? UserId { get; set; }
        public virtual int? ArticleId { get; set; }

        /// <summary>
        /// IsLock: آیا امکان رتبه دادن بسته شده است
        /// </summary>
        public virtual bool? IsLock { get; set; }

        /// <summary>
        /// IsActive: فعال؟
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// Rate: رتبه
        /// </summary>
        public virtual decimal? Rate { get; set; }

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
