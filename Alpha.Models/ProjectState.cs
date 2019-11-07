using System.Collections.Generic;

namespace Alpha.Models
{
    public class ProjectState : Entity
    {
        public ProjectState()
        {
            //this.Projects = new HashSet<Project>();
        }

        /// <summary>
        /// Title: عنوان
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// IsActive: فعال؟
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Projects: پروژه ها
        /// </summary>
        public virtual ISet<Project> Projects { get; set; }
    }
}
