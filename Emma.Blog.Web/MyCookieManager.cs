using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Emma.Blog.Web
{
    public class MyCookieManager : ICookieManager
    {
        public void AppendResponseCookie(HttpContext context, string key, string value, CookieOptions options)
        {

            foreach (var item in context.User.Claims)
            {
                Console.WriteLine("==================claimstype:" + item.Type + ",claimsValue:" + item.Value);
            }
            Console.WriteLine("AppendResponseCookie===================key:" + key + ",value:" + value);

            context.Response.Cookies.Append(key, value);
        }

        public void DeleteCookie(HttpContext context, string key, CookieOptions options)
        {
            Console.WriteLine(key);
        }

        public string GetRequestCookie(HttpContext context, string key)
        {
            var cookieValue = context.Request.Cookies[key];
            Console.WriteLine("GetRequestCookie===================" + key + ",value=" + cookieValue);
            return cookieValue;
        }
    }
}
