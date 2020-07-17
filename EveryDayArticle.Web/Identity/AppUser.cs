using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EveryDayArticle.Web.Identity
{
    public class AppUser:IdentityUser
    {
        public string City { get; set; }

        public string Picture { get; set; }

        public DateTime? BirthDay { get; set; }

        public int Gender { get; set; }
    }
}
