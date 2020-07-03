using System;
using System.Collections.Generic;
using System.Globalization;
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
    [Authorize(Roles ="admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly Message _message;

        public CategoryController(ICategoryService categoryService,Message message)
        {
            _categoryService = categoryService;
            _message = message;
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            ViewData["Message"] = _message;
            return View(new CategoryModel());
        }

        [HttpPost]
        public IActionResult AddCategory(CategoryModel model)
        {            
            var catResponse = _categoryService.GetByCategoryName(model.Name);
            if (catResponse.Success) {
                _message.Title = "Uyarı";
                _message.Content = "Kategori Bilgisi Zaten Sistemde Kayıtlı";
                _message.Css = "warning";
                ViewData["Message"] = _message;
                return View(model);
            }
            else {
                var response = _categoryService.Create(new Category() {
                    Name=model.Name
            });
                if (response.Success) {
                    return RedirectToAction("CategoryList", "Category");
                }
                else if(response.ErrorMessageCode==new MessageCode().OperationFailed) {
                  ViewData["Message"] = _categoryService.message;
                }
                else {
                    _message.Title = "Hata";
                    _message.Content = "Veri Eklenirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                    _message.Css = "danger";
                    ViewData["Message"] = _message;
                }
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model,string btn) {
            var response = _categoryService.GetById(model.Id);
            if (btn == "edit") {
                if (response.Success) {
                    ViewData["Message"] =_message ;
                    return View(new CategoryModel() {
                        Id = response.Extra.Id,
                        Name = response.Extra.Name
                    });
                }
                else {
                    if(new MessageCode().ErrorCreated == response.ErrorMessageCode) {
                        _message.Title = "Hata";
                        _message.Content = "Veriler Getirilirken Bir Hata Oluştu Lütfen Daha Sonra Tekrar Deneyiniz";
                        _message.Css = "danger";
                        ViewData["Message"] = _message;
                        return View(new CategoryModel());
                    } 
                }
            }
            else  {
                if (response.Success) {
                    Category cat = response.Extra;
                    cat.Id = model.Id;
                    cat.Name = model.Name;
                    var catResponse = _categoryService.Edit(cat);
                    if (catResponse.Success) {
                        return RedirectToAction("CategoryList","Category");
                    }
                    else {
                        ViewData["Message"] = _message;
                    }
                }
                else {
                    _message.Title = "Hata";
                    _message.Content = "Güncelleme İşlemi Yapılırken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                    _message.Css = "danger";
                    ViewData["Message"] = _message;
                    return View(model);
                }
                return View(model);
            }
            return View(new CategoryModel());
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            //Message message = new Message();
            var response = _categoryService.GetAll();
            if (response.Success) {
                ViewData["Message"] = _message;
                return View(new CategoryListModel() {
                    Categories=response.Extra
                });
            }
            else {
                if(new MessageCode().NoEntity == response.ErrorMessageCode) {
                    _message.Title = "Uyarı";
                    _message.Content = "Kayıtlı Kategori Bilgisi Bulunamadı";
                    _message.Css = "warning";
                }
                if (new MessageCode().ErrorCreated==response.ErrorMessageCode) {
                    _message.Title = "Hata";
                    _message.Content = "Veriler Getirilirken Bir Hata Oluştu Lütfen Daha Sonra Tekrar Deneyiniz";
                    _message.Css = "danger";
                }
            }
            
            ViewData["Message"] = _message;
            return View(new CategoryListModel());
        }

        [HttpPost]
        public IActionResult DeleteCategory(CategoryModel model) {
            var response = _categoryService.GetById(model.Id);
            if (response.Success) {
                var catRepsonse=_categoryService.Delete(response.Extra);
                if (catRepsonse.Success) {
                    return RedirectToAction("CategoryList","Category");
                }
            }
            _message.Title = "Hata";
            _message.Content = "Veriler Silinirken Bir Hata Meydan Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
            _message.Css = "danger";
            ViewData["Message"] = _message;
            return RedirectToAction("CategoryList","Category");
        }
    }
}