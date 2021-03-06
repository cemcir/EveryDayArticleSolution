﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.Web.Identity;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Mvc.Rendering;
using EveryDayArticle.Business.Enums;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using EveryDayArticle.Business.Abstract;

namespace EveryDayArticle.Web.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(Message message, IArticleService articleService, ICategoryService categoryService, ICommentService commentService, ILikedService likedService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) :base(message, articleService, categoryService, commentService, likedService, userManager, signInManager) {
            
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View(new LoginModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid) {

                AppUser user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null) {

                    if (await _userManager.IsLockedOutAsync(user)) {
                        ModelState.AddModelError("", "Saat başına yapılabilen maksimum istek sayısını aştınız");
                        return View(model);
                    }

                    if (_userManager.IsEmailConfirmedAsync(user).Result==false) {
                        ModelState.AddModelError("","Email adresiniz onaylanmamıştır Lütfen e-postanızı kontrol ediniz");
                        return View(model);
                    }

                    await _signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result =   await _signInManager.PasswordSignInAsync(user, model.Password,model.RememberMe,false);
                    if (result.Succeeded) {

                        await _userManager.ResetAccessFailedCountAsync(user);

                        if (TempData["ReturnUrl"] != null) {
                            return Redirect(TempData["ReturnUrl"].ToString());
                        }
                        return RedirectToAction("GetArticles","Article");
                    }
                    else {
                        await _userManager.AccessFailedAsync(user);

                        int fail = await _userManager.GetAccessFailedCountAsync(user);

                        ModelState.AddModelError("", $"{fail} kez başarısız giriş");

                        if (fail == 3) {
                            await _userManager.SetLockoutEndDateAsync(user, new System.DateTimeOffset(DateTime.Now.AddMinutes(60)));
                            ModelState.AddModelError("","Hesabınız 3 başarısız girişten dolayı 1 saat süreyle kitlenmiştir.Lütfen daha sonra tekrar deneyiniz");
                        }
                        else {
                            ModelState.AddModelError("","Email adresiniz veya şifreniz yanlış");
                        }
                    }
                }
                else {
                    ModelState.AddModelError("","Email adresiniz veya şifreniz yanlış");
                }
                
            }
            //ViewData["Message"] = _message;
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            ViewData["EmailErrorMessage"] = _message;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserModel model)
        {
            if (ModelState.IsValid) {

                var users = _userManager.Users.Where(u => u.PhoneNumber == model.PhoneNumber).FirstOrDefault();

                if (users != null) {
                   ModelState.AddModelError("", "Bu telefon numarası daha önceden kayıtlıdır");
                    return View(model);
                }

                AppUser user = new AppUser();
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                /*
                try {
                    string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string link = Url.Action("ConfirmEmail", "Account", new {
                        userId = user.Id,
                        token = confirmationToken
                    }, protocol: HttpContext.Request.Scheme);

                    string url = $"<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız.</h2><hr/><a href='{link}'>email doğrulama linki</a>";

                    Helper.EmailConfirmation.SendEmail(user.Email,"www.hergunbirmakale.com::Email doğrulama",url);

                    IdentityResult result = await _userManager.CreateAsync(user,model.Password);
                    if (result.Succeeded) {
                        return RedirectToAction("Login","Account");
                    }
                    else {
                        AddModelError(result);
                    }   
                }
                catch (Exception) {
                    _message.Content = "Bir Hata Meydana Geldi Lütfen Daha Sonra Tekrar Deneyiniz";
                    _message.Css = "danger";
                    ViewData["EmailErrorMessage"] = _message;
                }
                */
                
                IdentityResult result = await _userManager.CreateAsync(user,model.Password);

                if (result.Succeeded) {

                    string confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    string link = Url.Action("ConfirmEmail", "Account", new {
                        userId = user.Id,
                        token=confirmationToken
                    },protocol:HttpContext.Request.Scheme);

                    string url = $"<h2>Email adresinizi doğrulamak için lütfen aşağıdaki linke tıklayınız.</h2><hr/><a href='{link}'>email doğrulama linki</a>";

                    Helper.EmailConfirmation.SendEmail(user.Email, "www.hergunbirmakale.com::Email doğrulama", url);

                    return RedirectToAction("Login","Account");
                }
                else {
                    AddModelError(result); 
                }          
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm(string userId,string token) {
            TempData["userId"] = userId;
            TempData["token"] = token;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordConfirm(PasswordResetModel model) {            
            if (ModelState.IsValid) {
                if (TempData["token"] == null) {
                    ModelState.AddModelError("", "Geçersiz link şifrenizi yenileyemezsiniz");
                    return View(model);
                }
                string token = TempData["token"].ToString();
                string userId = TempData["userId"].ToString();

                AppUser user = await _userManager.FindByIdAsync(userId);
                if (user != null) {
                    IdentityResult result = await _userManager.ResetPasswordAsync(user, token, model.PasswordNew);
                    if (result.Succeeded) {
                        await _userManager.UpdateSecurityStampAsync(user);
                        return RedirectToAction("Login", "Account");
                    }
                    else {
                        AddModelError(result);
                        /*
                        foreach (var item in result.Errors) {
                            ModelState.AddModelError("", item.Description);
                        }
                        */
                    }
                }
                else {
                    ModelState.AddModelError("", "Bir hata meydana geldi. Lütfen daha sonra tekrar deneyiniz.");
                }
            }            
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetModel model)
        {
            if (ModelState.IsValid) {
                AppUser user = _userManager.FindByEmailAsync(model.Email).Result;

                if (user != null) {
                    string passwordResetToken = _userManager.GeneratePasswordResetTokenAsync(user).Result;

                    string passwordResetLink = Url.Action("ResetPasswordConfirm", "Account", new
                    {
                        userId = user.Id,
                        token = passwordResetToken

                    }, HttpContext.Request.Scheme);
                    /*
                    string html = $"<form action='/confirm' enctype='multipart/form - data' method='post'><input type='hidden' name='Id' value='{user.Id}'/><input type='hidden' name='Token' value='{passwordResetToken}'/><button type='submit'>Tıklayınız</button></form>";
                    */
                    string url = $"<h2>Şifrenizi yenilemek için lütfen aşağıdaki linke tıklayınız.</h2><hr/><a href='{passwordResetLink}'>şifre yenileme linki</a>";

                    Helper.PasswordReset.PasswordResetSendEmail(user.Email, "www.hergunbirmakale.com::Şifre sıfırlama", url);

                    ViewBag.status = "success";
                }
                else {
                    ModelState.AddModelError("", "Sistemde kayıtlı email adresi bulunamamıştır");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult UserEdit()
        {
            AppUser user = CurrentUser;

            UserModel model = user.Adapt<UserModel>();

            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UserEdit(UserModel model,IFormFile userPicture)
        {
            ModelState.Remove("Password");
            ViewBag.Gender = new SelectList(Enum.GetNames(typeof(Gender)));

            if (ModelState.IsValid) {

                AppUser user = CurrentUser;
                string phone = await _userManager.GetPhoneNumberAsync(user);

                if (phone != model.PhoneNumber) {
                    if (_userManager.Users.Any(u => u.PhoneNumber == model.PhoneNumber)) {
                        ModelState.AddModelError("","Bu telefon numarası başka üye tarafından kullanılmaktadır");
                        return View(model);
                    }
                }

                if(userPicture!=null && userPicture.Length > 0) {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(userPicture.FileName);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UserPicture", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userPicture.CopyToAsync(stream);
                        user.Picture = "/UserPicture/" + fileName;
                    }
                }

                user.UserName = model.UserName;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;
                user.City = model.City;
                user.BirthDay = model.BirthDay;
                user.Gender =(int) model.Gender;

                IdentityResult result= await _userManager.UpdateAsync(user);

                if (result.Succeeded) {

                    await _userManager.UpdateSecurityStampAsync(user);
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user,true);

                    ViewBag.success = "true";
                }
                else {
                    AddModelError(result);
                    /*
                    foreach (IdentityError item in result.Errors) {
                        ModelState.AddModelError("",item.Description);
                    }
                    */
                }
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult PasswordChange()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult PasswordChange(PasswordChangeModel model)
        {
            if (ModelState.IsValid) {

                AppUser user = CurrentUser;

                if (user != null) {
                    bool exits = _userManager.CheckPasswordAsync(user,model.PasswordOld).Result;

                    if (exits==true) {
                        IdentityResult result = _userManager.ChangePasswordAsync(user, model.PasswordOld, model.PasswordNew).Result;
                        if (result.Succeeded) {
                            _userManager.UpdateSecurityStampAsync(user);
                            _signInManager.SignOutAsync();
                            _signInManager.PasswordSignInAsync(user, model.PasswordNew, true, false);

                            ViewBag.success = "true";
                        }
                        else {
                            AddModelError(result);
                            /*
                            foreach (IdentityError item in result.Errors) {
                                ModelState.AddModelError("",item.Description);
                            }
                            */
                        }
                    }
                    else {
                        ModelState.AddModelError("","Eski şifreniz yanlış");
                    }
                }
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult LogOut() {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null) {
            IdentityResult result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded) {
                    _message.Content = "Email adresiniz onaylanmıştır Login ekranından giriş yapabilirsiniz";
                    _message.Css = "success";
                }
                else {
                    _message.Content = "Bir hata meydana geldi. lütfen daha sonra tekrar deneyiniz";
                    _message.Css = "danger";
                }
            }
            else {
                _message.Content = "Kullanıcı bulunamadı";
                _message.Css = "danger";
            }
            ViewData["Message"] = _message;
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            AppUser user = CurrentUser;
            UserModel model = user.Adapt<UserModel>();

            return View(model);
        }
    }
}