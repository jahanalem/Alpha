using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Alpha.DataAccess
{
    public class ArticleCategoryRepository : Repository<ArticleCategory>, IArticleCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public ArticleCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
