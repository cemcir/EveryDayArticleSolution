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
    public class BaseController : Controller
    {
        protected readonly Message _message;
        protected readonly ICategoryService _categoryService;
        protected readonly IArticleService _articleService;
        protected readonly ICommentService _commentService;
        protected readonly ILikedService _likedService;
        protected UserManager<AppUser> _userManager { get; }
        protected SignInManager<AppUser> _signInManager { get; }
        protected RoleManager<AppRole> _roleManager { get; }

        protected AppUser CurrentUser => _userManager.FindByNameAsync(User.Identity.Name).Result;

        public BaseController(Message message,IArticleService articleService,ICategoryService categoryService,ICommentService commentService,ILikedService likedService,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,RoleManager<AppRole> roleManager=null ) {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _message = message;
            _articleService = articleService;
            _categoryService = categoryService;
            _commentService = commentService;
            _likedService = likedService;
        }

        public void AddModelError(IdentityResult result) {

            foreach (var item in result.Errors) {
                ModelState.AddModelError("",item.Description);
            }
        }

        public ArticleModel GetArticleOfDay()
        {
            var response = _articleService.GetArticleOfDay();
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success)
            {
                if (response.Extra != null) {
                    AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    article.Id = response.Extra.Id;
                    article.Comments = new List<CommentModel>();
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    article.ImageUrl = user.Picture;
                    int commentCount = _commentService.GetCommentCountById(response.Extra.Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                    }
                    int likedCount = _likedService.GetLikedCount(article.Id);
                    if (likedCount != 0) {
                        article.Liked.LikedCount = likedCount;
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
            return article;
        }

        public ArticleModel GetArticleById(int Id)
        {
            var response = _articleService.GetArticleById(Id);
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
                        /*
                        var likedResponse = _likedService.AddLiked(new Liked() {
                            ArticleId = Id,
                            UserId = "abcd"
                        });
                        if (likedResponse.Success) {
                            int likedCount = _likedService.GetLikedCount(Id);
                            if (likedCount != 0) {
                                article.Liked.LikedCount = likedCount;
                            }
                        }
                        else {
                            _message.Content = "Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                            _message.Css = "danger";
                        }
                        */
                    }
                    int likedCount = _likedService.GetLikedCount(Id);
                    if (likedCount != 0) {
                        article.Liked.LikedCount = likedCount;
                    }
                    else {
                        article.Liked.LikedCount = likedCount;
                    }
                    if (IsCommentActive.IsActive) {
                        var comments = _commentService.GetCommentsById(Id);
                        if (comments.Success) {
                            article.Comments = comments.Extra.Select(i => new CommentModel() {
                                Content = i.Content,
                                AppUser = _userManager.FindByIdAsync(i.UserId).Result
                            }).ToList();
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
            return article;
        }
    }
}