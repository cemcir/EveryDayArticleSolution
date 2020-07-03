using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Email alanı gereklidir")]
        [EmailAddress(ErrorMessage = "Email adresiniz doğru formatta değil")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre alanı gereklidir")]
        [MinLength(4,ErrorMessage ="Şifreniz en az 4 karakterli olmalıdır")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
