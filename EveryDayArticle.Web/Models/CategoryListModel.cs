using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class CategoryListModel
    {
        public IEnumerable<Category> Categories { get; set; }   
    }
}
