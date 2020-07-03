﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EveryDayArticle.Web.Models;
using EveryDayArticle.DataAccess;
using EveryDayArticle.Entities;
using EveryDayArticle.Business.Abstract;
using Microsoft.AspNetCore.Identity;
using EveryDayArticle.Web.Identity;

namespace EveryDayArticle.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;

        public HomeController(ICategoryService categoryService)
        {
            _categoryService = categoryService;    
        }

        public IActionResult Index()
        {
            /*
            var response=_categoryService.Add(new Category() {
                Name = "Makine Mühendisliği"
            });
            if (response.Success) {
                return View();
            }
            */
            return View();    
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Demo()
        {
            return View();
        }
    }
}
