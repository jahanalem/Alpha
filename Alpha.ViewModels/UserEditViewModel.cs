using System;
using System.ComponentModel.DataAnnotations;

namespace Alpha.ViewModels
{
    public class UserEditViewModel
    {
        public virtual int Id { get; set; }
        public virtual bool IsActive { get; set; } = true;
        public virtual string UserName { get; set; }

        [EmailAddress]
        public virtual string Email { get; set; }
        public virtual string PhoneNumber { get; set; }

        public virtual DateTimeOffset? BirthDate { get; set; }
        [StringLength(450)]
        public virtual string FirstName { get; set; }
        [StringLength(450)]
        public virtual string LastName { get; set; }
        [StringLength(450)]
        public virtual string PhotoFileName { get; set; }
        public virtual bool IsEmailPublic { get; set; }
        public virtual string Location { set; get; }
    }
}
