using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Emma.Blog.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
namespace Emma.Blog.Web.Extensions
{
    public class JwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly JwtSettings _jwtSettings;
        public JwtDataFormat(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }
        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            return JwtTokenUtil.Encode(data.Principal.Claims, _jwtSettings);
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            throw new NotImplementedException();
        }

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {

            SecurityToken validatedToken;
            var claimsPrincipal = JwtTokenUtil.Decode(protectedText, _jwtSettings, out validatedToken);
            if(claimsPrincipal == null)
            {
                return null;
            }
            return new AuthenticationTicket(claimsPrincipal, CookieAuthenticationDefaults.AuthenticationScheme);
        }



    }
}
