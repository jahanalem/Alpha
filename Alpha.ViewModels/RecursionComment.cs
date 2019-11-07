using System.Collections.Generic;
using Alpha.Models;

namespace Alpha.ViewModels
{
    public class RecursionComment
    {
        public List<Comment> Comments { get; set; }
        public int? ParentId { get; set; }
    }
}
