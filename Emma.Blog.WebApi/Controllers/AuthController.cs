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
using Emma.Blog.Service.Account;
using Emma.Blog.Service.Auth;
using Emma.Blog.Data;

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

        /// <summary>
        /// 用于App端登录颁发token，以后app要拿着这个token访问接口，失效时间2个小时
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]

        public IActionResult Token(string username, string password)
        {
            try
            {
                UserService service = new UserService();
                var claimUser = service.Login(username, password);
                List<Claim> claims = claimUser.GetClaims();
                //签发token
                var token = JwtTokenUtil.Encode(claims, _jwtSettings);

                //签发refreshtoken
                claims.Add(new Claim("tokenType", "refresh"));
                var refreshToken = JwtTokenUtil.Encode(claims, _jwtSettings);

                return Ok(new { status = 0, token, refreshToken });
            }
            catch (ErrorMsgException ex)
            {
                return Ok(new { status = 1, message = ex.Message });
            }
        }



        /// <summary>
        /// token失效后，app需要使用refreshToken进行刷新token，来获取新的token，保证一个token在较短的时间使用，提高了安全性
        /// 扩展：可以设置refreshtoken失效时间为一个月，这样就可以是实现App一个月内的免登陆
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult RefreshToken(string refreshToken)
        {
            UserService service = new UserService();

            SecurityToken validatedToken;
            var claimsPrincipal = JwtTokenUtil.Decode(refreshToken, _jwtSettings, out validatedToken);

            //判断使用的是不是refreshtoken
            if (claimsPrincipal != null && claimsPrincipal.HasClaim(a => a.Type == "tokenType"))
            {

                //根据claim中的id再次从数据库找到user 使用最新的user信息重新签发token
                var userId = claimsPrincipal.Claims.First(a => a.Type == ClaimTypes.Sid).Value;
                var user = service.GetUser(Convert.ToInt64(userId));

                //重新签发token和refreshtoken
                List<Claim> claims = new ClaimUser(user).GetClaims();
                //签发token
                var token = JwtTokenUtil.Encode(claims, _jwtSettings);

                //签发refreshtoken
                claims.Add(new Claim("tokenType", "refresh"));
                var refreshtoken = JwtTokenUtil.Encode(claims, _jwtSettings);

                return Ok(new { token, refreshtoken });
            }
            //如果refreshtoken 失效了 说明该用户已经一个月没有和你的应用交互了 所以设置为未授权让其重新登录
            return Unauthorized();

        }



    }
}
