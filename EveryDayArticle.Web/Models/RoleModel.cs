using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class RoleModel
    {
        [Required(ErrorMessage ="Rol ismi gereklidir")]
        public string Name { get; set; }

        public string Id { get; set; }
    }
}
