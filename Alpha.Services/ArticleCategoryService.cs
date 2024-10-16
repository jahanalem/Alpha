using Alpha.DataAccess.UnitOfWork;
using Alpha.Models;
using Alpha.Services.Interfaces;
using Alpha.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpha.Services
{
    public class ArticleCategoryService : BaseService<ArticleCategory>, IArticleCategoryService
    {
        public ArticleCategoryService(IUnitOfWork unitOfWork, ILogger<ArticleCategoryService> logger)
            : base(unitOfWork, logger)
        {
        }

        public async Task<ArticleCategory> CreateOrUpdateAsync(ArticleCategoryViewModel model)
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

            return await _unitOfWork.Repository<ArticleCategory>().AddOrUpdateAsync(ac);
        }

        public async Task<List<ArticleCategory>> GetSelfAndDescendants(int id)
        {
            var result = new List<ArticleCategory>();
            var root = await _unitOfWork.Repository<ArticleCategory>().GetByCriteria(c => c.Id == id && c.IsActive).SingleOrDefaultAsync();
            result.Add(root);
            return await GetDescendantsRecursive(id, result);
        }

        private async Task<List<ArticleCategory>> GetDescendantsRecursive(int id, List<ArticleCategory> result)
        {
            var node = await _unitOfWork.Repository<ArticleCategory>().GetByCriteria(c => c.Id == id && c.IsActive).SingleOrDefaultAsync();
            if (node != null)
            {
                var list = await _unitOfWork.Repository<ArticleCategory>().GetByCriteria(c => c.ParentId == node.Id && c.IsActive).ToListAsync();
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

        public async Task<List<ArticleCategory>> GetAllAsync()
        {
            return await _unitOfWork.Repository<ArticleCategory>().GetAll().ToListAsync();
        }

        public async Task<ArticleCategory> GetByIdAsync(int id)
        {
            return await _unitOfWork.Repository<ArticleCategory>().GetByIdAsync(id);
        }

        public async Task<List<ArticleCategory>> GetByIsActiveAsync(bool isActive)
        {
            return await _unitOfWork.Repository<ArticleCategory>().GetByCriteria(c => c.IsActive == isActive).ToListAsync();
        }

        public async Task DeleteAsync(ArticleCategory articleCategory)
        {
            await _unitOfWork.Repository<ArticleCategory>().DeleteAsync(articleCategory);
            await _unitOfWork.CompleteAsync();
        }
    }
}
