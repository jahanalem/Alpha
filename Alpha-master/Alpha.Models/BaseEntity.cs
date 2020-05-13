using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Alpha.Models
{
    public class BaseEntity<TId>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TId Id { get; set; }

        [Display(Name = "Created Date")]
        public virtual DateTime? CreatedDate { get; set; }

        [Display(Name = "Modified Date")]
        public virtual DateTime? ModifiedDate { get; set; }
    }
}
