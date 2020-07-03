using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
