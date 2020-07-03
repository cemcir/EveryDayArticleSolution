using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class ArticleListModel
    {
        public List<Article> Articles { get; set; }

        public Category Category { get; set; }
    }
}
