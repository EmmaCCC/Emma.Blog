using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Emma.Blog.WebApi.Extensions;
using Emma.Blog.WebApi.Models;

namespace Emma.Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ValuesController : Controller
    {
        //[SignatureRequired]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { code = 402, msg = "token已失效" }); ;
        }

        //[SignatureRequired]
        [HttpGet("list")]
        public IActionResult List()
        {
            return Ok(new { code = 0, data = new { list = new List<int>() { 1, 2, 3 } } });
        }




        [Authorize]
        [HttpPost]
        public IActionResult Post(string username, string password)
        {
            var name = HttpContext.User.Identity.Name;

            return Ok(new { status = 0, name, username, password });
        }


        [Authorize]
        [HttpPost("GetData"), ValidateModel]
        public IActionResult GetData(UserViewModel user, BookViewModel book, List<string> imgUrls, string name)
        {
            return Ok(new { status = 0, user, book, imgUrls, name });
        }

    }
}
