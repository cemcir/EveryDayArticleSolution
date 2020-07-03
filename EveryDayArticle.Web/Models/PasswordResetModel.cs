using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class PasswordResetModel
    {
        /*
        [Required(ErrorMessage ="Email alanı gereklidir")]
        [EmailAddress(ErrorMessage ="Email alanı doğru formatta değil")]
        public string Email { get; set; }        
        */
        [Required(ErrorMessage ="Şifre alanı gereklidir")]
        [MinLength(4,ErrorMessage ="şifreniz en az 4 karakterli olmalıdır")]
        public string PasswordNew { get; set; }
        
    }
}
