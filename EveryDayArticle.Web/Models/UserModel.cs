using EveryDayArticle.Business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class UserModel
    {
        [Required(ErrorMessage ="Kullanıcı adı gereklidir")]
        public string UserName { get; set; }

        [RegularExpression(@"^(0(\d{3}) (\d{3}) (\d{2}) (\d{2}))$",ErrorMessage ="Telefon numaranız uygun formatta değil")]
        [Required(ErrorMessage ="Telefon bilgisi gereklidir")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage ="Email adresi gereklidir")]
        [EmailAddress(ErrorMessage ="Email adresiniz doğru formatta değil")]
        public string Email { get; set; }
        
        [Required(ErrorMessage ="Şifre gereklidir")]
        //[DataType(DataType.Password)]
        public string Password { get; set; }

        public string Picture { get; set; }

        public string City { get; set; }

        public DateTime? BirthDay { get; set; }

        public Gender Gender { get; set; }
    }
}
