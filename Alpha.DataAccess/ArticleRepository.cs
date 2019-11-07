using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Alpha.DataAccess
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        //private readonly IArticleRepository _repository;
        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Test()
        {

        }
    }
}
