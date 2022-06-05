using Blog.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Data.Concrete.EntityFramework.Mapping
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {

        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).ValueGeneratedOnAdd();
            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.Title).HasMaxLength(100);
            builder.Property(a => a.Content).IsRequired();
            builder.Property(a => a.Content).HasColumnType("NVARCHAR(MAX)");
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.SeoAuthor).IsRequired();
            builder.Property(a => a.SeoAuthor).HasMaxLength(50);
            builder.Property(a => a.SeoDescription).IsRequired();
            builder.Property(a => a.SeoDescription).HasMaxLength(150);
            builder.Property(a => a.SeoTags).IsRequired();
            builder.Property(a => a.SeoTags).HasMaxLength(70);
            builder.Property(a => a.ViewCount).IsRequired();
            builder.Property(a => a.CommentCount).IsRequired();
            builder.Property(a => a.Thumbnail).IsRequired();
            builder.Property(a => a.Thumbnail).HasMaxLength(250);
            builder.Property(a => a.CreatedByName).IsRequired();
            builder.Property(a => a.CreatedByName).HasMaxLength(50);
            builder.Property(a => a.ModifiedByName).IsRequired();
            builder.Property(a => a.ModifiedByName).HasMaxLength(50);
            builder.Property(a => a.CreatedDate).IsRequired();
            builder.Property(a => a.ModifiedDate).IsRequired();
            builder.Property(a => a.IsActive).IsRequired();
            builder.Property(a => a.IsDeleted).IsRequired();
            builder.Property(a => a.Note).HasMaxLength(500);
            builder.HasOne<Category>(a => a.Category).WithMany(c => c.Articles).HasForeignKey(a => a.CategoryId);
            builder.HasOne<User>(a => a.User).WithMany(u => u.Articles).HasForeignKey(a => a.UserId);
            builder.ToTable("Articles");

            builder.HasData(
                new Article
                {
                    Id = 1,
                    CategoryId = 1,
                    Title = "C# 9.0 yenilikler",
                    Content = "When working on large software projects, one thing you want to make sure of is that your various systems are properly designed so that they interact in a clever way. In particular, it’s always interesting to aim for high decoupling, because it allows you to better maintain and validate your codebase.",
                    Thumbnail = "Default.jpg",
                    SeoAuthor = "Ümit Sözen",
                    SeoDescription = "C# 9.0 yenilikler",
                    SeoTags = "C#, c# 9, .net",
                    IsActive = true,
                    IsDeleted = false,
                    CreatedByName = "InitialCreate",
                    CreatedDate = DateTime.Now,
                    ModifiedByName = "InitialCreate",
                    ModifiedDate = DateTime.Now,
                    Note= "C# 9.0 yenilikler",
                    UserId= 1,  
                    ViewCount=200,
                    CommentCount = 1
                },
                 new Article
                 {
                     Id = 2,
                     CategoryId = 2,
                     Title = "C++ 11 yenilikler",
                     Content = "C++ STL is a set of data structures and algorithms that we normally encounter during coding. For example, while solving a problem you wanted to use linked list, so will you create a linked list from scratch? The answer is no, you will use list built into c++ stl library. There are a lot of similar examples which I will be giving throughout this article",
                     Thumbnail = "Default.jpg",
                     SeoAuthor = "Ümit Sözen",
                     SeoDescription = "C++ 11 yenilikler",
                     SeoTags = "C++, C++ 11",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = DateTime.Now,
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = DateTime.Now,
                     Note = "C++ 11 yenilikler",
                     UserId = 1,
                     ViewCount = 300,
                     CommentCount = 1
                 },
                 new Article
                 {
                     Id = 3,
                     CategoryId = 3,
                     Title = "C 11 yenilikler",
                     Content = "C is a powerful general-purpose programming language. It can be used to develop software like operating systems, databases, compilers, and so on. C programming is an excellent language to learn to program for beginners",
                     Thumbnail = "Default.jpg",
                     SeoAuthor = "Ümit Sözen",
                     SeoDescription = "C 11 yenilikler",
                     SeoTags = "C",
                     IsActive = true,
                     IsDeleted = false,
                     CreatedByName = "InitialCreate",
                     CreatedDate = DateTime.Now,
                     ModifiedByName = "InitialCreate",
                     ModifiedDate = DateTime.Now,
                     Note = "C yenilikler",
                     UserId = 1,
                     ViewCount = 150,
                     CommentCount = 1
                 });

        }
    }
}
