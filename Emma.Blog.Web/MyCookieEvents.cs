using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emma.Blog.Web
{
    public class MyCookieEvents: CookieAuthenticationEvents
    {
        public override Task SignedIn(CookieSignedInContext context)
        {
            base.SignedIn(context);
            var claims = context.Principal.Claims;
         
            context.Response.Cookies.Append("token", "123");

            return Task.CompletedTask ;
        }

        public override Task SigningIn(CookieSigningInContext context)
        {
            base.SigningIn(context);
            var claims = context.HttpContext.User.Claims;
            return Task.CompletedTask;
        }

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            return Task.CompletedTask;
        }
    }
}
