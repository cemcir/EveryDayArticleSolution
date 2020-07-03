using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Entities
{
    public class AuthorArticle
    {
        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }
    }
}
