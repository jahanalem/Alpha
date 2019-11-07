namespace Alpha.Models
{
    public class ArticleTag : Entity
    {
        //[Key, Column(Order = 0)]
        public virtual int TagId { get; set; }

        //[Key, Column(Order = 1)]
        public virtual int ArticleId { get; set; }
        public virtual Article Article { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual int? Extra { get; set; }
    }
}
