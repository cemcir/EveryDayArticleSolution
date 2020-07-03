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
    public class ArticleService : Service<Article>, IArticleService
    {
        public Message message { get; set; }

        public ArticleService(IUnitOfWork unitOfWork, IRepository<Article> repository, Message message) : base(unitOfWork, repository) {

            this.message = message;
        }

        public bool checkTitleLength(string statement)
        {
            if (string.IsNullOrEmpty(statement) == false) {
                if (statement.Length < 3) {
                    return true;
                }
            }
            return false;
        }

        public bool checkTextLength(string statement)
        {
            if (string.IsNullOrEmpty(statement)==false) {
                if (statement.Length < 500) {
                    return true;
                }
            }
            return false;
        }

        public BaseResponse<Article> Edit(Article entity) {
            if (Validate(entity)) {
                entity.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Title);
                var response = this.Update(entity);
                return response;
            }
            return new BaseResponse<Article>(new MessageCode().OperationFailed);
        }

        public bool Validate(Article entity)
        {
            message.Css = "danger";
            bool valid = true;
            if (isEmpty(entity.Title)) {
                message.Messages.Add("Başlık Girmelisiniz");
                valid = false;
            }
            if (isEmpty(entity.Text)) {
                message.Messages.Add("Metin Girmelisiniz");
                valid = false;
            }
            if (isSelectedCategory(entity.CategoryId)) {
                message.Messages.Add("Kategori Seçiniz");
                valid = false;
            }
            if (checkTextLength(entity.Text)) {
                message.Messages.Add("Metin İçeriği 500 Karakterden Az Olamaz");
                valid = false;
            }
            if (checkTitleLength(entity.Title)) {
                message.Messages.Add("Başlık 3 Karakterden Az Olamaz");
                valid = false;
            }
            if (string.IsNullOrEmpty(entity.Title)==false) {
                if (isTitleValid(entity.Title)==false) {
                    message.Messages.Add("Başlık Alanı İçin Yanlış Giriş");
                    valid = false;
                }
            }
            return valid;
        }

        public BaseResponse<Article> Create(Article entity) {
            if (Validate(entity)) {
                entity.Title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(entity.Title);
                var response = this.Add(entity);
                return response;
            }
            return new BaseResponse<Article>(new MessageCode().OperationFailed);
        }

        public DateTime GetMaxArticleDate()
        {
            try {
                DateTime date=this._unitOfWork.Articles.GetMaxArticleDate();
                return date;
            }
            catch (Exception) {
                throw;
            }
        }

        public BaseResponse<List<Article>> GetArticleList()
        {
            try {
                List<Article> articles = _unitOfWork.Articles.GetArticleList();
                if (articles.Count > 0) {
                    return new BaseResponse<List<Article>>(articles);
                }
                return new BaseResponse<List<Article>>(null);
            }
            catch (Exception) {
                return new BaseResponse<List<Article>>(new MessageCode().ErrorCreated);
            }
        }

        public bool isSelectedCategory(int categoryId)
        {
            if (categoryId == 0) {
                return true;
            }
            return false;
        }

        public bool isTitleValid(string title)
        {
            char[] array = title.ToCharArray();
            Regex regex = new Regex(@"^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$");
            if (regex.IsMatch(title) == false || array[0] == ' ')
            {
                return false;
            }
            return true;
        }

        public BaseResponse<Article> GetArticleOfDay()
        {
            try {
                Article article = _unitOfWork.Articles.GetArticleOfDay();
                if (article != null) {
                    return new BaseResponse<Article>(article);
                }
                return new BaseResponse<Article>(null);
            }
            catch (Exception) {
                return new BaseResponse<Article>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<Article> GetArticleById(int Id) {
            try {
                var article = _unitOfWork.Articles.GetArticleById(Id);
                if (article != null) {
                    return new BaseResponse<Article>(article);
                }
                return new BaseResponse<Article>(null);
            }
            catch (Exception) {
                return new BaseResponse<Article>(new MessageCode().ErrorCreated);
            }
        }
    }
}
