using Alpha.Infrastructure;
using Alpha.Models;
using System.Collections.Generic;

namespace Alpha.ViewModels
{
    public class RecursionComment
    {
        public List<Comment> Comments { get; set; }
        public int? ParentId { get; set; }

        public CurrentUser CurrentUserInfo { get; set; }
    }
}
