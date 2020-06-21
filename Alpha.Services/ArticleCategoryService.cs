using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<ArticleCategory>> GetSelfAndDescendants(int id)
        {
            var result = new List<ArticleCategory>();
            var root = await _articleCategoryRepository.FetchByCriteria(c => c.Id == id && c.IsActive).SingleOrDefaultAsync();
            result.Add(root);
            return await GetDescendantsRecursive(id, result);
        }

        private async Task<List<ArticleCategory>> GetDescendantsRecursive(int id, List<ArticleCategory> result)
        {
            var node = await _articleCategoryRepository.FetchByCriteria(c => c.Id == id && c.IsActive).SingleOrDefaultAsync();
            if (node != null)
            {
                //result.Add(node);
                var list = await _articleCategoryRepository.FetchByCriteria(c => c.ParentId == node.Id && c.IsActive).ToListAsync();
                foreach (var n in list)
                {
                    result.Add(n);
                    if (n.ParentId != null)
                    {
                        await GetDescendantsRecursive(n.Id, result);
                    }
                }
            }
            return result;
        }
    }
}
