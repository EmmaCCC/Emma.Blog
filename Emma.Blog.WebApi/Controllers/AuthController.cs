﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Emma.Blog.Common;
using Emma.Blog.Service.Account;
using Emma.Blog.Service.Auth;
using Emma.Blog.Service.Account.Model;
using Emma.Blog.Data.Models;

namespace Emma.Blog.WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
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

        [HttpPost("Token")]
        //[CaptchaValidate]
        public IActionResult Token(LoginDto dto)
        {
            try
            {
                //return Ok(new { status = 0});
                //UserService service = new UserService();
                ClaimUser claimUser = null;
                if (dto.UserName == "admin" && dto.Password== "123")
                {
                    claimUser =  new ClaimUser(new User()
                    {
                        UserId = 1,
                        UserName = "admin",
                        Password = "123",
                        NickName = "songlin"
                    });
                }
                //var claimUser = service.Login(dto.UserName, dto.Password);
                if (claimUser == null)
                {
                    //string clientId = HttpContext.Request.Cookies["clientId"];
                    //string code = ValidateCode.GetCode(clientId);
                    return Ok(new
                    {
                        code = 1,
                        msg = "用户名或者密码错误"
                    });
                }
                List<Claim> claims = claimUser.GetClaims();
                //签发token
                var token = JwtTokenUtil.Encode(claims, _jwtSettings);

                //签发refreshtoken
                claims.Add(new Claim("tokenType", "refresh"));
                var refreshToken = JwtTokenUtil.Encode(claims, _jwtSettings);

                return Ok(new { code = 0, data = new { token, refreshToken, _jwtSettings.Expires, type = "Bear" } });
            }
            catch (Exception ex)
            {
                Common.LogHelper.Error("Error", ex);
                return Ok(new
                {
                    code = 1,
                    msg = ex.Message
                });
            }
        }



        /// <summary>
        /// token失效后，app需要使用refreshToken进行刷新token，来获取新的token，保证一个token在较短的时间使用，提高了安全性
        /// 扩展：可以设置refreshtoken失效时间为一个月，这样就可以是实现App一个月内的免登陆
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken(string refreshToken)
        {
            return Token(new LoginDto() { Password = "123", UserName = "admin" });

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


        /// <summary>
        /// 生成客户端唯一标识
        /// </summary>
        /// <returns></returns>
        [HttpGet("ClientId")]
        public IActionResult ClientId()
        {
            try
            {
                RedisClient client = new RedisClient();
                CheckCodeParam param = new CheckCodeParam();
                param.IsRequired = false;
                param.Code = "000000";
                var id = Guid.NewGuid().ToString();
                client.SetString(id, JsonHelper.Serialize(param), TimeSpan.FromDays(30));
                HttpContext.Response.Cookies.Append("clientId", id, new Microsoft.AspNetCore.Http.CookieOptions()
                {
                    Expires = DateTimeOffset.Now.AddDays(30)
                });

                return Ok(new { status = 0, data = Guid.NewGuid() });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 1, message = ex.Message });
            }
        }

        /// <summary>
        /// 检测是否需要验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("RequireCode")]
        public IActionResult RequireCode()
        {
            try
            {
                string clientId = HttpContext.Request.Cookies["clientId"];
                if (string.IsNullOrEmpty(clientId))
                {
                    return Unauthorized();
                }
                var result = ValidateCode.IsRequired(clientId);
                string code = string.Empty;
                if (result)
                {
                    code = ValidateCode.GetCode(clientId);
                }
                return Ok(new { status = 0, data = new { isRequired = result, code } });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 1, message = ex.Message });
            }
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCode")]
        public IActionResult GetCode()
        {
            try
            {
                string clientId = HttpContext.Request.Cookies["clientId"];
                if (string.IsNullOrEmpty(clientId))
                {
                    return Unauthorized();
                }
                string code = ValidateCode.GetCode(clientId);
                return Ok(new { status = 0, data = new { code } });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 1, message = ex.Message });
            }
        }

    }
}
