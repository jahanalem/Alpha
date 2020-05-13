using System;
using System.Collections.Generic;
using System.Text;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Alpha.Services.Interfaces;

namespace Alpha.Services
{
    public class AboutUsService : BaseService<IAboutUsRepository, AboutUs>, IAboutUsService
    {
        public AboutUsService(IAboutUsRepository obj) : base(obj)
        {
        }
    }
}
