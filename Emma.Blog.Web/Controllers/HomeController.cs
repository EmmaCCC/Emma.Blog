using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Emma.Blog.Data;
using Emma.Blog.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Emma.Blog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Emma.Blog.Service.RegisterLogin;

namespace Emma.Blog.Web.Controllers
{
    public class HomeController : Controller
    {

        private BlogContext _blogContext;
        private AccountService _accountService;
        public HomeController(BlogContext blogContext,AccountService accountService)
        {
            _blogContext = blogContext;
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            var user = new User();
            user.UserId = Guid.NewGuid();
            user.UserName = "songlin";
            user.Password = "password";
            _blogContext.Add(user);
            _blogContext.SaveChanges();
            return View();
        }

        [Authorize]
        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
