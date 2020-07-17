using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Business.Abstract;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.Entities;
using EveryDayArticle.Web.Identity;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EveryDayArticle.Web.Controllers
{
    public class LikedController : BaseController
    {
        public LikedController(Message message, IArticleService articleService, ICategoryService categoryService, ICommentService commentService, ILikedService likedService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager = null) :base(message,articleService,categoryService,commentService,likedService,userManager,signInManager) {
            
        }

        [HttpPost]
        public IActionResult AddLiked(LikedModel model) {            
            AppUser user = CurrentUser;
    
            var likedResponse = _likedService.AddLiked(new Liked() {
                ArticleId = model.ArticleId,
                UserId=user.Id
            });
            var article = new ArticleModel();
            article = GetArticleById(model.ArticleId);
            if (likedResponse.Success) { 
                /*
                int likedCount = _likedService.GetLikedCount(model.ArticleId);
                if (likedCount != 0) {
                    article.Liked.LikedCount = likedCount;
                }
                else {
                    article.Liked.LikedCount = likedCount;
                }
                */
                if (IsCommentActive.IsCommentButtonActive == true && IsCommentActive.IsActive==false) {
                    var commentResponse = _commentService.GetCommentsByUserId(user.Id, model.ArticleId);
                    if (commentResponse.Success) {
                        article.Comments = commentResponse.Extra.Select(i => new CommentModel() {
                            Content = i.Content,
                            AppUser = _userManager.FindByIdAsync(i.UserId).Result
                        }).ToList();
                    }
                    else {
                        _message.Content = "Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                        _message.Css="danger";
                    }
                }
                /*
                if (IsCommentActive.IsActive) {
                    var comments = _commentService.GetCommentsById(model.ArticleId);
                    if (comments.Success) {
                        article.Comments = comments.Extra.Select(i => new CommentModel() {
                            Content = i.Content,
                            UserId = i.UserId
                        }).ToList();
                    }
                } 
                */
            }
            else {
                _message.Content = "Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            ViewData["Message"] = _message;
            
            /*
            var response = _articleService.GetArticleById(model.ArticleId);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
                if (response.Extra != null) {
                    AppUser user = CurrentUser;
                    article.Id = response.Extra.Id;
                    article.Comments = new List<CommentModel>();
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    article.ImageUrl = user.Picture;
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
            */
            return View(article);
        }
    }
}