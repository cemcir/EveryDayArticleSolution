using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class PasswordChangeModel
    {
        [Required(ErrorMessage ="Eski şifreniz gereklidir")]
        [MinLength(4,ErrorMessage ="Şifreniz en az 4 karakterli olmalıdır")]
        public string PasswordOld { get; set; }

        [Required(ErrorMessage = "Yeni şifreniz gereklidir")]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmak olmalıdır")]
        public string PasswordNew { get; set; }

        [Required(ErrorMessage = "Onay yeni şifre gereklidir")]
        [MinLength(4, ErrorMessage = "Şifreniz en az 4 karakterli olmak zorundadır")]
        [Compare("PasswordNew",ErrorMessage ="Yeni şifreniz ve onay şifreniz birbirinden farklıdır")]
        public string PasswordConfirm { get; set; }
    }
}
