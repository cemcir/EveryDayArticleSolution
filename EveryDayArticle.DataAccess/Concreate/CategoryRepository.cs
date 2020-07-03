using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveryDayArticle.DataAccess.Concreate
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ArticleContext _appDbContext { get => _context as ArticleContext; }

        public CategoryRepository(ArticleContext context) : base(context) { }

        public Category GetByCategoryName(string name)
        {
            return _appDbContext.Categories.Where(c => c.Name == name).FirstOrDefault();
        }

        public List<Category> GetCategoryList()
        {
            return _appDbContext.Categories.Include(article => article.Articles).ToList();
        }

        public Category GetArticleByName(string name)
        {
            return _appDbContext.Categories.Include(category => category.Articles).Where(category => category.Name == name).FirstOrDefault();
        }
    }
}
