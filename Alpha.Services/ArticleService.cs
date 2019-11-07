using Alpha.Models;
using Alpha.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Alpha.DataAccess;
using Alpha.DataAccess.Interfaces;
using Alpha.DataAccess.UnitOfWork;
using Alpha.Services.Interfaces;
using Alpha.ViewModels.Helper;
using Microsoft.EntityFrameworkCore;

namespace Alpha.Services
{
    public class ArticleService : BaseService<IArticleRepository, Article>, IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTagRepository _articleTagRepository;
        private IArticleTagService _articleTagService;

        private IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork uow, IArticleRepository articleRepository, IArticleTagRepository articleTagRepository)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _articleTagRepository = articleTagRepository;
            _articleTagService = new ArticleTagService(_articleTagRepository);
            _unitOfWork = uow;
        }

        
        public async Task<ArticleTagListViewModel> FilterByTagAsync(int? tagId, int articlePage, int pageSize)
        {
            var articleList = new List<ArticleViewModel>();
            var totalItems = 0;
            List<int> articleIdList = new List<int>();
            if (tagId != null)
            {
                articleIdList = _articleTagRepository.FindAll(t => t.TagId == tagId).Skip((articlePage - 1) * pageSize).Take(pageSize).Select(p => p.ArticleId).ToList();
                totalItems = _articleTagRepository.FindAll(p => p.TagId == tagId).Count();
            }
            else
            {
                articleIdList = _articleRepository.GetAll().Skip((articlePage - 1) * pageSize).Take(pageSize).Select(p => p.Id).ToList();
                totalItems = _articleRepository.GetAll().Count();
            }

            foreach (var articleId in articleIdList)
            {
                Article art = await _articleRepository.FindByIdAsync(articleId);
                var tags = _articleTagService.GetTagsByArticleId(articleId);
                articleList.Add(new ArticleViewModel
                {
                    Article = art,
                    Tags = tags,
                });
            }

            PagingInfo pInfo = new PagingInfo
            {
                PageSize = 3,
                CurrentPage = articlePage,
                ItemsPerPage = 3,
                TotalItems = totalItems
            };
            var result = new ArticleTagListViewModel
            {
                ArticleViewModelList = articleList,
                PagingInfo = pInfo,
                TagId = tagId
            };
            return result;
        }

        public virtual List<Tag> GetTagsByArticleId(int articleId)
        {
            return _articleTagService.GetTagsByArticleId(articleId);
        }

        public virtual async Task<ArticleViewModel> CreateInstanceOfArticleViewModel(int articleId)
        {
            var vm = new ArticleViewModel();
            vm.Article = await FindByIdAsync(articleId);
            vm.Tags = GetTagsByArticleId(articleId);
            return vm;
        }

        public virtual async Task<List<ArticleViewModel>> GetAllOfArticleViewModel()
        {
            var result = new List<ArticleViewModel>();
            List<int> articleIds = _articleTagService.GetAll().Select(c => c.ArticleId).Distinct().ToList();
            foreach (var id in articleIds)
            {
                result.Add(await CreateInstanceOfArticleViewModel(id));
            }
            return result;
        }

        public virtual async Task<int> InsertAsync(ArticleViewModel viewModel)
        {
            using (_unitOfWork)
            {
                var articleId = _unitOfWork.Article.AddOrUpdate(viewModel.Article);
                if (viewModel.Tags != null)
                    foreach (var tag in viewModel.Tags.Where(t => t.IsActive == true))
                    {
                        var at = new ArticleTag()
                        {
                            ArticleId = articleId,
                            TagId = tag.Id
                        };
                        _unitOfWork.ArticleTag.AddOrUpdate(at);
                    }

                return await _unitOfWork.CommitAsync();
            }
        }
    }
}
