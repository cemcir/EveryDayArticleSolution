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
    public class ArticleController : Controller
    {
        private readonly Message _message;
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly ILikedService _likedService;
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }

        public ArticleController(Message message,ICategoryService categoryService,IArticleService articleService, ICommentService commentService,ILikedService likedService,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager) {
            _message = message;
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _likedService = likedService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetArticles()
        {
            var response = _articleService.GetArticleOfDay();
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
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
                else {
                    _message.Content = "Bu Güne Ait Kayıtlı Bir Makale Bulunamadı";
                    _message.Css = "warning";
                }
            }
            else if(response.ErrorMessageCode==new MessageCode().ErrorCreated) {
                _message.Content = "Makale Bilgisi Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            
            ViewData["Message"] = _message;
            //ViewData["Test"] = "false";
            return View(article);
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult GetArticleList()
        {
            var response = _articleService.GetArticleList();
            ViewData["Message"] = _message;
            if (response.Success) {
                if (response.Extra != null) {
                    return View(new ArticleListModel() {
                        Articles=response.Extra
                    });
                }
                else {
                    _message.Title = "Uyarı";
                    _message.Content = "Sistemde Kayıtlı Makale Bilgisi Bulunamadı";
                    _message.Css = "warning";
                }
            }
            else if(new MessageCode().ErrorCreated == response.ErrorMessageCode) {
                _message.Title = "Hata";
                _message.Content = "Makaleler Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            ViewData["Message"] = _message;
            return View(new ArticleListModel());
        }

        [HttpGet]
        [Authorize(Roles ="admin")]
        public IActionResult AddArticle()
        {
            var response = _categoryService.GetAll();
            if (response.Success) {
                ViewData["Message"] = _message;
                return View(new ArticleModel() {
                    Categories=response.Extra
                });
            }
            else if(new MessageCode().ErrorCreated == response.ErrorMessageCode) {
                _message.Title = "Hata";
                _message.Content = "Sayfa Yüklenirken Bir Hata Meydana Geldi";
                _message.Css = "danger";
            }
            else if(new MessageCode().NoEntity == response.ErrorMessageCode) {
                _message.Title = "Uyarı";
                _message.Content = "Sistemde Kayıtlı Kategori Bilgisi Olmadığından Sisteme Makale Ekleyemezsiniz";
                _message.Css = "warning";
            }
            ViewData["Message"] = _message;
            return View(new ArticleModel() {
                Categories = null
            });
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public IActionResult AddArticle(ArticleModel model)
        {           
            var articleResp = _articleService.GetAll();
            if (articleResp.Success) {
                DateTime date = _articleService.GetMaxArticleDate();
                var article = _articleService.Create(new Article() {
                    Title=model.Title,
                    Text=model.Text,
                    CreatedDate=DateTime.Now,
                    PublishDate=date.AddDays(1),
                    CategoryId=model.CategoryId,
                    UserId="abcd"
                });
                if (article.Success) {
                    return RedirectToAction("GetArticleList","Article");
                }
                else if (new MessageCode().OperationFailed==article.ErrorMessageCode) {
                    ViewData["Message"] = _articleService.message;
                }
            }
            else if(new MessageCode().NoEntity == articleResp.ErrorMessageCode) {
                 var articleResponse = _articleService.Create(new Article() {
                    Title = model.Title,
                    Text = model.Text,
                    CreatedDate = DateTime.Now,
                    PublishDate = DateTime.Today,
                    CategoryId = model.CategoryId,
                    UserId = "abcd"
                });
                if (articleResponse.Success) {
                    return RedirectToAction("GetArticleList", "Article");
                }
                else if (new MessageCode().OperationFailed == articleResponse.ErrorMessageCode) {
                    ViewData["Message"] = _articleService.message;
                }
            }
                        
            var categoryResponse = _categoryService.GetAll();
            if (categoryResponse.Success) {
                ViewData["Message"] = _message;
                return View(new ArticleModel() {
                    Categories = categoryResponse.Extra,
                    CategoryId=model.CategoryId
                });
            }
            else if (new MessageCode().ErrorCreated ==categoryResponse.ErrorMessageCode) {
                _message.Title = "Hata";
                _message.Content = "Sayfa Yüklenirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            
            ViewData["Message"] = _message;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public IActionResult EditArticle(ArticleModel model,string btn)
        {
            var response = _articleService.GetArticleById(model.Id);
            if (btn=="edit") {
                var article = new ArticleModel();
                if (response.Success) {
                    article.Id = response.Extra.Id;
                    article.Text = response.Extra.Text;
                    article.Title = response.Extra.Title;
                    article.CategoryId = response.Extra.CategoryId;
                    var categoryResponse = _categoryService.GetAll();
                    if (categoryResponse.Success) {
                        article.Categories = categoryResponse.Extra;
                    }
                    else {
                        _message.Content = "Veriler Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                        _message.Css = "danger";
                    }
                }
                else if (new MessageCode().ErrorCreated == response.ErrorMessageCode) {
                    _message.Content = "Veriler Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                    _message.Css = "danger";
                }
                
                ViewData["Message"] = _message;
                return View(article);
            }           
            else {
                //BaseResponse<Article> articleResponse = _articleService.GetArticleById(model.Id);                
                if (response.Success) {
                    Article art = response.Extra;
                    art.Title = model.Title;
                    art.Text = model.Text;
                    art.CategoryId = model.CategoryId;
                    
                    var editResponse = _articleService.Edit(art);
                    if (editResponse.Success) {
                        return RedirectToAction("GetArticleList", "Article");
                    }
                }
                var respCategory = _categoryService.GetAll();
                ViewData["Message"] = _articleService.message;
                model.Categories = respCategory.Extra;
                return View(model);               
            }           
        }       

        [HttpPost]
        [Authorize(Roles ="admin")]
        public IActionResult DeleteArticle(ArticleModel model)
        {
            BaseResponse<Article> articleResponse = _articleService.GetById(model.Id);
            var response = _articleService.Remove(articleResponse.Extra);
            if (response.Success) {
                return RedirectToAction("GetArticleList","Article");
            }
            _message.Title = "Hata";
            _message.Content = "Makale Bilgisi Silinirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
            _message.Css = "danger";
            ViewData["Message"] = _message;
            return RedirectToAction("GetArticleList","Article");
        }

        [HttpPost]
        public IActionResult GetArticles(ArticleModel model)
        {            
            var response = _articleService.GetArticleOfDay();
            var article = new ArticleModel();
            if (response.Success)
            {
                if (response.Extra != null) {
                    article.Id = response.Extra.Id;
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
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
        public IActionResult GetArticleByName(CategoryModel model) {
            var response = _categoryService.GetArticleByName(model.Name);
            var articles= new ArticleListModel();
            if (response.Success) {
                if (response.Extra != null) {
                    articles.Articles = response.Extra.Articles;
                    articles.Category = response.Extra;
                }
                else {
                    _message.Content = "Bu Kategoriye Ait Kayıtlı Makale Bilgisi Bulunamadı";
                    _message.Css = "warning";
                }
            }
            else if(response.ErrorMessageCode==new MessageCode().ErrorCreated) {
                _message.Content = "Makale Listesi Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar deneyiniz";
            }
            ViewData["Message"] = _message;
            return View(articles);
        }

        [HttpPost]
        public IActionResult GetArticleById(ArticleModel model) {
            var response = _articleService.GetById(model.Id);
            var article = new ArticleModel();
            article.Comment = new CommentModel();
            article.Liked = new LikedModel();
            if (response.Success) {
                if (response.Extra != null) {
                    AppUser user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                    article.Id = response.Extra.Id;
                    article.ImageUrl = user.Picture;
                    article.Comments = new List<CommentModel>();
                    article.Title = response.Extra.Title;
                    article.Text = response.Extra.Text;
                    int commentCount = _commentService.GetCommentCountById(response.Extra.Id);
                    if (commentCount != 0) {
                        article.Comment.CommentCount = commentCount;
                        int likedCount = _likedService.GetLikedCount(model.Id);
                        if (likedCount != 0) {
                            article.Liked.LikedCount = likedCount;
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
            //ViewData["Test"] = "false";
            return View(article);

            /*
            var response = _articleService.GetById(model.Id);
            var article = new ArticleModel();
            if (response.Success) {
                article.Article = response.Extra;
            }
            else if(new MessageCode().ErrorCreated == response.ErrorMessageCode) {
                _message.Content = "Makale Verileri Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _message.Css = "danger";
            }
            ViewData["Message"] = _message;
            return View(article);
            */
        }
    }
}