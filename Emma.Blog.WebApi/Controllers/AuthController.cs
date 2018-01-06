using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Emma.Blog.Service.Auth;
using Emma.Blog.Service.RegisterLogin;


namespace Emma.Blog.WebApi.Controllers
{
   
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {

        private readonly JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> jwtSettingsAccesser)
        {
            _jwtSettings = jwtSettingsAccesser.Value;
        }

        // GET api/values
        [HttpGet]

        public IActionResult Token(string username, string password)
        {

            List<Claim> claims = new List<Claim>();

            var token = JwtTokenUtil.Encode(claims, _jwtSettings);

            claims.Add(new Claim("tokenType", "refresh"));
            var refreshToken = JwtTokenUtil.Encode(claims, _jwtSettings);

            return Ok(new { token, refreshToken });
        }



        [HttpGet]

        public IActionResult RefreshToken(string refreshToken)
        {

            SecurityToken validatedToken;

            var claimsPrincipal = JwtTokenUtil.Decode(refreshToken, _jwtSettings, out validatedToken);

            if (claimsPrincipal != null && claimsPrincipal.HasClaim(a => a.Type == "tokenType"))
            {
                //重新签发
                List<Claim> claims = new List<Claim>();
              

                return Ok(new { token = "123" });
            }

            return Unauthorized();

        }

       

    }
}
