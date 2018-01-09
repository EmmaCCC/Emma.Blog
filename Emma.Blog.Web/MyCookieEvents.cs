using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Emma.Blog.Web
{
    public class MyCookieEvents: CookieAuthenticationEvents
    {

        public override Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            var claims = context.HttpContext.User.Claims;
            return base.ValidatePrincipal(context);
        }
    }
}
