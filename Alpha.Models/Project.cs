using System;
using System.Collections.Generic;
using Alpha.Models.Identity;

namespace Alpha.Models
{
    public class Project : Entity
    {
        public Project()
        {
            //this.AttachmentFiles = new HashSet<AttachmentFile>();
            //this.ProjectTags = new HashSet<ProjectTag>();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int? ProjectStateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int? TagId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Summary { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool? IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? FinishDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ISet<AttachmentFile> AttachmentFiles { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ProjectState ProjectState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ISet<ProjectTag> ProjectTags { get; set; }
    }
}
