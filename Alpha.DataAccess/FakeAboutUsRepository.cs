using System.Collections.Generic;
using System.Linq;
using Alpha.Models;

namespace Alpha.DataAccess
{
    public class FakeAboutUsRepository 
    {
        public IQueryable<AboutUs> AboutUs => new List<AboutUs>{
            new AboutUs{ IsActive=true, Description="Hello" }
        }.AsQueryable<AboutUs>();
    }
}
