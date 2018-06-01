using Emma.Blog.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emma.Blog.Web.Extensions
{
    public class EndRequestFilter : IResultFilter
    {
        public void OnResultExecuted(ResultExecutedContext context)
        {
            var dbContext = (DbContext)context.HttpContext.RequestServices.GetService(typeof(BlogContext));
            if (dbContext != null)
            {
                dbContext.Dispose();
            }
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
           
        }
    }
}
