namespace Alpha.Models
{
    public class ProjectTag : BaseEntity
    {
        //[Key, Column(Order = 0)]
        public int TagId { get; set; }
        //[Key, Column(Order = 1)]
        public int ProjectId { get; set; }
        public int? Extra { get; set; }
        public virtual Project Project { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
