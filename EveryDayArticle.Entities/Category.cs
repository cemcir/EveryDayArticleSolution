using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EveryDayArticle.Entities
{
    public class Category {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
