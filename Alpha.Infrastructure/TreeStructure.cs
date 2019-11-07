using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alpha.Models;

namespace Alpha.Infrastructure
{
    public static class TreeStructure
    {
        private static int row = 1;
        public static IList<Comment> BuildTree(this IList<Comment> source)
        {
            var groups = source.GroupBy(i => i.ParentId);

            var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

            if (roots.Count > 0)
            {
                var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
                for (int i = 0; i < roots.Count; i++)
                    AddChildren(roots[i], dict);
            }

            return roots;
        }

        private static void AddChildren(Comment node, IDictionary<int, List<Comment>> source)
        {
            if (source.ContainsKey(node.Id))
            {
                node.Replies = source[node.Id];
                for (int i = 0; i < node.Replies.Count; i++)
                    AddChildren(node.Replies[i], source);
            }
            else
            {
                node.Replies = new List<Comment>();
            }
        }

        public static string ShowComments(List<Comment> items)
        {
            StringBuilder output = new StringBuilder();
            output.Append("<ul>");
            var commentsWithNullParent = items.Where(p => p.ParentId == null).OrderByDescending(p=>p.CreatedDate).ToList();
            foreach (var item in commentsWithNullParent)
            {
                output.Append("<li>");
               
                output.Append($"<h5 id=\"commentNode_{item.Id}\"  class='mt-0'>Commenter Name</h5>");
                output.Append($"<div id=\"collapse_{item.Id}\" class=''>");
                output.Append($"<div class=\"card\" id=\"cardId_{item.Id}\">");
                output.Append(string.Format("<p>{0}</p>", item.Description));
                output.Append("</div>");
                string commentId = $"commentId_{item.Id}";
                output.Append($"<div class=\"comment-meta\" id=\"{commentId}\">");// open_div_comment-meta
                output.Append($"<span><input id=\"deleteC_{item.Id}_{item.ArticleId}\" type='submit' class='submitLink' value='delete'/></span>");
                string editCommentId = $"editC_{item.Id}_{item.ArticleId}";
                output.Append($"<span><input id=\"{editCommentId}\" type='submit' class='submitLink' value='edit'/></span>");
                output.Append($"<span>");
                string combinationId = $"replyC_{item.Id}_{item.ArticleId}";
                output.Append($"<a id=\"{combinationId}\" class=\"\" role=\"button\" data-toggle=\"collapse\" href=\"#replyComment_{item.Id}\" aria-expanded=\"false\" aria-controls=\"collapseExample\">reply</a>");
                output.Append($"</span>");
                output.Append($"<div class=\"collapse\" id=\"replyComment_{item.Id}\">");// open_div_commentReply
                output.Append("</div>");// close_div_commentReply
                output.Append("</div>");// close_div_comment-meta
                CreateComments(items, item, item.Id, output, row);
                output.Append("</div>");
                output.Append("</li>");
                //output.Append(result);
            }
            output.Append("</ul>");
            return output.ToString();
        }

        public static string CreateComments(List<Comment> items, Comment parentItem, int parentId, StringBuilder output, int row)
        {
            var count = items.Count(p => p.ParentId == parentId);
            if (count > 0)
            {
                output.Append("<ul>");
                foreach (var item in items.Where(p => p.ParentId == parentId).OrderByDescending(p=>p.CreatedDate).ToList())
                {
                    output.Append("<li>");
                    string collapseId = string.Format("collapse_{0}", item.Id);
                    string commentLink = $"<a id=\"commentNode_{item.Id}\" class=\"btn btn-link\" data-toggle=\"collapse\" href=\"#{collapseId}\" aria-expanded=\"True\" aria-controls=\"{collapseId}\">";
                    output.Append(commentLink);
                    string commentorName = "Jahan Alem";
                    string commentor = $"<span class=\"fa fa-plus\" aria-hidden=\"true\"> {commentorName}</span>";
                    output.Append(commentor);
                    output.Append("</a>");
                    //string str = string.Format("<h5 class=\"{0}\">Commenter Name</h5>", "mt-0");
                    //output.Append(str);
                    
                    output.Append($"<div class=\"collapse\" id=\"{collapseId}\">"); // open div collapse_
                    output.Append($"<div class=\"card\" id=\"cardId_{item.Id}\">");// open div cardId_
                    output.Append(string.Format("<p>{0}</p>", item.Description));
                    
                    output.Append("</div>"); // close div cardId_
                    output.Append($"<div class=\"comment-meta\" id=\"commentId_{item.Id}\">");// open_div_comment-meta
                    output.Append($"<span><input id=\"deleteC_{item.Id}_{item.ArticleId}\" type='submit' class='submitLink' value='delete'/></span>");
                    
                    output.Append($"<span><input id=\"editC_{item.Id}_{item.ArticleId}\" type='submit' class='submitLink' value='edit' /></span>");
                    output.Append($"<span>");
                    
                    output.Append($"<a id=\"replyC_{item.Id}_{item.ArticleId}\" class=\"\" role=\"button\" data-toggle=\"collapse\" href=\"#replyComment_{item.Id}\" aria-expanded=\"false\" aria-controls=\"collapseExample\">reply</a>");
                    output.Append($"</span>");
                    output.Append($"<div class=\"collapse\" id=\"replyComment_{item.Id}\">");// open_div_commentReply
                    output.Append("</div>");// close_div_commentReply
                    output.Append("</div>");// close_div_comment-meta
                    row = row + 1;
                    CreateComments(items, item, item.Id, output, row);
                    output.Append("</div>");
                    output.Append("</li>");
                }
                output.Append("</ul>");
            }
            return output.ToString();
        }

        private static void GenerateHtmlCodes(StringBuilder output)
        {
            output.Append("<li>");

        }

    }
}
