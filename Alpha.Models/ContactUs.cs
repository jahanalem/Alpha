namespace Alpha.Models
{
    public class ContactUs : Entity
    {
        /// <summary>
        /// FirstName: نام
        /// </summary>
        public virtual string FirstName { get; set; }

        /// <summary>
        /// LastName: نام خانوادگی
        /// </summary>
        public virtual string LastName { get; set; }

        /// <summary>
        /// Email: ایمیل
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// Tel: تلفن
        /// </summary>
        public virtual string Tel { get; set; }

        /// <summary>
        /// Title: عنوان
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// Description: توضیحات
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// IsActive: فعال؟
        /// </summary>
        public virtual bool? IsActive { get; set; }
    }
}
