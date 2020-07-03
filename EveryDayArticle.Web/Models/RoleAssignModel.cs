using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class RoleAssignModel
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public bool Exist { get; set; }
    }
}
