using Alpha.Models;
using System.Collections.Generic;
using System.Linq;

namespace Alpha.Infrastructure
{
    public static class StructureOfCommentList
    {
        public static List<Comment> Children(this Comment comment, List<Comment> list)
        {
            return list.Where(p => p.ParentId == comment.Id).ToList();
        }
        public static List<Comment> Children(this List<Comment> list, Comment comment)
        {
            return list.Where(p => p.ParentId == comment.Id).ToList();
        }

        public static List<Comment> Children(this List<Comment> list, int? id)
        {
            return list.Where(p => p.ParentId == id).ToList();
        }

        public static string AddDoubleQuotes(this string value)
        {
            return "\"" + value + "\"";
        }

        //public static void BuildTree(this List<Comment> items)
        //{
        //    items.ForEach(i => i.Replies = items.Where(ch => ch.ParentId == i.Id).ToList());
        //}
    }
}
