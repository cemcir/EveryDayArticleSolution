using EveryDayArticle.Web.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.CustomValidation
{
    public class CustomPasswordValidator : IPasswordValidator<AppUser>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user, string password) {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower())) {
                if (user.Email.Contains(user.UserName)==false) {
                    errors.Add(new IdentityError() {
                        Code = "PasswordContainsUserName",
                        Description = "şifre kullanıcı adı içeremez"
                    });
                }
            }

            if (password.ToLower().Contains("1234")) {
                errors.Add(new IdentityError() {
                    Code="PaswordContains1234",Description="şifre alanı ardışık sayı içeremez"
                });
            }

            if (password.ToLower().Contains(user.Email.ToLower())) {
                errors.Add(new IdentityError() {
                    Code="PasswordContainsEmail",Description="şifre alanı email adresinizi içeremez"
                });
            }

            if (errors.Count == 0) {
                return Task.FromResult(IdentityResult.Success);
            }
            else {
                return Task.FromResult(IdentityResult.Failed(errors.ToArray()));
            }
        }
    }
}
