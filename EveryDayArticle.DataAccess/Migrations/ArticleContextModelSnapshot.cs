﻿// <auto-generated />
using System;
using EveryDayArticle.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EveryDayArticle.DataAccess.Migrations
{
    [DbContext(typeof(ArticleContext))]
    partial class ArticleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EveryDayArticle.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CategoryId");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("SurName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("EveryDayArticle.Entities.AuthorArticle", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<int>("AuthorId");

                    b.HasKey("ArticleId", "AuthorId");

                    b.HasIndex("AuthorId");

                    b.ToTable("AuthorArticles");
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Comment", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<string>("UserId");

                    b.Property<DateTime>("CommentDate");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.HasKey("ArticleId", "UserId", "CommentDate");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Liked", b =>
                {
                    b.Property<int>("ArticleId");

                    b.Property<string>("UserId");

                    b.HasKey("ArticleId", "UserId");

                    b.ToTable("Likeds");
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Article", b =>
                {
                    b.HasOne("EveryDayArticle.Entities.Category", "Category")
                        .WithMany("Articles")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EveryDayArticle.Entities.AuthorArticle", b =>
                {
                    b.HasOne("EveryDayArticle.Entities.Article", "Article")
                        .WithMany("AuthorArticles")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("EveryDayArticle.Entities.Author", "Author")
                        .WithMany("AuthorArticles")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Comment", b =>
                {
                    b.HasOne("EveryDayArticle.Entities.Article", "Article")
                        .WithMany("Comments")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EveryDayArticle.Entities.Liked", b =>
                {
                    b.HasOne("EveryDayArticle.Entities.Article", "Article")
                        .WithMany("Likeds")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
