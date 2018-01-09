using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Service
{
    public static class ServiceProvider
    {
        public static IServiceProvider Provider;

        static ServiceProvider()
        {
            
        }
        public static object GetService(Type serviceType)
        {
            return Provider.GetService(serviceType: serviceType);

        }
    }
}
