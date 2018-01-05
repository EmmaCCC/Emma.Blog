using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Emma.Blog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace Emma.Blog.Web.Controllers
{
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class UserController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }


        public IActionResult Auth(string returnUrl)
        {
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
                new Claim(ClaimTypes.Name,"songlin"),
                new Claim(ClaimTypes.Role,"Admin"),
   
                new Claim("Persons", Newtonsoft.Json.JsonConvert.SerializeObject(persons)),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            string s = UserType.Normal.ToString();

            if (returnUrl != null)
            {
                return Redirect(returnUrl);
            }
            return View();
        }

        [Authorize]
        public IActionResult Info()
        {
            
            //ViewBag.Name = HttpContext.User.Claims.First(a => a.Type == ClaimTypes.Name).Value;
            //ViewBag.Role = HttpContext.User.Claims.First(a => a.Type == ClaimTypes.Role).Value;
            //ViewBag.Persons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Person>>(HttpContext.User.Claims.First(a => a.Type == "Persons").Value);
            return Content("Admin,Leader");
        }


        [MyAuth(Roles = new string[] { "Leader" })]
        public IActionResult Leader()
        {
           
            return Content("Leader");
        }

        [MyAuth(Roles = new string[] { "Admin" })]
        public IActionResult Admin()
        {

            return Content("Admin");
        }

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
    }
}

