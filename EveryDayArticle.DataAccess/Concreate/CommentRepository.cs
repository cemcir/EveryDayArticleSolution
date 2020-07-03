using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveryDayArticle.DataAccess.Concreate
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private ArticleContext _appDbContext { get => _context as ArticleContext; }

        public CommentRepository(ArticleContext context) : base(context) { }

        public int GetCommentCountById(int Id)
        {
            var comments= _appDbContext.Comments.Where(comment => comment.ArticleId == Id).ToList();
            return comments.Count;
        }

        public List<Comment> GetCommentsById(int Id) {
            return _appDbContext.Comments.Where(comment => comment.ArticleId == Id).ToList();
        }
    }
}
