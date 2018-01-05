using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Emma.Blog.WebApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Emma.Blog.WebApi.Controllers
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller
    {

        private JwtSettings _jwtSettings;
        public AuthController(IOptions<JwtSettings> jwtSettingsAccesser)
        {
            _jwtSettings = jwtSettingsAccesser.Value;
        }

        // GET api/values
        [HttpGet]

        public IActionResult Token(string username, string password)
        {
            if (!(username == "123" && password == "123"))
            {
                return BadRequest();
            }

            List<Person> persons = new List<Person>()
            {
                new Person()
                {
                     Age =12,
                     Name = "zhangsan"
                },
                new Person()
                {
                     Age =34,
                     Name = "lisi"
                },
            };

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid,"123"),
                new Claim(ClaimTypes.Name,"songlin"),
                new Claim(ClaimTypes.Role,"admin2"),
                new Claim("persons", Newtonsoft.Json.JsonConvert.SerializeObject(persons)),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.Now;
            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                now,
                now.AddMinutes(30),
                creds
                );

            var c = new Claim("tokenType", "refresh");
            claims.Add(c);
            var refreshToken = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                now,
                now.AddMinutes(50),
                creds
                );

            var handler = new JwtSecurityTokenHandler();

            return Ok(new { token = handler.WriteToken(token), refreshToken = handler.WriteToken(refreshToken) });
        }



        [HttpGet]

        public IActionResult RefreshToken(string refreshToken)
        {
            try
            {

                SecurityToken validateToken = new JwtSecurityToken();
                var handler = new JwtSecurityTokenHandler();
                var claimsPrincipal = handler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidAudience = _jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                }, out validateToken);

                if(!claimsPrincipal.HasClaim(a=>a.Type == "tokenType"))
                {
                    return Ok(new { token = "无效的刷新token" });
                }

         

                return Ok(new { token = "123" });
            }
            catch (Exception ex)
            {
                return Unauthorized();
                throw;
            }
         
           
        }

        // GET api/values/5

    }
}
