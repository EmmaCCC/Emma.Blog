using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Common
{
    public class Global
    {
        public static IConfiguration Configuration
        {
            get; set;
        }
    }
}
