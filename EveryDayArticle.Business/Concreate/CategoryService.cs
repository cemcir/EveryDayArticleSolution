using EveryDayArticle.Business.Abstract;
using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace EveryDayArticle.Business.Concreate
{
    public class CategoryService : Service<Category>, ICategoryService
    {
        public Message message { get; set; }

        public CategoryService(IUnitOfWork unitOfWork, IRepository<Category> repository,Message message) : base(unitOfWork, repository) {

            this.message = message;
        }

        public bool checkLength(string statement)
        {
            if (statement.Length < 3) {
                return true;
            }
            return false;
        }

        public BaseResponse<Category> Create(Category entity) {
            if (Validate(entity)) {
                entity.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Name);
                var response = this.Add(entity);
                return response;
            }
            return new BaseResponse<Category>(new MessageCode().OperationFailed);
        }

        public BaseResponse<Category> GetByCategoryName(string name) {
            try {
                Category category = this._unitOfWork.Categories.GetByCategoryName(name);
                if (category != null) {
                    return new BaseResponse<Category>(category);
                }
                return new BaseResponse<Category>(new MessageCode().NoEntity);
            }
            catch (Exception) {
                return new BaseResponse<Category>(new MessageCode().ErrorCreated);
            }
        }

        public bool Validate(Category entity) {
            if (isEmpty(entity.Name)==true) {
                message.Messages.Add("Kategori Girmelisiniz");
                message.Css = "danger";
                return false;
            }
            if (isNameValid(entity.Name)==false) {
                message.Messages.Add("Yanlış Giriş");
                message.Css = "danger";
                return false;
            }
            if (checkLength(entity.Name)) {
                message.Messages.Add("Kategori Uzunluğu 3 Karakterden Az Olamaz");
                message.Css = "danger";
                return false;
            }
            return true;
        }

        public bool isNameValid(string statement)
        {
            char[] array = statement.ToCharArray();
            Regex regex = new Regex(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$");
            if (regex.IsMatch(statement) == false || array[0] == ' ') {
                return false;
            }
            return true;
        }

        public bool isValid(string statement)
        {
            /*
            Regex regex = new Regex(@"^[A-Za-z ]$");
            if (regex.IsMatch(statement)==false) {
                return false;
            }
            return true;
            */
            //Regex regex = new Regex(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ ]{3,}$");
            char[] array = statement.ToCharArray();
            Regex regex = new Regex(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$");
            if (regex.IsMatch(statement)==false || array[0]==' ') {
                return false;
            }
            return true;
        }

        public BaseResponse<Category> Delete(Category entity) {
            var response = this.Remove(entity);
            return response;
        }

        public BaseResponse<Category> Edit(Category entity) {
            if (Validate(entity)) {
                entity.Name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Name);
                var response = this.Update(entity);
                return response;
            }
            return new BaseResponse<Category>(new MessageCode().OperationFailed);
        }

        public bool checkNameLength(string statement)
        {
            if (statement.Length < 3) {
                return true;
            }
            return false;
        }

        public BaseResponse<List<Category>> GetCategoryList()
        {
            try {
                var categories = _unitOfWork.Categories.GetCategoryList();
                if (categories.Count > 0) {
                    return new BaseResponse<List<Category>>(categories);
                }
                return new BaseResponse<List<Category>>(null);
            }
            catch (Exception) {
                return new BaseResponse<List<Category>>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<Category> GetArticleByName(string name)
        {
            try {
                var category = _unitOfWork.Categories.GetArticleByName(name);
                if (category.Articles.Count > 0) {
                    return new BaseResponse<Category>(category);
                }
                return new BaseResponse<Category>(null);
            }
            catch (Exception) {
                return new BaseResponse<Category>(new MessageCode().ErrorCreated);
            }
        }
    }
}
