using EveryDayArticle.Business.Abstract;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;
        private readonly Message _categoryMessage;
        public CategoryListViewComponent(ICategoryService categoryService,Message categoryMessage) {
            _categoryService = categoryService;
            _categoryMessage = categoryMessage;
        }

        public IViewComponentResult Invoke() {
            var category = new CategoryListViewModel();
            var response = _categoryService.GetCategoryList();
            if (response.Success) {
                category.Categories = response.Extra;  
            }  
            /*
            else if(response.ErrorMessageCode==new MessageCode().ErrorCreated) {
                _categoryMessage.Content = "Kategori Verileri Getirilirken Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                _categoryMessage.Css = "danger";
            }
            else if(response.ErrorMessageCode==new MessageCode().NoEntity) {
                _categoryMessage.Content = "Sistemde Kayıtlı Kategori Bilgisi Bulunamadı";
                _categoryMessage.Css = "warning";
            }
            */
            //category.CategoryMessage = _categoryMessage;
            return View(category);
        }
    }
}

