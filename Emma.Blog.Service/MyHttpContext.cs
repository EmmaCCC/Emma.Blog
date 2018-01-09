using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Service
{

    public static class MyHttpContext
    {

        public static HttpContext Current
        {
            get
            {
                object factory = ServiceProvider.GetService(serviceType: typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor));

                HttpContext context = ((IHttpContextAccessor)factory).HttpContext;
                return context;
            }
        }
    }

}
