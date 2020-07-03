using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class CommentModel
    {
        public string Content { get; set; }

        public DateTime CommentDate { get; set; }

        public int CommentCount { get; set; }

        public string UserId { get; set; }
    }
}
