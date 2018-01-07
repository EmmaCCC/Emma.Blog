using System;
using System.Collections.Generic;
using System.Text;
using Emma.Blog.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Emma.Blog.Service
{
    public class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            var httpContext = MyHttpContext.Current;
            var context =  (DbContext)httpContext.RequestServices.GetService(typeof(BlogContext));

            return context;
        }
    }
}
