using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
namespace Emma.Blog.Common
{
    public class Global
    {
        public static IConfiguration Configuration
        {
            get; set;
        }

        public static IServiceProvider ServiceProvider
        {
            get; set;
        }
    }
}
