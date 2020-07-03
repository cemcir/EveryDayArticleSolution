using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Business.Abstract;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.Entities;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace EveryDayArticle.Web.Controllers
{
    public class LikedController : Controller
    {
        private readonly Message _message;
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ILikedService _likedService;

        public LikedController(Message message, ICategoryService categoryService, IArticleService articleService, ICommentService commentService,ILikedService likedService) {
            _message = message;
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _likedService = likedService;
        }

        [HttpPost]
        public IActionResult AddLiked(LikedModel model) {
            var response = _articleService.GetArticleById(model.ArticleId);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
                if (response.Extra != null) {
                    article.Id = response.Extra.Id;
                    article.Comments = new List<CommentModel>();
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    int commentCount = _commentService.GetCommentCountById(response.Extra.Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                        var likedResponse = _likedService.AddLiked(new Liked() {
                            ArticleId = model.ArticleId,
                            UserId = "abcd"
                        });
                        if (likedResponse.Success) {
                            int likedCount = _likedService.GetLikedCount(model.ArticleId);
                            if (likedCount != 0) {
                                article.Liked.LikedCount = likedCount;
                            }
                        }
                        else {
                            _message.Content = "Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                            _message.Css = "danger";
                        }
                    }
                }
                else {
                    _message.Content = "Bu Güne Ait Kayıtlı Bir Makale Bulunamadı";
                    _message.Css = "warning";
                }
            }
            else if (response.ErrorMessageCode == new MessageCode().ErrorCreated) {
                _message.Content = "Makale Bilgisi Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }

            ViewData["Message"] = _message;
            //ViewData["Test"] = "false";
            return View(article);
        }
    }
}