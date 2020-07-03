using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.DataAccess.Abstract
{
    public interface IArticleRepository:IRepository<Article>
    {
        void AddArticle(Article entity);

        DateTime GetMaxArticleDate();

        List<Article> GetArticleList();

        Article GetArticleOfDay();

        Article GetArticleById(int Id);
    }
}
