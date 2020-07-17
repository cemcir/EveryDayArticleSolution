using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Business.Abstract;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using EveryDayArticle.Web.Identity;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EveryDayArticle.Web.Controllers
{
    [Authorize]
    public class CommentController : BaseController
    {
        public CommentController(Message message, IArticleService articleService, ICategoryService categoryService, ICommentService commentService, ILikedService likedService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null) : base(message, articleService, categoryService, commentService, likedService, userManager, signInManager){ }

        [HttpPost]
        public IActionResult AddComment(ArticleModel model)
        {
            IsCommentActive.IsCommentButtonActive = true;
            var article = new ArticleModel();
            //article = GetArticleById(model.Id);
            AppUser user = CurrentUser;

            var commentReponse = _commentService.Add(new Comment() {
                ArticleId = model.Id,
                Content = model.Comment.Content,
                UserId = user.Id,
                CommentDate = DateTime.Now
            });
            article = GetArticleById(model.Id);
            if (commentReponse.Success == true) {
                /*
                int commentCount = _commentService.GetCommentCountById(model.Id);
                if (commentCount != 0) {
                    article.Comment.CommentCount = commentCount;
                }
                */
                article = GetArticleById(model.Id);
            }
            else {
                _message.Content = "İşlem Gerçekleşirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
                ViewData["CommentMessage"] = _message;
            }
           

            if (IsCommentActive.IsCommentButtonActive == true && IsCommentActive.IsActive==false) {
                var commentResponse = _commentService.GetCommentsByUserId(user.Id,model.Id);
                if (commentReponse.Success) {
                    article.Comments = commentResponse.Extra.Select(i => new CommentModel() {
                        Content = i.Content,
                        AppUser = _userManager.FindByIdAsync(i.UserId).Result
                    }).ToList();
                }
                else {
                    _message.Content = "İşlem Gerçekleşirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                    _message.Css = "danger";
                    ViewData["CommentMessage"] = _message;
                }
            }
            /*
            if (IsCommentActive.IsActive) {
                var comments = _commentService.GetCommentsById(model.Id);
                if (comments.Success) {
                    article.Comments = comments.Extra.Select(i => new CommentModel() {
                        Content = i.Content,
                        UserId = i.UserId
                    }).ToList();                    
                }
            }
            */
            
            //var response = _articleService.GetArticleOfDay();
            /*
            var response = _articleService.GetById(model.Id);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
                if (response.Extra != null) {
                    AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Comments = new List<CommentModel>();
                    article.Text = response.Extra.Text;
                    
                    var commentReponse = _commentService.Add(new Comment() {
                        ArticleId = model.Id,
                        Content = model.Comment.Content,
                        UserId = user.Id,
                        CommentDate = DateTime.Now
                    });
                    if (commentReponse.Success == false) {
                        _message.Content = "Veri Eklenirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                        _message.Css = "danger";
                        ViewData["CommentMessage"] = _message;
                    }
                    int likedCount = _likedService.GetLikedCount(model.Id);
                    if (likedCount != 0) {
                        article.Liked.LikedCount = likedCount;
                    }
                    int commentCount = _commentService.GetCommentCountById(model.Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                        if (IsCommentActive.IsActive) {
                            var comments = _commentService.GetCommentsById(response.Extra.Id);
                            
                            if (comments.Success) {
                                article.Comments = comments.Extra.Select(i => new CommentModel()
                                {
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
            else if (response.ErrorMessageCode == new MessageCode().ErrorCreated)
            {
                _message.Content = "Makale Bilgisi Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            ViewData["Message"] = _message;
            article.Comment.Content = null;
            */
            return View(article);
        }

        [HttpPost]
        public IActionResult GetComments(int Id)
        {
            return View();
            /*
            var response = _articleService.GetArticleOfDay();
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            if (response.Success)
            {
                if (response.Extra != null)
                {
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    int commentCount = _commentService.GetCommentCountById(Id);
                    if (commentCount != 0)
                    {
                        article.Comment.CommentCount = commentCount;
                        var comments = _commentService.GetCommentsById(response.Extra.Id);
                        if (comments.Success)
                        {
                            article.Comments = comments.Extra.Select(i => new CommentModel()
                            {
                                Content = i.Content,
                                UserId = i.UserId
                            }).ToList();
                            IsCommentActive.IsActive = true;
                        }
                    }
                }
                else
                {
                    _message.Content = "Bu Güne Ait Kayıtlı Bir Makale Bulunamadı";
                    _message.Css = "warning";
                }
            }
            else if (response.ErrorMessageCode == new MessageCode().ErrorCreated)
            {
                _message.Content = "Makale Bilgisi Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            ViewData["Message"] = _message;
            return View(article);
            */
        }

        [HttpPost]
        public IActionResult GetCommentsById(int Id)
        {
            var response = _articleService.GetById(Id);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
                if (response.Extra != null) {
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    AppUser user = CurrentUser;
                    article.ImageUrl = user.Picture;
                    int commentCount = _commentService.GetCommentCountById(Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                        var comments = _commentService.GetCommentsById(response.Extra.Id);
                        if (comments.Success) {
                            article.Comments = comments.Extra.Select(i => new CommentModel() {
                                Content = i.Content,
                                AppUser = _userManager.FindByIdAsync(i.UserId).Result
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