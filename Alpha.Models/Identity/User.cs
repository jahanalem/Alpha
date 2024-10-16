﻿using Alpha.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Alpha.Models.Identity
{
    public class User : IdentityUser<int>, IBaseEntity
    {
        [StringLength(450)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(450)]
        [Required]
        public string LastName { get; set; }

        [NotMapped]
        public string DisplayName
        {
            get
            {
                var displayName = $"{FirstName} {LastName}";
                return string.IsNullOrWhiteSpace(displayName) ? UserName : displayName;
            }
        }

        [StringLength(450)]
        public string PhotoFileName { get; set; }

        public DateTimeOffset? BirthDate { get; set; }

        public bool IsEmailPublic { get; set; }

        public string Location { set; get; }

        public string IpAddress { set; get; }

        public string LoginProvider { get; set; }

        public bool IsActive { get; set; } = true;

        [NotMapped]
        [StringLength(maximumLength: 8, MinimumLength = 4, ErrorMessage = "The password must be between 4 and 8 character in length.")]
        public string Password { get; set; }

        public virtual ICollection<CommentLike> CommentLikes { get; set; }
        //public virtual ISet<CommentReply> CommentReplies { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<ArticleLike> ArticleLikes { get; set; }
        public virtual ICollection<Article> Articles { get; set; }

        // additional properties will go here

        public virtual ICollection<UserToken> UserTokens { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }

        public virtual ICollection<UserLogin> Logins { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }
    }
}
