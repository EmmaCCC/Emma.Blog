using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emma.Blog.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Emma.Blog.Data
{

    public static class DbInitializer
    {
        public static void Initialize(BlogContext context)
        {
            context.Database.EnsureCreated();
          
            // Look for any students.
            if (context.Users.Any())
            {
                return;   // DB has been seeded
            }

            context.Add(new User()
            {
                UserId = Guid.NewGuid(),
                NickName = "songlin"

            });

            context.SaveChanges();
        }
    }

}
