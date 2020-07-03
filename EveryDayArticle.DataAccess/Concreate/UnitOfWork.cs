using EveryDayArticle.DataAccess.Abstract;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.DataAccess.Concreate
{   
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ArticleContext _context;
        private IDbContextTransaction transaction;
        private CategoryRepository _categoryRepository;
        private ArticleRepository _articleRepository;
        private CommentRepository _commentRepository;
        private LikedRepository _likedRepository;

        public ICategoryRepository Categories => _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IArticleRepository Articles => _articleRepository = _articleRepository ?? new ArticleRepository(_context);

        public ICommentRepository Comments => _commentRepository =
            _commentRepository ?? new CommentRepository(_context);

        public ILikedRepository Likeds => _likedRepository = _likedRepository ?? new LikedRepository(_context);

        public UnitOfWork(ArticleContext appDbContext) {
            _context = appDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void BeginTransaction()
            {
                transaction = this._context.Database.BeginTransaction();
            }

        public void TransactionCommit()
        {
            transaction.Commit();
        }

        public void RolBack()
            {
                transaction.Rollback();
            }
    }
   
}
