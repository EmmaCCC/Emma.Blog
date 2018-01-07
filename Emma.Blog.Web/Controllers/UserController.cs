using Emma.Blog.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Emma.Blog.Data.Models;
using Emma.Blog.Service.Account;


namespace Emma.Blog.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly JwtSettings _jwtSettings;
   
        public UserController(IOptions<JwtSettings> jwtSettingsAccessor)
        {
            _jwtSettings = jwtSettingsAccessor.Value;
           
        }
        public IActionResult Jwt()
        {

            return Ok(_jwtSettings);

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            UserService service = new UserService();
            var claimUser = service.Register(user);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimUser.GetClaims(), CookieAuthenticationDefaults.AuthenticationScheme));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(_jwtSettings.Expires)
            });

            return RedirectToAction("Index","Home");

        }

       

        [Authorize]
        public IActionResult Info()
        {
            return View();
        }

    }
}
