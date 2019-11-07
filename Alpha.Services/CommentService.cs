using System;
using System.Collections.Generic;
using System.Text;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.Services.Interfaces;

namespace Alpha.Services
{
    public class CommentService : BaseService<ICommentRepository, Comment>, ICommentService
    {
        public CommentService(ICommentRepository obj) : base(obj)
        {
        }
    }
}
