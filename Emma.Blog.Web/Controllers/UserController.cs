using Emma.Blog.Data.Models;
using Emma.Blog.Service.Account;
using Emma.Blog.Service.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using Emma.Blog.Data;


namespace Emma.Blog.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserService service;

        public UserController(IOptions<JwtSettings> jwtSettingsAccessor)
        {
            _jwtSettings = jwtSettingsAccessor.Value;
            service = new UserService();

        }
        public IActionResult Jwt()
        {

            return Ok(_jwtSettings);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(User user)
        {


            var claimUser = service.Register(user);

            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimUser.GetClaims(), CookieAuthenticationDefaults.AuthenticationScheme));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(_jwtSettings.Expires)
            });

            return RedirectToAction("List");


        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Login(User user)
        {

            //这里采用异步方式登录，返回json 信息，当然也可以采用同步
            try
            {
                var claimUser = service.Login(user.UserName, user.Password);
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimUser.GetClaims(), CookieAuthenticationDefaults.AuthenticationScheme));
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, new AuthenticationProperties()
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddSeconds(_jwtSettings.Expires)
                });
                return Json(new { status = 0 });
            }
            catch (ErrorMsgException ex)
            {
                return Json(new { status = 1, message = ex.Message });
            }


        }


        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("index", "home");

        }

        public IActionResult List()
        {

            var list = service.GetPageList(1, 10);
            return View(list);
        }

        public IActionResult Delete(long id)
        {
            service.Delete(id);
            return RedirectToAction("List");
        }


        public IActionResult Info()
        {
            return View();
        }

    }
}
