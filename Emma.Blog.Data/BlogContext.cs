using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Emma.Blog.Data.Models;
using Microsoft.EntityFrameworkCore.Extensions;
using System.Linq;
namespace Emma.Blog.Data
{
    public class BlogContext : DbContext
    {
        public Guid Id;
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
          
            Id = Guid.NewGuid();
        }
       

        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }


    }
}
