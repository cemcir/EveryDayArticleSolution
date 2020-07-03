using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Abstract
{
    public interface IArticleService:IService<Article>,IValidate<Article>
    {
        BaseResponse<Article> Create(Article entity);

        BaseResponse<Article> Edit(Article entity);

        DateTime GetMaxArticleDate();

        BaseResponse<List<Article>> GetArticleList();

        BaseResponse<Article> GetArticleById(int Id);

        BaseResponse<Article> GetArticleOfDay();

        bool isSelectedCategory(int categoryId);

        bool checkTextLength(string statement);

        bool checkTitleLength(string statement);

        bool isTitleValid(string title);
    }
}
