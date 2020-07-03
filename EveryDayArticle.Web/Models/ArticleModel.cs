using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EveryDayArticle.Web.Models
{
    public class ArticleModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public DateTime PublishDate { get; set; }

        public string ImageUrl { get; set; }

        public string UserId { get; set; }

        public int CategoryId { get; set; }

        public Article Article { get; set; }

        public List<Category> Categories { get; set; }

        public CommentModel Comment { get; set; }

        public LikedModel Liked { get; set; }

        public List<CommentModel> Comments { get; set; }
    }
}
