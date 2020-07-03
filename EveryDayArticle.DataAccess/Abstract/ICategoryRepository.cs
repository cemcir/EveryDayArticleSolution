using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.DataAccess.Abstract
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByCategoryName(string name);

        List<Category> GetCategoryList();

        Category GetArticleByName(string name);
    }
}
