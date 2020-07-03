using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class LikedModel
    {
        public string UserId { get; set; }

        public int ArticleId { get; set; }

        public int LikedCount { get; set; }

    }
}
