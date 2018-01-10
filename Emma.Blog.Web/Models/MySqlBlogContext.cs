using System;
using Emma.Blog.Data;
using Emma.Blog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Emma.Blog.Web.Models
{
    public class MySqlBlogContext : DbContext
    {
        public Guid Id;
        public MySqlBlogContext(DbContextOptions<MySqlBlogContext> options) : base(options)
        {
            Id = Guid.NewGuid();
        }
       

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }



    }
}
