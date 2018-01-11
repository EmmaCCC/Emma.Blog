using System;
using Emma.Blog.Data.Models;
using Emma.Blog.Service.Account;
using Emma.Blog.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Emma.Blog.Common;

namespace Emma.Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {

            //UserService service = new UserService();
        
            //var claimUser = service.Register(new User()
            //{
            //     NickName = "mysql"
            //});

           
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
