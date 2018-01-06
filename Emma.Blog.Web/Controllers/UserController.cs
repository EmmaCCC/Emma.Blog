using Emma.Blog.Service.Auth;
using Emma.Blog.Service.RegisterLogin;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Emma.Blog.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly JwtSettings _jwtSettings;
        private AccountService _accountService;
        public AccountController(IOptions<JwtSettings> jwtSettingsOptions, AccountService accountService)
        {
            _jwtSettings = jwtSettingsOptions.Value;
            _accountService = accountService;
        }
        public IActionResult Jwt()
        {

            return Ok(_jwtSettings);

        }

        public IActionResult Login()
        {

            return Content("登录界面");

        }

        public IActionResult Auth(string returnUrl)
        {

            List<Claim> claims = _accountService.Login("123","456").GetClaims();

            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user, new AuthenticationProperties()
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddSeconds(_jwtSettings.Expires)
            });

            return Redirect(returnUrl);

        }

        [Authorize(Roles = "Leader,Admin")]
        public IActionResult Info()
        {
            var isTrue = this.HttpContext.User.Identity.AuthenticationType;
            return Ok(this.HttpContext.User.Identity.AuthenticationType);
            //return Content("Leader,Admin");

        }
        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {

            return Content("Admin");

        }
        [Authorize(Roles = "Leader")]
        public IActionResult Leader()
        {

            return Content("Leader");

        }

        public IActionResult Common()
        {

            return Content("Common");

        }
    }
}
