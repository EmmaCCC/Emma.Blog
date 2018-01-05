using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emma.Blog.Web
{
    public class MyAuth : AuthorizeAttribute
    {
        public new string[] Roles { get; set; }
    }
}
