using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EveryDayArticle.Entities
{
    public class Article
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public virtual List<Liked> Likeds { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        public virtual List<AuthorArticle> AuthorArticles { get; set; }

        public virtual List<Comment> Comments { get; set; }
    }
}
