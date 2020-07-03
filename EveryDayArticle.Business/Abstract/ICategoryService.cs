using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Abstract
{
    public interface ICategoryService : IService<Category>,IValidate<Category>
    {
        BaseResponse<Category> GetByCategoryName(string name);

        BaseResponse<Category> Create(Category entity);

        BaseResponse<Category> Delete(Category entity);

        BaseResponse<Category> Edit(Category entity);

        BaseResponse<List<Category>> GetCategoryList();

        BaseResponse<Category> GetArticleByName(string name);

        bool isNameValid(string statement);

        bool checkNameLength(string statement);
    }
}
