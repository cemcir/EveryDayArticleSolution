using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveryDayArticle.DataAccess.Concreate
{
    public class ArticleRepository:Repository<Article>,IArticleRepository
    {
        private ArticleContext _appDbContext { get => _context as ArticleContext; }

        public ArticleRepository(ArticleContext context) : base(context) { }

        public void AddArticle(Article entity)
        {
            _appDbContext.Articles.Add(entity);
            _appDbContext.SaveChanges();
            _appDbContext.Authors.Add(new Author() {

            });
            int maxId = _appDbContext.Articles.Max(x => x.Id);
        }

        public DateTime GetMaxArticleDate()
        {
            int maxId= _appDbContext.Articles.Max(x => x.Id);

            Article article = _appDbContext.Articles.Where(x => x.Id == maxId).FirstOrDefault();

            return article.PublishDate;
        }

        public List<Article> GetArticleList()
        {
            return _appDbContext.Articles.Include(c => c.Category).ToList();
        }

        public Article GetArticleOfDay()
        {
            return _appDbContext.Articles.Where(article => article.PublishDate == DateTime.Today).FirstOrDefault();
        }

        public Article GetArticleById(int Id)
        {
            return _appDbContext.Articles.Include(article => article.Category).Where(article => article.Id == Id).FirstOrDefault();
        }
    }
}
