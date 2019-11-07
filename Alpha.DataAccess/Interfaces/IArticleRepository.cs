using System.Collections.Generic;
using System.Threading.Tasks;
using Alpha.Models;
using Alpha.ViewModels;

namespace Alpha.DataAccess.Interfaces
{
    public interface IArticleRepository : IRepository<Article>
    {
        void Test();
    }
}
