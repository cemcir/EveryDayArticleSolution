using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EveryDayArticle.Entities
{
    public class Liked
    {
        [Required]
        public string UserId { get; set; }

        public virtual Article Article { get; set; }

        [Required]
        public int ArticleId { get; set; }
    }
}
