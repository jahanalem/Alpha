using Alpha.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    public class BaseEntity : IBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int Id { get; set; }

        [Display(Name = "Created Date")]
        public virtual DateTimeOffset? CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public virtual DateTimeOffset? ModifiedDate { get; set; }
    }
}
