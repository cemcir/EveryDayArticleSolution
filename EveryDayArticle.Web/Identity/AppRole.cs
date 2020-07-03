using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Identity
{
    public class AppRole:IdentityRole
    {
        public string RoleType { get; set; }
    }
}
