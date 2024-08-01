using System;
using BlogAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser>? ApplicationUsers { get; set; }
        public DbSet<Post>? Posts { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Tag>? Tags { get; set; }
        public DbSet<PostLike>? PostLikes { get; set; }
        public DbSet<CommentLike>? CommentLikes { get; set; }
        public DbSet<PostTag>? PostTags { get; set; }
        public DbSet<SubCategory>? SubCategories { get; set; }
        public DbSet<UsersPost>? UsersPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PostTag>().HasKey(a => new { a.PostId, a.TagId });
            modelBuilder.Entity<PostLike>().HasKey(a => new { a.PostId, a.UserId });
            modelBuilder.Entity<CommentLike>().HasKey(a => new { a.CommentId, a.UserId });
            modelBuilder.Entity<UsersPost>().HasKey(a => new { a.PostId, a.UsersId });

            modelBuilder.Entity<Comment>().HasOne(m => m.ParentComment).WithMany().HasForeignKey(m => m.CommentId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CommentLike>().HasOne(ab => ab.ApplicationUser).WithMany(a => a.CommentLikes).HasForeignKey(ab => ab.UserId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Post>().HasMany(p => p.Comments).WithOne(c => c.Post).HasForeignKey(c => c.PostId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>().HasMany(c => c.CommentLikes).WithOne(cl => cl.Comment).HasForeignKey(cl => cl.CommentId).OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Post>().HasMany(p => p.PostLikes).WithOne(pl => pl.Post).HasForeignKey(pl => pl.PostId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>().HasMany(c => c.Replies).WithOne(c => c.ParentComment).HasForeignKey(c => c.CommentId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
