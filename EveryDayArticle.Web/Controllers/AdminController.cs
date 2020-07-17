using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Web.Identity;
using EveryDayArticle.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.Business.Abstract;

namespace EveryDayArticle.Web.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : BaseController
    {
        public AdminController(Message message, IArticleService articleService, ICategoryService categoryService, ICommentService commentService, ILikedService likedService, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager  ) : base(message, articleService, categoryService, commentService, likedService, userManager, null,roleManager){ }

        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RoleCreate(RoleModel model)
        {
            AppRole role = new AppRole();

            role.Name = model.Name;
            IdentityResult result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded) {
                return RedirectToAction("Roles","Admin");
            }
            else {
                AddModelError(result);
            }

            return View(model);
        }

        public IActionResult Users()
        {
            return View(_userManager.Users.ToList());
        }

        public IActionResult Roles()
        {
            return View(_roleManager.Roles.ToList());
        }

        [HttpPost]
        public IActionResult RoleDelete(string Id)
        {
            AppRole role = _roleManager.FindByIdAsync(Id).Result;

            if (role != null) {
                IdentityResult result = _roleManager.DeleteAsync(role).Result;
            }
            return RedirectToAction("Roles", "Admin");
        }

        [HttpGet]
        public IActionResult RoleUpdate(string Id) {

            AppRole role = _roleManager.FindByIdAsync(Id).Result;

            if (role!= null) {
                return View(role.Adapt<RoleModel>());
            }

            return RedirectToAction("Roles","Admin");
        }

        [HttpPost]
        public IActionResult RoleUpdate(RoleModel model)
        {
            AppRole role = _roleManager.FindByIdAsync(model.Id).Result;

            if (ModelState.IsValid) {
                if (role != null) {
                    role.Name = model.Name;
                    IdentityResult result = _roleManager.UpdateAsync(role).Result;
                    if (result.Succeeded) {
                        return RedirectToAction("Roles","Admin");
                    }
                    else {
                        AddModelError(result);
                    }
                }
            }

            return View(model);
        }

        public IActionResult RoleAssign(string Id)
        {
            TempData["userId"] = Id;
            AppUser user = _userManager.FindByIdAsync(Id).Result;

            ViewBag.userName = user.UserName;
            IQueryable<AppRole> roles = _roleManager.Roles;

            List<string> userRoles= _userManager.GetRolesAsync(user).Result as List<string>;

            List<RoleAssignModel> roleAssignModels = new List<RoleAssignModel>();

            foreach (var role in roles) {

                RoleAssignModel r = new RoleAssignModel();
                r.RoleId = role.Id;
                r.RoleName = role.Name;
                if (userRoles.Contains(role.Name)) {
                    r.Exist = true;
                }
                else {
                    r.Exist = false;
                }
                roleAssignModels.Add(r);
            }

            return View(roleAssignModels);
        }

        [HttpPost]
        public async  Task<IActionResult> RoleAssign(List<RoleAssignModel> models)
        {
            AppUser user = _userManager.FindByIdAsync(TempData["userId"].ToString()).Result;

            foreach (var item in models) {
                if (item.Exist) {
                    await _userManager.AddToRoleAsync(user,item.RoleName);
                }
                else {
                    await _userManager.RemoveFromRoleAsync(user,item.RoleName);
                }
            }

            return RedirectToAction("Users","Admin");
        }
    }
}