using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Business.Abstract;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.Entities;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EveryDayArticle.Web.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly Message _message;
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ILikedService _likedService;

        public CommentController(Message message, ICategoryService categoryService, IArticleService articleService,ICommentService commentService,ILikedService likedService) {
            _message = message;
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _likedService = likedService;
        }

        [HttpPost]
        public IActionResult AddComment(ArticleModel model)
        {
            //var response = _articleService.GetArticleOfDay();
            var response = _articleService.GetById(model.Id);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            if (response.Success) {
                if (response.Extra != null) {
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Comments = new List<CommentModel>();
                    article.Text = response.Extra.Text;
                    var commentReponse = _commentService.Add(new Comment() {
                        ArticleId = model.Id,
                        Content = model.Comment.Content,
                        UserId = "abcd",
                        CommentDate=DateTime.Now
                    });
                    if (commentReponse.Success == false) {
                        _message.Content = "Veri Eklenirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                        _message.Css = "danger";
                        ViewData["CommentMessage"] = _message;
                    }
                    int commentCount = _commentService.GetCommentCountById(model.Id);
                    if (commentCount != 0) {
                         article.Comment.CommentCount = commentCount;
                        if (IsCommentActive.IsActive) {
                            var comments = _commentService.GetCommentsById(response.Extra.Id);
                            if (comments.Success) {
                                article.Comments = comments.Extra.Select(i => new CommentModel() {
                                    Content = i.Content,
                                    UserId = i.UserId
                                }).ToList();
                            }
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
            article.Comment.Content = null;
            return View(article);
        }

        [HttpPost]
        public IActionResult GetComments(int Id)
        {
            var response = _articleService.GetArticleOfDay();
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            if (response.Success) {
                if (response.Extra != null) {
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    int commentCount = _commentService.GetCommentCountById(Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                        var comments  = _commentService.GetCommentsById(response.Extra.Id);
                        if (comments.Success) {
                            article.Comments = comments.Extra.Select(i => new                   CommentModel() {
                                    Content=i.Content,
                                    UserId=i.UserId
                                }).ToList();
                            IsCommentActive.IsActive = true;
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
            return View(article);
        }

        [HttpPost]
        public IActionResult GetCommentsById(int Id) {
            var response = _articleService.GetById(Id);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
                if (response.Extra != null) {
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    int commentCount = _commentService.GetCommentCountById(Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                        var comments = _commentService.GetCommentsById(response.Extra.Id);
                        if (comments.Success) {
                            article.Comments = comments.Extra.Select(i => new CommentModel() {
                                Content = i.Content,
                                UserId = i.UserId
                            }).ToList();
                            IsCommentActive.IsActive = true;

                            int likedCount = _likedService.GetLikedCount(Id);
                            if (likedCount != 0) {
                                article.Liked.LikedCount = likedCount;
                            }
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
            return View(article);
        }

    }
}