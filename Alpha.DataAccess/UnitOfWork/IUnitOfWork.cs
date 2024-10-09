using Alpha.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Alpha.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }
        int Commit();
        Task<int> CommitAsync();
        void DisposeAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        IDbContextTransaction BeginTransaction();

        IAboutUsRepository AboutUs { get; }
        IArticleRepository Article { get; }
        IArticleLikeRepository ArticleLike { get; }
        IArticleTagRepository ArticleTag { get; }
        IAttachmentFileRepository AttachmentFile { get; }
        ICommentRepository Comment { get; }
        ICommentLikeRepository CommentLike { get; }
        IProjectRepository Project { get; }
        IProjectStateRepository ProjectState { get; }
        IProjectTagRepository ProjectTag { get; }
        IRatingRepository Rating { get; }
        ITagRepository Tag { get; }
        //IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity;

        //TRepository RepositoryFactory<TRepository, TEntity>() where TRepository : Repository<TEntity> where TEntity : Entity;
    }
}