using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alpha.DataAccess.Interfaces;
using Alpha.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace Alpha.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public ApplicationDbContext Context { get; }
        private readonly Dictionary<Type, object> _repositories;
        public UnitOfWork(ApplicationDbContext context)
        {
            Context = context;
            _repositories = new Dictionary<Type, object>();
        }


        #region Model Repository

        private IAboutUsRepository _aboutUs;
        public IAboutUsRepository AboutUs
        {
            get
            {
                if (_aboutUs == null) { this._aboutUs = new AboutUsRepository(Context); }

                return _aboutUs;
            }
        }

        private IArticleRepository _article;
        public IArticleRepository Article
        {
            get
            {
                if (_article == null) { this._article = new ArticleRepository(Context); }

                return _article;
            }
        }

        private IArticleLikeRepository _articleLike;
        public IArticleLikeRepository ArticleLike
        {
            get
            {
                if (_articleLike == null) { this._articleLike = new ArticleLikeRepository(Context); }

                return _articleLike;
            }
        }

        private IArticleTagRepository _articleTag;
        public IArticleTagRepository ArticleTag
        {
            get
            {
                if (_articleTag == null) { this._articleTag = new ArticleTagRepository(Context); }

                return _articleTag;
            }
        }

        private IAttachmentFileRepository _attachmentFile;
        public IAttachmentFileRepository AttachmentFile
        {
            get
            {
                if (_attachmentFile == null) { this._attachmentFile = new AttachmentFileRepository(Context); }

                return _attachmentFile;
            }
        }

        private ICommentRepository _comment;
        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null) { this._comment = new CommentRepository(Context); }

                return _comment;
            }
        }

        private ICommentLikeRepository _commentLike;
        public ICommentLikeRepository CommentLike
        {
            get
            {
                if (_commentLike == null) { this._commentLike = new CommentLikeRepository(Context); }

                return _commentLike;
            }
        }

        private IProjectRepository _project;
        public IProjectRepository Project
        {
            get
            {
                if (_project == null) { this._project = new ProjectRepository(Context); }

                return _project;
            }
        }

        private IProjectStateRepository _projectState;
        public IProjectStateRepository ProjectState
        {
            get
            {
                if (_projectState == null) { this._projectState = new ProjectStateRepository(Context); }

                return _projectState;
            }
        }

        private IProjectTagRepository _projectTag;
        public IProjectTagRepository ProjectTag
        {
            get
            {
                if (_projectTag == null) { this._projectTag = new ProjectTagRepository(Context); }

                return _projectTag;
            }
        }

        private IRatingRepository _rating;
        public IRatingRepository Rating
        {
            get
            {
                if (_rating == null) { this._rating = new RatingRepository(Context); }

                return _rating;
            }
        }


        private ITagRepository _tag;
        public ITagRepository Tag
        {
            get
            {
                if (_tag == null) { this._tag = new TagRepository(Context); }

                return _tag;
            }
        }

        #endregion

        public void Dispose()
        {
            Context.Dispose();
            GC.SuppressFinalize(this); //https://stackoverflow.com/questions/151051/when-should-i-use-gc-suppressfinalize
        }

        public async void DisposeAsync()
        {
            await Context.DisposeAsync();
            GC.SuppressFinalize(this);
        }

        public int Commit() => Context.SaveChanges();

        public async Task<int> CommitAsync() => await Context.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await Context.Database.BeginTransactionAsync();
        }
        public IDbContextTransaction BeginTransaction()
        {
            return Context.Database.BeginTransaction();
        }
        /*private IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity
        {
            // Checks if the Dictionary Key contains the Model class
            if (_repositories.Keys.Contains(typeof(TEntity)))
            {
                // Return the repository for that Model class
                return _repositories[typeof(TEntity)] as IRepository<TEntity>;
            }

            // If the repository for that Model class doesn't exist, create it
            Repository<TEntity> repository = new Repository<TEntity>(Context);

            // Add it to the dictionary
            _repositories.Add(typeof(TEntity), repository);

            return repository;
        }*/

        //public TRepository RepositoryFactory<TRepository, TEntity>() where TRepository : Repository<TEntity> where TEntity : Entity
        //{
        //    // Checks if the Dictionary Key contains the Model class
        //    if (_repositories.Keys.Contains(typeof(TRepository)))
        //    {
        //        // Return the repository for that Model class
        //        return _repositories[typeof(TRepository)] as TRepository;
        //    }

        //    // If the repository for that Model class doesn't exist, create it
        //    Object[] args = { Context };
        //    var repository = Activator.CreateInstance(typeof(TRepository), args) as TRepository;

        //    // Add it to the dictionary
        //    _repositories.Add(typeof(TRepository), repository);

        //    return repository;
        //}
    }
}
