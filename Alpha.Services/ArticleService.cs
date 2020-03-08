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
using Microsoft.EntityFrameworkCore.Storage;

namespace Alpha.Services
{
    public class ArticleService : BaseService<IArticleRepository, Article>, IArticleService
    {
        private IArticleRepository _articleRepository;
        private IArticleTagRepository _articleTagRepository;
        private IArticleTagService _articleTagService;
        private ITagRepository _tagRepository;
        private IUnitOfWork _unitOfWork;

        public ArticleService(IUnitOfWork uow, IArticleRepository articleRepository, IArticleTagRepository articleTagRepository, ITagRepository tagRepository)
            : base(articleRepository)
        {
            _articleRepository = articleRepository;
            _articleTagRepository = articleTagRepository;
            _articleTagService = new ArticleTagService(_articleTagRepository);
            _tagRepository = tagRepository;
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

        public virtual async Task<ArticleViewModel> GetArticleById(int articleId)
        {
            var vm = new ArticleViewModel();
            vm.Article = await FindByIdAsync(articleId);
            vm.Tags = GetTagsByArticleId(articleId);
            //vm.AllOfTags = _tagRepository.GetAll().Where(c => c.IsActive == true).ToList();
            return vm;
        }

        /// <summary>
        /// IsActive property is "true" in general set
        /// </summary>
        /// <param name="tagList">return related tags along with the general set tags</param>
        /// <returns></returns>
        public virtual List<Tag> SpecifyRelatedTagsInTheGeneralSet(List<Tag> tagList)
        {
            List<Tag> allAvailableTags = _tagRepository.GetAll().Where(c => c.IsActive == true).ToList();
            for (int t = 0; t < allAvailableTags.Count; t++)
            {
                allAvailableTags[t].IsActive = false;
            }
            foreach (var tag in tagList)
            {
                for (int t = 0; t < allAvailableTags.Count; t++)
                {
                    if (allAvailableTags[t].Id == tag.Id)
                    {
                        allAvailableTags[t].IsActive = true;
                    }
                }
            }
            return allAvailableTags;
        }

        public virtual async Task<List<ArticleViewModel>> GetAllOfArticleViewModel()
        {
            var result = new List<ArticleViewModel>();
            List<int> articleIds = _articleTagService.GetAll().Select(c => c.ArticleId).Distinct().ToList();
            foreach (var id in articleIds)
            {
                result.Add(await GetArticleById(id));
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns>It returns Id from new Article. If operation was not successfully it returns -1.</returns>
        public virtual async Task<int> InsertAsync(ArticleViewModel viewModel)
        {
            using (var transaction = _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var articleId = _unitOfWork.Article.AddOrUpdate(viewModel.Article);
                    if (viewModel.AllTags != null)
                        foreach (var tag in viewModel.AllTags.Where(t => t.IsActive == true))
                        {
                            var at = new ArticleTag()
                            {
                                ArticleId = articleId,
                                TagId = tag.Id
                            };
                            _unitOfWork.ArticleTag.AddOrUpdate(at);
                        }

                    await transaction.Result.CommitAsync();
                    return articleId;
                }
                catch (Exception e)
                {
                    await transaction.Result.RollbackAsync();
                    return -1;
                }
            }
        }
    }
}
