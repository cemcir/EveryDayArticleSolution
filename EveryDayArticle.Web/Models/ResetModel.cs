using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class ResetModel
    {
        [Required(ErrorMessage = "Email alanı gereklidir")]
        [EmailAddress(ErrorMessage = "Email alanı doğru formatta değil")]
        public string Email { get; set; }
    }
}
