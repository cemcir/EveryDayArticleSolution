using EveryDayArticle.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.DataAccess
{
    public class ArticleContext:DbContext
    {
        
        public ArticleContext(DbContextOptions options): base(options) { }
        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                UseSqlServer(@"Server=ASUS\SQLEXPRESS01;Database=ArticleDB;integrated security=true;");
        }
        */
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<AuthorArticle>()
                    .HasKey(author => new{author.ArticleId,author.AuthorId });

            modelBuilder.Entity<Liked>().HasKey(liked => new {liked.ArticleId,liked.UserId });

            modelBuilder.Entity<Comment>().HasKey(comment => new {comment.ArticleId,comment.UserId,comment.CommentDate });
        }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<AuthorArticle> AuthorArticles { get; set; }
        
        public DbSet<Comment> Comments { get; set; }

        public DbSet<Liked> Likeds { get; set; }
        
    }
}
