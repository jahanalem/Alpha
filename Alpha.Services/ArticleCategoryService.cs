using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class ArticleCategoryService : BaseService<IArticleCategoryRepository, ArticleCategory>, IArticleCategoryService
    {
        private IArticleCategoryRepository _articleCategoryRepository;
        //private IUnitOfWork _unitOfWork;
        public ArticleCategoryService(IArticleCategoryRepository articleCategoryRepository, IUnitOfWork unitOfWork)
            : base(articleCategoryRepository)
        {
            _articleCategoryRepository = articleCategoryRepository;
            //_unitOfWork = unitOfWork;
        }

        public async Task<int> CreateOrUpdateAsync(ArticleCategoryViewModel model)
        {
            if (model.Category.ParentId <= 0)
            {
                model.Category.ParentId = null;
            }
            var ac = new ArticleCategory
            {
                Id = model.Category.Id,
                Title = model.Category.Title.Trim(),
                IsActive = model.Category.IsActive,
                ParentId = model.Category.ParentId
            };
            return await base.AddOrUpdateAsync(ac);
        }
    }
}
